using CW_ToyShopping.DB;
using CW_ToyShopping.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CW_ToyShopping.Repository.BaseRepository
{
   public class RepositoryBase<T,TId>: IRepositoryBase<T>,IRepositoryBase2<T,TId> where T :class
    {
        public OracleDBContext _mysqlDBContext { get; set; }
        
        public RepositoryBase(OracleDBContext mysqlDBContext)
        {
            _mysqlDBContext = mysqlDBContext;
        }

        public Task<List<T>> GetAllAsync()
        {
            var Items = _mysqlDBContext.Set<T>().ToList();
            return Task.FromResult(Items);
        }
        public Task<IEnumerable<T>> GetbyConditionAsync(Expression<Func<T, bool>> exception)
        {
            return Task.FromResult(_mysqlDBContext.Set<T>().Where(exception).AsEnumerable());
        }
        public void Create(T entity)
        {
            _mysqlDBContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _mysqlDBContext.Set<T>().Update(entity);
        }
        public void Delelte(T entity)
        {
            _mysqlDBContext.Set<T>().Remove(entity);
        }
        public void DeleteList(IEnumerable<T> entity)
        {
            _mysqlDBContext.Set<T>().RemoveRange(entity);
        }
        public async Task<bool> SaveAsync()
        {
            return await _mysqlDBContext.SaveChangesAsync() > 0;
        }
        public async Task<T> GetByIdAsync(TId id)
        {
            return await _mysqlDBContext.Set<T>().FindAsync(id);
        }

        public async Task<bool> IsExistAsync(TId id)
        {
            return await _mysqlDBContext.Set<T>().FindAsync(id) != null;
        }

        public async Task<DataTable> GetDataTableBySqlAsync(string strSQL)
        {
            DataTable item = null;

            var conn = _mysqlDBContext.Database.GetDbConnection();

            if (conn.State != System.Data.ConnectionState.Open)
            {
                await conn.OpenAsync();
            }
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = strSQL;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    reader.Close();
                    conn.Close();
                    item = dt;
                }
            }
            return item;
        }
        public int ExecuteSql(string sql)
        {
          return _mysqlDBContext.Database.ExecuteSqlRaw(sql);
        }


    }
}
