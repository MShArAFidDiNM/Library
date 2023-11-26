using System.ComponentModel.DataAnnotations;

namespace EXAM_Module_6.Models
{
    public class BooksCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
