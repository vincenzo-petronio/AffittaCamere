using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Collections.Generic;
using System.Fabric;
using System.IO;

namespace AffittaCamere.RestApiStateless
{
    /// <summary>
    /// FabricRuntime crea un'istanza di questa classe per ogni istanza del tipo di servizio. 
    /// </summary>
    internal sealed class RestApiStateless : StatelessService
    {
        public RestApiStateless(StatelessServiceContext context)
            : base(context)
        { }
        
        /// <summary>
        /// Override facoltativo per creare listener (come TCP, HTTP) per questa istanza del servizio.
        /// </summary>
        /// <returns>Raccolta di listener.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        return new WebHostBuilder()
                                    .UseKestrel()
                                    .ConfigureServices(
                                        services => services
                                            .AddSingleton<StatelessServiceContext>(serviceContext))
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseStartup<Startup>()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url)
                                    .Build();
                    }))
            };
        }


        // N.B. L'override di RunAsync in questo caso non è necessario. 
        // Il RunAsync è necessario per fare operazioni long-running. 
        // Su Service Fabric è possibile ignorare il RunAsync e 
        // continuare ad accettare e gestire le richieste del client 
        // attraverso il communication listener.

    }
}
