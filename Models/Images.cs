using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cvDigiCore.Models
{
    public class Images
    {

        [Key]
        public int ID { get; set; }

        public string Path { get; set; }

        public int ProjectID { get; set; }
        public Project Project { get; set; }
    }
}
