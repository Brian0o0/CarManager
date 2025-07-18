using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using Service.BusinessModel;
using Service.Interface;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bảo vệ API này chỉ cho phép người dùng đã xác thực truy cập
    public class CarTypesController : ControllerBase
    {
        private readonly ICarTypeService _carTypeService;

        public CarTypesController(ICarTypeService carTypeService)
        {
            _carTypeService = carTypeService;
        }

        // GET: api/CarTypes
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CarTypeResponModel>>> GetCarTypes(
            string search = null, 
            int? page = null, 
            int? pageSize = null
            )
        {
            try
            {
                // Gọi service để lấy danh sách sản phẩm
                var cars = await _carTypeService.GetCarTypesAsync(
                    search = search,
                    page = page,
                    pageSize = page
                );

                // Kiểm tra dữ liệu trả về
                if (cars == null)
                {
                    return NotFound(new { Message = "No cartypes found." });
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

        // GET: api/CarTypes/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CarTypeResponModel>> GetCarType(int id)
        {
            var carType = await _carTypeService.GetCarTypeByIdAsync(id);
            if (carType == null)
            {
                return NotFound(new { Message = $"CarType with ID {id} not found." });
            }
            return Ok(carType);
        }

        // PUT: api/CarTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only Admins can delete car types
        public async Task<IActionResult> PutCarType(int id, CarTypeRequestModel carType)
        {
            try
            {
                var existingCarType = await _carTypeService.GetCarTypeByidDefaultAsync(id);

                if (existingCarType == null)
                {
                    return NotFound(new { Message = $"CarType with ID {id} not found." });
                }
                await _carTypeService.UpdateCarTypeAsync(id, carType);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing the request.", Details = ex.Message });
            }
            return NoContent();
        }

        // POST: api/CarTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")] // Only Admins can delete car types
        public async Task<ActionResult<CarTypeResponModel>> PostCarType(CarTypeRequestModel carType)
        {
            try
            {
                var createdCarType = await _carTypeService.AddCarTypeAsync(carType);

                return CreatedAtAction(
                    nameof(GetCarType),
                    new { id = createdCarType.CarTypeId },
                    createdCarType
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

        // DELETE: api/CarTypes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admins can delete car types
        public async Task<IActionResult> DeleteCarType(int id)
        {
            var carType = await _carTypeService.GetCarTypeByidDefaultAsync(id);
            if (carType == null)
            {
                return NotFound(new { Message = $"CarType with id {id} not found." });
            }

            try
            {
                await _carTypeService.DeleteCarTypeAsync(id);
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
