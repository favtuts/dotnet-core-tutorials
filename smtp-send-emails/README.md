# How to Send Emails in C# .NET via SMTP
* https://tuts.heomi.net/how-to-send-emails-in-c-net-via-smtp/

# Create New Project Console Application

```bash
$ cd smtp-send-emails
$ dotnet new console -n SmtpTool
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

