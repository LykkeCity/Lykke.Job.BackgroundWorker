using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Job.BackgroundWorker.Core.Domain.Clients;

namespace Lykke.Job.BackgroundWorker.Core.Services.PersonalData
{
    public interface IPersonalDataService
    {
        Task<IFullPersonalData> GetFullAsync(string id);
        Task<IEnumerable<IFullPersonalData>> GetFullAsync(IEnumerable<string> id);
        Task UpdateAsync(IPersonalData personalData);
    }
}