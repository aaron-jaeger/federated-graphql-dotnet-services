FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/AuthorManagement/AuthorManagement.Api/AuthorManagement.Api.csproj", "AuthorManagement/AuthorManagement.Api/"]
RUN dotnet restore "AuthorManagement/AuthorManagement.Api/AuthorManagement.Api.csproj"
COPY . .
WORKDIR "src/AuthorManagement/AuthorManagement.Api"
RUN dotnet build "AuthorManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthorManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthorManagement.Api.dll"]