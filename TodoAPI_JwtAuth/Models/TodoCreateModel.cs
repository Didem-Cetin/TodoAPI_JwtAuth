using System.ComponentModel.DataAnnotations;

namespace TodoAPI_JwtAuth.Models
{
    public class TodoCreateModel
    {
        [StringLength(500)]
        public string Text { get; set; }
        [StringLength(150)]
        public string Issue { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
