using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class ContactController : Controller
{

    public IActionResult ContactMe()
{
    return View("~/Views/Home/ContactMe.cshtml", new ContactFormModel());
}


    [HttpGet]
    public IActionResult Index()
    {
        return View(new ContactFormModel());
    }

    [HttpPost]
    public async Task<IActionResult> Send(ContactFormModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", model);

        try
        {
            var message = new MailMessage();
            message.From = new MailAddress("scanthis.app@gmail.com");
            message.To.Add("idir.bylka@yahoo.co.uk");
            message.Subject = $"New Contact Message from {model.Name}";
            message.Body = $"Name: {model.Name}\nEmail: {model.Email}\n\nMessage:\n{model.Message}";
            message.IsBodyHtml = false;

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("scanthis.app@gmail.com", "spmb xfwr evjz wxmx");
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }

            TempData["Success"] = "Your message has been sent!";
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Failed to send email. Please try again later.");
            return View("~/Views/Home/ContactMe.cshtml", model);
        }
    }
}
