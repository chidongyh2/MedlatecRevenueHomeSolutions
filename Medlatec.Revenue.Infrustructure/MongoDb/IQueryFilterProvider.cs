using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Core.Infrastructure.MongoDb
{
    public abstract class IQueryFilterProvider
    {
        /// <summary>
        /// Tạo một bộ lọc để phân trang (phân trang cần được thực hiện sau khi sắp xếp).
        /// </summary>
        /// <typeparam name="T">Kiểu thực thể</typeparam>
        /// <param name="pageAt">Trang hiện tại.</param>
        /// <param name="pageSize">Số phần tử trên 1 trang.</param>
        /// <returns></returns>
        public abstract Func<IMongoQueryable<T>, IMongoQueryable<T>> Page<T>(int pageAt = 1, int pageSize = 20);

        /// <summary>
        /// Tạo một bộ lọc sắp xếp kết quả truy vấn
        /// </summary>
        /// <typeparam name="T">Kiểu thực thể</typeparam>
        /// <typeparam name="TKey">Kiểu dữ liệu của thuộc tính cần sắp xếp.</typeparam>
        /// <param name="sorter">Sorter.</param>
        /// <param name="descending">Nếu là true thì sẽ sắp xếp theo thứ tự giảm dần.</param>
        /// <returns></returns>
        public abstract Func<IMongoQueryable<T>, IMongoQueryable<T>> Sort<T, TKey>(Expression<Func<T, TKey>> sorter, bool descending = false);

        /// <summary>
        /// Tạo một sắp xếp.
        /// </summary>
        /// <typeparam name="T">Kiểu thực thể</typeparam>
        /// <param name="descending">Nếu là true thì sẽ sắp xếp theo thứ tự giảm dần.</param>
        /// <param name="sortExprs">Danh sách tên các cột cần sắp xếp</param>
        /// <returns>IQueryable</returns>
        public abstract Func<IMongoQueryable<T>, IQueryable<T>> CreateSort<T>(bool descending = false, params string[] sortExprs);

        /// <summary>
        /// Tạo một sắp xếp.
        /// </summary>
        /// <typeparam name="T">Kiểu thực thể</typeparam>       
        /// <param name="sortExprs">Danh sách sắp xếp Tên của cột cần sắp xếp | sắp xếp theo thứ tự tăng dần hay giảm dần</param>
        /// <returns>IQueryable</returns>
        public abstract Func<IMongoQueryable<T>, IQueryable<T>> CreateSort<T>(Dictionary<string, bool> sortExprs);
    }
}
