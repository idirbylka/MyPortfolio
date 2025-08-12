using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models
{
    public class CvModel
    {
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public byte[] FileContent { get; set; }

        [Required]
        public string ContentType { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}