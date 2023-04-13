using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace ProjectManager.Teacher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logConfig = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("env"))
                       ? "nlog.config"
                       : "nlog.Development.config";
            var logger = NLogBuilder.ConfigureNLog(logConfig).GetCurrentClassLogger();
            logger.Debug("init");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })
                .UseNLog();
    }
}
