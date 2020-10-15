using System;
using System.Threading.Tasks;

namespace CDWSVCAPI.Services
{
    public interface ISubscriptionsService
    {
        public Task<SubscriptionModel[]> GetSubscriptions(Guid usr, string hash);
        public Task SubmitErrorLog(string message, string stacktrace, int severity);
    }
}