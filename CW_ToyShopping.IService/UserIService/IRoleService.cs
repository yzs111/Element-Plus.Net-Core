using CW_ToyShopping.Common.Helpers.Output;
using CW_ToyShopping.Common.Helpers.Page;
using CW_ToyShopping.Enity.UserModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CW_ToyShopping.IService.UserIService
{
   public interface IRoleService
    {
        IResponseOutput GetRoleList(PageInput<Role> input);

        Task<IResponseOutput> CreateRole(RoleDto roleDto);

        Task<IResponseOutput> EditRole(RoleDto roleDto);

        Task<IResponseOutput> DeleteRole(string id);

        Task<IResponseOutput> GetDetial(string id);
    }
}
