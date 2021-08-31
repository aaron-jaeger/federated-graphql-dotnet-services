#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/BookManagement/BookManagement.Api/BookManagement.Api.csproj", "src/BookManagement/BookManagement.Api/"]
COPY ["src/Shared/Core.Application/Core.Application.csproj", "src/Shared/Core.Application/"]
COPY ["src/Shared/Core.Infrastructure/Core.Infrastructure.csproj", "src/Shared/Core.Infrastructure/"]
COPY ["src/BookManagement/BookManagement.Domain/BookManagement.Domain.csproj", "src/BookManagement/BookManagement.Domain/"]
COPY ["src/Shared/Core.Domain/Core.Domain.csproj", "src/Shared/Core.Domain/"]
COPY ["src/BookManagement/BookManagement.Infrastructure/BookManagement.Infrastructure.csproj", "src/BookManagement/BookManagement.Infrastructure/"]
RUN dotnet restore "src/BookManagement/BookManagement.Api/BookManagement.Api.csproj"
COPY . .
WORKDIR "/src/src/BookManagement/BookManagement.Api"
RUN dotnet build "BookManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookManagement.Api.dll"]