namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportBookDTO[]), new XmlRootAttribute("Books"));

            var reader = new StringReader(xmlString);

            var xmlBooks = (ImportBookDTO[])serializer.Deserialize(reader);

            reader.Close();

            StringBuilder sb = new StringBuilder();

            List<Book> books = new List<Book>();

            foreach (var book in xmlBooks)
            {
                if (!IsValid(book) || !Enum.IsDefined(typeof(Genre), book.Genre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Book currentBook = new Book
                {
                    Name = book.Name,
                    Genre = (Genre)book.Genre,
                    Price = book.Price,
                    Pages = book.Pages,
                    PublishedOn = DateTime.ParseExact(book.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                };

                books.Add(currentBook);

                sb.AppendLine(string.Format(SuccessfullyImportedBook, currentBook.Name, currentBook.Price));
            }

            context.Books.AddRange(books);

            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var jsonAuthors = JsonConvert.DeserializeObject<ImportAuthorDTO[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var author in jsonAuthors)
            {
                if (!IsValid(author) || context.Authors.Any(x => x.Email == author.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Author currentAuthor = new Author
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Phone = author.Phone,
                    Email = author.Email
                };

                foreach (var bookId in author.Books)
                {
                    if (!context.Books.Any(x => x.Id == bookId.Id))
                    {
                        continue;
                    }

                    currentAuthor.AuthorsBooks.Add(new AuthorBook { BookId = bookId.Id.Value });
                }

                if (currentAuthor.AuthorsBooks.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                context.Add(currentAuthor);

                context.SaveChanges();

                sb.AppendLine(string.Format(SuccessfullyImportedAuthor,
                    currentAuthor.FirstName + " " + currentAuthor.LastName,
                    currentAuthor.AuthorsBooks.Count));
            }

            return sb.ToString();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}