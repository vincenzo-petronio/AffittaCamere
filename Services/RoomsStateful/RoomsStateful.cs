using AffittaCamere.RoomsService.Interfaces;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace AffittaCamere.RoomsStateful
{
    /// <summary>
    /// Il runtime di Service Fabric crea un'istanza di questa classe per ogni replica del servizio.
    /// </summary>
    internal sealed class RoomsStateful : StatefulService, IRoomsService
    {
        private const string RoomsDictionaryKeyName = "roomsDictionaryKeyName";
        private IReliableDictionary<string, RoomData> roomsDictionary;


        public RoomsStateful(StatefulServiceContext context)
            : base(context)
        { }

        public async Task<bool> AddOrUpdateRoomAsync(RoomData room, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RoomData>> GetAllRoomsAsync(CancellationToken cancellationToken)
        {
            List<RoomData> roomsResult = new List<RoomData>();
            using (var tx = this.StateManager.CreateTransaction())
            {
                var rooms = await roomsDictionary.CreateEnumerableAsync(tx);
                var roomsEnumerator = rooms.GetAsyncEnumerator();
                while (await roomsEnumerator.MoveNextAsync(cancellationToken))
                {
                    roomsResult.Add(roomsEnumerator.Current.Value);
                }

                await tx.CommitAsync();
            }
            return roomsResult;
        }

        /// <summary>
        /// Override facoltativo per creare listener (ad esempio HTTP, servizio remoto, WCF e così via) per consentire a questa replica del servizio di gestire richieste client o utente.
        /// </summary>
        /// <remarks>
        /// Per altre informazioni sulle comunicazioni dei servizi, vedere https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>Raccolta di listener.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();

            //return new[]
            //{
            //    new ServiceReplicaListener(ctx => 
            //        new FabricTransportServiceRemotingListener(ctx, this)
            //    )
            //};
        }

        /// <summary>
        /// Questo è il punto di ingresso principale della replica del servizio.
        /// Questo metodo viene eseguito quando questa replica del servizio diventa primaria e il relativo stato consente operazioni di scrittura.
        /// </summary>
        /// <param name="cancellationToken">Annullato quando Service Fabric deve arrestare questa replica del servizio.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            this.roomsDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, RoomData>>(RoomsDictionaryKeyName);

            var partition = (Int64RangePartitionInformation)this.Partition.PartitionInfo;



            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
