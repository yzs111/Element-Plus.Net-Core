using CW_ToyShopping.IService.PublicIService;
using CW_ToyShopping.IService.UserIService;
using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.IService
{
   public interface IServiceWrapper
    {
        IUserService IUserService { get; }

        IRoleService IRoleService { get; }

        IAuthorService IAuthorService { get; }

        IMenuService IMenuService { get; }
    }
}
