namespace CarSales.Data.ErrorMessages
{
    public static class CarErrorMessages
    {
        //Brand
        public const string BrandRequired = "Brand is required!";

        //Model
        public const string ModelRequired = "Model is required!";
        public const string MinModelLength = "Model length must be bigger than 1!";
        public const string MaxModelLength = "Model length must not exceed 30!";

        //Year
        public const string YearRequired = "Year is required!";
        public const string YearRange = "Year must be between 1800 and 2026!";

        //Price
        public const string PriceRequired = "Price is required!";
        public const string PriceRange = "Price must be between 1 and 50 000 000";

        //Fuel
        public const string FuelRequired = "Fuel is required!";

        //Transmission
        public const string TransmissionRequired = "Transmission is required!";

        //Color
        public const string ColorRequired = "Color is required!";

        //Power
        public const string PowerRequired = "Power is required!";
        public const string PowerRange = "Power must be between 1 and 10 000";

        //Engine Volume
        public const string EngineVolumeRequired = "Engine volume is required!";
        public const string EngineVolumeRange = "Engine volume must be between 1 and 50 000";

        //Description
        public const string DescriptionMaxLength = "Description must be lower or equal to 2000!";
    }
}
