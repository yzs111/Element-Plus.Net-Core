using CW_ToyShopping.Common.Helpers;
using CW_ToyShopping.Common.Helpers.Output;
using CW_ToyShopping.Common.Helpers.Page;
using CW_ToyShopping.Enity.AdminModels.UserModels;
using CW_ToyShopping.Enity.UserModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CW_ToyShopping.IService.UserIService
{
   public interface IUserService
    {
        IResponseOutput GetUserList(PageInput<User> input);

        Task<IResponseOutput> CreateUsers(UserDto registerUser);

        Task<IResponseOutput> EditUser(UserDto registerUser);

        Task<IResponseOutput> DeteleUser(string id);


        Task<IResponseOutput> GetUserDetial(string UserId);


        Task<IResponseOutput> Updateload(string id, string Url);


        Task<IResponseOutput> UpdatePassWord(ResetPasswordDto resetPasswordDto);

        Task<IResponseOutput> WXSentMessage(string context);
    }
}
