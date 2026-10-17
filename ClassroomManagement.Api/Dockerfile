FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine

RUN addgroup app1 && adduser -S -G app1 app1
WORKDIR /app
RUN chown app1:app1 /app
USER app1

COPY --chown=app1:app1 ClassroomManagement.Api.csproj .
RUN dotnet restore

COPY --chown=app1:app1 . .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
CMD ["dotnet", "watch", "run", "--no-launch-profile"]