# Usar la imagen base del SDK de .NET 8.0 para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos del proyecto
COPY . .

# Restaurar dependencias
RUN dotnet restore

# Compilar el proyecto en modo Release
RUN dotnet build -c Release -o /app/build

# Publicar el proyecto
RUN dotnet publish -c Release -o /app/publish

# Usar la imagen base del runtime de .NET 8.0 para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados desde la etapa de compilación
COPY --from=build /app/publish .

# Establecer la variable de entorno para que la app escuche en el puerto 5204
ENV ASPNETCORE_URLS=http://+:5204

# Exponer el puerto que usa la aplicación 
EXPOSE 5204

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "Gestion_Cartuchos.dll"]
