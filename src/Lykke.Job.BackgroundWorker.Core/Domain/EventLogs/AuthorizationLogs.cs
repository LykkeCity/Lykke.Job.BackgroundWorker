using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Job.BackgroundWorker.Core.Domain.EventLogs
{
    public interface IAuthorizationLogRecord
    {
        string ClientId { get; set; }
        DateTime DateTime { get; set; }
        string UserAgent { get; set; }

        string Ip { get; set; }
        string Country { get; set; }
        string City { get; set; }
    }

    public class AuthorizationLogRecord : IAuthorizationLogRecord
    {
        public AuthorizationLogRecord(string clientId, string userAgent, string ip)
        {
            ClientId = clientId;
            DateTime = DateTime.UtcNow;
            UserAgent = userAgent;
            Ip = ip;
        }

        public string ClientId { get; set; }
        public DateTime DateTime { get; set; }
        public string UserAgent { get; set; }
        public string Ip { get; set; }
        public string Device { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
    }

    public interface IAuthorizationLogsRepository
    {
        /// <summary>
        /// Adds auth log record
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Record Id</returns>
        Task<string> AddRecordAsync(IAuthorizationLogRecord record);

        Task<IEnumerable<IAuthorizationLogRecord>> GetAsync(string clientId, DateTime from, DateTime to);

        Task<IAuthorizationLogRecord> GetRecordAsync(string clientId, string recordId);

        Task UpdateGeoDataAsync(string clientId, string recordId, string country, string city);
    }
}