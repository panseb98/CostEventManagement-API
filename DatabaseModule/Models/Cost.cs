using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostEventManegement.DatabaseModule.Models
{
    public class Cost
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int PayerId { get; set; }
        public User Payer { get; set; }
        public virtual List<UserCost> UserCosts { get; set; }
    }
}
