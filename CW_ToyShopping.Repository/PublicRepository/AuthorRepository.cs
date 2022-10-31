
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CW_ToyShopping.IRepository.PublicIRepository;
using CW_ToyShopping.Common.Data;
using CW_ToyShopping.Enity.UserModels;
using CW_ToyShopping.Repository.BaseRepository;
using CW_ToyShopping.DB;
using CW_ToyShopping.Enity.PublicModels;
using System.Threading.Tasks;

namespace CW_ToyShopping.Repository.PublicRepository
{
    public class AuthorRepository:RepositoryBase<Author, Guid>, IAuthorRepository
    {
        public AuthorRepository(OracleDBContext _mysqlDBContext) :base(_mysqlDBContext)
        {

        }
     
    }
}
