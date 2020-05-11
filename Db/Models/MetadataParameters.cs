using System;
using System.Collections.Generic;

namespace Webapp
{
    public partial class MetadataParameters
    {
        public MetadataParameters()
        {
            ExperimentParams = new HashSet<ExperimentParams>();
            TestParams = new HashSet<TestParams>();
        }

        public int MetadataParameterId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual ICollection<ExperimentParams> ExperimentParams { get; set; }
        public virtual ICollection<TestParams> TestParams { get; set; }
    }
}
