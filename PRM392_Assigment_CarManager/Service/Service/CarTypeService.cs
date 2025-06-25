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
    public class CarTypeService : ICarTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CarTypeResponModel> AddCarTypeAsync(CarTypeRequestModel carTypeRequest)
        {
            try
            {
                var carType = _mapper.Map<CarType>(carTypeRequest);
                await _unitOfWork.CarTypeRepository.Create(carType);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<CarTypeResponModel>(carType);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding cartype: " + ex.Message, ex);
            }
        }

        public async Task DeleteCarTypeAsync(int id)
        { 
            try
            {
                var cartype = await _unitOfWork.CarTypeRepository.GetByIdAsync(id);
                if (cartype == null)
                    throw new Exception($"CarType with id {id} not found.");

                _unitOfWork.CarTypeRepository.Delete(cartype);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting cartype: " + ex.Message, ex);
            }
        }

        public async Task<CarTypeResponModel> GetCarTypeByIdAsync(int id)
        {
            var carTypes = await _unitOfWork.CarTypeRepository.GetAllAsyn(
                  fillter: c => c.CarTypeId == id);
            var carType = carTypes.FirstOrDefault();
            if (carType == null)
            {
                return null;
            }
            return _mapper.Map<CarTypeResponModel>(carType);
        }

        public async Task<CarTypeResponModel> GetCarTypeByidDefaultAsync(int id)
        {
            var cartype = await _unitOfWork.CarTypeRepository.GetByIdAsync(id);
            return _mapper.Map<CarTypeResponModel>(cartype);
        }

        public async Task<IEnumerable<CarTypeResponModel>> GetCarTypesAsync(string search = null, int? page = null, int? pageSize = null)
        {
            var carTypes = await _unitOfWork.CarTypeRepository.GetAllAsyn(
                fillter: c => (string.IsNullOrEmpty(search) || c.TypeName.Contains(search)),
                currentPage: page,
                pagesize: pageSize
            );
            return _mapper.Map<IEnumerable<CarTypeResponModel>>(carTypes);
        }

        public async Task UpdateCarTypeAsync(int id, CarTypeRequestModel carTypeRequest)
        {
            try
            {
                // Lấy thông tin sản phẩm từ cơ sở dữ liệu theo ID
                var existingCarType = await _unitOfWork.CarTypeRepository.GetByIdAsync(id);

                if (existingCarType == null)
                {
                    throw new Exception($"Cartype with ID {id} not found.");
                }

                // Chỉ cập nhật những trường cần thiết
                existingCarType.TypeName = carTypeRequest.TypeName;
                existingCarType.Description = existingCarType.Description;
              

                // Lưu thay đổi
                _unitOfWork.CarTypeRepository.Update(existingCarType);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating CarType: " + ex.Message, ex);
            }
        }
    }
}
