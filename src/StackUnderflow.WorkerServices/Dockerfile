#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/StackUnderflow.WorkerServices/StackUnderflow.WorkerServices.csproj", "src/StackUnderflow.WorkerServices/"]
COPY ["src/StackUnderflow.Core/StackUnderflow.Core.csproj", "src/StackUnderflow.Core/"]
COPY ["src/StackUnderflow.Common/StackUnderflow.Common.csproj", "src/StackUnderflow.Common/"]
RUN dotnet restore "src/StackUnderflow.WorkerServices/StackUnderflow.WorkerServices.csproj"
COPY . .
WORKDIR "/src/src/StackUnderflow.WorkerServices"
RUN dotnet build "StackUnderflow.WorkerServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StackUnderflow.WorkerServices.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["src/StackUnderflow.WorkerServices/entrypoint.sh", "src/entrypoint.sh"]
COPY ["src/StackUnderflow.WorkerServices/wait-for-it.sh", "src/wait-for-it.sh"]
CMD ["/bin/bash", "src/entrypoint.sh"]
