using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesApp.Data
{
    public class ItemNotPossibleToDeleteException : Exception
    {
        public ItemNotPossibleToDeleteException(string message) : base(message)
        {

        }
    }
}
