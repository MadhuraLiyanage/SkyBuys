using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyBuys.Models
{
    public class SubInventory
    {
        [Key]
        [MaxLength(50)]
        public string SubInventoryCode { get; set; }
        public string SubInventoryDescription { get; set; }
        public bool Active { get; set; } = true;
    }
}
