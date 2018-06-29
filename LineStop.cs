using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportLinesLib
{
    public class LineStop
    {
        public string id { get; set; }
        public string name { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }
        public List<TransportLine> tlines { get; set; }

        public LineStop(string id, string name, double lon, double lat)
        {
            this.id = id;
            this.name = name;
            this.lon = lon;
            this.lat = lat;
            this.tlines = new List<TransportLine>();
        }
    }
}
