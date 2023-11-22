using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SkyBuys.Models
{
    public class ItemDefinition
    {
        [Key]
        public long RecId { get; set; }
        [MaxLength(32)]
        public string ItemNumber { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string MainCategory { get; set; }
        public string? SubCategory { get; set; }
        public string? Brand { get; set; }
        public string? ItemSize { get; set; }
    }
}
