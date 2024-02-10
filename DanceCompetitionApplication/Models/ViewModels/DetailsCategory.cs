using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DanceCompetitionApplication.Models.ViewModels
{
    public class DetailsCategory
    {
        // viewmodel to store information needed to present to /Category/Details/{id}

        // selected category information
        public CategoryDto SelectedCategory {  get; set; }

        // all performances in the system related to the selected category
        public IEnumerable<PerformanceDto> RelatedPerformances { get; set; }
    }
}