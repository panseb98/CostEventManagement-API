using CostEventManegement.DatabaseModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostEventManegement.EventModule.Models
{
    public class CostDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public int CurrencyId { get; set; }
        public int EventId { get; set; }
        public int PayerId { get; set; }
        public virtual List<int> Users { get; set; }
    }
}
