using AffittaCamere.UserActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AffittaCamere.UserActor
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
    internal class UserActor : Actor, IUserActor
    {
        private const string NumberRoomKeyName = "NumberRoomKeyName";
        private const string TimeLeftKeyName = "TimeLeftKeyName";
        private IActorTimer actorTimer;

        /// <summary>
        /// Inizializza una nuova istanza di UserActor
        /// </summary>
        /// <param name="actorService">Elemento Microsoft.ServiceFabric.Actors.Runtime.ActorService che ospiterà questa istanza dell'attore.</param>
        /// <param name="actorId">Elemento Microsoft.ServiceFabric.Actors.ActorId per questa istanza dell'attore.</param>
        public UserActor(ActorService actorService, ActorId actorId)
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

        protected override Task OnDeactivateAsync()
        {
            if (actorTimer != null)
            {
                UnregisterTimer(actorTimer);
            }

            return base.OnDeactivateAsync();
        }

        private async Task SetExpiration(object state)
        {
            ConditionalValue<int> dateTimeLeft = await this.StateManager.TryGetStateAsync<int>(TimeLeftKeyName, default(CancellationToken));
            if (dateTimeLeft.HasValue)
            {
                int after = dateTimeLeft.Value - 1;
                await this.StateManager.SetStateAsync(TimeLeftKeyName, after, default(CancellationToken));
            }
            else
            {
                await this.StateManager.SetStateAsync(TimeLeftKeyName, 1440, default(CancellationToken)); // 24hh = 1440 mm
            }
        }

        #region [ Impl Interfaces ]

        public Task SetRoomReservedAsync(int roomNumber, CancellationToken cancellationToken)
        {
            actorTimer = this.RegisterTimer(SetExpiration, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(60));

            return this.StateManager.SetStateAsync(NumberRoomKeyName, roomNumber, cancellationToken);
        }

        public Task UnsetRoomReservedAsync(CancellationToken cancellationToken)
        {
            this.StateManager.RemoveStateAsync(TimeLeftKeyName, cancellationToken);
            return this.StateManager.TryRemoveStateAsync(NumberRoomKeyName, cancellationToken);
        }

        public async Task<bool> CanReserve(CancellationToken cancellationToken)
        {
            ConditionalValue<int> val = await this.StateManager.TryGetStateAsync<int>(NumberRoomKeyName, cancellationToken);
            return val.HasValue;
        }

        public async Task<int> TimeToDeparture(CancellationToken cancellationToken)
        {
            ConditionalValue<int> dateTimeLeft = await this.StateManager.TryGetStateAsync<int>(TimeLeftKeyName, default(CancellationToken));
            return dateTimeLeft.Value;
        }

        #endregion
    }
}
