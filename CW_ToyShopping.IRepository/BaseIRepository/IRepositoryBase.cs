using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CW_ToyShopping.IRepository
{
  public interface IRepositoryBase<T>
    {
        Task<List<T>> GetAllAsync();

        Task<IEnumerable<T>> GetbyConditionAsync(Expression<Func<T,bool>> exception);

        void Create(T entity);

        void Update(T entity);

        void Delelte(T entity);

        void DeleteList(IEnumerable<T> entity);

        Task<bool> SaveAsync();

        /// <summary>
        ///  执行返回原生的SQL语句
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        Task<DataTable> GetDataTableBySqlAsync(string strSQL);

        int ExecuteSql(string sql);
    }
}
