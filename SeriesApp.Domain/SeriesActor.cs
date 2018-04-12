using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesApp.Domain
{
    public class SeriesActor
    {
        public int SeriesId { get; set; }
        public int ActorId { get; set; }
        public Series Series { get; set; }
        public Actor Actor { get; set; }
    }
}
