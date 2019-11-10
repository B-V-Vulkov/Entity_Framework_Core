namespace BookShop
{
    using Data;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                int result = RemoveBooks(db);
                Console.WriteLine(result);
            }
        }

        //P01 Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command) 
        {
            StringBuilder stringBuilder = new StringBuilder();

            var books = context.Books
                .Select(b => new
                {
                    b.Title,
                    b.AgeRestriction
                })
                .Where(b => b.AgeRestriction.ToString().ToUpper() == command.ToUpper())
                .OrderBy(b => b.Title)
                .ToList();

            foreach (var book in books)
            {
                stringBuilder.AppendLine(book.Title);
            }

            return stringBuilder.ToString().TrimEnd();
        }

        //P02 Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var books = context.Books
                .Where(b => b.Copies < 5000 && b.EditionType.ToString() == "Gold")
                .Select(b => new
                {
                    b.BookId,
                    b.Title,
                })
                .OrderBy(b => b.BookId)
                .ToList();

            foreach (var book in books)
            {
                stringBuilder.AppendLine(book.Title);
            }

            return stringBuilder.ToString().TrimEnd();
        }

        //P03. Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price,
                })
                .OrderByDescending(b => b.Price)
                .ToList();

            foreach (var book in books)
            {
                stringBuilder.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        //P04. Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year) 
        {
            StringBuilder stringBuilder = new StringBuilder();

            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {
                    b.BookId,
                    b.Title,
                })
                .OrderBy(b => b.BookId)
                .ToList();

            foreach (var book in books)
            {
                stringBuilder.AppendLine($"{book.Title}");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        //P05. Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categores = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToUpper())
                .ToList();

            StringBuilder stringBuilder = new StringBuilder();

            var books = context.Books
                .Where(b => b.BookCategories
                    .Any(bc => categores.Contains(bc.Category.Name.ToUpper())))
                .Select(b => new
                {
                    b.Title
                })
                .OrderBy(b => b.Title)
                .ToList();

            foreach (var book in books)
            {
                stringBuilder.AppendLine($"{book.Title}");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        //P06. Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var splittedDate = date.Split('-');

            int day = int.Parse(splittedDate[0]);
            int month = int.Parse(splittedDate[1]);
            int year = int.Parse(splittedDate[2]);

            var currentDate = new DateTime(year, month, day);

            var books = context.Books
                .Where(b => b.ReleaseDate < currentDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    b.ReleaseDate,
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            foreach (var book in books)
            {
                stringBuilder.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        //P07. Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}"
                })
                .OrderBy(a => a.FullName)
                .ToList();

            foreach (var author in authors)
            {
                stringBuilder.AppendLine($"{author.FullName}");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        //P08. Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var books = context.Books
                .Where(b => b.Title.ToUpper().Contains(input.ToUpper()))
                .Select(b => new
                {
                    b.Title
                })
                .OrderBy(b => b.Title)
                .ToList();

            foreach (var book in books)
            {
                stringBuilder.AppendLine($"{book.Title}");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        //P09. Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var books = context.Books
                .Where(b => b.Author.LastName.ToUpper().StartsWith(input.ToUpper()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    Author = $"{b.Author.FirstName} {b.Author.LastName}",
                })
                .ToList();

            foreach (var book in books)
            {
                stringBuilder.AppendLine($"{book.Title} ({book.Author})");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        //P10. Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var count = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();

            return count;
        }

        //P11. Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var authors = context.Authors
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}",
                    Copies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(c => c.Copies)
                .ToList();

            foreach (var author in authors)
            {
                stringBuilder.AppendLine($"{author.FullName} - {author.Copies}");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        //P12. Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var categores = context.Categories
                .Select(c => new
                {
                    c.Name,
                    Profit = c.CategoryBooks.Sum(b => b.Book.Copies * b.Book.Price)
                })
                .OrderByDescending(p => p.Profit)
                .ToList();

            foreach (var categoy in categores)
            {
                stringBuilder.AppendLine($"{categoy.Name} ${categoy.Profit}");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        //P13. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var categores = context.Categories
                .Select(c => new
                {
                    c.Name,
                    Books = c.CategoryBooks
                    .OrderByDescending(b => b.Book.ReleaseDate)
                    .Take(3)
                    .Select(b => new
                    {
                        b.Book.Title,
                        b.Book.ReleaseDate.Value.Year,
                    })
                })
                .OrderBy(c => c.Name)
                .ToList();


            foreach (var categoy in categores)
            {
                stringBuilder.AppendLine($"--{categoy.Name}");

                foreach (var book in categoy.Books)
                {
                    stringBuilder.AppendLine($"{book.Title} ({book.Year})");
                }
            }

            return stringBuilder.ToString().TrimEnd();
        }

        //P14. Increase Prices
        public static void IncreasePrices(BookShopContext context) 
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        //P15. Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            foreach (var book in books)
            {
                context.Books.Remove(book);
            }

            context.SaveChanges();

            return books.Count();
        }
    }
}
