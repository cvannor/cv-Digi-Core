using cvDigiCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cvDigiCore.ViewModels
{
    public class AboutViewModel
    {
        public Profile Profile { get; set; }

        public IList<Category> Categories { get; set; }
    }
}
