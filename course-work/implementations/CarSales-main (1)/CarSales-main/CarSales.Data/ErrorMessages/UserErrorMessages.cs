namespace CarSales.Data.ErrorMessages
{
    public static class UserErrorMessages
    {
        //Username
        public const string UsernameRequired = "Username is required!";
        public const string MinUsernameLength = "Username length must be at least 4 characters!";
        public const string MaxUsernameLength = "Username length must not exceed 20 characters!";

        //Password
        public const string PasswordRequired = "Password is required!";
        public const string MinPasswordLength = "Password must be at least 5 characters!";
        public const string MaxPasswordLength = "Password must not exceed 50 characters!";

        //First name
        public const string FirstNameRequired = "First name is required!";
        public const string MinFirstNameLength = "First name must be at least 2 characters!";
        public const string MaxFirstNameLength = "First name must not exceed 30 characters!";
        public const string FirstNameRegex = "First name can contain letters and hyphens!";

        //Last name
        public const string LastNameRequired = "Last name is required!";
        public const string MinLastNameLength = "Last name must be at least 2 characters!";
        public const string MaxLastNameLength = "Last name must not exceed 30 characters!";
        public const string LastNameRegex = "Last name can contain letters and hyphens!";

        //Age
        public const string AgeRange = "Age must be between 1 and 120!";
    }
}
