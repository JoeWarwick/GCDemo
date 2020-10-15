using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CDWSVCAPI.Services
{
    public class SubscriptionsService : ISubscriptionsService
    {
        public Task<SubscriptionModel[]> GetSubscriptions(Guid usr, string hash)
        {
            throw new NotImplementedException();
        }

        public Task SubmitErrorLog(string message, string stacktrace, int severity)
        {
            throw new NotImplementedException();
        }
    }
}
