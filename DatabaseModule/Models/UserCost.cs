using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostEventManegement.DatabaseModule.Models
{
    public class UserCost
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public int CostId { get; set; }
        public Cost Cost { get; set; }
        public int PayerId { get; set; }
        public User Payer { get; set; }
        public int DebtorId { get; set; }
        public User Debtor { get; set; }
    }
}
