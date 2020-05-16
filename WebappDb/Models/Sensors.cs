using System;
using System.Collections.Generic;

namespace WebappDb
{
    public partial class Sensors
    {
        public Sensors()
        {
            ExperimentSensors = new HashSet<ExperimentSensors>();
            ProcessingSensors = new HashSet<ProcessingSensors>();
        }

        public int SensorId { get; set; }
        public string Metadata { get; set; }
        public int DataTypeId { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public int CommunicationProtocolId { get; set; }

        public string Name { get; set; }

        public virtual CommunicationProtocols CommunicationProtocol { get; set; }
        public virtual Datatypes DataType { get; set; }
        public virtual ICollection<ExperimentSensors> ExperimentSensors { get; set; }
        public virtual ICollection<ProcessingSensors> ProcessingSensors { get; set; }
    }
}
