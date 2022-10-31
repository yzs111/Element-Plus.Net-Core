using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Common.Helpers.Output
{
    /// <summary>
    /// 响应数据输出接口
    /// </summary>
    public interface IResponseOutput
    {
        bool Success { get; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; }
    }
    /// <summary>
    /// 响应数据输出泛型接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResponseOutput<T> : IResponseOutput
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        T Data { get; }
    }
}
