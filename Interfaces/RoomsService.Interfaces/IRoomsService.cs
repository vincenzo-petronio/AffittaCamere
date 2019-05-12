using Microsoft.ServiceFabric.Services.Remoting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AffittaCamere.RoomsService.Interfaces
{
    public interface IRoomsService : IService
    {
        Task<List<RoomData>> GetAllRoomsAsync(CancellationToken cancellationToken);

        Task<bool> AddOrUpdateRoomAsync(RoomData room, CancellationToken cancellationToken);
    }
}
