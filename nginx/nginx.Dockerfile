FROM nginx

# NOTE!!!
# Even though we keep nginx.local.conf inside /nginx solution folder,
# docker tool is not aware of it since it is only a Visual Studio construct.
# On the filesystem nginx.local.conf is on the same level as nginx.Dockerfile
COPY nginx/nginx.local.conf /etc/nginx/nginx.conf
#COPY nginx/id-local.crt /etc/ssl/certs/id-local.globomantics.com.crt
#COPY nginx/id-local.key /etc/ssl/private/id-local.globomantics.com.key