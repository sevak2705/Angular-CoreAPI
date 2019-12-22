using System.ComponentModel.DataAnnotations;

namespace CoreAPI.Dto
{
    public class UserForRegistrationDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8,MinimumLength = 4, ErrorMessage = "Password must be between 4 to 8 character")]
        public string Password { get; set; }    
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}