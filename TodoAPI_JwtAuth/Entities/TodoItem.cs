using System.ComponentModel.DataAnnotations;

namespace TodoAPI_JwtAuth.Entities
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }
        [StringLength(500)]
        public string Text { get; set; }
        [StringLength(150)]
        public string Issue { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Completed { get; set; }
    }
}
