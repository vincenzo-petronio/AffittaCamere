using AffittaCamere.RoomsService.Interfaces;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace RoomsService
{
    /// <summary>
    /// Il runtime di Service Fabric crea un'istanza di questa classe per ogni istanza del servizio.
    /// </summary>
    internal sealed class RoomsService : StatelessService, IRoomsService
    {
        public RoomsService(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Override facoltativo per creare listener (ad esempio TCP, HTTP) per consentire questa replica del servizio di gestire richieste client o utente.
        /// </summary>
        /// <returns>Raccolta di listener.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            // Qui è possibile ritornare una collection di listener, cioè il servizio
            // è in ascolto su più endpoint (potenzialmente su protocolli diversi).
            // La coppia nome:indirizzo viene passata come oggetto JSON quando un client
            // richiede un listener per una istanza di servizio (se stateless) o replica (se stateful).
            // Il nome del listener deve essere unico in caso di più listener, ma può essere omesso
            // se viene ritornato un singolo listener.
            return this.CreateServiceRemotingInstanceListeners();
        }

        /// <summary>
        /// Questo è il punto di ingresso principale dell'istanza del servizio.
        /// </summary>
        /// <param name="cancellationToken">Annullato quando Service Fabric deve arrestare questa istanza del servizio.</param>
        //protected override async Task RunAsync(CancellationToken cancellationToken)
        //{
        //    // TODO: sostituire il codice di esempio seguente con la logica personalizzata 
        //    //       oppure rimuovere questo override di RunAsync se non è necessario nel servizio.

        //    long iterations = 0;

        //    while (true)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();

        //        ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

        //        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        //    }
        //}

        #region [ Impl Interfaces ]

        public async Task<bool> AddOrUpdateRoomAsync(RoomData room, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RoomData>> GetAllRoomsAsync(CancellationToken cancellationToken)
        {
            List<RoomData> rooms = new List<RoomData>();

            for (int i = 1; i <= 10; i++)
            {
                rooms.Add(new RoomData() { Name = "Room " + i });
            }
            return rooms;

        }

        #endregion
    }
}
