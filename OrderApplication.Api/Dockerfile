#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OrderApplication.Api/OrderApplication.Api.csproj", "OrderApplication.Api/"]
COPY ["OrderApplication.Model/OrderApplication.Model.csproj", "OrderApplication.Model/"]
COPY ["OrderApplication.Core/OrderApplication.Core.csproj", "OrderApplication.Core/"]
COPY ["OrderApplication.Business/OrderApplication.Business.csproj", "OrderApplication.Business/"]
RUN dotnet restore "OrderApplication.Api/OrderApplication.Api.csproj"
COPY . .
WORKDIR "/src/OrderApplication.Api"
RUN dotnet build "OrderApplication.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderApplication.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderApplication.Api.dll"]