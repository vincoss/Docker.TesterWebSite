## Resources
https://hub.docker.com/_/microsoft-dotnet-core-aspnet/

NOTE:
The samples are written for cmd.exe.

## Build & tag
docker build -f Dockerfile.windows-x64 --no-cache -t vincoss/dockertesterweb:1.0.0-windows .
docker build -f Dockerfile.ubuntu-x64 --no-cache -t vincoss/dockertesterweb:1.0.0-bionic .

## Run
docker run -it --rm -p 8003:80 --name dockertesterweb -h dockertester --ip 10.1.2.3 -v dockertester:C:/Docker.TesterWebSite vincoss/dockertesterweb:1.0.0-windows

## Run Windows using Linux contaners
docker run -it --rm -p 8003:80 --name dockertesterweb -h dockertester --ip 10.1.2.3 -v C:/Docker.TesterWebSite/:/var/appdata vincoss/dockertesterweb:1.0.0-bionic

## Error logs
docker logs --tail 50 --follow --timestamps dockertesterweb

## Show running container IP
docker inspect -f "{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}" dockertesterweb

##------------------------------------------------ Test

## Browse
http://localhost/api/diagnostics
http://dockertester/api/diagnostics
http://localhost:8003/api/diagnostics
http://localhost:8003/api/diagnostics/getAppDataFileList
http://localhost:8003/api/diagnostics/getNetworkInfo
http://{ip-here}/api/diagnostics
