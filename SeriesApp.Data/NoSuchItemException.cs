using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesApp.Data
{
    public class NoSuchItemException : Exception
    {
        public NoSuchItemException(string message) : base(message)
        {
            
        }
    }
}
