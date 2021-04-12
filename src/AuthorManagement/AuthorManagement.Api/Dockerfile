#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/AuthorManagement/AuthorManagement.Api/AuthorManagement.Api.csproj", "src/AuthorManagement/AuthorManagement.Api/"]
COPY ["src/Shared/Core.Application/Core.Application.csproj", "src/Shared/Core.Application/"]
COPY ["src/Shared/Core.Infrastructure/Core.Infrastructure.csproj", "src/Shared/Core.Infrastructure/"]
COPY ["src/AuthorManagement/AuthorManagement.Infrastructure/AuthorManagement.Infrastructure.csproj", "src/AuthorManagement/AuthorManagement.Infrastructure/"]
COPY ["src/AuthorManagement/AuthorManagement.Domain/AuthorManagement.Domain.csproj", "src/AuthorManagement/AuthorManagement.Domain/"]
COPY ["src/Shared/Core.Domain/Core.Domain.csproj", "src/Shared/Core.Domain/"]
RUN dotnet restore "src/AuthorManagement/AuthorManagement.Api/AuthorManagement.Api.csproj"
COPY . .
WORKDIR "/src/src/AuthorManagement/AuthorManagement.Api"
RUN dotnet build "AuthorManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthorManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthorManagement.Api.dll"]