# Connette e visualizza lo stato del Cluster SF 
Connect-ServiceFabricCluster


# Test dei packages
Test-ServiceFabricApplicationPackage "C:\Workspace_VS\AffittaCamere\AffittaCamere\pkg\Debug"

# Import del modulo per aggiungere i successivi comandi
Import-Module 'C:\Program Files\Microsoft SDKs\Service Fabric\Tools\PSModule\ServiceFabricSDK\ServiceFabricSDK.psm1'

# Copia dei package sul Cluster (copia i package ma su Service Fabric Explorer non c'è nulla)
$packagesLocation = "C:\Workspace_VS\AffittaCamere\AffittaCamere\pkg\Debug"
$clusterManifest = Get-ServiceFabricClusterManifest
$connectionString = Get-ImageStoreConnectionStringFromClusterManifest -ClusterManifest $clusterManifest
Copy-ServiceFabricApplicationPackage -ApplicationPackagePath $packagesLocation -ApplicationPackagePathInImageStore "AffittaCamerePackage" $connectionString

# Registrazione dei package sul Cluster (su Service Fabric Explorer mostra l'application type)
Register-ServiceFabricApplicationType -ApplicationPathInImageStore "AffittaCamerePackage"

# Crea l'istanza dell'application
New-ServiceFabricApplication -ApplicationName "fabric:/AffittaCamere" -ApplicationTypeName "AffittaCamereType" -ApplicationTypeVersion "1.0.0"