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
        
        var emailSettings = configuration.GetSection("Email").Get<EmailSettings>();
        Console.WriteLine($"Sending from Email: {emailSettings?.Sender}");

        // Test sending email via System.Net.Mail
        var plainTextEmailRequest = new EmailRequest() {
            FromEmail=emailSettings.Sender,
            ToEmail=emailSettings.Recipient,
            Subject="Hello world PlainText", 
            Body="This is a test email sent using C#.NET",
            IsHtmlBody = false             
        };
        //SendEmailViaNetMail(plainTextEmailRequest, smtpSettings);

        var htmlEmailRequest = new EmailRequest() {
            FromEmail=emailSettings.Sender,
            ToEmail=emailSettings.Recipient,
            Subject="Hello world HtmlFormat", 
            Body="<em>It's great to use HTML in mail!!</em>",
            IsHtmlBody = true             
        };

        SendEmailViaNetMail(htmlEmailRequest, smtpSettings);

        var attachmentEmailRequest = new EmailRequest() {
            FromEmail=emailSettings.Sender,
            ToEmail=emailSettings.Recipient,
            Subject="Hello world Attachment", 
            Body="This is a test email sent using C#.NET",
            IsHtmlBody=false,
            Attachments = new List<string> {"instruction.txt", "8-ball_rules_bca.pdf"}
        };

        //SendEmailViaNetMail(attachmentEmailRequest, smtpSettings);
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

    public class EmailRequest {
        public string Subject {get; set;}
        public string Body {get; set;}
        public bool IsHtmlBody {get; set;}
        public string FromEmail {get; set;}
        public string ToEmail {get; set;}
        public List<string> Attachments {get; set;}
    }

    public static void SendEmailViaNetMail(EmailRequest emailRequest, SmtpSettings smtpSettings) {        
        var senderEmail = emailRequest.FromEmail;
        var recipientEmail = emailRequest.ToEmail;
        
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(senderEmail);
        mailMessage.To.Add(recipientEmail);
        mailMessage.Subject = emailRequest.Subject;

        mailMessage.Body = emailRequest.Body;
        mailMessage.IsBodyHtml = emailRequest.IsHtmlBody;
        
        if (emailRequest.Attachments != null && emailRequest.Attachments.Count>0) {
            foreach (var filePath in emailRequest.Attachments) {
                // Create a new Attachment object
                Attachment attachment = new Attachment(filePath);

                // Add the attachment to the MailMessage object
                mailMessage.Attachments.Add(attachment);
            }           
        }

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