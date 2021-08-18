FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DevryDeveloperClub/DevryDeveloperClub.csproj", "DevryDeveloperClub/"]
RUN dotnet restore "DevryDeveloperClub/DevryDeveloperClub.csproj"
COPY . .
WORKDIR "/src/DevryDeveloperClub"
RUN dotnet build "DevryDeveloperClub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevryDeveloperClub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevryDeveloperClub.dll"]
