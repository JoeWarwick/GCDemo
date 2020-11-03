using CDWRepository;
using CDWSVCAPI.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static CDWRepository.CDWSVCModel;

namespace CDWSVCAPI.Services
{
    public class SubscriptionsService : ISubscriptionsService
    {
        private CDWSVCModel _model;
        private ILogger _logger;

        public SubscriptionsService(CDWSVCModel model, ILogger logger) 
        {
            this._model = model;
            this._logger = logger;
        }
        public SubscriptionModel[] GetSubscriptions(Guid usr, string hash)
        {
            var user = _model.CDWSVCUsers.FirstOrDefault(u => u.Id == usr.ToString());
            if (!BLHelper.isValidUser(user, hash))
            {
                _logger.LogWarning("Invalid user attempted access", new[] { user.Id });
                return null;
            }

            var set = _model.Subscribables.Where(f => f.Active && f.IsArtifact && f.Owner.Id == user.Id).OrderByDescending(f => f.Added).ToList();

            var feeds = set.Select(f => {
                var res = DtoHelper<SubscriptionModel>.Map(f);
                res.Url = $"/api/feed/{usr}/{hash}";
                res.Icon = f.SourceGroup.Logo;
                return res;
            });

            return feeds.ToArray();
        }

        public async Task<int> SubmitErrorLog(string userId, string message, string stacktrace, int severity)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            var hash = Convert.ToBase64String(sha1.ComputeHash(Encoding.Unicode.GetBytes(stacktrace)));
            var existing = _model.ClientErrorLogs.FirstOrDefault(c => c.UserId.ToString() == userId && c.StackHash == hash);

            if (existing == null)
            {
                var errorSubmission = new ClientErrorLog
                {
                    UserId = Guid.Parse(userId),
                    ErrorMessage = message,
                    StackTrace = stacktrace,
                    StackHash = hash
                };
                _model.ClientErrorLogs.Add(errorSubmission);
            }
            else
            {
                existing.SubmissionCount = existing.SubmissionCount + 1;
            }
            return await _model.SaveChangesAsync();
        }
    }
}
