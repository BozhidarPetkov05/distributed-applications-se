namespace CarSales.Data.DataConstraints
{
    public static class UserConstraints
    {
        //Username
        public const int MinUsernameLength = 4;
        public const int MaxUsernameLength = 20;

        //Password
        public const int MinPasswordLength = 5;
        public const int MaxPasswordLength = 50;

        //First name
        public const int MinFirstNameLength = 2;
        public const int MaxFirstNameLength = 30;
        public const string FirstNameRegex = "^[A-Za-z]+(-[A-Za-z]+)*$";

        //Last name
        public const int MinLastNameLength = 2;
        public const int MaxLastNameLength = 30;
        public const string LastNameRegex = "^[A-Za-z]+(-[A-Za-z]+)*$";

        //Age
        public const int MinAgeValue = 1;
        public const int MaxAgeValue = 120;
    }
}
