using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Data;
using MyPortfolio.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Ganss.Xss;

namespace MyPortfolio.Controllers
{
    public class Proj_ectsController : Controller
    {

        private readonly ApplicationDbContext _db;

        // Handles project-related actions

        public Proj_ectsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Displays all projects
        public async Task<IActionResult> Index()
        {
            List<Project> projects = await _db.Projects.Include(p => p.ScreenShots).ToListAsync();
            return View(projects);
        }

        // public async Task<IActionResult> ProjectPreview(int id)
        // {
        //     var project = await _db.Projects.Include(p => p.ScreenShots).FirstOrDefaultAsync(p => p.Id == id);
        //     if (project == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(project);
        // }


        // Displays the Add Project form
        public IActionResult AddProjects()
        {
            return View();
        }

        // Handles the form submission for adding a new project
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProjects(Project obj, List<IFormFile> screenShots)
        {
            if (!ModelState.IsValid)
                return View(obj);

            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedTags.Clear();
            sanitizer.AllowedTags.UnionWith(new[] { "p", "br", "ul", "ol", "li", "strong", "b", "em", "i", "u", "a" });
            sanitizer.AllowedAttributes.Clear();
            sanitizer.AllowedAttributes.UnionWith(new[] { "href", "target" });

            obj.Description = sanitizer.Sanitize(obj.Description ?? string.Empty);
            obj.Technologies = sanitizer.Sanitize(obj.Technologies ?? string.Empty);

            if (screenShots != null && screenShots.Count > 0)
            {
                foreach (var file in screenShots)
                {
                    if (file.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);
                        obj.ScreenShots.Add(new Screenshot
                        {
                            FileName = Path.GetFileName(file.FileName),
                            ContentType = file.ContentType,
                            FileContent = memoryStream.ToArray(),
                            Project = obj
                        });
                    }
                }
            }

            _db.Projects.Add(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        /// Displays the details of a specific project
        public async Task<IActionResult> GetScreenshot(int id)
        {
            var screenshot = await _db.Screenshots.FindAsync(id);
            if (screenshot == null)
            {
                return NotFound();
            }
            return File(screenshot.FileContent, screenshot.ContentType);
        }

        // Handles the deletion of  a project
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _db.Projects
                .Include(p => p.ScreenShots)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                TempData["Error"] = "Project not found.";
                return RedirectToAction(nameof(Index));
            }

            if (project.ScreenShots != null && project.ScreenShots.Count > 0)
            {
                _db.Screenshots.RemoveRange(project.ScreenShots);
            }

            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Project deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}