using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CW_ToyShopping.Enity.UserModels
{
   public class RegisterUser
    {

        [Required,MinLength(4)]
        public string UserName { get; set; }

        public string Email { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        public DateTimeOffset BirthDate { get; set; }

        public string PhoneNumber { get; set; }

    }
}
