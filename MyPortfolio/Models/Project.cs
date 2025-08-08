using System.ComponentModel.DataAnnotations;


namespace MyPortfolio.Models
    
{
    public class Project
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Project title is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage ="Title must be between 3 and 10 Charachters!")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Project description is required.")]
        public string Description { get; set; } = string.Empty;
        public string? Technologies { get; set; }
        [Required(ErrorMessage = "Project image is required.")]
        public ICollection<Screenshot> ScreenShots { get; set; } = new List<Screenshot>();
        public string? LiveUrl { get; set; }
        public string? RepoUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? Category { get; set; }
        public int? Order { get; set; }
        public string? Tags { get; set; }
    }
}
