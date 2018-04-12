using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesApp.Domain
{
    public class Series
    {
        public Series()
        {
            Actors = new List<SeriesActor>();
            Genres = new List<SeriesGenre>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProductionCompanyId { get; set; }
        public ProductionCompany ProductionCompany { get; set; }
        public List<SeriesActor> Actors { get; set; }
        public List<SeriesGenre> Genres { get; set; }
    }
}
