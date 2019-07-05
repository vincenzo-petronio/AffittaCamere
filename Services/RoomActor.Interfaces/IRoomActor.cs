using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading;
using System.Threading.Tasks;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace AffittaCamere.RoomActor.Interfaces
{
    /// <summary>
    /// Questa interfaccia consente di definire i metodi esposti da un actor.
    /// I client usano questa interfaccia per interagire con l'actor che la implementa.
    /// </summary>
    public interface IRoomActor : IActor
    {
        Task UpdateRoomInfoAsync(RoomInfo roomInfo, CancellationToken cancellationToken);

        Task<RoomInfo> GetRoomInfoAsync(CancellationToken cancellationToken);
    }
}
