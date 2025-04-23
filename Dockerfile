# Base image olarak .NET SDK'y� al�yoruz
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# �al��ma dizini olu�turuyoruz
WORKDIR /app

# Proje dosyas�n� kopyalay�p restore ediyoruz
COPY *.csproj ./  
RUN dotnet restore  

# Proje dosyalar�n� kopyalay�p uygulamay� build ediyoruz
COPY . ./  
RUN dotnet publish -c Release -o out  

# �al��t�r�labilir imaj olu�turuluyor
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .  

# Projeyi ba�lat�yoruz
ENTRYPOINT ["dotnet", "LibraryAutomation.dll"]  

# Port a��yoruz
EXPOSE 80  
