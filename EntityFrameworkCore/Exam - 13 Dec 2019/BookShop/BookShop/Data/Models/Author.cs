using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Data.Models
{
    public class Author
    {
        public Author()
        {
            AuthorsBooks = new HashSet<AuthorBook>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string FirstName  { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$")]
        public string Phone { get; set; }


        public virtual ICollection<AuthorBook> AuthorsBooks  { get; set; }
    }
}
