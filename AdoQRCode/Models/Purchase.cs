using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoQRCode.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public Guid PurchaseGuid { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int ProductId { get; set; }
        public string CustomerName { get; set; }
        public byte[] PurchaseQr { get; set; }
        public byte[] ShippingQr { get; set; }
    }
}
