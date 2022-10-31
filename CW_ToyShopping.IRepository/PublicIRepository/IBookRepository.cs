using CW_ToyShopping.Enity.PublicModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.IRepository.PublicIRepository
{
   public interface IBookRepository:IRepositoryBase<Book>, IRepositoryBase2<Book,int>
    {
       
    }
}
