# Web Server
Web server uses ASP.NET Core 3 Preview.

### Setup (Development machine)
The solution is compiled and dockerized. Setup detail is similar to App Server. The development is in Windows environment, and uses Linux container in Docker.

The following docker command in Command Prompt is to compile and push container images to [Azure Container Registry](https://docs.microsoft.com/en-us/azure/container-registry/).

```cmd
> cd <folder location>

> docker login <acr address>.azurecr.io -u <Username> -p <Password>

> docker build -t web-latency .

> docker tag web-latency <acr address>.azurecr.io/web-latency

> docker push <acr address>.azurecr.io/web-latency
```

### Setup (Server)
Similarly, install Docker in web server, pull the images down and run the container.

```sh
#Install Docker
$ wget https://raw.githubusercontent.com/guangying94/LatencyApp/master/App%20Server/install-docker.sh
$ sudo sh install-docker.sh

#Login to azure container registry
$ sudo docker login <acr address>.azurecr.io -u <Username> -p <Password>

#Pull Docker Image down
$ sudo docker pull <acr address>.azurecr.io/web-latency

#Run container
$ sudo docker run -d -p 80:80 <acr address>.azurecr.io/web-latency
```