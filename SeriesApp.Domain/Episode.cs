using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesApp.Domain
{
    public class Episode
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SeriesId { get; set; }
        public Series Series { get; set; }
        public int SeasonNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
