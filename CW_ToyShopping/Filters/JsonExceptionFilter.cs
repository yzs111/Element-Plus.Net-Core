using CW_ToyShopping.Common.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW_ToyShopping.Filters
{
    public class JsonExceptionFilter:IExceptionFilter
    {
        private ILogger Logger { get; }
        private ILoggerFactory LoggerFactory { get; }
        private IHostEnvironment Environment { get; }
        public JsonExceptionFilter(IHostEnvironment env, ILogger logger, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger("AppLogger");
            Environment = env;
        }
        public void OnException(ExceptionContext context)
        {
            var error = new ApiError();

            if (Environment.IsDevelopment())
            {
                error.Message = context.Exception.Message;
                error.Detail = context.Exception.ToString();
            }
            else
            {
                error.Message = "服务器出错";
                error.Detail = error.Message = context.Exception.Message;
            }
            context.Result = new ObjectResult(error)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            StringBuilder sb = new StringBuilder();
            sb.Append($"服务器发生异常:{ context.Exception.Message}");
            sb.Append(context.Exception.ToString());
            //格式化和写入重要的日志消息。
            Logger.LogCritical(sb.ToString());
        }
    }
}
