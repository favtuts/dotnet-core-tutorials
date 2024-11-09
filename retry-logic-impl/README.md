# How to implement effective retry logic in C#
* https://tuts.heomi.net/how-to-implement-effective-retry-logic-in-c/

# Create New Project Console Application

Create new console app:
```bash
$ cd retry-logic-impl
$ dotnet new console -n RetryLogicDemo
```

Run the console app:
```sh
$ cd RetryLogicDemo
$ dotnet run
```

Publish the console app:
```sh
$ dotnet publish
```

# Configure CSharp Debugging in VSCode
* https://stackoverflow.com/questions/72601702/how-to-debug-dotnet-core-source-code-using-visual-studio-code
* https://code.visualstudio.com/docs/csharp/debugger-settings
* https://learn.microsoft.com/en-us/dotnet/core/tutorials/debugging-with-visual-studio-code?pivots=dotnet-8-0


First create the tasks in the file `tasks.json`, the `preLaunchTask` field will run the taskName before debugging the program.
```json
{
	"version": "2.0.0",
	"tasks": [
        {
            "label": "buildretrylogicdemoapp",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "${workspaceFolder}/RetryLogicDemo.csproj",
                "/property:GenerateFullPaths=true",
                "/consoleloggerparameters:NoSummary"
            ],
            "problemMatcher": "$msCompile"
        },
        {
            "label": "cleanretrylogicdemoapp",
            "command": "dotnet",
            "type": "process",
            "args": [
                "clean",
                "${workspaceFolder}/RetryLogicDemo.csproj",
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
            "preLaunchTask": "buildretrylogicdemoapp",
            "program": "${workspaceFolder}/bin/Debug/net8.0/RetryLogicDemo.dll",
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

# Install Dependencies

```bash
dotnet add package Newtonsoft.Json
```

# REST API - ready to use
* https://restful-api.dev/

This provide the real REST API where your data is securely stored in a real database


# App-vNext/Polly
* https://github.com/App-vNext/Polly

Polly is a .NET resilience and transient-fault-handling library that allows developers to express resilience strategies such as Retry, Circuit Breaker, Hedging, Timeout, Rate Limiter and Fallback in a fluent and thread-safe manner.

```bash
dotnet add package Polly.Core
dotnet add package Polly.Extensions
dotnet add package Polly.RateLimiting
dotnet add package Polly.Testing
dotnet add package Polly
```
