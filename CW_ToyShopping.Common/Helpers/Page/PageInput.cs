using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Common.Helpers.Page
{
   public class PageInput<T>
    {
        /// <summary>
        ///  当前页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 当前总条数
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 查询条件
        /// </summary>
        public T Filter { get; set; }
    }
}
