using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cvDigiCore.Models
{
    public class Profile
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Resume { get; set; }
        public string ProfilePicture { get; set; }
        public string Logo { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
