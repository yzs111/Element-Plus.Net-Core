using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CW_ToyShopping.Enity.UserModels
{
   public class Role: IdentityRole
    {
        /// <summary>
        ///  角色简介
        /// </summary>
        [Column("ROLEDESC")]
        public string ROLEDESC { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Column("IsEnble")]
        public bool IsEnble { get; set; }

        /// <summary>
        ///  创建人
        /// </summary>
        [Column("CREATEPERSON")]
        [MaxLength(50)]
        public string CREATEPERSON { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        [Column("CREATEDATE")]
        public DateTimeOffset CREATEDATE { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [Column("UPDATEPERSON")]
        [MaxLength(50)]
        public string UPDATEPERSON { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("UPDATEDATE")]
        public DateTimeOffset UPDATEDATE { get; set; }
    }
}
