using CarSales.Contracts.DTOs.Request.Car;
using CarSales.Contracts.DTOs.Response.Car;
using CarSales.Contracts.Interfaces;
using CarSales.Data.Entities;
using CarSales.Data.Enums;
using CarSales.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;
        public CarsController(ICarService carService)
        {
            _carService = carService;
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CarPageRequest request)
        {
            bool isAdmin = User.HasClaim("isAdmin", "True");
            var response = await _carService.GetAllCarsPagedAsync(request, isAdmin);
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Car? car = await _carService.GetByIdAsync(id);
            if (car is null)
            {
                throw new NotFoundException("The car was not found!");
            }

            CarDetailedResponse response = _carService.MapToDetailedResponse(car);

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CarRequest request)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            if (request is null)
            {
                return BadRequest(new { message = "Invalid car data provided." });
            }

            Guid loggedUserId = Guid.Parse(User.FindFirstValue("loggedUserId"));
            Car car = _carService.CreateCar(request, loggedUserId);
            await _carService.AddAsync(car);
            // return 201 with created car id so the client can upload photos linked to this car
            return CreatedAtAction(nameof(Get), new { id = car.Id }, new { id = car.Id });
        }

        [Authorize]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromBody] CarRequest request, [FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            Car? car = await _carService.GetByIdAsync(id);
            if (car is null)
            {
                throw new NotFoundException("The car was not found!");
            }

            Guid loggedUserId = Guid.Parse(User.FindFirstValue("loggedUserId"));
            if (!User.HasClaim("isAdmin", "True") && loggedUserId != car.UserId)
            {
                throw new ForbidException("You cannot update cars other than yours!");
            }

            Car updatedCar = await _carService.UpdateCar(request, car);
            await _carService.UpdateAsync(updatedCar);
            CarUpdatedResponse response = _carService.MapToCarUpdatedResponse(updatedCar);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Car? car = await _carService.GetByIdAsync(id);
            if (car is null)
            {
                throw new NotFoundException("The car was not found!");
            }

            Guid loggedUserId = Guid.Parse(User.FindFirstValue("loggedUserId"));
            if (!User.HasClaim("isAdmin", "True") && loggedUserId != car.UserId)
            {
                throw new ForbidException("You cannot delete cars other than yours!");
            }

            CarUpdatedResponse response = _carService.MapToCarUpdatedResponse(car);
            await _carService.DeleteAsync(car);
            return Ok(response);
        }

        [HttpGet("enums/car-options")]
        public IActionResult GetCarOptions()
        {
            // return both name and numeric value for each enum entry so client can post numeric values
            var brands = Enum.GetValues(typeof(BrandEnum)).Cast<BrandEnum>().Select(e => new { Name = e.ToString(), Value = (int)e }).ToArray();
            var fuels = Enum.GetValues(typeof(FuelEnum)).Cast<FuelEnum>().Select(e => new { Name = e.ToString(), Value = (int)e }).ToArray();
            var transmissions = Enum.GetValues(typeof(TransmissionEnum)).Cast<TransmissionEnum>().Select(e => new { Name = e.ToString(), Value = (int)e }).ToArray();
            var colors = Enum.GetValues(typeof(ColorEnum)).Cast<ColorEnum>().Select(e => new { Name = e.ToString(), Value = (int)e }).ToArray();

            var options = new
            {
                Brands = brands,
                Fuels = fuels,
                Transmissions = transmissions,
                Colors = colors
            };

            return Ok(options);
        }
    }
}
