using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Enity.PublicModels
{
   public class AuthirDto
    {        
        public Guid AuthorID { get; set; }

        public string NAME { get; set; }

        public DateTimeOffset BIRTHDATA { get; set; }

        public string EMAIL { get; set; }
    }
}
