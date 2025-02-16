# Étape 1 : Build de l'application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copier les fichiers du projet
COPY ProductApi/ProductApi.csproj ./ProductApi/
RUN dotnet restore ProductApi/ProductApi.csproj

# Copier tout le code source et compiler
COPY ProductApi/ ./ProductApi/
WORKDIR /app/ProductApi

# Publier uniquement le projet principal (exclure les tests)
RUN dotnet publish ProductApi.csproj -c Release -o /app/out --no-restore

# Étape 2 : Image de runtime légère
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copier uniquement les fichiers nécessaires
COPY --from=build /app/out .

# Exposer les ports HTTP/HTTPS
EXPOSE 5000 5001

# Commande pour lancer l’API
ENTRYPOINT ["dotnet", "ProductApi.dll"]
