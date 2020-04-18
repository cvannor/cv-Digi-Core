using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cvDigiCore.Models
{
    public class Bridge
    {
        public int ProjectId { get; set; }
        public int? ParentId { get; set; }
        public int CategoryId { get; set; }

        public virtual Project Project { get; set; }
        public virtual Category Category { get; set; }

    }
}
