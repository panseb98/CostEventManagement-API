using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostEventManegement.DatabaseModule.Models
{
	public class Log
	{
		public int Id { get; set; }
        public string Method { get; set; }
        public string LogBody { get; set; }
	}
}

