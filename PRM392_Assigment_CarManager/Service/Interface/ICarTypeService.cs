using Service.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ICarTypeService
    {
        Task<CarTypeResponModel> GetCarTypeByIdAsync(int id);
        Task<IEnumerable<CarTypeResponModel>> GetCarTypesAsync(
               string search = null,
               int? page = null,
               int? pageSize = null
        );
        Task<CarTypeResponModel> AddCarTypeAsync(CarTypeRequestModel carTypeRequest);
        Task UpdateCarTypeAsync(int id, CarTypeRequestModel carTypeRequest);
        Task DeleteCarTypeAsync(int id);
        Task<CarTypeResponModel> GetCarTypeByidDefaultAsync(int id);
    }
}
