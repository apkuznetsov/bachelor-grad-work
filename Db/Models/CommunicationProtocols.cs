using System;
using System.Collections.Generic;

namespace Webapp
{
    public partial class CommunicationProtocols
    {
        public CommunicationProtocols()
        {
            Sensors = new HashSet<Sensor>();
        }

        public int CommunicationProtocolId { get; set; }
        public string ProtocolName { get; set; }

        public virtual ICollection<Sensor> Sensors { get; set; }
    }
}
