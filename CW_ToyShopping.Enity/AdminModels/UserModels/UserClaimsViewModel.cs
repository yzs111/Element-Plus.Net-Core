using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Enity.UserModels
{
   public class UserClaimsViewModel
    {
       public string RoleId { get; set; }
       public List<UserClaim> Cliams { get; set; }
       public UserClaimsViewModel()
        {
            Cliams = new List<UserClaim>();
        }
    }
}
