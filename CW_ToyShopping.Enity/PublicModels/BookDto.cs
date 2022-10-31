using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Enity.PublicModels
{
   public class BookDto
    {
        public Guid ID { get; set; }

        public string Title { get; set; }

        public string Descriptioin { get; set; }

        public int Pages { get; set; }

        public int AuthorId { get; set; }
    }
}
