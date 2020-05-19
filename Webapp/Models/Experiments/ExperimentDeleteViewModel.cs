﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Webapp.Models.Experiments
{
    public class ExperimentDeleteViewModel
    {
        public int ExperimentId { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Metadata { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; }
    }
}