using System.ComponentModel.DataAnnotations;

namespace WEB2Project.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 6)]
        public string Password { get; set; }
    }
}