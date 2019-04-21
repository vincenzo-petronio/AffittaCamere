using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AffittaCamere.WebStateless
{
    internal static class Program
    {
        /// <summary>
        /// Questo è il punto di ingresso del processo host del servizio.
        /// </summary>
        private static void Main()
        {
            try
            {
                // Il file ServiceManifest.XML consente di definire uno o più nomi di tipo di servizio.
                // La registrazione di un servizio consente di associare un nome di tipo di servizio a un tipo .NET.
                // Quando Service Fabric crea un'istanza di questo tipo di servizio,
                // nel processo host viene creata un'istanza della classe.

                ServiceRuntime.RegisterServiceAsync("WebStatelessType",
                    context => new WebStateless(context)).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(WebStateless).Name);

                // Impedisce che questo processo host venga terminato in modo che i servizi rimangano in esecuzione. 
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
