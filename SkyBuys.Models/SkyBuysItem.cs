using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyBuys.Models
{
    [Keyless]
    public class SkyBuysItem
    { 
        public string? Brand { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string? Size { get; set; }
        public string? ImageURL { get; set; }
        public string Sku { get; set; } //ItemCode
        public string RrpPrice { get; set; }
        public string Category { get; set; }
        public string? SubCategory { get; set; }
        public string Location { get; set; }
        public string? AdditionProductImage { get; set; }
        public string? AdditionalProductVideo { get; set; }
        public string Type { get; set; }
        public double SOH { get; set; }
    }
}
