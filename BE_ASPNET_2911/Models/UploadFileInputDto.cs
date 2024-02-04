using System.ComponentModel.DataAnnotations;

namespace BE_ASPNET_2911.Models
{
    public class UploadFileInputDto
    {
        [Required]
        public IFormFile? File { get; set; }
    }
}
