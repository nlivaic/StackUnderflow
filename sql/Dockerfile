FROM postgres

COPY sql/InitializeStackUnderflowDb.sql /docker-entrypoint-initdb.d/
COPY sql/InitializeStackUnderflowIdentityDb.sql /docker-entrypoint-initdb.d/
RUN echo "en_US UTF-8" >> /etc/locale.gen && locale-gen
