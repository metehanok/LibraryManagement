# Base image olarak .NET SDK'yý alýyoruz
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Çalýþma dizini oluþturuyoruz
WORKDIR /app

# Proje dosyasýný kopyalayýp restore ediyoruz
COPY *.csproj ./  
RUN dotnet restore  

# Proje dosyalarýný kopyalayýp uygulamayý build ediyoruz
COPY . ./  
RUN dotnet publish -c Release -o out  

# Çalýþtýrýlabilir imaj oluþturuluyor
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .  

# Projeyi baþlatýyoruz
ENTRYPOINT ["dotnet", "LibraryAutomation.dll"]  

# Port açýyoruz
EXPOSE 80  
