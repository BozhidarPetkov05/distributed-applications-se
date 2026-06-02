using CarSales.Contracts.DTOs.Request;
using CarSales.Contracts.Interfaces;
using CarSales.Data.Entities;
using CarSales.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CarSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnvironment;
        private readonly IPhotoService _photoService;
        public PhotosController(IWebHostEnvironment webHostEnvironment, IPhotoService photoService)
        {
            _webHostEnvironment = webHostEnvironment;
            _photoService = photoService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] FileUpload fileUpload)
        {
            try
            {
                if (fileUpload?.File != null && fileUpload.File.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileUpload.File.FileName);
                    string baseUrl = $"{Request.Scheme}://{Request.Host}";
                    Photo photo = new Photo()
                    {
                        ImagePath = $"{baseUrl}/uploads/{fileName}",
                        CreatedAt = DateTime.UtcNow,
                        LastChange = DateTime.UtcNow,
                        CarId = fileUpload.CarId,
                    };

                    using (FileStream fs = System.IO.File.Create(path + fileName))
                    {
                        await fileUpload.File.CopyToAsync(fs);
                        await fs.FlushAsync();
                    }

                    await _photoService.AddAsync(photo);
                    return Ok(new { message = "Upload done.", success = true, photo = photo });
                }
                else
                {
                    throw new BadRequestException("No file provided!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }
    }
}
