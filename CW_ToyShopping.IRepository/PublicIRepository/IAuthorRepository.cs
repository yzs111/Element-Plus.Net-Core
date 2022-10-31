using CW_ToyShopping.Enity.PublicModels;
using CW_ToyShopping.Enity.UserModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CW_ToyShopping.IRepository.PublicIRepository
{
   public interface IAuthorRepository:IRepositoryBase<Author>, IRepositoryBase2<Author, Guid>
    {

    }
}
