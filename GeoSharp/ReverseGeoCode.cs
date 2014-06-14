using GeoSharp.KDTree;
using System.Collections.Generic;
using System.IO;

//All the geocoding data can be gotten from: http://download.geonames.org/export/dump/
namespace GeoSharp
{
    /*
     * Hmmm, might be nice to get in a simple version that does country->regoin/province->district/municipality/prefecture
     * probably with a nice BVH using a decemated representation from OSM
     */
    public class ReverseGeoCode
    {
        private KDTree<GeoName> Tree;

        public ReverseGeoCode(string Database, bool MajorPlacesOnly)
        {
            using(var db = new FileStream(Database,FileMode.Open))
            {
                Initialize(db, MajorPlacesOnly);
            }
        }

        public ReverseGeoCode(Stream Input, bool MajorPlacesOnly)
        {
            Initialize(Input, MajorPlacesOnly);
        }

        public string NearestPlaceName(double Latitude, double Longitude)
        {
            return Tree.FindNearest(new GeoName(Latitude, Longitude)).Name;
        }

        public GeoName NearestPlace(double Latitude, double Longitude)
        {
            return Tree.FindNearest(new GeoName(Latitude, Longitude));
        }

        private void Initialize(Stream Input, bool MajorPlacesOnly)
        {
            List<GeoName> Places = new List<GeoName>();
            using (StreamReader db = new StreamReader(Input))
            {
                string Line;
                while (!db.EndOfStream && (Line = db.ReadLine()) != null)
                {
                    var Place = new GeoName(Line);
                    if (!MajorPlacesOnly || Place.FeatureClass != GeoFeatureClass.City)
                        Places.Add(Place);
                }
            }

            Tree = new KDTree<GeoName>(Places.ToArray());
        }
    }
}
