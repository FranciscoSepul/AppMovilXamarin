using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using PDRProvBackEnd.Contexts;
using Microsoft.EntityFrameworkCore;
using Serilog.Configuration;
using Serilog.Settings.Configuration;
using Serilog.Sinks.Email;

namespace PDRProvBackEnd
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            try
            {
              
                var host = CreateHostBuilder(args).Build();

                Log.Information("Iniciando el host");

                var config = host.Services.GetRequiredService<IConfiguration>();

                var connStringPawa = config.GetConnectionString("PDRConnection");
                if (!string.IsNullOrWhiteSpace(connStringPawa) &&
                    connStringPawa.StartsWith("PAWA"))
                {
                    Log.Information("PDRConnection: Variable de entorno");
                }
                else if (!connStringPawa.StartsWith("PAWA"))
                    Log.Warning("PDRConnection: Directo en appsettings.<>.json");
                else
                    Log.Error("PDRConnection: No definida.");

                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "El host terminó de manera inesperada");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    //.WriteTo.SmtpEmail(
                    //    fromEmail: "qa-intercambio@amf.cl",
                    //    toEmail: "pawafapawa@gmail.com",
                    //    mailSubject: "{Level} en PawaApi - {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}",
                    //    smtpServer: "mail.amf.cl",
                    //    userName: "qa-intercambio@amf.cl",
                    //    password: "amf2019*$")
                    )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
