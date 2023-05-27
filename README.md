# Domivice.Users.Web - ASP.NET Core 6.0 Server

The users API

## Upgrade NuGet Packages

NuGet packages get frequently updated.

To upgrade this solution to the latest version of all NuGet packages, use the dotnet-outdated tool.


Install dotnet-outdated tool:

```
dotnet tool install --global dotnet-outdated-tool
```

Upgrade only to new minor versions of packages

```
dotnet outdated --upgrade --version-lock Major
```

Upgrade to all new versions of packages (more likely to include breaking API changes)

```
dotnet outdated --upgrade
```


## Run

Linux/OS X:

```
sh build.sh
```

Windows:

```
build.bat
```
## Run in Docker

```
cd src/Domivice.Users.Web
docker build -t domivice.users.web .
docker run -p 5000:8080 domivice.users.web
```
### Create Certificate MacOs

```
#Creating the CA
openssl genrsa -des3 -out domivice.CA.key 2048
openssl req -x509 -new -nodes -key domivice.CA.key -sha256 -days 1825 -out domivice.CA.pem

#Creating the CA-Signed Certs

#Step1
openssl genrsa -out api.domivice.dev.key 2048
openssl req -new -key api.domivice.dev.key -out api.domivice.dev.csr

#Step2:Create ext file (api.domivice.dev.ext) with content below
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment
subjectAltName = @alt_names

[alt_names]
DNS.1 = api.domivice.dev
DNS.2 = users-api-service
DNS.3 = localhost

#Step3
openssl x509 -req -in api.domivice.dev.csr -CA domivice.CA.pem -CAkey domivice.CA.key \
-CAcreateserial -out api.domivice.dev.crt -days 825 -sha256 -extfile api.domivice.dev.ext

#Step4
openssl pkcs12 -export -out api.domivice.dev.pfx -inkey api.domivice.dev.key -in api.domivice.dev.crt

```

### Push to Harbor

## Users.Api
```
docker login --username admin core.harbor.domivice.dev 
docker image build --tag core.harbor.domivice.dev/domivice/users.api:[Tag] . -f Dockerfile  --build-arg PACKAGE_SOURCE_USERNAME=[UserName] --build-arg PACKAGE_SOURCE_PASSWORD=[Password]
docker image push core.harbor.domivice.dev/domivice/users.api:[Tag]
```

## IdentityProvider.Migrator
```
docker login --username admin core.harbor.domivice.dev 
docker image build --tag core.harbor.domivice.dev/domivice/users.migration:[Tag] . -f Migration.Dockerfile
docker image push core.harbor.domivice.dev/domivice/users.migration:[Tag] 
```