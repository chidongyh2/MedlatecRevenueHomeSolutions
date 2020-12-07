using Medlatec.Revenue.Infrustructure.Models;
using Medlatec.Revenue.LocationManagement.Infrastructure.ModelMeta;
using Medlatec.Revenue.LocationManagement.Infrastructure.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medlatec.Revenue.LocationManagement.Infrastructure.IService
{
    public interface ILocationService
    {
        Task<ActionResultResponse> Insert(LocationMeta location);

        Task<ActionResultResponse> Delete(string id);

        Task<SearchResult<LocationHistoryViewModel>> Search(string keyword, int page, int pageSize);

        Task<ActionResultResponse> Inserts(IList<LocationMeta> location);
        Task<List<LocationHistoryViewModel>> LatestLocationUsers();
    }
}
