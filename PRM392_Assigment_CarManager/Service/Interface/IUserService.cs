using Service.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IUserService
    {
        Task<UserResponModel> Login(string email, string password);
        Task<UserResponModel> GetAccountByIdAsync(int id);
        Task<IEnumerable<UserResponModel>> GetAccountsAsync(string search = null, string sortBy = null, int? page = null, int? pageSize = null);
        Task<UserResponModel> AddAccountAsync(UserRequestModel userRequest);
        Task UpdateAccountAsync(int id, UserRequestModel userRequest);
        Task DeleteAccountAsync(int id);
        Task<UserResponModel> GetAccountByidDefaultAsync(int id);
    }
}
