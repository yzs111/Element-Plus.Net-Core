using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Common.Helpers
{
   public class SessionHelper
    {
        public void WriteSession<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
          
        }
    }
}
