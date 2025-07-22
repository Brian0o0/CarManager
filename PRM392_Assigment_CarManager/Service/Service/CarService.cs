using AutoMapper;
using Repositories.Interface;
using Repositories.Models;
using Service.BusinessModel;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CarResponModel> AddCarAsync(CarRequestModel carRequest)
        {
            try
            {
                var car = _mapper.Map<Car>(carRequest);
                await _unitOfWork.CarRepository.Create(car);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<CarResponModel>(car);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding car: " + ex.Message, ex);
            }
        }

        public async Task DeleteCarAsync(int id)
        {
            try
            {
                var car = await _unitOfWork.CarRepository.GetByIdAsync(id);
                if (car == null)
                    throw new Exception($"Car with id {id} not found.");

                _unitOfWork.CarRepository.Delete(car);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting car: " + ex.Message, ex);
            }
        }

        public async Task<CarResponModel> GetCarByIdAsync(int id)
        {
            var cars = await _unitOfWork.CarRepository.GetAllAsyn(
                 fillter: c => c.CarId == id,
                 includes: c => c.CarType
                 );
            var car = cars.FirstOrDefault();
            if (car == null)
            {
                return null;
            }
            return _mapper.Map<CarResponModel>(car);
        }

        public async Task<CarResponModel> GetCarByidDefaultAsync(int id)
        {
            var car = await _unitOfWork.CarRepository.GetByIdAsync(id);
            return _mapper.Map<CarResponModel>(car);
        }

        public async Task<IEnumerable<CarResponModel>> GetCarsAsync(
            string search = null,
            string sortBy = null,
            int? page = null,
            int? pageSize = null,
            int? minManufactureYear = null,
            int? maxManufactureYear = null,
            int? minMileage = null,
            int? maxMileage = null,
            string color = null,
            decimal? minAskingPrice = null,
            decimal? maxAskingPrice = null,
            string carType = null) // Added CarType for filtering
        {
            // Lấy dữ liệu xe từ repository với các điều kiện lọc và sắp xếp
            var cars = await _unitOfWork.CarRepository.GetAllAsyn(
                // Logic lọc dựa trên các trường của Car
                fillter: c =>
                    // Lọc theo CarName (tên xe) hoặc Model
                    (string.IsNullOrEmpty(search) ||
                     c.Make.Contains(search) || // Tìm kiếm theo hãng xe
                     c.Model.Contains(search) // Tìm kiếm theo model
                    ) &&
                    // Lọc theo khoảng năm sản xuất
                    (!minManufactureYear.HasValue || c.ManufactureYear >= minManufactureYear.Value) &&
                    (!maxManufactureYear.HasValue || c.ManufactureYear <= maxManufactureYear.Value) &&
                    // Lọc theo khoảng số km đã đi
                    (!minMileage.HasValue || c.Mileage >= minMileage.Value) &&
                    (!maxMileage.HasValue || c.Mileage <= maxMileage.Value) &&
                    // Lọc theo màu sắc
                    (string.IsNullOrEmpty(color) || c.Color.Contains(color)) &&
                    // Lọc theo khoảng giá bán
                    (!minAskingPrice.HasValue || c.AskingPrice >= minAskingPrice.Value) &&
                    (!maxAskingPrice.HasValue || c.AskingPrice <= maxAskingPrice.Value) &&
                    // Lọc theo loại xe (CarType) - cần join hoặc include CarType để truy cập TypeName
                    (string.IsNullOrEmpty(carType) || (c.CarType != null && c.CarType.TypeName.Contains(carType))),
                // Logic sắp xếp: Wrap the switch expression within a lambda for orderBy
                orderBy: q => sortBy switch
                {
                    "make" => q.OrderBy(c => c.Make),
                    "model" => q.OrderBy(c => c.Model),
                    "year" => q.OrderBy(c => c.ManufactureYear),
                    "price" => q.OrderBy(c => c.AskingPrice),
                    "mileage" => q.OrderBy(c => c.Mileage),
                    "listingdate" => q.OrderByDescending(c => c.ListingDate), // Sắp xếp theo ngày đăng mới nhất
                    _ => q.OrderBy(c => c.CarId) // Mặc định sắp xếp theo CarID
                },
                currentPage: page,
                pagesize: pageSize,
                includes: c => c.CarType
            );
            return _mapper.Map<IEnumerable<CarResponModel>>(cars);
        }

        public async Task UpdateCarAsync(int id, CarRequestModel carRequest)
        {
            try
            {
                // Lấy thông tin sản phẩm từ cơ sở dữ liệu theo ID
                var existingCar = await _unitOfWork.CarRepository.GetByIdAsync(id);

                if (existingCar == null)
                {
                    throw new Exception($"Car with ID {id} not found.");
                }

                // Chỉ cập nhật những trường cần thiết
                existingCar.CarName = carRequest.CarName;
                existingCar.Make = carRequest.Make;
                existingCar.Model = carRequest.Model;
                existingCar.ManufactureYear = carRequest.ManufactureYear;
                existingCar.CarTypeId = carRequest.CarTypeId;
                existingCar.Color = carRequest.Color;
                existingCar.Mileage = carRequest.Mileage;
                existingCar.LicensePlate = carRequest.LicensePlate;
                existingCar.ListingDate = carRequest.ListingDate;
                existingCar.AskingPrice = carRequest.AskingPrice;
                existingCar.Description = carRequest.Description;
                existingCar.Status = carRequest.Status;
                existingCar.SellerId = carRequest.SellerId;
                existingCar.Image = carRequest.Image;
                // Lưu thay đổi
                _unitOfWork.CarRepository.Update(existingCar);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating Car: " + ex.Message, ex);
            }
        }
    }
}
