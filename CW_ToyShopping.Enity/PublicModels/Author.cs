using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CW_ToyShopping.Enity.PublicModels
{
  [Table("SYS_AUTHOR")]
  public class Author
    {
        [Key]
        [Column("AuthorID")]
        [Required]
        public Guid AuthorID { get; set; }

        [MaxLength(20)]
        [Column("NAME")]
        public string NAME { get; set; }

        [MaxLength(40)]
        [Column("BIRTHDATA")]
        public DateTimeOffset BIRTHDATA { get; set; }

        [Column("EMAIL")]
        public string EMAIL { get; set; }
    }
}
