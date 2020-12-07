using Medlatec.Revenue.LocationManagement.Infrastructure.Models;
using Medlatec.Revenue.LocationManagement.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medlatec.Revenue.LocationManagement.Infrastructure.IRepository
{
    public interface ILocationRepository
    {
        Task<long> Save(List<LocationHistory> locations);

        Task<long> Update(List<LocationHistory> locations);

        Task<Task> Delete(string id);

        Task<string> Add(LocationHistory employeeLocations);

        Task<LocationHistory> GetInfo(string employeeId);

        Task<List<LocationHistoryViewModel>> Search(string keyword, int page, int pageSize, out long totalRows);
    }
}
