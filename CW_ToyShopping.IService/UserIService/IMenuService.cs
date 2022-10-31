using CW_ToyShopping.Common.Helpers.Output;
using CW_ToyShopping.Enity.AdminModels.MenuModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CW_ToyShopping.IService.UserIService
{
   public interface IMenuService
    {
        Task<IResponseOutput> MenuDtos();

        Task<IResponseOutput> CreateMenu(MenuDto menuDto);

        Task<IResponseOutput> UpdateMenu(MenuDto menuDto);

        Task<IResponseOutput> DeteleMenu(int MenuId);
    }
}
