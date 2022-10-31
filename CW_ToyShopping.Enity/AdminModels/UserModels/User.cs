using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CW_ToyShopping.Enity.UserModels
{
    [Table("SYS_USERINFO")]
    public class User:IdentityUser
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Column("PRACTIICALNAME")]
        [MaxLength(20)]
        public string PRACTIICALNAME { get; set; }

        /// <summary>
        /// 名族
        /// </summary>
        [MaxLength(20)]
        [Column("NATION")]
        public string NATION { get; set; }

        // 职业
        [Column("OCCUPATION")]
        [MaxLength(50)]
        public string OCCUPATION { get; set; }

        //年龄
        [Column("AGE")]
        public int AGE { get; set; }

        [MinLength(18)]
        [Column("PASSWORD")]
        public string PASSWORD { get; set; }

        // 性别
        [Column("SEX")]
        [MaxLength(1)]
        public string SEX { get; set; }
        // 身份证
        [Column("NumberID")]
        [MaxLength(50)]
        public string NumberID { get; set; }
        // 简介
        [MaxLength(1000)]
        [Column("BRIEF")]
        public string BRIEF { get; set; }

        // 头像
        [Column("IMAGEURL")]
        [MaxLength(100)]
        public string IMAGEURL { get; set; }
        // 国籍
        [MaxLength(20)]
        [Column("COUNTRY")]
        public string COUNTRY { get; set; }

        // 省份
        [MaxLength(50)]
        [Column("PROVINCE")]
        public string PROVINCE { get; set; }

        //城市
        [MaxLength(50)]
        [Column("CITY")]
        public string CITY { get; set; }

        /// <summary>
        /// 小区
        /// </summary>
        [MaxLength(50)]
        [Column("AREA")]
        public string AREA { get; set; }

        // 城镇
        [MaxLength(50)]
        [Column("TOWN")]
        public string TOWN { get; set; }

        // 出生日期
        [Column("BIRTHDATE")]
        public DateTimeOffset BIRTHDATE { get; set; }

        // 创建人
        [Column("CREATEPERSON")]
        [MaxLength(50)]
        public string CREATEPERSON { get; set; }

        // 创建时间
        [Column("CREATEDATE")]
        public DateTimeOffset CREATEDATE { get; set; }

        // 修改人
        [Column("UPDATEPERSON")]
        [MaxLength(50)]
        public string UPDATEPERSON { get; set; }

        // 修改时间
        [Column("UPDATEDATE")]
        public DateTimeOffset UPDATEDATE { get; set; }
    }
}
