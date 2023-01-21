using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostEventManegement.EventModule.Models
{
    public class EventVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int DefaultCurrencyId { get; set; }
        public List<CostDTO> Costs { get; set; }
        public List<UserBalanceVM> Users { get; set; }
    }
}
