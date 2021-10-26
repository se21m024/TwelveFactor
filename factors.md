# TwelveFactor
## A containerized MicroService following the 12 factor principles (https://12factor.net/)
## The Twelve Factors
### Author: se21m024

<br/>
10 out of 12 factors were achieved.
<br/>
<br/>

# I. Codebase
## Requirement
One codebase tracked in revision control, many deploys
## Solution (IMPLEMENTED #1)
A GitHub repository was created to track the code base. Various commits have been made.
<br/>
https://github.com/se21m024/TwelveFactor
<br/>
<br/>

# II. Dependencies
## Requirement
Explicitly declare and isolate dependencies
## Solution (IMPLEMENTED #2)
External dependencies (NuGet packages) are explicitly declared in the .csproj file.
The app was build self-contained so all dependencies (even System.* libraries) are published into the app directory.
<br/>
<br/>
Dockerfile build command:

RUN dotnet publish -c release -o /DockerOutput/Website --self-contained -r ubuntu.20.04-x64
<br/>
<br/>

# III. Config
## Requirement
Store config in the environment
## Solution (IMPLEMENTED #3)
The GetNewGuid method returns GUIDs with uppercase or lowercase letters, depending on the environment variable UPPERCASEGUIDS (true -> upper case, false -> lower case).
The delay for the GetNewGuid method is also configurable in milliseconds with the  environment variable GETNEWGUIDDELAYMS.

GuidController.cs
<br/>
Convert.ToBoolean(Environment.GetEnvironmentVariable("UPPERCASEGUIDS"))
<br/>
Convert.ToInt32(Environment.GetEnvironmentVariable("GETNEWGUIDDELAYMS"))
<br/>
<br/>
The environment variables can be set by passing the docker build argument upper_case_guids and get_new_guid_delay_ms when building the image (e.g. for staging or production):
In the solution folder execute the following command:

docker build -f TwelveFactor\Dockerfile --force-rm –-build-arg upper_case_guids=false --build-arg get_new_guid_delay_ms=5000 -t twelvefactor-image:1.0 . 
<br/>
<br/>
To do so, the environment variables must be linked to the docker build argument in the Dockerfile:
<br/>
ARG upper_case_guids=true
<br/>
ENV UPPERCASEGUIDS=$upper_case_guids

ARG get_new_guid_delay_ms=2000
<br/>
ENV GETNEWGUIDDELAYMS=$get_new_guid_delay_ms
<br/>
<br/>
The environment variables can also be (optionally) set by passing them directly to docker run when starting the app:

docker run --rm -it -p 6000:80 --name twelvefactor-app -e UPPERCASEGUIDS=false -e GETNEWGUIDDELAYMS=10000 twelvefactor-image:1.0
<br/>
<br/>

# IV. Backing services
## Requirement
Treat backing services as attached resources
## Solution (NOT IMPLEMENTED #1)
This app does not depend on any backing services, e.g. databases or external APIs. If the app would depend on a database, the database would not be part of this solution and the connection string to the database would be configurable as environment variable. Therefore the app could be easily configured to use different database endpoints. The URLs of external APIs would also be configurable as environment variables.
<br/>
<br/>

# V. Build, release, run
## Requirement
Strictly separate build and run stages
## Solution (IMPLEMENTED #4)
The app configuration (in this case the environment variable UPPERCASEGUIDS) can be set during the image build process (build stage) as well when starting the app (run stage).
<br/>
* Build stage: The image is built (docker build).
* Release stage: The tagged image is distributed (provide the image e.g. as file: TODO: insert dropbox link!).
* Run stage: The tagged image is started within a container (docker run).
<br/>
<br/>

# VI. Processes
## Requirement
Execute the app as one or more stateless processes
## Solution (IMPLEMENTED #5)
The app is a self-hosted, self-contained, stateless ASP.NET 5 REST API.
<br/>
<br/>

# VII. Port binding
## Requirement
Export services via port binding
## Solution (IMPLEMENTED #6)
The container port 80 was bound to the host port 6000.
During development, this was achieved by configuring the port binding in the JetBrains Rider debug settings.
In staging / production this is achieved by explicitly setting the docker run parameter '-p 6000:80'.
<br/>
<br/>

# VIII. Concurrency
## Requirement
Scale out via the process model
## Solution (IMPLEMENTED #7)
It would be possible to run multiple instances of the app in parallel because they would not interfere with one another. This is achieved by implementing the app as stateless REST API. To run multiple instances simultaneously, Kubernetes could be used.
<br/>
<br/>

# IX. Disposability
## Requirement
Maximize robustness with fast startup and graceful shutdown
## Solution (IMPLEMENTED #8)
Because the app is implemented as ASP.NET 5 web API, the SIGTERM signal emitted by the docker stop command can be caught with the IHostApplicationLifetime interface, injected into the controller. 
The GetNewGuid method uses a linked cancellation token so the Task.Delay method is cancelled when the client closed the connection or the app receives the SIGTERM signal. When this linked token is cancelled, the app terminates the current activity and returns the HTTS status 503 - Service Unavailable to the client.
If the app would depend on long running tasks (except of the demo Task.Delay) or IO bound tasks, this linked cancellation tokens would be passed to these calls to cancel the corresponding tasks.
<br/>
<br/>

# X. Dev/prod parity
## Requirement
Keep development, staging, and production as similar as possible
## Solution (IMPLEMENTED #9)
While developing the project in JetBrains Rider, the app was deployed to a local Docker container for debugging (see https://blog.jetbrains.com/dotnet/2018/07/18/debugging-asp-net-core-apps-local-docker-container/). Because the app is deployed as a Docker container, the staging, production and development environment are as similar as possible.
<br/>
<br/>

# XI. Logs
## Requirement
Treat logs as event streams
## Solution (IMPLEMENTED #10)
The app is configured to log to standard output only. Therefore the logs can be treated as an event stream for further processing.

Program.cs
<br/>
Host.CreateDefaultBuilder(args)<br/>
&nbsp;&nbsp;&nbsp;&nbsp;.ConfigureLogging(logging =><br/>
&nbsp;&nbsp;&nbsp;&nbsp;{<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;logging.ClearProviders();<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;logging.AddConsole();<br/>
&nbsp;&nbsp;&nbsp;&nbsp;})
<br/>
<br/>

# XII. Admin processes
## Requirement
Run admin/management tasks as one-off 
## Solution (NOT IMPLEMENTED #2)
If the app would e.g. connect to a database, a migration script for that database would be added to the repository. The script could then be executed from within the app container by the app administrator.
