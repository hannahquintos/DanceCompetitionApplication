using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DanceCompetitionApplication.Models.ViewModels
{
    public class DetailsDancer
    {
        public DancerDto SelectedDancer { get; set; } 
        public IEnumerable<PerformanceDto> PerformancesForDancer { get; set; }
    }
}