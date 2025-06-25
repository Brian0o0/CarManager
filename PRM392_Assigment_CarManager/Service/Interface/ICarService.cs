using Service.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ICarService
    {
        Task<CarResponModel> GetCarByIdAsync(int id);
        Task<IEnumerable<CarResponModel>> GetCarsAsync(
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
            string carType = null
        );
        Task<CarResponModel> AddCarAsync(CarRequestModel carRequest);
        Task UpdateCarAsync(int id, CarRequestModel carRequest);
        Task DeleteCarAsync(int id);
        Task<CarResponModel> GetCarByidDefaultAsync(int id);
    }
}
