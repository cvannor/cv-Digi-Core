using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using cvDigiCore.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using cvDigiCore.ViewModels;

namespace cvDigiCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db )
        {
            _logger = logger;
            _db = db;
        }


        [HttpGet]
        public IActionResult GetAllProjectsAndCategories()
        {
            //Not working for case where multiple projects have the same category!!!


            var bridges = _db.Bridges.Include(b => b.Category).Include(b => b.Project).ToList();
            //var cats = _db.Category.Include(c => c.Bridges).ThenInclude(b => b.)
            var projects = _db.Project.Include(b => b.Images).ToList();

            Dictionary<int, object> values = new Dictionary<int, object>();
            List<object> projectList = new List<object>();
            HashSet<int> idSet = new HashSet<int>();
            List<object> images = new List<object>();


            foreach(var p in projects)
            {
                if (!idSet.Contains(p.ID))
                {
                    projectList.Add(new
                    {
                        name = p.Name,
                        id = p.ID,
                        date = p.DateAdded,
                        img = p.Images[0].Path
                    });

                    //foreach (var img in p.Images)
                    //{
                    //    images.Add(new
                    //    {
                    //        id = p.ID,
                    //        path = img.Path
                    //    });
                    //}

                }
                idSet.Add(p.ID);

            }

            foreach (var b in bridges)
            {
                var val2 = new
                {
                    name = b.Category.Name,
                    parent = b.Category.ParentId,
                    project = b.ProjectId
                };
                if (!values.ContainsKey(b.CategoryId) || b.ParentId != null)
                {
                    values.Add(b.Category.ID, val2);

                };


            };



            string json = JsonConvert.SerializeObject(values);


            return Json(new
            {
                success = true,
                catlength = values.Count(),
                projects = projectList,
                categories = json,
            });
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Portfolio()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Resume()
        {
            return View();
        }

        public IActionResult About()
        {
            var profile = _db.Profile.FirstOrDefault();
            IList<Category> cats = _db.Category.ToList();
            var model = new AboutViewModel
            {
                Profile = profile,
                Categories = cats

            };
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
