using System;
using System.Collections.Generic;

namespace WebappDb
{
    public partial class Experiments
    {
        public Experiments()
        {
            ExperimentParams = new HashSet<ExperimentParams>();
            ExperimentSensors = new HashSet<ExperimentSensors>();
            ExperimentTags = new HashSet<ExperimentTags>();
            Tests = new HashSet<Tests>();
        }

        public int ExperimentId { get; set; }
        public string Metadata { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ExperimentParams> ExperimentParams { get; set; }
        public virtual ICollection<ExperimentSensors> ExperimentSensors { get; set; }
        public virtual ICollection<ExperimentTags> ExperimentTags { get; set; }
        public virtual ICollection<Tests> Tests { get; set; }
    }
}
