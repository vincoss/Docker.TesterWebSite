
## Install Certificate into linux container

1. Have .pfx file ready
2. Run 
	openssl pkcs12 -in X509Sample.pfx -nocerts -out Root.pem -nodes
	openssl pkcs12 -in X509Sample.pfx -nokeys -out Root.crt -nodes
3. Copy the files into mapped volume folder
4. Run
	docker exec -it dockertesterweb bash
	cd /var/appdata/
	cp Root.crt /usr/local/share/ca-certificates/Root.crt
	update-ca-certificates --fresh
5. Check
	if not found then import again