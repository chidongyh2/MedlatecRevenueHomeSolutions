using CMS_Core.Infrastructure.MongoDb;
using Medlatec.Revenue.LocationManagement.Infrastructure.IRepository;
using Medlatec.Revenue.LocationManagement.Infrastructure.Models;
using Medlatec.Revenue.LocationManagement.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Medlatec.Revenue.LocationManagement.Infrastructure.Repository
{
    public class LocationRepository : MongoRepositoryBase, ILocationRepository
    {
        private readonly IMongoDbRepository<LocationHistory, string> _locationMongoDbRepository;
        public LocationRepository(IMongoDbContext context) : base(context)
        {
            _locationMongoDbRepository = Context.GetRepository<LocationHistory, string>();
        }
        public async Task<string> Add(LocationHistory employeeLocations)
        {
            var t = await _locationMongoDbRepository.AddAsync(employeeLocations);
            return t.Id;
        }

        public Task<Task> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<LocationHistory> GetInfo(string employeeId)
        {
            throw new NotImplementedException();
        }

        public async Task<long> Save(List<LocationHistory> locations)
        {
            var result = await _locationMongoDbRepository.AddManyAsync(locations);
            return result;
        }

        public Task<List<LocationHistoryViewModel>> Search(string keyword, int page, int pageSize, out long totalRows)
        {
            Expression<Func<LocationHistory, bool>> spec = x => x.Id != null;
            var sort = Context.Filters.Sort<LocationHistory, DateTime>(x => x.CreateTime, true);
            var paging = Context.Filters.Page<LocationHistory>(1, 20);
            totalRows = _locationMongoDbRepository.Count(spec);
            var items = _locationMongoDbRepository.GetsAsAsync(x => new LocationHistoryViewModel {
                Id = x.Id,
                CreateTime = x.CreateTime,
                UserId = x.UserId,
                UserFullName = x.UserFullName,
                Lng = x.Lng,
                Lat = x.Lat,
                Geo = x.Geo
            }, spec, sort, paging);
            return items;
        }

        public Task<long> Update(List<LocationHistory> locations)
        {
            throw new NotImplementedException();
        }
    }
}
