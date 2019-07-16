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
        public Guid Id { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public string Name { get; set; }

        [DataMember(IsRequired = true, Order = 3)]
        public bool IsAvailable { get; set; }

        [DataMember(IsRequired = true, Order = 4)]
        public int Number { get; set; }

        [DataMember(IsRequired = true, Order = 5)]
        public string User { get; set; }
    }
}
