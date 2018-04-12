using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesApp.Domain
{
    public class Genre
    {
        public Genre()
        {
            Series = new List<SeriesGenre>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SeriesGenre> Series { get; set; }
    }
}
