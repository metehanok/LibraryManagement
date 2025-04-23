# .NET SDK imaj�n� kullan�yoruz
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build a�amas�
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# ��z�m dosyas�n� kopyala
COPY ["LibraryManagement.sln", "./"]

# Projeleri kopyala
COPY ["LibraryManagement.WebAPI/LibraryManagement.WebAPI.csproj", "LibraryManagement.WebAPI/"]
COPY ["LibraryManagementAPI.Core/LibraryManagementAPI.Core.csproj", "LibraryManagementAPI.Core/"]
COPY ["LibraryManagementAPI.Data/LibraryManagementAPI.Data.csproj", "LibraryManagementAPI.Data/"]
COPY ["LibraryManagementAPI.Service/LibraryManagementAPI.Service.csproj", "LibraryManagementAPI.Service/"]


# NuGet restore i�lemi
RUN dotnet restore "LibraryManagement.sln"

# Projeleri build et
COPY . . 
RUN dotnet build "LibraryManagement.sln" -c Release -o /app/build

# Uygulama dosyalar�n� ��kar
FROM build AS publish
RUN dotnet publish "LibraryManagement.sln" -c Release -o /app/publish

# �al��t�rma a�amas�
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 

# Burada do�ru .dll dosyas�n� belirtiyoruz
ENTRYPOINT ["dotnet", "LibraryManagement.WebAPI.dll"]
