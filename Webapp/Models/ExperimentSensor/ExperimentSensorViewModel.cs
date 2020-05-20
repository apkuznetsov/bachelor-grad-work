using System.ComponentModel.DataAnnotations;

namespace Webapp.Models.ExperimentSensor
{
    public class ExperimentSensorViewModel
    {
        public int ExperimentSensorId { get; set; }

        public int ExperimentId { get; set; }

        public int SensorId { get; set; }

        [Display(Name = "Датчик")]
        public string SensorName { get; set; }
    }
}
