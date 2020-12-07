using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medlatec.Revenue.LocationManagement.Infrastructure.ModelMeta
{
    public class LocationMeta
    {
        public string  UserId { get; set; }
        public string UserFullName { get; set; }
        public Double Lat { get; set; }
        public Double Lng { get; set; }
        public string Geo { get; set; }
    }
}
