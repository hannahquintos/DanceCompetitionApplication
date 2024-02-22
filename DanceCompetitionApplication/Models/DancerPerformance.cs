using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DanceCompetitionApplication.Models
{
    public class DancerPerformance
    {
        //what describes a dancer performance?

        //bridging table - explicit technique to describe many to many relationship between dancers and performances

        //primary key
        [Key]
        public int DancerPerformanceId { get; set; }

        //foreign key to dancer entity
        // 1 to many relationship
        [ForeignKey("Dancer")]
        public int? DancerId { get; set; }
        public virtual Dancer Dancer { get; set; }

        //foreign key to performance entity
        // 1 to many relationship
        [ForeignKey("Performance")]
        public int? PerformanceId { get; set; }
        public virtual Performance Performance { get; set; }
    }
}