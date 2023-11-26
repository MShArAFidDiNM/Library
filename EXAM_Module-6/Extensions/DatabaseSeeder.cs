using Bogus;
using Bogus.DataSets;
using EXAM_Module_6.Data;
using EXAM_Module_6.Models;
using Microsoft.EntityFrameworkCore;

namespace EXAM_Module_6.Extensoions
{
    public static class DatabaseSeeder
    {
        static Faker _faker = new Faker();

        public static void SeedDatabase(this IServiceCollection _, IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<BookshopDbContext>>();
            using var context = new BookshopDbContext(options);

            CreateCategories(context);
            CreateAuthors(context);
            CreateBooks(context);


            context.SaveChanges();
           
        }

        private static void CreateCategories(BookshopDbContext context)
        {
            if (context.BooksCategories.Any()) return;
            List<BooksCategory> categories = new();

            for (int i = 0; i < 10; i++)
            {
                categories.Add(new BooksCategory()
                {
                    Name =_faker.Commerce.Department(),

                }) ;
            }
            context.AddRange(categories);
            context.SaveChanges();
        }
        private static void CreateAuthors(BookshopDbContext context)
        {
            if (context.Authors.Any()) return;
            List<Author> authors = new();
            for (int i = 0; i < 10; i++)
            {
                authors.Add(new Author()
                {
                    FullName = _faker.Name.FullName(),
                    Birthdate = _faker.Date.Between(DateTime.Now.AddYears(-200), DateTime.Now.AddYears(-50))
                });
            }
            context.AddRange(authors);
            context.SaveChanges();
        }
        private static void CreateBooks(BookshopDbContext context)
        {
            if (context.Books.Any()) return;
            List<Books> books = new();
            var authors = context.Authors.ToList();
            var categorys = context.BooksCategories.ToList();
            for (int i = 0; i < 50; i++)
            {
                var randomAuthor = _faker.PickRandom(authors);
                var randomCategorys = _faker.PickRandom(categorys);
                books.Add(new Books()
                {
                    Name = _faker.Name.JobTitle(),
                    BooksCategoryId = randomCategorys.Id,
                    Description = _faker.Name.JobDescriptor(),
                    Price = _faker.Random.Decimal(10000, 1000000),
                    AuthorId = randomAuthor.Id,
                });
            }
            context.Books.AddRange(books);
            context.SaveChanges();

        }
    }
}
