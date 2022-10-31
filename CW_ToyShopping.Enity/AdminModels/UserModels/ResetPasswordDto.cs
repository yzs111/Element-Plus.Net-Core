using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Enity.AdminModels.UserModels
{
   public class ResetPasswordDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }

        //
        public string Code { get; set; }

    }
}
