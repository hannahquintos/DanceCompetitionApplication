using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DanceCompetitionApplication.Models.ViewModels
{
    public class DetailsPerformance
    {
        public PerformanceDto SelectedPerformance { get; set; }

        public IEnumerable<DancerDto> DancersInPerformance { get; set; }

        public IEnumerable<DancerDto> AvailableDancers { get; set; }
    }
}