FROM mcr.microsoft.com/mssql/server:2022-latest

USER root
WORKDIR /usr/src/app
COPY schema.sql entrypoint.sh ./
RUN chmod +x entrypoint.sh && chown -R mssql /usr/src/app

USER mssql
ENV ACCEPT_EULA=Y
EXPOSE 1433
CMD ["./entrypoint.sh"]