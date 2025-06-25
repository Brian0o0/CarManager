using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BusinessModel
{
    public class CarTypeResponModel
    {
        public int CarTypeId { get; set; }

        public string TypeName { get; set; }

        public string Description { get; set; }
    }

    public class CarTypeRequestModel
    {
        public string TypeName { get; set; }

        public string Description { get; set; }
    }
}
