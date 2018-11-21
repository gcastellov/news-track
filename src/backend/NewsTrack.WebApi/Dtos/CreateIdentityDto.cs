using System.ComponentModel.DataAnnotations;

namespace NewsTrack.WebApi.Dtos
{
    public class CreateIdentityDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}