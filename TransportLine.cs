using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportLinesLib
{
    public class TransportLine
    {
        public string id { get; set; }
        public string shortName { get; set; }
        public string longName { get; set; }
        public string mode { get; set; }
        public string type { get; set; }
        public string color { get; set; }
        public string textColor { get; set; }

        public TransportLine(string id)
        {
            this.id = id;
        }
    }
}

