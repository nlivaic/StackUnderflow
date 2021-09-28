#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/StackUnderflow.Migrations/StackUnderflow.Migrations.csproj", "src/StackUnderflow.Migrations/"]
RUN dotnet restore "src/StackUnderflow.Migrations/StackUnderflow.Migrations.csproj"
COPY . .
WORKDIR "/src/src/StackUnderflow.Migrations"
RUN dotnet build "StackUnderflow.Migrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StackUnderflow.Migrations.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["src/StackUnderflow.Migrations/entrypoint.sh", "src/entrypoint.sh"]
COPY ["src/StackUnderflow.Migrations/wait-for-it.sh", "src/wait-for-it.sh"]
CMD ["/bin/bash", "src/entrypoint.sh"]