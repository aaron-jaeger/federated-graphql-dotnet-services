FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/BookManagement/BookManagement.Api/BookManagement.Api.csproj", "BookManagement/BookManagement.Api/"]
RUN dotnet restore "BookManagement/BookManagement.Api/BookManagement.Api.csproj"
COPY . .
WORKDIR "src/BookManagement/BookManagement.Api"
RUN dotnet build "BookManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookManagement.Api.dll"]