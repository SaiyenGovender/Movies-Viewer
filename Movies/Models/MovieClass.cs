using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Movies.Models
{
    public class MovieClass
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Category { get; set; }

        public string Rating { get; set; }
    }

    
}
