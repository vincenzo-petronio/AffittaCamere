using System;
using System.Collections.Generic;
using System.Text;

namespace AffittaCamere.DataAccess.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public int Number { get; set; }
    }
}
