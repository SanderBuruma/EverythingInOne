FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["PersonalWebsite.csproj", "./"]
RUN dotnet restore "PersonalWebsite.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "PersonalWebsite.csproj" -c Release -o /app/build
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_10.x | bash - && \
    apt-get install -y build-essential nodejs

FROM build AS publish
RUN dotnet publish "PersonalWebsite.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PersonalWebsite.dll"]
