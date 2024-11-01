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

        // Test sending email via System.Net.Mail
        SendEmailViaNetMail(configuration);
    }

    public class SmtpSettings {
        public string Host { get; set; }   
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string From { get; set; }
    }

    public class EmailSettings {
        public string Sender {get; set;}
        public string Recipient {get; set;}        
    }

    public static void SendEmailViaNetMail(IConfiguration configuration) {
        var smtpSettings = configuration.GetSection("Smtp").Get<SmtpSettings>();
        var emailSettings = configuration.GetSection("Email").Get<EmailSettings>();
        var senderEmail = emailSettings.Sender;
        var recipientEmail = emailSettings.Recipient;
        
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(senderEmail);
        mailMessage.To.Add(recipientEmail);
        mailMessage.Subject = "Hello world";
        mailMessage.Body = "This is a test email sent using C#.NET";

        SmtpClient smtpClient= new SmtpClient();
        smtpClient.Host = smtpSettings.Host;
        smtpClient.Port = smtpSettings.Port;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password);
        smtpClient.EnableSsl = smtpSettings.EnableSsl;
        try
        {
            smtpClient.Send(mailMessage);
            Console.WriteLine("Email Sent Successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}