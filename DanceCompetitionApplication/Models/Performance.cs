using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DanceCompetitionApplication.Models
{
    public class Performance
    {
        //what describes a performance?

        //primary key
        [Key]
        public int PerformanceId { get; set; }

        public DateTime PerformanceTime { get; set; }

        public string RoutineName { get; set; }

        public string Studio { get; set; }

        //a performance has a category id
        //a category has many performances
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }

    public class PerformanceDto
    {
        public int PerformanceId { get; set; }

        public DateTime PerformanceTime { get; set; }

        public string RoutineName { get; set; }

        public string Studio { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

    }
}