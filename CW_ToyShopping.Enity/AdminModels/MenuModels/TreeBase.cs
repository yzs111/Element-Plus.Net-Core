using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CW_ToyShopping.Common.Helpers
{
   public class TreeBase<T>
    {
        private bool isLinked = false;

        /// <summary>
        /// 是否已创建连接
        /// </summary>
        public bool IsLinked 
        {
            get { return isLinked; }
            set { isLinked = value; }
        }
        /// <summary>
        /// 父节点
        /// </summary>
        public T Parent { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<T> Children { get; set; }
    }
}
