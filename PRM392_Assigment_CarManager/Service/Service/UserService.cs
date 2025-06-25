using AutoMapper;
using AutoMapper.Internal;
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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<UserResponModel> AddAccountAsync(UserRequestModel userRequest)
        {
            try
            {
                var user = _mapper.Map<User>(userRequest);
                await _unitOfWork.UserRepository.Create(user);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<UserResponModel>(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding user: " + ex.Message, ex);
            }
        }

        public async Task DeleteAccountAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (user == null)
                    throw new Exception($"User with id {id} not found.");

                _unitOfWork.UserRepository.Delete(user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting user: " + ex.Message, ex);
            }
        }

        public async Task<UserResponModel> GetAccountByIdAsync(int id)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsyn(
                 fillter: c => c.UserId == id);
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserResponModel>(user);
        }

        public async Task<UserResponModel> GetAccountByidDefaultAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            return _mapper.Map<UserResponModel>(user);
        }

        public async Task<IEnumerable<UserResponModel>> GetAccountsAsync(string search = null, string sortBy = null, int? page = null, int? pageSize = null)
        {
            var accounts = await _unitOfWork.UserRepository.GetAllAsyn(
             fillter: a => string.IsNullOrEmpty(search) || a.FullName.Contains(search) || a.Email.ToString().Contains(search),
            orderBy: sortBy switch
            {
                "name" => q => q.OrderBy(a => a.FullName),
                "role" => q => q.OrderBy(a => a.UserType),
                "email" => q => q.OrderBy(a => a.Email),
                _ => q => q.OrderBy(a => a.UserId)
            },
                 currentPage: page,
                 pagesize: pageSize
            );
            return _mapper.Map<IEnumerable<UserResponModel>>(accounts);       
        }

        public async Task<UserResponModel> Login(string email, string password)
        {
            var user = await _unitOfWork.UserRepository.GetAllAsyn(
                fillter: a => a.Email == email && a.PasswordHash == password
                );
            var account = user.FirstOrDefault();
            if (account == null)
            {
                return null;
            }
            return _mapper.Map<UserResponModel>(account);
        }

        public async Task UpdateAccountAsync(int id, UserRequestModel userRequest)
        {
            try
            {
                // Lấy thông tin sản phẩm từ cơ sở dữ liệu theo ID
                var existingUser = await _unitOfWork.UserRepository.GetByIdAsync(id);

                if (existingUser == null)
                {
                    throw new Exception($"User with ID {id} not found.");
                }

                // Chỉ cập nhật những trường cần thiết
                existingUser.Username = userRequest.Username;
                existingUser.FullName = userRequest.FullName;
                existingUser.Email = userRequest.Email;
                existingUser.PhoneNumber = userRequest.PhoneNumber;
                existingUser.Address = userRequest.Address;
                existingUser.RegistrationDate = userRequest.RegistrationDate;

                // Lưu thay đổi
                _unitOfWork.UserRepository.Update(existingUser);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating user: " + ex.Message, ex);
            }
        }
    }
}
