
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace SpredMedia.CommonLibrary
{
    public static class SeriLogExtension
    {
        public static Logger SerilogRegister(IConfiguration config)
        {
            string Service = config.GetSection("MicroServices").GetValue<string>("Identifier");
            var blobconnectionstring = config.GetSection("ConnectionStrings").GetValue<string>("AzureConnection");

            //var log = new LoggerConfiguration()
            //    .WriteTo
            //    .AzureBlobStorage
            //            (blobconnectionstring,
            //              LogEventLevel.Information,
            //             "spredtestlogs",
            //             storageFileName: "/"+Service+"/log{yyyy}{MM}{dd}.txt",
            //             writeInBatches: true, 
            //             period: TimeSpan.FromSeconds(15), 
            //             batchPostingLimit: 100)
            //            .CreateLogger();

            var log = new LoggerConfiguration()
                       .MinimumLevel.Debug()
                       .MinimumLevel.Override("Microsoft", LogEventLevel.Information) // Adjust the log levels as needed
                       .Enrich.FromLogContext()
                       .WriteTo.Console() // Configure Serilog to log to the console
                       .CreateLogger();
            return log;
        }
    }
}
