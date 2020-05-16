using System;
using System.Collections.Generic;

namespace WebappDb
{
    public partial class Tests
    {
        public Tests()
        {
            ProcessingTests = new HashSet<ProcessingTests>();
            TestParams = new HashSet<TestParams>();
            TestStorageFiles = new HashSet<TestStorageFiles>();
        }

        public int TestId { get; set; }
        public string Metadata { get; set; }
        public int ExperimentId { get; set; }
        public DateTime StartedTime { get; set; }
        public DateTime? EndedTime { get; set; }
        public string Name { get; set; }

        public virtual Experiments Experiment { get; set; }
        public virtual ICollection<ProcessingTests> ProcessingTests { get; set; }
        public virtual ICollection<TestParams> TestParams { get; set; }
        public virtual ICollection<TestStorageFiles> TestStorageFiles { get; set; }
    }
}
