using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EXAM_Module_6.Models
{
    public class Books
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("Book Name")]
        public string Name  { get; set; }

        [Required]     
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Required]
        [DisplayName("Author Id")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        [Required]
        [DisplayName("Category Id")]
        public int BooksCategoryId { get;set; }
        public BooksCategory BooksCategory { get; set; }

    }
}
