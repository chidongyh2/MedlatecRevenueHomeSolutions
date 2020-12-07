using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Core.Infrastructure.MongoDb
{
    public class MongoDbRepositoryBase : IDisposable
    {
        protected IMongoDbContext Context;
        public MongoDbRepositoryBase(IMongoDbContext context)
        {
            Context = context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                Context = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
