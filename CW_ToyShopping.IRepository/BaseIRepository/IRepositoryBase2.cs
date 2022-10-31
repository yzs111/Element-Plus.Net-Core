
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CW_ToyShopping.IRepository
{
    public interface IRepositoryBase2<T,TId>
    {
        Task<T> GetByIdAsync(TId id);

        Task<bool> IsExistAsync(TId id);
    }
}
