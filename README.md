# TwelveFactor
## A containerized MicroService following the 12 factor principles (https://12factor.net/)
## Readme
### Author: se21m024
<br/>
<br/>

## 1. Overview
A self-hosted, self-contained, stateless ASP.NET 5 REST API was implemented to demonstrate 10 of the 12 factors. A Docker image is available for this project.
<br/>
<br/>

## 2. Sources
The sources for this project are checked in at this GitHub repository:
https://github.com/se21m024/TwelveFactor
<br/>
<br/>

## 3. Docker Image
The Docker image containing the app can be received from this link:
<br/>
https://drive.google.com/uc?id=1O7qPQ-fbq7t3bYQJ389WAPuJmeECIrxi&export=download
<br/>
<br/>

## 4. Starting the app
### 4.1. From sources
#### 4.1.1. Building a Docker image
To build a Docker image from the project sources move to the solution directory that contains the TwelveFactor.sln file and execute the following command line command:

docker build -f TwelveFactor\Dockerfile --force-rm -t twelvefactor-image:1.0 .
<br/>
<br/>
The environment variables UPPERCASEGUIDS and GETNEWGUIDDELAYMS can be set by providing their corresponding build arguments upper_case_guids and get_new_guid_delay_ms:

docker build -f TwelveFactor\Dockerfile --force-rm --build-arg upper_case_guids=**'insert-value'** --build-arg get_new_guid_delay_ms=**'insert-value'** -t twelvefactor-image:1.0 . 
<br/>
<br/>
e.g.:

docker build -f TwelveFactor\Dockerfile --force-rm --build-arg upper_case_guids=false --build-arg get_new_guid_delay_ms=5000 -t twelvefactor-image:1.0 . 
<br/>
<br/>

#### 4.1.2. Starting the app
To start the app, make sure the image has been built successfully. To access the API via the local port 6100 provide the parameter '-p 6100:80' and execute the following command:

docker run --rm -it -p 6100:80 --name twelvefactor-app twelvefactor-image:1.0

The environment variables UPPERCASEGUIDS and GETNEWGUIDDELAYMS can be overwritten by passing them with the '-e' parameter:

docker run --rm -it -p 6100:80 --name twelvefactor-app -e UPPERCASEGUIDS=false -e GETNEWGUIDDELAYMS=10000 twelvefactor-image:1.0
<br/>
<br/>

### 4.2. From image
#### 4.2.1 Import the image
Download the image from: https://drive.google.com/uc?id=1O7qPQ-fbq7t3bYQJ389WAPuJmeECIrxi&export=download
<br/>
Then execute the following command:
<br/>
docker load -i /PATH-TO/twelvefactor-image.tgz
<br/>
e.g.:
<br/>
docker load -i C:\_FH\_DEL\DockerImages\twelvefactor-image.tgz
<br/>
<br/>

#### 4.2.2 Start the app
See '4.1.2. Starting the app'
<br/>
<br/>

## 5. Features
The easiest way to interact with the API is to call its Swagger page:
<br/>
http://localhost:6100/swagger/index.html
<br/>
<br/>

### 5.1. Create new GUID
The GET method /Guid/New returns a random GUID.
<br/>
By editing the environment variables UPPERCASEGUIDS and GETNEWGUIDDELAYMS the behaviour of the method can be modified:
* UPPERCASEGUIDS
<br/>
&nbsp;&nbsp;&nbsp;&nbsp;- true -> The GUID returned contains uppercase letters.
<br/>
&nbsp;&nbsp;&nbsp;&nbsp;- false -> The GUID returned contains lowercase letters.
* GETNEWGUIDDELAYMS
&nbsp;&nbsp;&nbsp;&nbsp;- Value in milliseconds to delay the response of the method.
<br/>
<br/>

### 5.2. Compare two GUIDs
The GET method /Guid/Compare/{firstGuid}/{secondsGuid} returns true if the two provided GUIDs are equal and false if they are not equal.
<br/>
If at least one of the provided strings is not a valid GUID, HTTP Status 400 Bad Request is returned.
<br/>
<br/>

### 5.3. Validate GUID
The GET method /GUID/Validate/{guid} returns true if the provided string is a valid GUID and false if it is not a valid GUID.
<br/>
<br/>
