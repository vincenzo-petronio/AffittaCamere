﻿using AffittaCamere.RoomActor.Interfaces;
using AffittaCamere.UserActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;
using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace AffittaCamere.RoomActor
{
    /// <remarks>
    /// Questa classe rappresenta un actor.
    /// Ogni ActorID è associato a un'istanza di questa classe.
    /// L'attributo StatePersistence determina la persistenza e la replica dello stato dell'actor:
    ///  - Persisted: lo stato viene scritto su disco e replicato.
    ///  - Volatile: lo stato viene mantenuto solo in memoria e replicato.
    ///  - None: lo stato viene mantenuto solo in memoria e non viene replicato.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class RoomActor : Actor, IRoomActor
    {
        private const string RoomInfoKeyName = "RoomInfoKeyName";

        /// <summary>
        /// Inizializza una nuova istanza di RoomActor
        /// </summary>
        /// <param name="actorService">Elemento Microsoft.ServiceFabric.Actors.Runtime.ActorService che ospiterà questa istanza dell'attore.</param>
        /// <param name="actorId">Elemento Microsoft.ServiceFabric.Actors.ActorId per questa istanza dell'attore.</param>
        public RoomActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        /// <summary>
        /// Questo metodo viene chiamato ogni volta che si attiva un actor.
        /// Un actor viene attivato la prima volta che viene richiamato uno dei relativi metodi.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            // StateManager è l'archivio privato degli stati di questo actor.
            // I dati archiviati in StateManager verranno replicati per garantire una disponibilità elevata per gli actor che usano l'archiviazione volatile o persistente degli stati.
            // In StateManager è possibile salvare qualsiasi oggetto serializzabile.
            // Per altre informazioni, vedere https://aka.ms/servicefabricactorsstateserialization

            return Task.CompletedTask;
        }


        #region [ Impl Interfaces ]

        public async Task<RoomInfo> GetRoomInfoAsync(CancellationToken cancellationToken)
        {
            var roomInfoFromState = await this.StateManager.TryGetStateAsync<RoomInfo>(RoomInfoKeyName, cancellationToken);

            return roomInfoFromState.HasValue ? roomInfoFromState.Value : null;
        }

        public async Task UpdateRoomInfoAsync(RoomInfo roomInfo, CancellationToken cancellationToken)
        {
            if (roomInfo == null) throw new ArgumentNullException();

            // N.B. In alcune situazioni una best-practices è salvare ogni proprietà con una key diversa, così da non dover
            // riscrivere tutto l'oggetto per ogni proprietà da cambiare.
            
            var userActorProxy = ActorProxy.Create<IUserActor>(new ActorId(roomInfo.User), new Uri("fabric:/AffittaCamere/UserActorService"));
            if (roomInfo.Reserved)
            {
                await userActorProxy.SetRoomReservedAsync(roomInfo.Number, cancellationToken);
                await this.StateManager.SetStateAsync<RoomInfo>(RoomInfoKeyName, roomInfo, cancellationToken);
            }
            else
            {
                await userActorProxy.UnsetRoomReservedAsync(cancellationToken);
                roomInfo.User = string.Empty;
                await this.StateManager.SetStateAsync<RoomInfo>(RoomInfoKeyName, roomInfo, cancellationToken);
            }

        }

        #endregion
    }
}
