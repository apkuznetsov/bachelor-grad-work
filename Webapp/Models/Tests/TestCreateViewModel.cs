using System.ComponentModel.DataAnnotations;

namespace Webapp.Models.Tests
{
    public class TestCreateViewModel
    {
        public int TestId { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Metadata { get; set; }

        public int ExperimentId { get; set; }

        [Display(Name = "Эксперимент")]
        public string ExperimentName { get; set; }
    }
}
