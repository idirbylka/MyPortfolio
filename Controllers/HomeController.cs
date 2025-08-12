using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Models;
using System.Net;
using System.Net.Mail;
using MyPortfolio.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace MyPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db; 
        }

        

        public async Task<IActionResult> Index()
        {
            List<Project> projects = await _db.Projects.Include(p => p.ScreenShots).ToListAsync();

            var vm = new HomeIndexViewModel()
            {
                ContactForm = new ContactFormModel(),
                Projects = projects
            };
            return View(vm);
        }
        public IActionResult AboutMe()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactFormModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            try
            {

                var smtpUser = Environment.GetEnvironmentVariable("SMTP_USER");
                var smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS");
                var smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "smtp.gmail.com";
                var smtpPort = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out var p) ? p : 587;

                var message = new MailMessage();
                message.From = new MailAddress(smtpUser);
                message.To.Add("idir.bylka@yahoo.co.uk");
                message.Subject = $"New Contact Message from {model.Name}";
                message.Body = $"Name: {model.Name}\nEmail: {model.Email}\n\nMessage:\n{model.Message}";
                message.IsBodyHtml = false;

                using (var smtp = new SmtpClient(smtpHost, smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }
                
                TempData["Success"] = "Your message has been sent!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                ModelState.AddModelError("", "Failed to send email. Please try again later.");
                return View("Index", model);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
