# .NET SDK imajýný kullanýyoruz
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build aþamasý
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Çözüm dosyasýný kopyala
COPY ["LibraryManagement.sln", "./"]

# Projeleri kopyala
COPY ["LibraryManagement.WebAPI/LibraryManagement.WebAPI.csproj", "LibraryManagement.WebAPI/"]
COPY ["LibraryManagementAPI.Core/LibraryManagementAPI.Core.csproj", "LibraryManagementAPI.Core/"]
COPY ["LibraryManagementAPI.Data/LibraryManagementAPI.Data.csproj", "LibraryManagementAPI.Data/"]
COPY ["LibraryManagementAPI.Service/LibraryManagementAPI.Service.csproj", "LibraryManagementAPI.Service/"]


# NuGet restore iþlemi
RUN dotnet restore "LibraryManagement.sln"

# Projeleri build et
COPY . . 
RUN dotnet build "LibraryManagement.sln" -c Release -o /app/build

# Uygulama dosyalarýný çýkar
FROM build AS publish
RUN dotnet publish "LibraryManagement.sln" -c Release -o /app/publish

# Çalýþtýrma aþamasý
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 

# Burada doðru .dll dosyasýný belirtiyoruz
ENTRYPOINT ["dotnet", "LibraryManagement.WebAPI.dll"]
