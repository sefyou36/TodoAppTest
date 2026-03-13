# 1. Utiliser le SDK 9.0 pour la compilation
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# 2. Copier les fichiers .csproj et restaurer (le reste ne change pas)
COPY ["TodoApp.api/TodoApp.api.csproj", "TodoApp.api/"]
COPY ["TodoApp.Application/TodoApp.Application.csproj", "TodoApp.Application/"]
COPY ["TodoApp.Domain/TodoApp.Domain.csproj", "TodoApp.Domain/"]
COPY ["TodoApp.Infrastructure/TodoApp.Infrastructure.csproj", "TodoApp.Infrastructure/"]
RUN dotnet restore "TodoApp.api/TodoApp.api.csproj"

# 3. Copier tout le reste et compiler
COPY . .
WORKDIR "/app/TodoApp.api"
RUN dotnet publish -c Release -o /out

# 4. Utiliser le Runtime 9.0 pour l'exécution
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .

# Lancer l'application
ENTRYPOINT ["dotnet", "TodoApp.api.dll"]