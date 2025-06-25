using Repositories.Interface;
using Repositories.Models;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CarManagementContext _context;
        private IGenericRepository<Car> _carReposityory;
        private IGenericRepository<CarType> _cartypeReposityory;
        private IGenericRepository<Transaction> _transactionReposityory;
        private IGenericRepository<User> _userReposityory;


        public UnitOfWork(CarManagementContext context)
        {
            _context = context;
        }

        public IGenericRepository<Car> CarRepository => _carReposityory ??= new GenericRepository<Car>(_context);
        public IGenericRepository<CarType> CarTypeRepository => _cartypeReposityory ??= new GenericRepository<CarType>(_context);

        public IGenericRepository<Transaction> TransactionRepository => _transactionReposityory ??= new GenericRepository<Transaction>(_context);

        public IGenericRepository<User> UserRepository => _userReposityory ??= new GenericRepository<User>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
