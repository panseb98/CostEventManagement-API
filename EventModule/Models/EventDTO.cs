using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostEventManegement.EventModule.Models
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int DefaultCurrencyId { get; set; }
        public string DefaultCurrencyCode { get; set; }
        public bool AutomaticConvert { get; set; }
        public List<SimpleUserVM> Users { get; set; }
    }
}
