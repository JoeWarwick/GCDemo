using System;
using System.Threading.Tasks;

namespace CDWSVCAPI.Services
{
    public interface ISubscriptionsService
    {
        public SubscriptionModel[] GetSubscriptions(Guid usr, string hash);
        public Task<int> SubmitErrorLog(string userid, string message, string stacktrace, int severity);
    }
}