using CW_ToyShopping.DB;
using CW_ToyShopping.Enity.AdminModels.MenuModels;
using CW_ToyShopping.IRepository.UserlRepository;
using CW_ToyShopping.Repository.BaseRepository;

namespace CW_ToyShopping.Repository.UserRepository
{
   public class Menuepository: RepositoryBase<Menu, string>, IMenuepository
    {
        public Menuepository(OracleDBContext _mysqlDBContext) : base(_mysqlDBContext)
        {
           
        }
    }
}
