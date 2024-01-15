using System.ComponentModel.DataAnnotations;

namespace AwareTest.Model.Model
{
    public class AuthenticateRequestModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
