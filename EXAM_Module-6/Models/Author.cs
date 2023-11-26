using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EXAM_Module_6.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [Required]
        [DisplayName("Birthdate")]
        public DateTime Birthdate { get; set; }
        public virtual ICollection<Books> Books { get; set; }
    }
}
