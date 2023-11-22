using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyBuys.Models
{
    [Keyless]
    public class SkyBuysPromo
    {
        public string PromotionId { get; set; }
        public string PromotionType { get; set; }
        public string OnSaleFrom { get; set; }
        public string OnSaleTo { get; set; }
        public string PromotionDescription { get; set; }
        public string Sku { get; set; }
        public string StandardSalesPrice { get; set; }
        public string PromotionSalesPrice { get; set; }
    }
}
