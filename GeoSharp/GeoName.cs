using System;
using System.Collections.Generic;

namespace GeoSharp
{
    public class GeoName : KDTree.IKDComparator<GeoName>
    {
        public string Name;
        public bool IsMajorPlace;
        public double Latitude;
        public double Longitude;

        public GeoName(string Data)
        {
            string[] Names = Data.Split('\t');
            if (Names.Length < 6) //we only use the first 6, there are actually 19
                throw new ArgumentException("Invalid GeoName Record");

            this.Name = Names[1];
            this.IsMajorPlace = Names[6].Equals("P");
            this.Latitude = double.Parse(Names[4]);
            this.Longitude = double.Parse(Names[5]);
        }

        public GeoName(double Latitude, double Longitude)
        {
            Name = "Search";
            this.Latitude = Latitude;
            this.Longitude = Longitude;
        }

        private double Deg2Rad(double Deg)
        {
            return (Deg * Math.PI / 180.0);
        }

        // The following methods are used purely for the KD-Tree
        // They don't convert lat/lon to any particular coordinate system
        public double GetX()
        {
            return Math.Cos(Deg2Rad(Latitude)) * Math.Cos(Deg2Rad(Longitude));
        }

        public double GetY()
        {
            return Math.Cos(Deg2Rad(Latitude)) * Math.Sin(Deg2Rad(Longitude));
        }

        public double GetZ()
        {
            return Math.Sin(Deg2Rad(Latitude));
        }

        public override string ToString()
        {
            return Name;
        }

        internal class CompareX : IComparer<GeoName>
        {
            public int Compare(GeoName x, GeoName y)
            {
                return x.GetX().CompareTo(y.GetX());
            }
        }

        internal class CompareY : IComparer<GeoName>
        {
            public int Compare(GeoName x, GeoName y)
            {
                return x.GetY().CompareTo(y.GetY());
            }
        }

        internal class CompareZ : IComparer<GeoName>
        {
            public int Compare(GeoName x, GeoName y)
            {
                return x.GetZ().CompareTo(y.GetZ());
            }
        }

        public IComparer<GeoName> Comparator(KDTree.Axis Axis)
        {
 	        switch(Axis)
            {
                case KDTree.Axis.X: return new CompareX();
                case KDTree.Axis.Y: return new CompareY();
                case KDTree.Axis.Z: return new CompareZ();
            }

            throw new ArgumentException("Invalid Axis");
        }

        public double SquaredDistance(GeoName Location)
        {
            double x = GetX() - Location.GetX();
            double y = GetY() - Location.GetY();
            double z = GetZ() - Location.GetZ();
            return (x * x) + (y * y) + (z * z);
        }

        public double AxisSquaredDistance(GeoName Location, KDTree.Axis Axis)
        {
            double Distance;
            if (Axis == KDTree.Axis.X)
                Distance = GetX() - Location.GetX();
            else if (Axis == KDTree.Axis.Y)
                Distance = GetY() - Location.GetY();
            else
                Distance = GetZ() - Location.GetZ();

            return Distance * Distance;
        }
    }
}
