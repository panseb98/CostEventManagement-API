using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostEventManegement.DatabaseModule.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int DefaultCurrencyId { get; set; }
        public Currency DefaultCurrency { get; set; }
        public bool AutomaticConvert { get; set; }
        public virtual List<EventUser> EventUsers { get; set; }
    }
}
