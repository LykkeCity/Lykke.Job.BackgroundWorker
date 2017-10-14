using System;
using System.Threading.Tasks;

using Common;

using Lykke.Job.BackgroundWorker.Contract.Contexts;
using Lykke.Job.BackgroundWorker.Core.Domain.EventLogs;
using Lykke.Job.BackgroundWorker.Core.Services.Geospatial;

namespace Lykke.Job.BackgroundWorker.Components.Workers
{
    public class SetAuthLogGeolocationWorker : IWorker
    {
        private readonly ISrvIpGetLocation _srvIpGetLocation;
        private readonly IAuthorizationLogsRepository _authorizationLogsRepository;
        private SetAuthLogRecordGeolocationContext _context;

        public SetAuthLogGeolocationWorker(ISrvIpGetLocation srvIpGetLocation,
            IAuthorizationLogsRepository authorizationLogsRepository)
        {
            _srvIpGetLocation = srvIpGetLocation;
            _authorizationLogsRepository = authorizationLogsRepository;
        }

        public async Task DoWork()
        {
            if (_context == null)
            {
                throw new InvalidOperationException("Context was not set.");
            }

            var authRecord = await _authorizationLogsRepository.GetRecordAsync(_context.ClientId, _context.AuthLogRecordId);

            if (authRecord != null)
            {
                var geo = await _srvIpGetLocation.GetDataAsync(authRecord.Ip);
                await _authorizationLogsRepository.UpdateGeoDataAsync(_context.ClientId, _context.AuthLogRecordId, geo.CountryCode, geo.City);
            }
        }

        public void SetContext(string contextJson)
        {
            _context = contextJson.DeserializeJson<SetAuthLogRecordGeolocationContext>();
            if (_context == null)
            {
                throw new ArgumentException(nameof(contextJson));
            }
        }
    }
}
