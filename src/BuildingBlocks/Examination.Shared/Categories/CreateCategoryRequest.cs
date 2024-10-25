using System.ComponentModel.DataAnnotations;

namespace Examination.Shared.Categories
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string UrlPath { get; set; }
    }
}
