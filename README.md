# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

# Getting Started

1.	[Service Fabric Runtime 6.3.187.9494](https://download.microsoft.com/download/7/6/8/76834E9D-91D5-43E3-8CF4-3D954564AB53/MicrosoftServiceFabric.6.3.187.9494.exe) 
Installare da riga di comando passando come parametro `MicrosoftServiceFabric.6.3.187.9494.exe /AcceptEula`

2.	[Windows .NET SDK 3.2.187.9494](https://download.microsoft.com/download/7/6/8/76834E9D-91D5-43E3-8CF4-3D954564AB53/MicrosoftServiceFabricSDK.3.2.187.msi)

# Proxy
Per configurare NuGet con il proxy:
1.  [Download Windows x86 Commandline latest version](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe)
2.  Eseguire i seguenti comandi:
    ```
    nuget.exe config -set http_proxy=http://HOST:PORT
    nuget.exe config -set http_proxy.user=USER
    nuget.exe config -set http_proxy.password=PSW
    ```
# Params
[GUI = localhost:9110](http://localhost:9110/)


# Build and Test
TODO: Describe and show how to build your code and run the tests. 

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 
