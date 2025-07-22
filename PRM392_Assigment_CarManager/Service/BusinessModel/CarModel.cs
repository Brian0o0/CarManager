using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BusinessModel
{
    public class CarResponModel
    {
        public int CarId { get; set; }

        public string Make { get; set; }

        public string CarName { get; set; }

        public string Model { get; set; }

        public int ManufactureYear { get; set; }

        public int? CarTypeId { get; set; }

        public string CarTypeName { get; set; }

        public string Color { get; set; }

        public int? Mileage { get; set; }

        public string LicensePlate { get; set; }

        public decimal AskingPrice { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string Image { get; set; }

        public DateOnly? ListingDate { get; set; }

        public int? SellerId { get; set; }
    }
    public class CarRequestModel
    {
        public string CarName { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public int ManufactureYear { get; set; }

        public int? CarTypeId { get; set; }

        public string Color { get; set; }

        public int? Mileage { get; set; }

        public string LicensePlate { get; set; }

        public decimal AskingPrice { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string Image { get; set; }

        public DateOnly? ListingDate { get; set; }

        public int? SellerId { get; set; }
    }
}
