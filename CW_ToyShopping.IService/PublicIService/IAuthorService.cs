using CW_ToyShopping.Common.Helpers.Output;
using CW_ToyShopping.Enity.PublicModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CW_ToyShopping.IService.PublicIService
{
   public interface IAuthorService
    {
       Task<IResponseOutput> GetAuthirList();

        Task<IResponseOutput> CreateAuthor(AuthirDto authirDto);

        Task<IResponseOutput> UpdateAuthor(AuthirDto authirDto);

        Task<DataTable> PrintAuthir(List<DicModel> dic);
    }
}
