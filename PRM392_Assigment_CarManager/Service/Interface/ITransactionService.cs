using Service.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ITransactionService
    {
        Task<TransactionResponModel> GetTransactionByIdAsync(int id);
        Task<IEnumerable<TransactionResponModel>> GetTransactionsAsync(
               string search = null,
               int? page = null,
               int? pageSize = null
        );
        Task<TransactionResponModel> AddTransactionAsync(TransactionRequestModel transactionRequest);
        Task UpdateTransactionAsync(int id, TransactionRequestModel transactionRequest);
        Task DeleteTransactionAsync(int id);
        Task<TransactionResponModel> GetTransactionByidDefaultAsync(int id);
    }
}
