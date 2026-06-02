using CarSales.Data.DataConstraints;
using CarSales.Data.Enums;
using CarSales.Data.ErrorMessages;
using System.ComponentModel.DataAnnotations;

namespace CarSales.Contracts.DTOs.Request.Car
{
    public class CarRequest
    {
        [Required(ErrorMessage = CarErrorMessages.BrandRequired)]
        public BrandEnum Brand { get; set; }

        [Required(ErrorMessage = CarErrorMessages.ModelRequired)]
        [MinLength(CarConstraints.MinModelLength, ErrorMessage = CarErrorMessages.MinModelLength)]
        [MaxLength(CarConstraints.MaxModelLength, ErrorMessage = CarErrorMessages.MaxModelLength)]
        public required string Model { get; set; }

        [Required(ErrorMessage = CarErrorMessages.YearRequired)]
        [Range(CarConstraints.MinYearValue, CarConstraints.MaxYearValue, ErrorMessage = CarErrorMessages.YearRange)]
        public int Year { get; set; }

        //ако има проблем с цената - от тук е
        [Required(ErrorMessage = CarErrorMessages.PriceRequired)]
        [Range(CarConstraints.MinPriceValue, CarConstraints.MaxPriceValue, ErrorMessage = CarErrorMessages.PriceRange)]
        public required double Price { get; set; }

        [Required(ErrorMessage = CarErrorMessages.FuelRequired)]
        public FuelEnum Fuel { get; set; }

        [Required(ErrorMessage = CarErrorMessages.TransmissionRequired)]
        public TransmissionEnum Transmission { get; set; }

        [Required(ErrorMessage = CarErrorMessages.ColorRequired)]
        public ColorEnum Color { get; set; }

        [Required(ErrorMessage = CarErrorMessages.PowerRequired)]
        [Range(CarConstraints.PowerMinValue, CarConstraints.PowerMaxValue, ErrorMessage = CarErrorMessages.PowerRange)]
        public int Power { get; set; }

        [Required(ErrorMessage = CarErrorMessages.EngineVolumeRequired)]
        [Range(CarConstraints.EngineVolumeMinValue, CarConstraints.EngineVolumeMaxValue, ErrorMessage = CarErrorMessages.EngineVolumeRange)]
        public int EngineVolume { get; set; }

        [MaxLength(CarConstraints.DescriptionMaxLength, ErrorMessage = CarErrorMessages.DescriptionMaxLength)]
        public string? Description { get; set; }

        public ICollection<string> PhotoUrls { get; set; } = new List<string>();
    }
}
