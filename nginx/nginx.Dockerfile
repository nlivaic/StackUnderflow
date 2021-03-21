FROM nginx

# NOTE!!!
# Even though we keep nginx.local.conf inside /nginx solution folder,
# docker tool is not aware of it since it is only a Visual Studio construct.
COPY nginx/nginx.local.conf /etc/nginx/nginx.conf
COPY nginx/stack-underflow.crt /etc/ssl/certs/id-local.stack-underflow.com.crt
COPY nginx/stack-underflow.key /etc/ssl/private/id-local.stack-underflow.com.key