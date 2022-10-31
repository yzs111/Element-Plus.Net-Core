using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace CW_ToyShopping.Enity.UserModels
{
    public static class ClaimsStore
    {
        public static List<Claim> RoleClaims = new List<Claim>()
        {
            new Claim("Create","Create Role"),
            new Claim("Edit","Edit Role"),
            new Claim("Delete","Delete Role"),
            new Claim("Get","Get Role"),
        };
    }
}
