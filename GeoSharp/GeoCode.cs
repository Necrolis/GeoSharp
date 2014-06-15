using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GeoSharp
{
    class GeoCode
    {
        /* I'm torn between using OSM data, regional heirachical polygons (though this is more for the reverse lookups) or the geoname stuff
         * For now I'm going to just focus on parsing out the names to allow for non-strict structuring etc.
         * Later on I'll probably provide a bit of both depending on what resolutions/accuracies are required
         */
        private Dictionary<string, GeoName> Names;

        public GeoCode(string Database, bool MajorPlacesOnly)
        {
            using (FileStream fs = new FileStream(Database, FileMode.Open))
            {
                Initialize(fs, MajorPlacesOnly);
            }
        }

        public GeoCode(Stream Database, bool MajorPlacesOnly)
        {
            Initialize(Database, MajorPlacesOnly);
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

            Names = new Dictionary<string, GeoName>();
            foreach(var p in Places)
            {

            }
        }

        public List<GeoName> GetPossibleLocations(string Location)
        {
            List<GeoName> Places = new List<GeoName>();


            return Places;
        }
    }
}
