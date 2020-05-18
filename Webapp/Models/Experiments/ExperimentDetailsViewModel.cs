using System;
using System.ComponentModel.DataAnnotations;

namespace Webapp.Models.Experiments
{
    public class ExperimentDetailsViewModel
    {
        public int ExperimentId { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Metadata { get; set; }

        [Display(Name = "Дата создания")]
        public DateTimeOffset CreatedAt { get; set; }
    }
}
