using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Common.Helpers
{
    /// <summary>
    ///  数据转换类型
    /// </summary>
   public static class UtilConvert
    {
        public static long ToLong(this object s)
        {
            if (s == null || s == DBNull.Value)
                return 0L;

            long.TryParse(s.ToString(), out long result);
            return result;
        }
    }
}
