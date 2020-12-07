using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Core.Infrastructure.MongoDb
{
    public class MongoRepositoryBase : IDisposable
    {
        protected IMongoDbContext Context;

        /// <summary>
        /// Khởi tạo class<see cref="RepositoryBase"/>
        /// </summary>
        /// <param name="context">Mongodb context</param>
        protected MongoRepositoryBase(IMongoDbContext context)
        {
            Context = context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
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
