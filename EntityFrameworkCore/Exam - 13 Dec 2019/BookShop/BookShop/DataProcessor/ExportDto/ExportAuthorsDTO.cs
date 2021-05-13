namespace BookShop.DataProcessor.ExportDto
{
    public class ExportAuthorsDTO
    {
        public string AuthorName { get; set; }

        public ExportAuthorBooksDTO[] Books { get; set; }
    }
}
