targetScope = 'subscription'

param location string = 'francecentral'
param resourceGroupName string = 'rg-todo-app-dev'

// Création du groupe de ressources
resource rg 'Microsoft.Resources/resourceGroups@2023-07-01' = {
  name: resourceGroupName
  location: location
}

output rgName string = rg.name
