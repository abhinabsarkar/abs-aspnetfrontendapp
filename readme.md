# Docker hub image - abhinabsarkar/abs-aspnetfrontendapp
A sample docker app built on Alpine linux 3.1 version using C# dotnet core version 3.1. This demo front end application displays on its home page details of the server on which it runs, including the host name & ip address. The front end app also has a "Backend Service" page which invokes a demo API and displays the result.

The docker image can be downloaded from docker hub by running the below command 
```bash
docker pull abhinabsarkar/abs-aspnetfrontendapp:<version>
```
Size of the docker image is 114 MB.

The application can be run on any kubernetes cluster. A sample deployment.yaml with config map to invoke any "Backend Service" (GET API which doesn't require any parameters) can be found under the kubernetes folder. 

### To build the image, run docker build from the root directory of the application
```bash
# Build the abs image
docker build -t abs-aspnetfrontendapp:v1.0.0 .
# Run the docker container locally
# The release version runs the container at port 80 although the app is running at port 5000
docker run --name abs-aspnetfrontendapp-container -d -p 8002:80 abs-aspnetfrontendapp:v1.0.0
# Check the status of the container
docker ps -a | findstr abs-aspnetfrontendapp-container
# Test the app
curl http://localhost:8002
# log into the running container 
docker exec -it abs-aspnetfrontendapp-container /bin/bash
docker exec -it abs-aspnetfrontendapp-container <command>
# Remove the container
docker rm abs-aspnetfrontendapp-container -f
# Remove the image
docker rmi abs-aspnetfrontendapp:v1.0.0
# Push the image to docker hub
docker login
# Tag the local image & map it to the docker repo
docker tag local-image:tagname new-repo:tagname
# eg: docker tag abs-aspnetfrontendapp:v1.0.0 abhinabsarkar/abs-aspnetfrontendapp:v1.0.0
# push the tagged image to the docker hub
docker push new-repo:tagname
# eg: docker push abhinabsarkar/abs-aspnetfrontendapp:v1.0.0
```

### To run the application on any Kubernetes platform (including Docker-Desktop) with ConfigMap
```bash
# Create namespace
kubectl create namespace abs
# Run the deployment config. A sample deployment config is placed under the kubernetes folder 
kubectl apply -f abs-aspnetfrontendapp.yaml -n abs
# Check the config map
kubectl describe configmap/aspnetfrontendapp -n abs
# Check if the application pod is running 
kubectl get all -n abs -o wide
# Peek into the running pod in k8s for debugging 
kubectl  -n <namespace> exec -it <pod-name> -- /bin/bash
kubectl  -n abs exec -it aspnetfrontendapp-548d449d5-h9zz7 -- /bin/bash
kubectl -n <namespace> exec --stdin --tty <pod-name> -- /bin/bash
# Delete the namespace & all the resources
kubectl delete namespace abs
```