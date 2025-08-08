using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Data;
using MyPortfolio.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MyPortfolio.Controllers
{
    public class CvController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CvController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Cv/
        public async Task<IActionResult> Index()
        {
            var cv = await _context.Cvs.OrderByDescending(c => c.UploadedAt).FirstOrDefaultAsync();
            return View(cv);
        }

        // GET: /Cv/Upload
        public IActionResult Upload()
        {
            return View();
        }

        // POST: /Cv/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a PDF file to upload.");
                return View();
            }

            if (file.ContentType != "application/pdf")
            {
                ModelState.AddModelError("file", "Only PDF files are allowed.");
                return View();
            }

            var cv = new CvModel
            {
                FileName = Path.GetFileName(file.FileName),
                ContentType = file.ContentType
            };

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                cv.FileContent = memoryStream.ToArray();
            }

            _context.Cvs.Add(cv);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Cv/ViewPdf/{id}
        public async Task<IActionResult> ViewPdf(int id)
        {
            var cv = await _context.Cvs.FindAsync(id);
            if (cv == null)
            {
                return NotFound();
            }

            return File(cv.FileContent, cv.ContentType);
        }

        // GET: /Cv/Download/{id}
        public async Task<IActionResult> Download(int id)
        {
            var cv = await _context.Cvs.FindAsync(id);
            if (cv == null)
            {
                return NotFound();
            }

            return File(cv.FileContent, cv.ContentType, cv.FileName);
        }
    }
}