using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IUnitOfWork
    {
        IGenericRepository<Car> CarRepository { get; }
        IGenericRepository<CarType> CarTypeRepository { get; }
        IGenericRepository<Transaction> TransactionRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
