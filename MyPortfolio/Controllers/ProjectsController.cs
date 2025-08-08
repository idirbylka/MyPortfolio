using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Data;
using MyPortfolio.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MyPortfolio.Controllers
{
    public class ProjectsController : Controller
    {

        private readonly ApplicationDbContext _db;

        public ProjectsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Project> projects = await _db.Projects.Include(p => p.ScreenShots).ToListAsync();
            return View(projects);
        }

        public async Task<IActionResult> ProjectPreview(int id)
        {
            var project = await _db.Projects.Include(p => p.ScreenShots).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        public IActionResult AddProjects()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProjects(Project obj, List<IFormFile> screenShots)
        {
            if (ModelState.IsValid)
            {
                if (screenShots != null && screenShots.Count > 0)
                {
                    foreach (var file in screenShots)
                    {
                        if (file.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await file.CopyToAsync(memoryStream);
                                var screenshot = new Screenshot
                                {
                                    FileName = Path.GetFileName(file.FileName),
                                    ContentType = file.ContentType,
                                    FileContent = memoryStream.ToArray(),
                                    Project = obj
                                };
                                obj.ScreenShots.Add(screenshot);
                            }
                        }
                    }
                }

                _db.Projects.Add(obj);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public async Task<IActionResult> GetScreenshot(int id)
        {
            var screenshot = await _db.Screenshots.FindAsync(id);
            if (screenshot == null)
            {
                return NotFound();
            }
            return File(screenshot.FileContent, screenshot.ContentType);
        }
    }
}