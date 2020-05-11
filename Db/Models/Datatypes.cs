using System;
using System.Collections.Generic;

namespace Webapp
{
    public partial class Datatypes
    {
        public Datatypes()
        {
            Sensors = new HashSet<Sensor>();
        }

        public int DataTypeId { get; set; }
        public string Metadata { get; set; }
        public string Schema { get; set; }

        public virtual ICollection<Sensor> Sensors { get; set; }
    }
}
