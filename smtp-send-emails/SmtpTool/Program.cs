using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {        
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        Console.WriteLine($"Environment is {environment}");

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            ;

        IConfiguration configuration = builder.Build();

        var emailHost = configuration["Smtp:Host"];
        Console.WriteLine($"Email Host is: {emailHost}");

        var smtpSettings = configuration.GetSection("Smtp").Get<SmtpSettings>();
        Console.WriteLine($"Smtp Username: {smtpSettings?.Username}");
    }

    public class SmtpSettings {
        public string Host { get; set; }   
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
    }
}