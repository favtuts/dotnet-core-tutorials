# How to Send Emails in C# .NET via SMTP
* https://tuts.heomi.net/how-to-send-emails-in-c-net-via-smtp/

# Create New Project Console Application

Create new console app:
```bash
$ cd smtp-send-emails
$ dotnet new console -n SmtpTool
```

Run the console app:
```sh
$ dotnet run
```

Publish the console app:
```sh
$ dotnet publish
```


# Configure CSharp Debugging
* https://stackoverflow.com/questions/72601702/how-to-debug-dotnet-core-source-code-using-visual-studio-code
* https://code.visualstudio.com/docs/csharp/debugger-settings
* https://learn.microsoft.com/en-us/dotnet/core/tutorials/debugging-with-visual-studio-code?pivots=dotnet-8-0


First create the tasks in the file `tasks.json`, the `preLaunchTask` field will run the taskName before debugging the program.
```json
{
	"version": "2.0.0",
	"tasks": [
        {
            "label": "buildsmtpapp",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "${workspaceFolder}/SmtpTool.csproj",
                "/property:GenerateFullPaths=true",
                "/consoleloggerparameters:NoSummary"
            ],
            "problemMatcher": "$msCompile"
        },
        {
            "label": "cleansmtpapp",
            "command": "dotnet",
            "type": "process",
            "args": [
                "clean",
                "${workspaceFolder}/SmtpTool.csproj",
                "/consoleloggerparameters:NoSummary"
            ],
            "problemMatcher": "$msCompile"
        }       
    ]
}
```

Then create the `launch.json` configuration file:
```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": "Run Console App",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "buildsmtpapp",
            "program": "${workspaceFolder}/bin/Debug/net8.0/SmtpTool.dll",
            "args": [],
            "cwd": "${workspaceFolder}",
            "console": "internalConsole",
            "stopAtEntry": false,
            "justMyCode": false, // should be false, as we want to debug 3rd party source code
            "requireExactSource": false, // https://github.com/OmniSharp/omnisharp-vscode/blob/master/debugger-launchjson.md#require-exact-source
            "suppressJITOptimizations": true, // it's better to set true for local debugging
            "enableStepFiltering": false, // to step into properties
            "symbolOptions": {
                "searchMicrosoftSymbolServer": true, // get pdb files from ms symbol server
                "searchNuGetOrgSymbolServer": true,
                "moduleFilter": {
                    "mode": "loadAllButExcluded",
                    "excludedModules": []
                }
            },
            "logging": { // you can delete it if all is ok
                "moduleLoad": true,
                "engineLogging": true,
                "trace": true
            }
        }
    ]
}
```

Some configuration fields:
* PreLaunchTask: runs the associated taskName in tasks.json before debugging your program
* Program: The program field is set to the path of the application dll or .NET Core host executable to launch. Form: `"${workspaceFolder}/bin/Debug/<target-framework>/<project-name.dll>".`
* Cwd: The working directory of the target process.
* Args: These are the arguments that will be passed to your program.
* Stop at Entry: If you need to stop at the entry point of the target, you can optionally set stopAtEntry to be "true".


# Build AppSettings for Development

When developing C# .NET applications, it's common to store configuration settings in a JSON file, typically named `appsettings.json` . 

To read from `appsettings.json`, we need to add the `Microsoft.Extensions.Configuration` package.

Run the following command.
```bash
dotnet add package Microsoft.Extensions.Configuration 
dotnet add package Microsoft.Extensions.Configuration.Json 
dotnet add package Microsoft.Extensions.Configuration.Binder
```

In the root of your project, create a file named `appsettings.json` and add some configuration settings.
```json
{  
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": "587",
    "Username": "<YourGmailUserName>",
    "Password": "<YourGmailPassword>",
    "From": "Your Name"
  }
}
```

Add your appsettings files to your project for each environment with "Copy to Output Directory" so that it is included with the build. You should add your appSettings files in your `.csproj` file as bellow:
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Binder" Version="8.0.2" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="8.0.1" />
  </ItemGroup>

  <ItemGroup>
    <None Update="appsettings.DEV.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Update="appsettings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Update="appsettings.PROD.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Update="appsettings.QA.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>

</Project>
```

Then you can inject and reach IConfiguration that reads appsettings file according to the environment via below code.
```cs
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
    }
}
```

Now time to run. But first do dotnet restore and then dotnet run.
```sh
$ dotnet restore
$ ASPNETCORE_ENVIRONMENT=DEV dotnet run

Environment is DEV
Email Host is: smtp.gmail.com
```

The following approach will load the content from the JSON file, then bind the content to two strongly-typed options/settings classes, which can be a lot cleaner than going value-by-value:
```cs
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
```

And the updated appsettings.json content:
```json
{
    "Smtp": {
        "Host": "smtp.gmail.com",
        "Port": "587",
        "Username": "<YourGmailUserName>",
        "Password": "<YourGmailPassword>",
        "EnableSsl": true,
        "From": "Your Name"
    },
    "Email": {
        "Sender": "<SenderEmail>",
        "Recipient": "<RecipientEmail>"
    }
}
```

# Sending email using System.Net.Mail

Sending plain text email:
```cs
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
```

Sending an Email With Attachments:
```cs

```

# Sending email using MailKit

