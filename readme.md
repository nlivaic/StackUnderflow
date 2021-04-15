### Getting Started

#### Set environment variables

`.env` file example:

    CONNECTIONSTRINGS__STACKUNDERFLOWDBCONNECTION=
    CONNECTIONSTRINGS__STACKUNDERFLOWIDENTITYDB=
    IDP__AUTHORITY=
    IDP__APINAME=
    LOGS__URL=
    FACEBOOK__CLIENTSECRET=
    FACEBOOK__CLIENTID=
    EMAILSETTINGS__DONOTREPLY=
    EMAILSETTINGS__HOST=
    EMAILSETTINGS__PORT=
    EMAILSETTINGS__USERNAME=
    EMAILSETTINGS__PASSWORD=
    POSTGRES_USER=
    POSTGRES_PASSWORD=

### Generating cert for your local development box

`.crt` and `.key` files for Api and Identity Server are committed to source control. These are self-signed certificates with dummy password of `rootpw`, so there is no security leaks there.
These were created with below commands:

    sudo openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout ./nginx/id-local.key -out ./nginx/id-local.crt -config ./nginx/id-local.conf -passin pass:rootpw
    sudo openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout ./nginx/api-local.key -out ./nginx/api-local.crt -config ./nginx/api-local.conf -passin pass:rootpw

What you need to do is create the certs yourself and add them to your computers CA store. Follow instructions below:

1. Go to **solution root folder** and execute below lines from **WSL2**:

   sudo openssl pkcs12 -export -out ./nginx/id-local.pfx -inkey ./nginx/id-local.key -in ./nginx/id-local.crt
   sudo openssl pkcs12 -export -out ./nginx/api-local.pfx -inkey ./nginx/api-local.key -in ./nginx/api-local.crt

2. Then go to ./nginx, right-click on both `.pfx` files and install them to `LocalMachine` -> `Trusted Root Certification Authorities`.

For more details consult: https://bit.ly/3eWOHH2

### HOSTS

Add to local hosts file:

    # Stack Underflow DNS
    127.0.0.1	    api-local.stack-underflow.com
    127.0.0.1	    id-local.stack-underflow.com

### Setting up Kubernetes

1. Enable k8s on Docker Desktop. Make sure you check the `Show system containers (advanced)`.
2. Add command to Powershell: https://bit.ly/3dDx6BF
3. Install k8s dashboard: https://bit.ly/3wynUaa
