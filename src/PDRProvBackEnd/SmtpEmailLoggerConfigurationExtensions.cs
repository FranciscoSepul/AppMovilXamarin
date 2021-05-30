using System.Net;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Email;

namespace Serilog
{
    public static class SmtpEmailLoggerConfigurationExtensions
    {
        public static LoggerConfiguration SmtpEmail(this LoggerSinkConfiguration loggerConfiguration, 
            string userName, 
            string password,
            string fromEmail, 
            string toEmail,
            string smtpServer,
            string mailSubject = "{Level}",
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Error,  
            string outputTemplate = "[{Level}] {Message}{NewLine}{Exception}{NewLine}Error occured at - {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}",
            int port = 25, 
            bool enableSsl = false)
        {
            return
                loggerConfiguration
                    .Email
                    (
                        new EmailConnectionInfo
                        {
                            MailServer = smtpServer,
                            NetworkCredentials = new NetworkCredential(userName: userName, password: password),
                            EmailSubject = mailSubject,
                            FromEmail = fromEmail,
                            ToEmail = toEmail,
                            EnableSsl = enableSsl,
                            Port = port
                        },
                        restrictedToMinimumLevel: restrictedToMinimumLevel,
                        outputTemplate: outputTemplate
                    );
        }
    }
}