using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyBuys.Models
{
    public class Soh
    {
        [Key]
        public long RecId { get; set; }
        public string OrganizationCode { get; set; }
        [MaxLength(50)]
        public string SubinventoryCode { get; set; }
        [MaxLength(32)]
        public string ItemNumber { get; set; }
        public double PrimaryQuantity { get; set; }
    }
}
