using System.ComponentModel.DataAnnotations;

namespace NewsTrack.WebApi.Dtos
{
    public class AuthenticationDto
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}