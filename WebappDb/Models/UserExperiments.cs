using System;
using System.Collections.Generic;

namespace WebappDb
{
    public partial class UserExperiments
    {
        public int UserExperimentsId { get; set; }
        public int UserId { get; set; }
        public int ExperimentId { get; set; }
    }
}
