using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading;
using System.Threading.Tasks;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace AffittaCamere.UserActor.Interfaces
{
    /// <summary>
    /// Questa interfaccia consente di definire i metodi esposti da un actor.
    /// I client usano questa interfaccia per interagire con l'actor che la implementa.
    /// </summary>
    public interface IUserActor : IActor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomNumber"></param>
        /// <returns></returns>
        Task SetRoomReservedAsync(int roomNumber, CancellationToken cancellationToken);

        Task UnsetRoomReservedAsync(CancellationToken cancellation);

        /// <summary>
        /// true se l'user non ha già una camera riservata
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> CanReserve(CancellationToken cancellationToken);
    }
}
