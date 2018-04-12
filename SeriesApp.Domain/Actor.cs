using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesApp.Domain
{
    public class Actor
    {
        public Actor()
        {
            Series = new List<SeriesActor>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public List<SeriesActor> Series { get; set; }
    }
}
