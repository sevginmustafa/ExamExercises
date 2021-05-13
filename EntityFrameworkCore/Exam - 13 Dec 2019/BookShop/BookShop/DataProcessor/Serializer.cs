namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new ExportAuthorsDTO
                {
                    AuthorName = a.FirstName + " " + a.LastName,
                    Books = a.AuthorsBooks.Select(b => new ExportAuthorBooksDTO
                    {
                        BookName = b.Book.Name,
                        BookPrice = b.Book.Price.ToString("f2")
                    })
                    .OrderByDescending(b => decimal.Parse(b.BookPrice))
                    .ToArray()
                })
                .ToArray()
                .OrderByDescending(a => a.Books.Count())
                .ThenBy(a => a.AuthorName);

            var result = JsonConvert.SerializeObject(authors, Formatting.Indented);

            return result;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var books = context.Books
                .Where(b => b.PublishedOn < date && b.Genre.ToString() == "Science")
                .OrderByDescending(b => b.PublishedOn)
                .Select(b => new ExportBooksDTO
                {
                    Pages = b.Pages,
                    Name = b.Name,
                    Date = b.PublishedOn.ToString("MM/dd/yyyy")
                })
                .OrderByDescending(b => b.Pages)
                .Take(10)
                .ToArray();

            var namespaces = new XmlSerializerNamespaces();

            namespaces.Add("", "");

            var serializer = new XmlSerializer(typeof(ExportBooksDTO[]), new XmlRootAttribute("Books"));

            var writer = new StringWriter();

            serializer.Serialize(writer, books, namespaces);

            writer.Close();

            return writer.ToString();
        }
    }
}