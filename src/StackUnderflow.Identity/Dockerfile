#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 7000

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/StackUnderflow.Identity/StackUnderflow.Identity.csproj", "src/StackUnderflow.Identity/"]
COPY ["src/StackUnderflow.Common/StackUnderflow.Common.csproj", "src/StackUnderflow.Common/"]
RUN dotnet restore "src/StackUnderflow.Identity/StackUnderflow.Identity.csproj"
COPY . .
WORKDIR "/src/src/StackUnderflow.Identity"
RUN dotnet build "StackUnderflow.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StackUnderflow.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StackUnderflow.Identity.dll"]
