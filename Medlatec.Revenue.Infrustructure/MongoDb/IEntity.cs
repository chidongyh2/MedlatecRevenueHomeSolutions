using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Core.Infrastructure.MongoDb
{
    public interface IEntity<Tkey>
    {
        Tkey Id { get; set; }
    }
}
