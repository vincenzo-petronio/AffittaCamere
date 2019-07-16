using System.Runtime.Serialization;

namespace AffittaCamere.RoomActor.Interfaces
{
    [DataContract]
    public class RoomInfo
    {
        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public bool Reserved { get; set; }

        [DataMember]
        public string User { get; set; }
    }
}
