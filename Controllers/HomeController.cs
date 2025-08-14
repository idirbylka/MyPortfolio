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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage([Bind(Prefix = "ContactForm")] ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
                var projectsInvalid = await _db.Projects.Include(p => p.ScreenShots).ToListAsync();
                var vmInvalid = new HomeIndexViewModel { ContactForm = model, Projects = projectsInvalid };
                return View("Index", vmInvalid);
            }

            // Read SMTP settings from environment (Render dashboard -> Environment)
            var smtpUser = Environment.GetEnvironmentVariable("SMTP_USER");
            var smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS");
            var smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "smtp.gmail.com";
            var smtpPort = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out var port) ? port : 587;
            var toEmail  = Environment.GetEnvironmentVariable("CONTACT_TO") ?? "idir.bylka@yahoo.co.uk";

            // Guard: make sure required env vars exist in Render
            if (string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPass))
            {
                ModelState.AddModelError("", "Email service is not configured. Please try again later.");
                _logger.LogError("Missing SMTP credentials in environment variables.");
                var projectsMissing = await _db.Projects.Include(p => p.ScreenShots).ToListAsync();
                var vmMissing = new HomeIndexViewModel { ContactForm = model, Projects = projectsMissing };
                return View("Index", vmMissing);
            }

            try
            {
                using var message = new MailMessage
                {
                    From = new MailAddress(smtpUser),
                    Subject = $"New Contact Message from {model.Name}",
                    Body = $"Name: {model.Name}\nEmail: {model.Email}\n\nMessage:\n{model.Message}",
                    IsBodyHtml = false
                };
                message.To.Add(toEmail);
                // Optional: add Reply-To so you can reply directly to the sender
                if (!string.IsNullOrWhiteSpace(model.Email))
                    message.ReplyToList.Add(new MailAddress(model.Email, model.Name ?? model.Email));

                using var smtp = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Timeout = 10000
                };

                await smtp.SendMailAsync(message);

                TempData["Success"] = "Your message has been sent!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email via SMTP host {Host}:{Port}", smtpHost, smtpPort);
                ModelState.AddModelError("", "Failed to send email. Please try again later.");

                var projects = await _db.Projects.Include(p => p.ScreenShots).ToListAsync();
                var vm = new HomeIndexViewModel { ContactForm = model, Projects = projects };
                return View("Index", vm);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
