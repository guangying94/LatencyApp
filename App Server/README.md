# Instruction
The app server is using flask to host simple API app that query database directly, or via integration tier.

## Setup
#### Step 1 - Install Docker
There are many ways to install Docker in Ubuntu. One of the option is to use scripts provided by Docker in [GitHub](https://github.com/docker/docker-install)

To simplify the deployment, store the command in install-docker.sh, and run the script.

```sh
wget https://raw.githubusercontent.com/guangying94/LatencyApp/master/App%20Server/install-docker.sh
sudo sh install-docker.sh
```

#### Step 2 - Dockerize Image & Push to Registry
Place the python script, Dockerfile, and requirements.txt in same folder. You can build the image within the enviornament, or create a continuous integration pipeline in DevOps.

This sample illustrate how to build the docker image locally, and store images locally. For production, it is recommended to push the container image in container registry, like [Azure Container Registry](https://docs.microsoft.com/en-us/azure/container-registry/).

```sh
#To login to container registry
sudo docker login <acr address>.azurecr.io -u <Username> -p <Password>

#To build container image
sudo docker build -t internet-app .

#To tag contaniner image
sudo docker tag internet-app <acr address>.azurecr.io/internet-app

#To push container image to ACR
sudo docker push <acr address>.azurecr.io/internet-app
```

#### Step 3 - Run Container Image
Once done, you can run the container image locally. To run the container, use following command. Note that this Flask python app is using port 5000.

```sh
#Run container image
sudo docker run -d -p 5000:5000 <acr address>.azurecr.io/internet-app
```

#### Step 4 - Setup for Intranet
The setup is similar for intranet zone. Note that the private IP and ddatabase instance is different and may need to modify in code.
