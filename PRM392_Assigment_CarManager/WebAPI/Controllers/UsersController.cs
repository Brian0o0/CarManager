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
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Admin")] // Only Admins should see all users
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(string search = null, string sortBy = null, int? page = null, int? pageSize = null)
        {
            try
            {
                var users = await _userService.GetAccountsAsync(
                    search = search,
                    sortBy = sortBy,
                    page = page,
                    pageSize = page
                );

                if (users == null)
                {
                    return NotFound(new { Message = "No users found." });
                }

                // Trả về dữ liệu cùng mã trạng thái HTTP 200 OK
                return Ok(new
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    Items = users
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

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Seller,Buyer,Admin")] // Admin can see any user. Sellers/Buyers can see their own profile and potentially public info of others.
        public async Task<ActionResult<UserResponModel>> GetUser(int id)
        {
            var user = await _userService.GetAccountByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }
            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Seller,Buyer,Admin")] // Users can update their own profile; Admin can update any profile
        public async Task<IActionResult> PutUser(int id, UserRequestModel user)
        {
            try
            {
                var existingUser = await _userService.GetAccountByidDefaultAsync(id);

                if (existingUser == null)
                {
                    return NotFound(new { Message = $"User with ID {id} not found." });
                }
                await _userService.UpdateAccountAsync(id, user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing the request.", Details = ex.Message });
            }
            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AllowAnonymous] // New users register without authentication
        public async Task<ActionResult<UserResponModel>> PostUser(UserRequestModel user)
        {
            try
            {
                var createdUser = await _userService.AddAccountAsync(user);

                return CreatedAtAction(
                    nameof(GetUser),
                    new { id = createdUser.UserId },
                    createdUser
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

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admins can delete users
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetAccountByidDefaultAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = $"User with id {id} not found." });
            }

            try
            {
                await _userService.DeleteAccountAsync(id);
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
