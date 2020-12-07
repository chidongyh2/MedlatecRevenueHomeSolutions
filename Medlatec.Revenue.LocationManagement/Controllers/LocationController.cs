using System.Collections.Generic;
using System.Threading.Tasks;
using Medlatec.Revenue.LocationManagement.Infrastructure.IService;
using Medlatec.Revenue.LocationManagement.Infrastructure.ModelMeta;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medlatec.Revenue.LocationManagement.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly ILatestLocationService _latestLocationService;
        public LocationController(ILocationService locationService, ILatestLocationService latestLocationService)
        {
            _locationService = locationService;
            _latestLocationService = latestLocationService;
        }

        [HttpPost]
        public async Task<IActionResult> Insert(LocationMeta locationMeta)
        {
            var result = await _locationService.Insert(locationMeta);
            if (result.Code > 0)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int page = 1, int pageSize = 20)
        {
            var result = await _locationService.Search(keyword, page, pageSize);
            return Ok(result);
        }

        [Route("adds")]
        [HttpPost]
        public async Task<IActionResult> Insert(IList<LocationMeta> locationMeta)
        {
            var result = await _locationService.Inserts(locationMeta);
            if (result.Code > 0)
                return Ok(result);

            return BadRequest(result);
        }

        [Route("latestLocationUsers")]
        [HttpGet]
        public async Task<IActionResult> LatestLocationUsers()
        {
            var result = await _locationService.LatestLocationUsers();
            return Ok(result);
        }

    }
}
