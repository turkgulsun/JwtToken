FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["JwtToken.Api/JwtToken.Api.csproj", "JwtToken.Api/"]
RUN dotnet restore "JwtToken.Api/JwtToken.Api.csproj"
COPY . .
WORKDIR "/src/JwtToken.Api"
RUN dotnet build "JwtToken.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JwtToken.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JwtToken.Api.dll"]
