using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostEventManegement.EventModule.Models
{
    public class SettleUserEvent
    {
        public int EventId { get; set; }
        public int FirstUserId { get; set; }
        public int SecondUserId { get; set; }
    }
}
