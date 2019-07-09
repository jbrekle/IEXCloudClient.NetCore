using System;
using System.Threading;
using System.Threading.Tasks;

namespace IEXCloudClient.NetCore
{
    public class Throttler
    {
        private const double REQUEST_PER_MINUTE_LIMIT = 1000;
        private const double MS_BETWEEN_REQUEST = 60 / REQUEST_PER_MINUTE_LIMIT * 1000;
        private DateTime _lastRequest;

        public async Task Throttle()
        {
            var msSinceLast = (long)(DateTime.Now - _lastRequest).TotalMilliseconds;

            var diff = MS_BETWEEN_REQUEST - msSinceLast;

            if (diff > 0)
                await Task.Delay((int)diff);

            _lastRequest = DateTime.Now;
        }
    }
}