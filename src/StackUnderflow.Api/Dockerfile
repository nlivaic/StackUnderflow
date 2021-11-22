#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
COPY nginx/id-local.crt /usr/local/share/ca-certificates/id-local.crt
RUN update-ca-certificates
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/StackUnderflow.Api/StackUnderflow.Api.csproj", "src/StackUnderflow.Api/"]
COPY ["src/StackUnderflow.Data/StackUnderflow.Data.csproj", "src/StackUnderflow.Data/"]
COPY ["src/StackUnderflow.Common/StackUnderflow.Common.csproj", "src/StackUnderflow.Common/"]
COPY ["src/StackUnderflow.Core/StackUnderflow.Core.csproj", "src/StackUnderflow.Core/"]
RUN dotnet restore "src/StackUnderflow.Api/StackUnderflow.Api.csproj"
COPY . .
WORKDIR "/src/src/StackUnderflow.Api"
RUN dotnet build "StackUnderflow.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StackUnderflow.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StackUnderflow.Api.dll"]