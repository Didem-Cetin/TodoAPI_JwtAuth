using System.ComponentModel.DataAnnotations;

namespace TodoAPI_JwtAuth.MVC.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Completed { get; set; }
    }
    public class TodoCreate
    {
        [Required]
        [StringLength(500)]
        public string Text { get; set; }
        public string Issue { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
