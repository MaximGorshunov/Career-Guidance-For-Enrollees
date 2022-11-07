using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exeptions
{
    public class BuisnessExeption : Exception
    {
        public BuisnessExeption (string message) : base(message) 
        {
        }
    }
}
