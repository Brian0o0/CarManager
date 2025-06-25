using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using Service.BusinessModel;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        // GET: api/Cars
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CarResponModel>>> GetCars(
            string search = null,
            string sortBy = null,
            int? page = null,
            int? pageSize = null,
            int? minManufactureYear = null,
            int? maxManufactureYear = null,
            int? minMileage = null,
            int? maxMileage = null,
            string color = null,
            decimal? minAskingPrice = null,
            decimal? maxAskingPrice = null,
            string carType = null
            )
        {
            try
            {
                // Gọi service để lấy danh sách sản phẩm
                var cars = await _carService.GetCarsAsync(
                    search: search,
                    sortBy: sortBy,
                    page: page,
                    pageSize: pageSize,
                    minManufactureYear: minManufactureYear,
                    maxManufactureYear: maxManufactureYear,
                    minMileage: minMileage,
                    maxMileage: maxMileage,
                    color: color,
                    minAskingPrice: minAskingPrice,
                    maxAskingPrice: maxAskingPrice,
                    carType: carType
                );

                // Kiểm tra dữ liệu trả về
                if (cars == null)
                {
                    return NotFound(new { Message = "No cars found." });
                }

                // Trả về dữ liệu cùng mã trạng thái HTTP 200 OK
                return Ok(new
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    Items = cars
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while processing the request.",
                    Details = ex.Message
                });
            }
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CarResponModel>> GetCar(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound(new { Message = $"Car with ID {id} not found." });
            }
            return Ok(car);
        }

        // PUT: api/Cars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCar(int id, CarRequestModel car)
        {
            try
            {
                var existingCar = await _carService.GetCarByidDefaultAsync(id);

                if (existingCar == null)
                {
                    return NotFound(new { Message = $"Car with ID {id} not found." });
                }
                await _carService.UpdateCarAsync(id, car);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing the request.", Details = ex.Message });
            }
            return NoContent();
        }

        // POST: api/Cars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CarResponModel>> PostCar(CarRequestModel car)
        {
            try
            {
                var createdCar = await _carService.AddCarAsync(car);

                return CreatedAtAction(
                    nameof(GetCar),
                    new { id = createdCar.CarId },
                    createdCar
                );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while processing the request.",
                    Details = ex.Message
                });
            }
        }

        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _carService.GetCarByidDefaultAsync(id);
            if (car == null)
            {
                return NotFound(new { Message = $"Car with id {id} not found." });
            }

            try
            {
                await _carService.DeleteCarAsync(id);
                return NoContent(); 
            }
            catch (Exception ex)
            {            
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while deleting the product.",
                    Details = ex.Message
                });
            }
        }
    }
}
