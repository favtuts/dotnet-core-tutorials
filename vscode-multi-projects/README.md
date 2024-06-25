# Debugging Multiple .NET Projects in VS Code
* https://tuts.heomi.net/debugging-multiple-net-projects-in-vs-code/

# First Steps

Create Frontend webapp project
```
dotnet new webapp --name "Frontend"
```

Create Backend webapi project
```
dotnet new webapi --name "Backend"
```

Create a solution and add two projects
```
dotnet new sln --name AwesomeSolution
dotnet sln add Frontend
dotnet sln add Backend
```

# Launch Configurations

Normally, VSCode will automatic understands your .NET projects and add files for debug and launch the application. If not we can create tasks manually: `CTRL+SHIP+P` -> `Tasks:Configure default build tasks`, then put the build task as follows:
```json
{
	"version": "2.0.0",
	"tasks": [		
		{
            "label": "buildbackend",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "${workspaceFolder}/Backend/Backend.csproj",
                "/property:GenerateFullPaths=true",
                "/consoleloggerparameters:NoSummary"
            ],
            "problemMatcher": "$msCompile"
        },
        {
            "label": "buildfrontend",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "${workspaceFolder}/Frontend/Frontend.csproj",
                "/property:GenerateFullPaths=true",
                "/consoleloggerparameters:NoSummary"
            ],
            "problemMatcher": "$msCompile"
        },
	]
}
```

Now we have 2 tasks related to the two individual projects, let's create the "launch.json" witht he following json
```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": ".NET Core Launch (Frontend)",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "buildfrontend",
            "program": "${workspaceFolder}/Frontend/bin/Debug/net8.0/Frontend.dll",
            "args": [],
            "cwd": "${workspaceFolder}/Frontend",
            "stopAtEntry": false,
            "serverReadyAction": {
                "action": "openExternally",
                "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
            },
            "env": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            },
            "sourceFileMap": {
                "/Views": "${workspaceFolder}/Views"
            }
        },
        {
            "name": ".NET Core Launch (Backend)",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "buildbackend",
            "program": "${workspaceFolder}/Backend/bin/Debug/net8.0/Backend.dll",
            "args": [],
            "cwd": "${workspaceFolder}/Backend",
            "stopAtEntry": false,
            "serverReadyAction": {
                "action": "openExternally",
                "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
            },
            "env": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            },
            "sourceFileMap": {
                "/Views": "${workspaceFolder}/Views"
            }
        }
    ]
}
```

# Compounds Configuration

After the configurations array, add a "compounds" section.
Specify a name that you want and in the configurations it's important to specify the same name as we used in the configurations section.
```json
  "compounds": [
    {
      "name": "Frontend & Backend",
      "configurations": [
        ".NET Core Launch (Frontend)",
        ".NET Core Launch (Backend)"
      ],
      "stopAll": true
    }
  ]
```

Press "F5" or on the Play button to launch the new launch configuration with multiple projects.

You can check Swagger Document
* https://localhost:7278/swagger/index.html

# Add Clean Tasks

A clean task is often useful if your project is not building for strange and/or unknown reasons - sometimes these issues can be caused by the intermediate build files that are not recreated from repeated builds. A clean task will remove these files, which will get recreated during the next build.

To add a clean task, add the following JSON snippet to the tasks array in your `tasks.json`:
```json
{
    "label": "clean",
    "command": "dotnet",
    "type": "process",
    "args": [
        "clean",
        "/consoleloggerparameters:NoSummary"
    ],
    "problemMatcher": "$msCompile"
},
```

You can then run this task in one of two ways:

1. From the Run menu select Run Task and select clean from the list.
2. Hit `Ctrl+Shift+p` to open the Command Palette and enter tasks. Then select `Tasks: Run Task` and select clean from the list.