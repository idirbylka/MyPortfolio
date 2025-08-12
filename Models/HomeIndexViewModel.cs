
using System.Collections.Generic;

namespace MyPortfolio.Models
{
    public class HomeIndexViewModel
    {
        public ContactFormModel ContactForm { get; set; } = new ContactFormModel();

        public List<Project> Projects { get; set; } = new List<Project>();
    }
}
