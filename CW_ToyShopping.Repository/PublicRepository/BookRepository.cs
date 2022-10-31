using CW_ToyShopping.Common.Data;
using CW_ToyShopping.IRepository.PublicIRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CW_ToyShopping.Repository.BaseRepository;
using CW_ToyShopping.Enity.PublicModels;
using CW_ToyShopping.DB;

namespace CW_ToyShopping.Repository.PublicRepository
{
   public class BookRepository: RepositoryBase<Book, int>, IBookRepository
    {
       public BookRepository(OracleDBContext _mysqlDBContext) :base(_mysqlDBContext)
        {

        }
    }
}
