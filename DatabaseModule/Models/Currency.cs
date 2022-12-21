using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostEventManegement.DatabaseModule.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
