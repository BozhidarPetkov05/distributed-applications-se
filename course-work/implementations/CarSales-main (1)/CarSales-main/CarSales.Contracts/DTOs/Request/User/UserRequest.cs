using CarSales.Data.DataConstraints;
using CarSales.Data.ErrorMessages;
using System.ComponentModel.DataAnnotations;

namespace CarSales.Contracts.DTOs.Request.User
{
    public class UserRequest
    {
        [Required(ErrorMessage = UserErrorMessages.UsernameRequired)]
        [MinLength(UserConstraints.MinUsernameLength, ErrorMessage = UserErrorMessages.MinUsernameLength)]
        [MaxLength(UserConstraints.MaxUsernameLength, ErrorMessage = UserErrorMessages.MaxUsernameLength)]
        public required string Username { get; set; }

        [Required(ErrorMessage = UserErrorMessages.PasswordRequired)]
        [MinLength(UserConstraints.MinPasswordLength, ErrorMessage = UserErrorMessages.MinPasswordLength)]
        [MaxLength(UserConstraints.MaxPasswordLength, ErrorMessage = UserErrorMessages.MaxPasswordLength)]
        public required string Password { get; set; }

        [Required(ErrorMessage = UserErrorMessages.FirstNameRequired)]
        [MinLength(UserConstraints.MinFirstNameLength, ErrorMessage = UserErrorMessages.MinFirstNameLength)]
        [MaxLength(UserConstraints.MaxFirstNameLength, ErrorMessage = UserErrorMessages.MaxFirstNameLength)]
        [RegularExpression(UserConstraints.FirstNameRegex, ErrorMessage = UserErrorMessages.FirstNameRegex)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = UserErrorMessages.LastNameRequired)]
        [MinLength(UserConstraints.MinLastNameLength, ErrorMessage = UserErrorMessages.MinLastNameLength)]
        [MaxLength(UserConstraints.MaxLastNameLength, ErrorMessage = UserErrorMessages.MaxLastNameLength)]
        [RegularExpression(UserConstraints.LastNameRegex, ErrorMessage = UserErrorMessages.LastNameRegex)]
        public required string LastName { get; set; }

        [Range(UserConstraints.MinAgeValue, UserConstraints.MaxAgeValue, ErrorMessage = UserErrorMessages.AgeRange)]
        public int? Age { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
