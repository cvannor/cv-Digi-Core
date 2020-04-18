using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cvDigiCore.Models
{
    public class Category
    {

        public Category()
        {
          
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public int? ParentId { get; set; }
        public virtual IList<Bridge> Bridges { get; set; }
    }
    
}
