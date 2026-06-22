using System.ComponentModel.DataAnnotations;

namespace Library_Management.DTO
{
    public class BookUpdateDTO
    {
        [Required(ErrorMessage = "Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Year is required.")]
        [Range(1000, 2100, ErrorMessage = "Year must be between 1000 and 2100.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "PageCount is required.")]
        [Range(1, 10000, ErrorMessage = "PageCount must be between 1 and 10000.")]
        public int PageCount { get; set; }

        [Required(ErrorMessage = "AuthorId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "AuthorId must be a positive integer.")]
        public int AuthorId { get; set; }
    }
}
