using Medlatec.Revenue.Infrustructure.Models;
using Medlatec.Revenue.LocationManagement.Infrastructure.IRepository;
using Medlatec.Revenue.LocationManagement.Infrastructure.IService;
using Medlatec.Revenue.LocationManagement.Infrastructure.ModelMeta;
using Medlatec.Revenue.LocationManagement.Infrastructure.Models;
using Medlatec.Revenue.LocationManagement.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medlatec.Revenue.LocationManagement.Infrastructure.Service
{
    public class LocationService : ILocationService
    {
        private ILocationRepository _locationRepository;
        private ILatestLocationRepository _latestLocationRepository;
        public LocationService(ILocationRepository locationRepository, ILatestLocationRepository latestLocationRepository)
        {
            _locationRepository = locationRepository;
            _latestLocationRepository = latestLocationRepository;
        }
        public Task<ActionResultResponse> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResultResponse> Insert(LocationMeta location)
        {
            var id = await _locationRepository.Add(new Models.LocationHistory
            {
                UserId = location.UserId,
                UserFullName = location.UserFullName,
                Lng = location.Lng,
                Lat = location.Lat,
                Geo = location.Geo
            });

            // handle for insert last location
            var lastLocationUserExist = await _latestLocationRepository.GetByUserId(location.UserId);

            if (lastLocationUserExist != null)
            {
                var removeLastLocationResult = await _latestLocationRepository.RemoveById(lastLocationUserExist.Id);

                if (removeLastLocationResult < 0)
                    return new ActionResultResponse<string>(-5, "Something went wrong. Please contact administrator !", "Remove latest location failed !", id);
            }

            await _latestLocationRepository.Add(new LatestLocation(location.UserId, location.UserFullName, location.Lat, location.Lng, location.Geo));
            // end handle for insert last location

            if (!string.IsNullOrEmpty(id))
            {
                return new ActionResultResponse<string>(200, "", "Add location successfully !", id);
            }
            return new ActionResultResponse<string>(-1, "", "Add location scucessfully", null);
        }

        public async Task<ActionResultResponse> Inserts(IList<LocationMeta> locations)
        {
            var result = await _locationRepository.Save(locations.Select(x => new Models.LocationHistory
            {
                UserId = x.UserId,
                UserFullName = x.UserFullName,
                Lng = x.Lng,
                Lat = x.Lat,
                Geo = x.Geo
            }).ToList());

            // handle for insert last location
            foreach(var location in locations)
            {
                var lastLocationUserExist = await _latestLocationRepository.GetByUserId(location.UserId);

                if (lastLocationUserExist != null)
                {
                    var removeLastLocationResult = await _latestLocationRepository.RemoveById(lastLocationUserExist.Id);

                    if (removeLastLocationResult < 0)
                        return new ActionResultResponse(-5, "Something went wrong. Please contact administrator !", "Remove latest location failed !");
                }

                await _latestLocationRepository.Add(new LatestLocation(location.UserId, location.UserFullName, location.Lat, location.Lng, location.Geo));
            }
            // end handle for insert last location
            if (result > 0)
            {
                return new ActionResultResponse<long>(200, "", "Add location successfully !", result);
            }
            return new ActionResultResponse<long>(-1, "", "Add location scucessfully", 0);
        }

        public async Task<List<LocationHistoryViewModel>> LatestLocationUsers()
        {
            var items = await _latestLocationRepository.LatestLocationUsers();
            return items;
        }

        public async Task<SearchResult<LocationHistoryViewModel>> Search(string keyword, int page, int pageSize)
        {
            var items = await _locationRepository.Search(keyword, page, pageSize,
                out var totalRows);
           
            return new SearchResult<LocationHistoryViewModel>
            {
                Items = items,
                TotalRows = totalRows
            };
        }
    }
}
