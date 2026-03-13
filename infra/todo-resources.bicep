param location string = resourceGroup().location
param appName string = 'todo-api-${uniqueString(resourceGroup().id)}'
param sqlServerName string = 'sql-todo-${uniqueString(resourceGroup().id)}'

@secure()
param sqlAdminPassword string

// 1. Le "Plan" (Le moteur du serveur)
resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: 'plan-todo-app'
  location: location
  sku: {
    name: 'F1' // Niveau Gratuit pour nos tests
  }
  kind: 'linux'
  properties: {
    reserved: true // Obligatoire pour Linux/Docker
  }
}

// 2. L'App Service (La vitrine pour ton API Docker)
resource webApp 'Microsoft.Web/sites@2022-09-01' = {
  name: appName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOCKER|mcr.microsoft.com/dotnet/samples:aspnetapp' // Image temporaire par défaut
    }
  }
}

// 3. Le serveur SQL
resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: 'sqladmin'
    administratorLoginPassword: sqlAdminPassword
  }
}

// 4. La base de données SQL
resource sqlDatabase 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  parent: sqlServer
  name: 'TodoDb'
  location: location
  sku: {
    name: 'Basic' // Le moins cher possible
  }
}

// 5. Le coffre-fort (Key Vault)
resource keyVault 'Microsoft.KeyVault/vaults@2023-02-01' = {
  name: 'kv-todo-${uniqueString(resourceGroup().id)}'
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    accessPolicies: [] // On configurera les accès plus tard
    enabledForDeployment: true
    enabledForTemplateDeployment: true
  }
}

// Ajouter le mot de passe SQL dans le coffre
resource sqlSecret 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: keyVault
  name: 'SqlPassword'
  properties: {
    value: sqlAdminPassword
  }
}

// 6. Le registre d'images Docker (ACR)
resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: 'acrtodo${uniqueString(resourceGroup().id)}' // Doit être unique et sans tirets
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true // On l'active pour simplifier la connexion de l'App Service
  }
}

output acrLoginServer string = acr.properties.loginServer
