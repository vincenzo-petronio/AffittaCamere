using System;
using System.Runtime.Serialization;

namespace AffittaCamere.RoomsService.Interfaces
{
    [DataContract]
    public class RoomData
    {
        public RoomData()
        {
        }

        [DataMember(IsRequired = true, Order = 1)]
        public string Name { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public bool IsAvailable { get; set; }
    }
}
