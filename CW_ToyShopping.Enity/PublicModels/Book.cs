using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CW_ToyShopping.Enity.PublicModels
{
    [Table("SYS_BOOK")]
    public class Book
    {
        [Key]
        [Required]
        [Column("BOOKID")]
        public Guid BOOKID { get; set; }

        [MaxLength(100)]
        [Column("Title")]
        public string Title { get; set; }

        [MaxLength(500)]
        [Column("Description")]
        public string Description { get; set; }

        [Column("pages")]
        public int pages { get; set; }

        [Column("AuthorId")]
        public int AuthorId { get; set; }
    }
}
