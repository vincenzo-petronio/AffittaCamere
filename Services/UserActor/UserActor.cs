using AffittaCamere.UserActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Data;
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

        #region [ Impl Interfaces ]

        public Task SetRoomReservedAsync(int roomNumber, CancellationToken cancellationToken)
        {
            return this.StateManager.SetStateAsync(NumberRoomKeyName, roomNumber, cancellationToken);
        }

        public async Task<bool> CanReserve(CancellationToken cancellationToken)
        {
            ConditionalValue<int> val = await this.StateManager.TryGetStateAsync<int>(NumberRoomKeyName, cancellationToken);
            return val.HasValue;
        }

        public Task UnsetRoomReservedAsync(CancellationToken cancellationToken)
        {
            return this.StateManager.TryRemoveStateAsync(NumberRoomKeyName, cancellationToken);
        }



        #endregion
    }
}
