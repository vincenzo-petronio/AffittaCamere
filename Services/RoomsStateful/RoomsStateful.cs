using AffittaCamere.RoomActor.Interfaces;
using AffittaCamere.RoomsService.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
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
        private IReliableDictionary<Guid, RoomData> roomsDictionary;

        public RoomsStateful(StatefulServiceContext context)
            : base(context)
        { }


        #region [ Impl Interfaces ]

        public async Task AddOrUpdateRoomAsync(RoomData room, CancellationToken cancellationToken)
        {
            using (var tx = this.StateManager.CreateTransaction())
            {
                await roomsDictionary.AddOrUpdateAsync(tx, room.Id, room, (id, value) => room);
                await tx.CommitAsync();
            }

            try
            {
                var roomActor = ActorProxy.Create<IRoomActor>(new ActorId(room.Number), new Uri("fabric:/AffittaCamere/RoomActorService"));

                RoomInfo roomInfo = new RoomInfo()
                {
                    Number = room.Number,
                    Reserved = !room.IsAvailable
                };
                await roomActor.UpdateRoomInfoAsync(roomInfo, cancellationToken);
            }
            catch (Exception)
            {

            }

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

            foreach (RoomData r in roomsResult)
            {
                try
                {
                    var roomActor = ActorProxy.Create<IRoomActor>(new ActorId(r.Number), new Uri("fabric:/AffittaCamere/RoomActorService"));
                    var info = await roomActor.GetRoomInfoAsync(cancellationToken);
                    r.IsAvailable = info == null ? false : !info.Reserved;
                }
                catch (Exception)
                {
                }
            }

            return roomsResult;
        }

        public async Task ReserveOrReleaseRoom(int roomNumber, bool reserve, CancellationToken cancellationToken)
        {
            try
            {
                var roomActor = ActorProxy.Create<IRoomActor>(new ActorId(roomNumber), new Uri("fabric:/AffittaCamere/RoomActorService"));
                var info = await roomActor.GetRoomInfoAsync(cancellationToken);
                info.Reserved = reserve;

                RoomInfo roomInfo = new RoomInfo()
                {
                    Number = roomNumber,
                    Reserved = reserve
                };
                await roomActor.UpdateRoomInfoAsync(roomInfo, cancellationToken);
            }
            catch (Exception)
            {

            }
        }

        #endregion

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
            this.roomsDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, RoomData>>(RoomsDictionaryKeyName);

            var partition = (Int64RangePartitionInformation)this.Partition.PartitionInfo;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
