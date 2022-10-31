using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Enity.UserModels
{
   public class UserDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// // 身份证
        /// </summary>
        public string NumberID { get; set; }

        // 密码
        public string Password { get; set; }

        /// <summary>
        ///  电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string PRACTIICALNAME { get; set; }
        /// <summary>
        /// 名族
        /// </summary>
        public string NATION { get; set; }

        // 职业
        public string OCCUPATION { get; set; }

        //年龄
        public int AGE { get; set; }

        // 性别
        public string SEX { get; set; }

        // 简介
        public string BRIEF { get; set; }

        // 头像
        public string IMAGEURL { get; set; }
        // 国籍
        public string COUNTRY { get; set; }

        // 省份
        public string PROVINCE { get; set; }

        //城市
        public string CITY { get; set; }

        // 小区
        public string AREA { get; set; }

        // 城镇
        public string TOWN { get; set; }

        // 出生日期
        public DateTimeOffset BIRTHDATE { get; set; }

        // 创建人
        public string CREATEPERSON { get; set; }

        // 创建时间
        public DateTimeOffset CREATEDATE { get; set; }

        // 修改人
        public string UPDATEPERSON { get; set; }

        // 修改时间
        public DateTimeOffset UPDATEDATE { get; set; }
    }
}
