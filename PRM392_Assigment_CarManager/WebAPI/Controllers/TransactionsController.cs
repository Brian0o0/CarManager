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
    [Authorize] // Bảo vệ API này chỉ cho phép người dùng đã xác thực truy cập
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // GET: api/Transactions
        [HttpGet]
        [Authorize(Roles = "Seller,Buyer,Admin")] // Adjust based on strictness
        public async Task<ActionResult<IEnumerable<TransactionResponModel>>> GetTransactions(string search = null, int? page = null, int? pageSize = null)
        {
            try
            {
                // Gọi service để lấy danh sách sản phẩm
                var transactions = await _transactionService.GetTransactionsAsync(
                search = search,
                page = page,
                pageSize = page
            );

            // Kiểm tra dữ liệu trả về
            if (transactions == null)
            {
                return NotFound(new { Message = "No transactions found." });
            }

            // Trả về dữ liệu cùng mã trạng thái HTTP 200 OK
            return Ok(new
            {
                PageIndex = page,
                PageSize = pageSize,
                Items = transactions
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

        // GET: api/Transactions
        [HttpGet("GetByUserID")]
        [Authorize(Roles = "Seller,Buyer,Admin")] // Adjust based on strictness
        public async Task<ActionResult<IEnumerable<TransactionResponModel>>> GetTransactionsByUserId(int id, int? page = null, int? pageSize = null)
        {
            try
            {
                // Gọi service để lấy danh sách sản phẩm
                var transactions = await _transactionService.GetTransactionsByUserIdAsync(
                id = id,
                page = page,
                pageSize = page
            );

                // Kiểm tra dữ liệu trả về
                if (transactions == null)
                {
                    return NotFound(new { Message = "No transactions found." });
                }

                // Trả về dữ liệu cùng mã trạng thái HTTP 200 OK
                return Ok(new
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    Items = transactions
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

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Seller,Buyer,Admin")] // Adjust based on strictness
        public async Task<ActionResult<TransactionResponModel>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound(new { Message = $"Transaction with ID {id} not found." });
            }
            return Ok(transaction);
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Seller,Admin")] // Adjust based on strictness
        public async Task<IActionResult> PutTransaction(int id, TransactionRequestModel transaction)
        {
            try
            {
                var existingTransaction = await _transactionService.GetTransactionByidDefaultAsync(id);

                if (existingTransaction == null)
                {
                    return NotFound(new { Message = $"Transaction with ID {id} not found." });
                }
                await _transactionService.UpdateTransactionAsync(id, transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing the request.", Details = ex.Message });
            }
            return NoContent();
        }

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Seller,Admin")] // Chỉ cho phép người dùng có vai trò Admin hoặc User tạo giao dịch
        public async Task<ActionResult<TransactionResponModel>> PostTransaction(TransactionRequestModel transaction)
        {
            try
            {
                var createdTransaction= await _transactionService.AddTransactionAsync(transaction);

                return CreatedAtAction(
                    nameof(GetTransaction),
                    new { id = createdTransaction.TransactionId },
                    createdTransaction
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

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admins can delete transactions
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionByidDefaultAsync(id);
            if (transaction == null)
            {
                return NotFound(new { Message = $"Transaction with id {id} not found." });
            }

            try
            {
                await _transactionService.DeleteTransactionAsync(id);
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
