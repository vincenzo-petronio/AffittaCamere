using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AffittaCamere.RoomsService.Interfaces
{
    /// <summary>
    /// Service Proxy
    /// </summary>
    interface IRoomsServiceProxy
    {
        /// <summary>
        /// Restituisce la lista di tutte le stanze.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<RoomData>> GetAllRoomsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Aggiunge o aggiorna una stanza.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdateRoomAsync(RoomData room, CancellationToken cancellationToken);
    }
}
