using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Enity.UserModels
{
   public class UserClaim
    {
        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public bool IsSelected { get; set; }
    }
}
