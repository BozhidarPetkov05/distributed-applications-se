using CarSales.Data.DataConstraints;
using CarSales.Data.ErrorMessages;
using System.ComponentModel.DataAnnotations;

namespace CarSales.Contracts.DTOs.Request
{
    public class AuthTokenRequest
    {
        [Required(ErrorMessage = UserErrorMessages.UsernameRequired)]
        [MinLength(UserConstraints.MinUsernameLength, ErrorMessage = UserErrorMessages.MinUsernameLength)]
        [MaxLength(UserConstraints.MaxUsernameLength, ErrorMessage = UserErrorMessages.MaxUsernameLength)]
        public required string Username { get; set; }

        [Required(ErrorMessage = UserErrorMessages.PasswordRequired)]
        [MinLength(UserConstraints.MinPasswordLength, ErrorMessage = UserErrorMessages.MinPasswordLength)]
        [MaxLength(UserConstraints.MaxPasswordLength, ErrorMessage = UserErrorMessages.MaxPasswordLength)]
        public required string Password { get; set; }
    }
}
