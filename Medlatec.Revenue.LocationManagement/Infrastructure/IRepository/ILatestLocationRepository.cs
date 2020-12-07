using Medlatec.Revenue.LocationManagement.Infrastructure.Models;
using Medlatec.Revenue.LocationManagement.Infrastructure.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medlatec.Revenue.LocationManagement.Infrastructure.IRepository
{
    public interface ILatestLocationRepository
    {
        Task<LatestLocation> GetByUserId(string userId);
        Task<long> RemoveById(string id);
        Task<LatestLocation> Add(LatestLocation latestLocation);
        Task<List<LocationHistoryViewModel>> LatestLocationUsers();
    }
}
