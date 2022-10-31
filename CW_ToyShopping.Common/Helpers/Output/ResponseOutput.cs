using CW_ToyShopping.Common.Helpers.Output;
using System;
using System.Collections.Generic;
using System.Text;


namespace CW_ToyShopping.Common.Helpers.Output
{
   public class ResponseOutput<T>: IResponseOutput
    {
        //指示Newtonsoft。Json。JsonSerializer不序列化公共字段或公共读/写属性值。
        public bool Success { get; private set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code => Success ? 1 : 0;

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; private set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; private set; }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="msg">消息</param>
        public ResponseOutput<T> Ok(T data, string msg = null)
        {
            Success = true;
            Data = data;
            Msg = msg;

            return this;
        }
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public ResponseOutput<T> NotOk(string msg = null, T data = default(T))
        {
            Success = false;
            Msg = msg;
            Data = data;

            return this;
        }
    }

    /// <summary>
    /// 响应数据静态输出
    /// </summary>
    public static partial class ResponseOutput
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static IResponseOutput Ok<T>(T data = default(T), string msg = null)
        {
            return new ResponseOutput<T>().Ok(data, msg);
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static IResponseOutput Ok()
        {
            return Ok<string>();
        }
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static IResponseOutput NotOk<T>(string msg = null, T data = default(T))
        {
            return new ResponseOutput<T>().NotOk(msg, data);
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static IResponseOutput NotOk(string msg = null)
        {
            return new ResponseOutput<string>().NotOk(msg);
        }

    }
}
