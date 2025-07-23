using AutoMapper;
using Repositories.Interface;
using Repositories.Models;
using Service.BusinessModel;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<TransactionResponModel> AddTransactionAsync(TransactionRequestModel transactionRequest)
        {
            try
            {
                var transaction = _mapper.Map<Transaction>(transactionRequest);
                await _unitOfWork.TransactionRepository.Create(transaction);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<TransactionResponModel>(transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding transaction: " + ex.Message, ex);
            }
        }

        public async Task DeleteTransactionAsync(int id)
        {
            try
            {
                var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(id);
                if (transaction == null)
                    throw new Exception($"transaction with id {id} not found.");

                _unitOfWork.TransactionRepository.Delete(transaction);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting transaction: " + ex.Message, ex);
            }
        }

        public async Task<TransactionResponModel> GetTransactionByIdAsync(int id)
        {
            var transactions = await _unitOfWork.TransactionRepository.GetAllAsyn(
                 fillter: t => t.TransactionId == id);
            var transaction = transactions.FirstOrDefault();
            if (transaction == null)
            {
                return null;
            }
            return _mapper.Map<TransactionResponModel>(transaction);
        }

        public async Task<TransactionResponModel> GetTransactionByidDefaultAsync(int id)
        {
            var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(id);
            return _mapper.Map<TransactionResponModel>(transaction);
        }

        public async Task<IEnumerable<TransactionResponModel>> GetTransactionsAsync(string search = null, int? page = null, int? pageSize = null)
        {
            var transactions = await _unitOfWork.TransactionRepository.GetAllAsyn(
               fillter: c => (string.IsNullOrEmpty(search) || c.TransactionStatus.Contains(search)),
                currentPage: page,
                pagesize: pageSize
           );
            return _mapper.Map<IEnumerable<TransactionResponModel>>(transactions);
        }

        public async Task<IEnumerable<TransactionResponModel>> GetTransactionsByUserIdAsync(int id, int? page = null, int? pageSize = null)
        {
            var transactions = await _unitOfWork.TransactionRepository.GetAllAsyn(
               fillter: c => c.BuyerId == id,
                currentPage: page,
                pagesize: pageSize
           );
            return _mapper.Map<IEnumerable<TransactionResponModel>>(transactions);
        }

        public async Task UpdateTransactionAsync(int id, TransactionRequestModel transactionRequest)
        {
            try
            {
                // Lấy thông tin sản phẩm từ cơ sở dữ liệu theo ID
                var existingtransaction = await _unitOfWork.TransactionRepository.GetByIdAsync(id);

                if (existingtransaction == null)
                {
                    throw new Exception($"Transaction with ID {id} not found.");
                }

                // Chỉ cập nhật những trường cần thiết
                existingtransaction.CarId = transactionRequest.CarId;
                existingtransaction.BuyerId = transactionRequest.BuyerId;
                existingtransaction.TransactionDate = transactionRequest.TransactionDate;
                existingtransaction.TransactionStatus = transactionRequest.TransactionStatus;
                existingtransaction.SellingPrice = transactionRequest.SellingPrice;

                // Lưu thay đổi
                _unitOfWork.TransactionRepository.Update(existingtransaction);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating transaction: " + ex.Message, ex);
            }
        }
    }
}
