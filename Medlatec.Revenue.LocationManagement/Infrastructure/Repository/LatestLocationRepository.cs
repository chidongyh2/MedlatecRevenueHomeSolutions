using CMS_Core.Infrastructure.MongoDb;
using Medlatec.Revenue.Infrustructure.Models;
using Medlatec.Revenue.LocationManagement.Infrastructure.IRepository;
using Medlatec.Revenue.LocationManagement.Infrastructure.Models;
using Medlatec.Revenue.LocationManagement.Infrastructure.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medlatec.Revenue.LocationManagement.Infrastructure.Repository
{
    public class LatestLocationRepository : MongoRepositoryBase, ILatestLocationRepository
    {
        private readonly IMongoDbRepository<LatestLocation, string> _latestLocationRepository;
        public LatestLocationRepository(IMongoDbContext context) : base(context)
        {
            _latestLocationRepository = Context.GetRepository<LatestLocation, string>();
        }

        public async Task<LatestLocation> Add(LatestLocation latestLocation)
        {
            var result = await _latestLocationRepository.AddAsync(latestLocation);
            return result;
        }

        public async Task<LatestLocation> GetByUserId(string userId)
        {
            return await _latestLocationRepository.GetAsync(x => x.UserId == userId);
        }

        public async Task<List<LocationHistoryViewModel>> LatestLocationUsers()
        {
            var items = await _latestLocationRepository.GetsAsAsync(x => new LocationHistoryViewModel
            {
                Id = x.Id,
                CreateTime = x.CreateTime,
                UserId = x.UserId,
                UserFullName = x.UserFullName,
                Lng = x.Lng,
                Lat = x.Lat,
                Geo = x.Geo
            });
            return items;
        }

        public async Task<long> RemoveById(string id)
        {
            return await _latestLocationRepository.DeleteAsync(x => x.Id == id);
        }
    }
}
