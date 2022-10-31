using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Common.Helpers
{
    public class AjaxMsgModel
    {
        public string Msg { get; set; }
        public AjaxStatu Statu { get; set; }
        public string BackUrl { get; set; }
        public object Data { get; set; }
        public string biz { get; set; }
    }
    public enum AjaxStatu
    {
        ok = 1,
        err = 2,
        none = 3,
        nologin = 4,
        noperm = 5
    }
}
