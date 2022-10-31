using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CW_ToyShopping.Enity.AdminModels.MenuModels
{
    [Table("SYS_MENU")]
  public class Menu
    {
        [Key]
        [Required]
        [Column("MENUID")]
        public int MENUID { get; set; }

        [Column("AUTHNAME")]
        [Required]
        public string AUTHNAME { get; set; }

        [Column("PATH")]
        public string PATH { get; set; }

        [Column("PID")]
        public int PID { get; set; }

        [Column("RootIntroduction")]
        [MaxLength(50)]
        public string RootIntroduction { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Column("IsEnble")]
        public bool IsEnble { get; set; }

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
