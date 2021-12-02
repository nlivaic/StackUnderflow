# Getting Started

## Install a Docker host

E.g. Docker Desktop

## Set environment variables

`.env` file example:

    MessageBroker__Writer__SharedAccessKeyName=
    MessageBroker__Writer__SharedAccessKey=
    MessageBroker__Reader__SharedAccessKeyName=
    MessageBroker__Reader__SharedAccessKey=
    Facebook__ClientSecret=
    Facebook__ClientId=
    EmailSettings__Username=
    EmailSettings__Password=
    POSTGRES_USER=
    POSTGRES_PASSWORD=

# Generating cert for your local development box

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

# Hosts file

Add to local hosts file:

    # Stack Underflow DNS
    127.0.0.1	    api-local.stack-underflow.com
    127.0.0.1	    id-local.stack-underflow.com
