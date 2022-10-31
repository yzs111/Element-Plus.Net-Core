using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Common.Helpers.Page
{
   public class PageOutput<T>
    {
        /// <summary>
        /// 返回的条数
        /// </summary>
        public long Total { get; set; } = 0;

        // 返回的集合
        public IList<T> List { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 查询条数
        /// </summary>
        public int PageSize { get; set; }
    }
}
