using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cvDigiCore.Models
{
    public class Project
    {

        public Project()
        {
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public string Description { get; set; }
        public IList<Images> Images { get; set; }

        [NotMapped]
        public IList<int?> Parents { get; set; }
        public virtual IList<Bridge> Bridges { get; set; }




    }
}
