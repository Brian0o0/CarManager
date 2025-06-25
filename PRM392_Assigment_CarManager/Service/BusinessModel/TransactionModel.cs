using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BusinessModel
{
    public class TransactionResponModel
    {
        public int TransactionId { get; set; }

        public int CarId { get; set; }

        public int BuyerId { get; set; }

        public DateTime? TransactionDate { get; set; }

        public decimal SellingPrice { get; set; }

        public string TransactionStatus { get; set; }

    }
    public class TransactionRequestModel
    {
        public int CarId { get; set; }
        public int BuyerId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal SellingPrice { get; set; }
        public string TransactionStatus { get; set; }
    }
}