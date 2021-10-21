# TwelveFactor
## A containerized MicroService following the 12 factor principles (https://12factor.net/)
## The Twelve Factors
### Author: se21m024
<br/><br/>
# I. Codebase
## Requirement
One codebase tracked in revision control, many deploys
## Solution (DONE)
A GitHub repository was created to track the code base. Various commits have been made.<br/>
https://github.com/se21m024/TwelveFactor
<br/><br/>

# II. Dependencies
## Requriement
Explicitly declare and isolate dependencies
## Solution
.csproj mit den verlinkten Abhängigkeiten NuGet
Wie schaut es mit System Libs aus?
todo
<br/><br/>

# III. Config
## Requriement
Store config in the environment
## Solution
todo
<br/><br/>

# IV. Backing services
## Requriement
Treat backing services as attached resources
## Solution
todo
<br/><br/>

# V. Build, release, run
## Requriement
Strictly separate build and run stages
## Solution
lt. Video: Release = Build nehmen und mit konfigurierten Env. Variablen kombinieren, damit die fertige Anwednung in weiterer Folge fix fertig gestartet werden kann.
todo
<br/><br/>

# VI. Processes
## Requriement
Execute the app as one or more stateless processes
## Solution
Rest Server.
lt. video muss Jeder Prozess self contained sein
todo
<br/><br/>

# VII. Port binding
## Requriement
Export services via port binding
## Solution
The container port 80 was bound to the host port 6000 (in the Rider debug settings).
ws wird als api nach außen angeboten.
todo
<br/><br/>

# VIII. Concurrency
## Requriement
Scale out via the process model
## Solution
Es muss möglich sein, mehere Instanzen der App-Prozesse gleichzeitig zu starten, ohne dass sie sich in die Quere kommen. -> wird erreicht, da der Service statless ist und alle Prozesse auf dieselbe DB zugreifen, die gleichzeitigen (fake) Zugriff unterstützt.
theoretisch könnte man die Hochskalierung mit Kubernetes umsetzen.
todo
<br/><br/>

# IX. Disposability
## Requriement
Maximize robustness with fast startup and graceful shutdown
## Solution
ASP.NET already. Using canellation tokens for all long running tasks or IO bound tasks.
todo
<br/><br/>

# X. Dev/prod parity
## Requriement
Keep development, staging, and production as similar as possible
## Solution
todo
While developing the project in JetBrains Rider, the app was deployed to a local Docker container for debugging (https://blog.jetbrains.com/dotnet/2018/07/18/debugging-asp-net-core-apps-local-docker-container/). As the app is deployed as a Docker container, the production environment and the development (and staging) environemtn ar as similar as possible.
<br/><br/>

# XI. Logs
## Requriement
Treat logs as event streams
## Solution
ConfigureLogging -> AddConsole
todo
<br/><br/>

# XII. Admin processes
## Requriement
Run admin/management tasks as one-off 
## Solution
todo
