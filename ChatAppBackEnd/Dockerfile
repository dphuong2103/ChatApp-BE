#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ChatAppBackEnd/ChatAppBackEnd.csproj", "ChatAppBackEnd/"]
RUN dotnet restore "ChatAppBackEnd/ChatAppBackEnd.csproj"
COPY . .
WORKDIR "/src/ChatAppBackEnd"
RUN dotnet build "ChatAppBackEnd.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatAppBackEnd.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["ChatAppBackEnd/Https/midouz.online.pfx", "/https/midouz.online.pfx"]
COPY ["ChatAppBackEnd/Https/midouz_online.crt","/usr/local/share/ca-certificates/midouz_online.crt"]
RUN update-ca-certificates
ENTRYPOINT ["dotnet", "ChatAppBackEnd.dll"]