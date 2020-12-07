using CMS_Core.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Medlatec.Revenue.LocationManagement.Infrastructure.Models
{
    public class LatestLocation : IEntity<string>
    {
        /// <summary>
        /// Lấy về hoặc gán giá trị cho id của entity (Bản ghi primary của 1 entity)
        /// </summary>     
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public Double Lat { get; set; }
        public Double Lng { get; set; }
        public string Geo { get; set; }

        public LatestLocation()
        {
            CreateTime = DateTime.Now;
        }

        public LatestLocation(string userId, string userFullName, double lat, double lng, string geo)
        {
            UserId = userId;
            UserFullName = userFullName;
            Lat = lat;
            Lng = lng;
            Geo = geo;
            CreateTime = DateTime.Now;
        }
    }
}
