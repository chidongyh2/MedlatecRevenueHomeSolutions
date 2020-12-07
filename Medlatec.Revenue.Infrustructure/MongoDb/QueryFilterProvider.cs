using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Core.Infrastructure.MongoDb
{
    public class QueryFilterProvider : IQueryFilterProvider
    {
        #region Overrides of QueryFilterProvider

        /// <summary>
        /// Tạo một bộ lọc để phân trang (phân trang cần được thực hiện sau khi sắp xếp).
        /// </summary>
        /// <typeparam name="T">Kiểu thực thể</typeparam>
        /// <param name="pageAt">Trang hiện tại.</param>
        /// <param name="pageSize">Số phần tử trên 1 trang.</param>
        /// <returns></returns>
        public override Func<IMongoQueryable<T>, IMongoQueryable<T>> Page<T>(int pageAt = 1, int pageSize = 20)
        {
            var myPage = pageAt < 1 ? 1 : pageAt;
            var myPageSize = pageSize <= 0 ? 20 : pageSize;
            return source => source.Skip((myPage - 1) * pageSize).Take(myPageSize);
        }

        /// <summary>
        /// Tạo một bộ lọc sắp xếp kết quả truy vấn
        /// </summary>
        /// <typeparam name="T">Kiểu thực thể</typeparam>
        /// <typeparam name="TKey">Kiểu dữ liệu của thuộc tính cần sắp xếp.</typeparam>
        /// <param name="sorter">Sorter.</param>
        /// <param name="descending">Nếu là true thì sẽ sắp xếp theo thứ tự giảm dần.</param>
        /// <returns></returns>
        public override Func<IMongoQueryable<T>, IMongoQueryable<T>> Sort<T, TKey>(Expression<Func<T, TKey>> sorter, bool descending = false)
        {
            return source => descending ? source.OrderByDescending(sorter) : source.OrderBy(sorter);
        }

        /// <summary>
        /// Tạo một sắp xếp.
        /// </summary>
        /// <typeparam name="T">Kiểu thực thể</typeparam>
        /// <param name="descending">Nếu là true thì sẽ sắp xếp theo thứ tự giảm dần.</param>
        /// <param name="sortExprs">Danh sách tên các cột cần sắp xếp</param>
        /// <returns>IQueryable</returns>
        public override Func<IMongoQueryable<T>, IQueryable<T>> CreateSort<T>(bool descending = false, params string[] sortExprs)
        {
            return source =>
            {
                if (sortExprs == null)
                {
                    return source;
                }

                var type = typeof(T);
                var parameter = Expression.Parameter(type, "p");
                var isFirst = true;
                MethodCallExpression resultExp = null;
                foreach (var sortExpr in sortExprs)
                {
                    var property = type.GetProperty(sortExpr);
                    if (property == null)
                    {
                        continue;
                    }
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);

                    if (isFirst)
                    {
                        resultExp = Expression.Call(typeof(MongoQueryable), descending ? "OrderByDescending" : "OrderBy",
                                                    new[] { type, property.PropertyType }, source.Expression,
                                                    Expression.Quote(orderByExp));
                        isFirst = false;
                    }
                    else
                    {
                        resultExp = Expression.Call(typeof(MongoQueryable), descending ? "ThenByDescending" : "ThenBy",
                                                    new[] { type, property.PropertyType }, resultExp,
                                                    Expression.Quote(orderByExp));
                    }
                }

                return resultExp == null ? source : source.Provider.CreateQuery<T>(resultExp);
            };
        }

        public override Func<IMongoQueryable<T>, IQueryable<T>> CreateSort<T>(Dictionary<string, bool> sortExprs)
        {
            return source =>
            {
                if (sortExprs == null)
                {
                    return source;
                }

                var type = typeof(T);
                var parameter = Expression.Parameter(type, "p");
                var isFirst = true;
                MethodCallExpression resultExp = null;
                foreach (var sortExpr in sortExprs)
                {
                    var property = type.GetProperty(sortExpr.Key);
                    if (property == null)
                    {
                        continue;
                    }
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);

                    if (isFirst)
                    {
                        resultExp = Expression.Call(typeof(MongoQueryable), sortExpr.Value ? "OrderByDescending" : "OrderBy",
                                                    new[] { type, property.PropertyType }, source.Expression,
                                                    Expression.Quote(orderByExp));
                        isFirst = false;
                    }
                    else
                    {
                        resultExp = Expression.Call(typeof(MongoQueryable), sortExpr.Value ? "ThenByDescending" : "ThenBy",
                                                    new[] { type, property.PropertyType }, resultExp,
                                                    Expression.Quote(orderByExp));
                    }
                }

                return resultExp == null ? source : source.Provider.CreateQuery<T>(resultExp);
            };
        }
        #endregion
    }
}
