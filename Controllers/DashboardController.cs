using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using cvDigiCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using cvDigiCore.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace cvDigiCore.Controllers
{
    [Authorize]
  
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;


        public DashboardController(ApplicationDbContext db, IHostingEnvironment hosting, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _db = db;
            this.hostingEnvironment = hosting;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [BindProperty]
        public Profile ProfileEntity { get; set; }

        [BindProperty]
        public Project ProjectEntity { get; set; }

        [BindProperty]
        public Category CategoryEntity { get; set; }

        public IEnumerable<Project> Projects { get; set; }

        
        public IActionResult Index()
        {
            
            return View();
            
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult>GetCategories(int? id)
        {

            IList<Bridge> bridgesAll = _db.Bridges.Include(i => i.Category).ToList();

            var parents = Enumerable.Empty<int?>().ToList();
            var categories = await _db.Category.ToListAsync();
            
            if (bridgesAll != null)
            {
                foreach (var b in bridgesAll)
                {
                    //if(b.ProjectId == id && (b.ParentId != null && b.ParentId != 0))
                    //{
                    //    categories.Add(b.Category);
                    //}
                        
                    if (!parents.Contains(b.ParentId) && (b.ParentId != null || b.ParentId != 0))
                    {
                        parents.Add(b.ParentId);
                    }
                }
            }
            


                   
            Dictionary<int, object> values = new Dictionary<int, object>();

            foreach (var c in categories)
            {
                var obj = new
                {
                    name = c.Name,
                    parent = c.ParentId
                };
               
                values.Add(c.ID, obj);


            };

            string json = JsonConvert.SerializeObject(values);

            if (categories == null)
            {
                //return NotFound();
                return Json(new { success = false, msg = "Cat get failed, category does not exist" });
            }


            return Json(new
            {
                success = true,
                categories = json,
                p = parents
            });

        }


        [HttpDelete]
        public async Task<IActionResult>DeleteCategory(int id)
        {
            var cat = await _db.Category.FirstOrDefaultAsync(u => u.ID == id);
            var bridges = await _db.Bridges.Include(u=> u.Category).Where(u => u.CategoryId == id || u.ParentId == id).ToListAsync();
            if (cat == null)
            {
                //return NotFound();
                return Json(new { success = false, msg = "Cat delete failed, category does not exist" });
            }

            foreach(var b in bridges)
            {
                if(b.ParentId == id)
                {
                    b.ParentId = null;
                    b.Category.ParentId = null;
                    _db.Category.Update(b.Category);
                    _db.Bridges.Update(b);
                }
                else
                {
                    _db.Bridges.Remove(b);

                }
            }

            _db.Category.Remove(cat);
            await _db.SaveChangesAsync();
            return Json(new { success = true, msg = "Category delete success" });


        }

        [HttpDelete]
        public async Task<IActionResult>DeleteImage(int id, int imgCount)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            var image = await _db.Images.FirstOrDefaultAsync(u => u.ID == id);
            if (image == null)
            {
                //return NotFound();
                return Json(new { success = false, msg = "Image delete failed" });
            }


            IList<Images> currentImages = _db.Images.Where(i => i.ProjectID == image.ProjectID).ToList();

            if (imgCount >= currentImages.Count())
            {
                return Json(new { success = false, msg = "Please add an image before deleting this one!"});

            }

            //string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "fileDirectory/Projects/Images/" + image.Path);
            //if (System.IO.File.Exists(uploadsFolder))
            //{
            //    proc.WaitForExit();
            //    System.IO.File.Delete(uploadsFolder);
            //}

            _db.Images.Remove(image);
            await _db.SaveChangesAsync();
            return Json(new { success = true, msg = "Image delete success" });
        }

        public async Task<IActionResult>DeleteProject(int id)
        {
            var project = await _db.Project.FirstOrDefaultAsync(u => u.ID == id);
            if(project == null)
            {
                //return NotFound();
                return Json(new { success = false, msg = "Project delete failed" });
            }

            //IList<Images> currentImages = _db.Images.Where(i => i.ProjectID == id).ToList();

            //string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "fileDirectory/Projects/Images");

            //foreach (var img in currentImages)
            //{
            //    string fullPath = Path.Combine(uploadsFolder + "/" + img.Path);
            //    if (System.IO.File.Exists(fullPath))
            //    {
            //        System.IO.File.Delete(fullPath);
            //    }

            //}
            IList<Bridge> bridges = await _db.Bridges.Where(i => i.ProjectId == id).ToListAsync();
            foreach(var b in bridges)
            {
                var cat = _db.Category.Find(b.CategoryId);
                cat.ParentId = null;
                _db.Category.Update(cat);
                _db.Bridges.Remove(b);
            }
            _db.Project.Remove(project);
            await _db.SaveChangesAsync();
            return Json(new { success = true, msg = "Project delete success" });


        }

        [HttpPost]
        public IActionResult AddCategory(string name)
        {
            if(name != null)
            {
                Category newCat = new Category { Name = name };
                _db.Category.Add(newCat);
                _db.SaveChanges();


                return Json(new
                {
                    success = true,
                    msg = "Category Added!",
                    id = newCat.ID
                });

            }
            else
            {
                return Json(new
                {
                    success = false,
                    msg = "No valid name!",
                }); 

            }

        }

    


        bool isEqual(IList<Bridge> bridges, Bridge b, int id)
        {
            foreach (var bridge in bridges)
            {
                if (bridge.Category.ID == b.Category.ID)
                {
                    return true;
                }
            }
            return false;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Project(IList<IFormFile> Images, IList<Bridge> pBridges)
        {
            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "fileDirectory/Projects/Images");
            IList<Images> images = new List<Images>();
            IList<Images> currentImages = _db.Images.Where(i => i.ProjectID == ProjectEntity.ID).ToList();
            IList<Bridge> currentBridges = _db.Bridges.Include(i => i.Category).Where(i => i.ProjectId == ProjectEntity.ID).ToList();
            IList<Category> barray = new List<Category>();
            Dictionary<int,Bridge> bridgeDict = new Dictionary<int,Bridge>();
            Dictionary<int, Bridge> currrentbridgeDict = new Dictionary<int, Bridge>();

            foreach (var b in pBridges.ToList())
            {
                if(b == null || b.Category.ID == 0)
                {
                    pBridges.Remove(b);
                }
                else
                {
                    if (b.Category.ParentId != null)
                    {
                        b.ParentId = b.Category.ParentId;
                    }
                    bridgeDict.Add(b.Category.ID, b);
                }
                

            }
            if (ProjectEntity.ID == 0)
            {
                foreach (IFormFile image in Images)
                {
                    if(image != null)
                    {
                        string fileName = image.FileName;
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        if (!System.IO.File.Exists(filePath))
                        {
                            image.CopyTo(new FileStream(filePath, FileMode.Create));

                        }
                        Images newImage = new Images
                        {
                            Path = fileName,
                            Project = ProjectEntity,
                        };
                        images.Add(newImage);
                    }
                }
                ProjectEntity.Images = images;
                ProjectEntity.DateAdded = DateTime.Now;

                _db.Project.Add(ProjectEntity);
                _db.SaveChanges();

                ProjectEntity.Bridges = new List<Bridge>();
                foreach (var b in pBridges)
                {
                    b.Project = ProjectEntity;
                    var current = _db.Category.Include(e => e.Bridges).Single(e => e.ID == b.Category.ID);
                    current.Bridges.Add(b);
                    current.ParentId = b.Category.ParentId;
                    _db.Category.Update(current);
                    ProjectEntity.Bridges.Add(b);

                    
              
                }  
                ViewBag.isNew = true;
            }
            else
            {
                
                HashSet<string> imagePaths = new HashSet<string>();
                foreach(var img in currentImages)
                {
                    imagePaths.Add(img.Path);
                }

                if(currentBridges.Count() > 0)
                {
                    foreach (var b in currentBridges)
                    {
                        if (bridgeDict.ContainsKey(b.Category.ID) == false)
                        {
                            var remove = b.Category.Bridges.Where(r=> r.CategoryId == b.Category.ID && r.ProjectId == ProjectEntity.ID).Single();
                            var catBridge = _db.Bridges.Include(i => i.Category).Where(i => i.CategoryId == b.Category.ID).ToList();

                            if (catBridge.Count() <= 1)
                            {
                                b.Category.ParentId = null;
                                _db.Category.Update(b.Category);

                            }
                            _db.Bridges.Remove(remove);
                        }
                        else
                        {
                            currrentbridgeDict.Add(b.Category.ID, b);
                        }
                    }

                    foreach (var b in pBridges)
                    {
                        if(currrentbridgeDict.ContainsKey(b.Category.ID) == false)
                        {
                            b.Project = ProjectEntity;
                            b.ParentId = b.Category.ParentId;
                            var current = _db.Category.Include(e => e.Bridges).Single(e => e.ID == b.Category.ID);
                            current.Bridges.Add(b);
                            current.ParentId = b.Category.ParentId;
                            _db.Category.Update(current);

                            _db.SaveChanges();
                        }
                        else
                        {
                            var current = _db.Bridges.Include(e => e.Category).Single(e => e.Category.ID == b.Category.ID && e.ProjectId == ProjectEntity.ID);
                            current.Category.ParentId = b.Category.ParentId;
                            current.ParentId = b.Category.ParentId;
                            _db.Bridges.Update(current);

                        }
                    }

                }
                else
                {
                    ProjectEntity.Bridges = new List<Bridge>();
                    foreach (var b in pBridges)
                    {

                        b.Project = ProjectEntity;
                        var current = _db.Category.Include(e => e.Bridges).Single(e => e.ID == b.Category.ID);
                        current.Bridges.Add(b);
                        current.ParentId = b.Category.ParentId;
                        _db.Category.Update(current);
                        ProjectEntity.Bridges.Add(b);

                        _db.SaveChanges();

                    }

                }

                _db.SaveChanges();

                foreach (IFormFile image in Images)
                {
                    if (image != null && !imagePaths.Contains(image.FileName))
                    {
                        string fileName = image.FileName;
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        if (!System.IO.File.Exists(filePath))
                        {
                            image.CopyTo(new FileStream(filePath, FileMode.Create));

                        }
                        Images newImage = new Images
                        {
                            Path = fileName,
                            Project = ProjectEntity,
                        };
                        _db.Images.Add(newImage);
                    }
                }
                
                _db.Project.Update(ProjectEntity);

                ViewBag.isNew = false;

            }

            _db.SaveChanges();

            return RedirectToAction("Project", new { id = ProjectEntity.ID });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(IFormFile Resume, IFormFile Logo, IFormFile ProfilePicture,  string ResumePrev, string LogoPrev, string ProfilePicPrev)
        {
            List <IFormFile> files = new List<IFormFile>
            {
                Resume,
                Logo,
                ProfilePicture
            };
            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "fileDirectory/Profile");
            string[] paths = new string[3];
            
            if(ProfileEntity.ID == 0)
            {
                int i = 0;
                foreach(IFormFile file in files)
                {
                    if(file != null)
                    {
                        string uniqueFileName = "123-" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        file.CopyTo(new FileStream(filePath, FileMode.Create));
                        paths[i] = file.FileName;
                        i++;

                    }
                }

                ProfileEntity.Resume = paths[0];
                ProfileEntity.Logo = paths[1];
                ProfileEntity.ProfilePicture = paths[2];
                _db.Profile.Add(ProfileEntity);
            }
            else
            {
                if(Resume != null)
                {
                    if (Resume.FileName != ResumePrev && Resume.FileName != null)
                    {
                        ProfileEntity.Resume = Resume.FileName;
                        string webRootPath = hostingEnvironment.WebRootPath;
                        var fullPath = webRootPath + "/fileDirectory/" + "123-" + ResumePrev;
                        /*if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }*/
                        string uniqueFileName = "123-" + Resume.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        Resume.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }
                else
                {
                    ProfileEntity.Resume = ResumePrev;
                }
                

                if(Logo != null)
                {
                    if (Logo.FileName != LogoPrev && Logo.FileName != null)
                    {
                        ProfileEntity.Logo = Logo.FileName;
                        string webRootPath = hostingEnvironment.WebRootPath;
                        var fullPath = webRootPath + "/fileDirectory/"  + "123-" + LogoPrev;
                        /*if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }*/
                        string uniqueFileName = "123-" + Logo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        Logo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }
                else
                {
                    ProfileEntity.Logo = LogoPrev;
                }
                
                if(ProfilePicture != null)
                {
                    if (ProfilePicture.FileName != ProfilePicPrev && ProfilePicture.FileName != null)
                    {
                        ProfileEntity.ProfilePicture = ProfilePicPrev;
                        string webRootPath = hostingEnvironment.WebRootPath;
                        var fullPath = webRootPath + "/fileDirectory/" + "123-" + ProfilePicPrev;
                        /*if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }*/
                        string uniqueFileName = "123-" + ProfilePicture.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        ProfilePicture.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }
                else
                {
                    ProfileEntity.ProfilePicture = ProfilePicPrev;
                }

                _db.Profile.Update(ProfileEntity);
            }
            _db.SaveChanges();
            ViewBag.isNew = false;
            return View(ProfileEntity);
        }

        
        public async Task<IActionResult> ProfileAsync()
        {
            var Profile = await _db.Profile.FirstOrDefaultAsync();
            if(Profile == null)
            {
                ViewBag.isNew = true;
                return View();
            }
            else
            {
                ViewBag.isNew = false;
                return View(Profile);
            }            
            
        }

        public async Task<IActionResult> ProjectAsync(int? id)
        {
            ViewBag.Categories = await _db.Category.ToListAsync();
            Dictionary<int, Category> values = new Dictionary<int, Category>();

      
            var parents = Enumerable.Empty<int?>().ToList();
            if (id == null || id == 0)
            {
                ViewBag.isNew = true;
                return View();

            }
            else
            {
                IList<Bridge> bridgesAll = _db.Bridges.Include(i => i.Category).ToList();
                var bridges = Enumerable.Empty<Bridge>().ToList();
                var children = Enumerable.Empty<int>().ToList();
                if (bridges != null)
                {
                    foreach (var b in bridgesAll)
                    {
                        if(b.ProjectId == id)
                        {
                            if (b.ParentId == null)
                            {
                                parents.Add(b.CategoryId);

                            }
                            else
                            {
                                children.Add(b.CategoryId);

                            }
                            bridges.Add(b);

                        }
                        if(!parents.Contains(b.ParentId) && (b.ParentId != null && b.ParentId != 0))
                        {
                            parents.Add(b.ParentId);
                        }                        
                    }
                }
            
                var Project = await _db.Project.FindAsync(id);
                
                if(bridges != null && bridges.Count() > 0)
                {
                    Project.Bridges = bridges;

                }
                Project.Parents = parents;
                ViewBag.Children = children;
                
                IList<Images> currentImages = _db.Images.Where(i => i.ProjectID == id).ToList();
                if (currentImages.Count() > 0)
                {
                    Project.Images = currentImages;

                }
                ViewBag.isNew = false;
                return View(Project);

            }
            
        }

        public async Task<IActionResult> ProjectsAsync()
        {
            Projects = await _db.Project.ToListAsync();

            return View(Projects);
        }

  
        public async Task<IActionResult> CategoriesAsync()
        {
            var categories = await _db.Category.ToListAsync();

       
             return View(categories);

            

        }

    }
}