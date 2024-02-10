using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DanceCompetitionApplication.Models.ViewModels
{
    public class UpdatePerformance
    {
        // viewmodel to store information needed to present to /Performance/Update/{id}

        // existing performance information
        public PerformanceDto SelectedPerformance { get; set; }

        // all categories in the system to choose from when updating a performance
        public IEnumerable<CategoryDto> CategoryOptions { get; set; }
    }
}