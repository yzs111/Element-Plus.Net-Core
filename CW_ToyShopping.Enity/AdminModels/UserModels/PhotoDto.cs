using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Enity.AdminModels.UserModels
{
   public class PhotoDto
    {
        public string EntId { get; set; }
        public int CrtUser { get; set; }
        public IFormCollection Files { get; set; }
    }
}
