# Étape 1 : Utiliser une image de base .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Définir le répertoire de travail
WORKDIR /app

# Copier les fichiers du projet dans le conteneur
COPY ./ProductApi ./ProductApi
COPY ./ProductApi.Tests ./ProductApi.Tests

# Restaurer les dépendances
RUN dotnet restore ./ProductApi/ProductApi.csproj

# Construire le projet en mode Release
RUN dotnet build ./ProductApi/ProductApi.csproj -c Release

# Publier le projet dans le répertoire "out"
RUN dotnet publish ./ProductApi/ProductApi.csproj -c Release -o /app/out

# Étape 2 : Utiliser une image de base runtime pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Définir le répertoire de travail
WORKDIR /app

# Copier les fichiers publiés depuis l'étape précédente
COPY --from=build /app/out .

# Exposer le port sur lequel l'application va écouter
EXPOSE 5000 5001

# Lancer l'application
ENTRYPOINT ["dotnet", "ProductApi.dll"]
