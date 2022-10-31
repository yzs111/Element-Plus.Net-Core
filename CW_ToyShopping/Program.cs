using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CW_ToyShopping
{
    public class Program
    {
        public static void Main(string[] args)
        {
           CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) {

            var builder = Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 });

            // ʹ��NLog��Ϊ��־��¼
            builder.ConfigureLogging(logging =>
            {
                // ���ԭ�е���־��
                logging.ClearProviders();
            })
            .UseNLog();

            return builder;
        }
    }
}
