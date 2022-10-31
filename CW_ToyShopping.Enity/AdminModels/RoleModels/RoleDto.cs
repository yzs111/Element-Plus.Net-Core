using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Enity.UserModels
{
   public class RoleDto
    {

        /// <summary>
        ///  角色Id
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  角色简介
        /// </summary>
        public string ROLEDESC { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnble { get; set; }

        /// <summary>
        ///  创建人
        /// </summary>
        public string CREATEPERSON { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTimeOffset CREATEDATE { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UPDATEPERSON { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTimeOffset UPDATEDATE { get; set; }
    }
}
