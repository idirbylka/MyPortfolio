using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPortfolio.Models
{
    public class Screenshot
    {
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public byte[] FileContent { get; set; }

        [Required]
        public string ContentType { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
    }
}