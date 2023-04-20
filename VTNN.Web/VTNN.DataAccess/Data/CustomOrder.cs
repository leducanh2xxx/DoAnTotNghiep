using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTNN.DataAccess.Data
{
    public class CustomOrder
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public DateTime Created_At { get; set; }
        public decimal Amount { get; set; }
    }
}
