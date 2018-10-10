using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace NewsTrack.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            logger.Info("Initializing application...");

            try
            {
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .UseApplicationInsights()
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Trace);
                    })
                    .UseNLog()
                    .Build();

                logger.Info("Running application...");

                host.Run();
            }
            catch (Exception e)
            {
                logger.Error(e, "The application has crashed. Check this out.");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}
