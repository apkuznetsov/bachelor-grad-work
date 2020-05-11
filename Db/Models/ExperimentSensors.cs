using System;
using System.Collections.Generic;

namespace Webapp
{
    public partial class ExperimentSensors
    {
        public int ExperimentId { get; set; }
        public int ExperimentSensorId { get; set; }
        public int SensorId { get; set; }

        public virtual Experiments Experiment { get; set; }
        public virtual Sensor Sensor { get; set; }
    }
}
