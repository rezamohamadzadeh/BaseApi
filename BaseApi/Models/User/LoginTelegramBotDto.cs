
using System.ComponentModel.DataAnnotations;

namespace BaseApi.Models.User
{
    public class LoginTelegramBotDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string UserType { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
