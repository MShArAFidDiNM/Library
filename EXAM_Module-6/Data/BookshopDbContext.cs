using EXAM_Module_6.Models;
using Microsoft.EntityFrameworkCore;
namespace EXAM_Module_6.Data
{
    public class BookshopDbContext : DbContext
    {
        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<BooksCategory> BooksCategories { get; set; }

        public BookshopDbContext(DbContextOptions<BookshopDbContext> options) :
            base(options)
        {
            Database.Migrate();
        }
    }
}
