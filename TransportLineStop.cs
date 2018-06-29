using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace TransportLinesLib
{

    public class TransportLineStop
    {
        public List<LineStop> lStops;
        
        public TransportLineStop()
        {
            lStops = new List<LineStop>();
        }


        public void searchLineStop(double lon, double lat, int dist)
        {
            // Requete sur metromobilite.fr pour récupérer les arrets
            WebRequest req = WebRequest.Create($"http://data.metromobilite.fr/api/linesNear/json?x={lon}&y={lat}&dist={dist}&details=true");
            WebResponse resp = req.GetResponse();

            Stream dataStream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            List<LineStopMetro> lStopMetros = JsonConvert.DeserializeObject<List<LineStopMetro>>(reader.ReadToEnd());


            // recopie la liste récupérée par API lStopMetros et la recopie dans la liste lStops 
            // en supprimant les doublons et en allant chercher les infos API des lignes de transport
            LineStop lst;
            lStops = new List<LineStop>();

            foreach (LineStopMetro lStopMetro in lStopMetros)
            {
                // if (!lStops.Exists(defineCriteria()) { 
                lst = lStops.Find(el => el.name.Equals(lStopMetro.name));
                if (lst == null)
                {   // l'arret n'existe pas encore -> j'ajoute l'arret
                    lst = new LineStop(lStopMetro.id, lStopMetro.name, lStopMetro.lon, lStopMetro.lat);
                    foreach (string line in lStopMetro.lines)
                    {
                        lst.tlines.Add(searchTransportLine(line));
                    }
                    lStops.Add(lst);
                }
                else
                {   // l'arret existe deja -> ajouter les lignes qui n'ont pas encore été ajoutées
                    foreach (string line in lStopMetro.lines)
                    {
                        if (!lst.tlines.Exists(el => el.id.Equals(line)))
                        {
                            lst.tlines.Add(searchTransportLine(line));
                        }
                    }
                }
            }

        }


        static TransportLine searchTransportLine(string id)
        {
            WebRequest req = WebRequest.Create($"http://data.metromobilite.fr/api/routers/default/index/routes?codes={id}");
            WebResponse resp = req.GetResponse();

            Stream dataStream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            List<TransportLine> tLines = JsonConvert.DeserializeObject<List<TransportLine>>(reader.ReadToEnd());
            reader.Close();
            resp.Close();

            if (tLines.Count > 0)
            {
                return tLines[0];
            }
            else
            {
                TransportLine tLine = new TransportLine(id);
                return tLine;
            }
        }
    }
}
