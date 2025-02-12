# Étape 1 : Build de l'application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Optimisation : ne copier que le csproj d'abord pour mieux utiliser le cache Docker
COPY *.csproj ./
RUN dotnet restore

# Copier le reste et compiler
COPY . .
RUN dotnet publish -c Release -o out

# Étape 2 : Image de runtime légère
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copier uniquement les fichiers nécessaires
COPY --from=build /app/out .

# Exposer les ports HTTP/HTTPS
EXPOSE 5000 5001

# Commande pour lancer l’API
CMD ["dotnet", "ProductApi.dll"]
