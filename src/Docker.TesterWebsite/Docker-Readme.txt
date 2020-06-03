## Resources
https://hub.docker.com/_/microsoft-dotnet-core-aspnet/

NOTE:
The samples are written for cmd.exe.

## Build & tag
docker build --no-cache -t vincoss/dockertesterweb:1.0.0-windows .
docker build -f Dockerfile.ubuntu-x64 --no-cache -t vincoss/dockertesterweb:1.0.0-bionic .

## Run
docker run -it --rm -p 8001:80 --name dockertesterweb -h dockertester --ip 10.1.2.3 -v dockertester:c:/temp/Docker.TesterWebSite vincoss/dockertesterweb:1.0.0-windows

## Run Windows using Linux contaners
docker run -it --rm -p 8003:80 --name dockertesterweb -h dockertester --ip 10.1.2.3 -v c:/temp/Docker.TesterWebSite:/var/appdata vincoss/dockertesterweb:1.0.0-bionic

## Error logs
docker logs --tail 50 --follow --timestamps dockertesterweb

## Show running container IP
docker inspect -f "{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}" dockertesterweb

##------------------------------------------------ Test

## Browse
https://localhost/api/diagnostics
https://dockertester/api/diagnostics
https://localhost:8003/api/diagnostics
https://localhost:8003/api/diagnostics/getDataFileList
https://{ip-here}/api/diagnostics
