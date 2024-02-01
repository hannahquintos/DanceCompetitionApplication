using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionApplication.Models
{
    public class Category
    {
        //what describes a category?

        //primary key
        [Key]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        //a category has many performances
        public ICollection<Performance> Performances { get; set; }
    }
}