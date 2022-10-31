using CW_ToyShopping.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Enity.AdminModels.MenuModels
{
   public class MenuDto: TreeBase<MenuDto>
    { 
        public int MENUID { get; set; }

        public string AUTHNAME { get; set; }   

        public string PATH { get; set; } 

        public int PID { get; set; }

        public string RootIntroduction { get; set; }

        public bool IsEnble { get; set; }

        public string CREATEPERSON { get; set; }

        public DateTimeOffset CREATEDATE { get; set; }

        public string UPDATEPERSON { get; set; }

        public DateTimeOffset UPDATEDATE { get; set; }
    }
}
