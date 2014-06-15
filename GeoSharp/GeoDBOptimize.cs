using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeoSharp
{
    public struct PointD //why doesn't .Net have a non-windows specific version of this :'(
    {
        public double X;
        public double Y;

        public PointD(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static PointD operator -(PointD a, PointD b)
        {
            return new PointD(a.X - b.X, a.Y - b.Y);
        }
    }

    public struct OptStats
    {
        public long RecordsIn;
        public long RecordsOut;

        public OptStats(long In, long Out)
        {
            RecordsIn = In;
            RecordsOut = Out;
        }
    }

    public class GeoDBOptimize
    {
        //copied from: http://stackoverflow.com/questions/10673740/how-to-check-if-a-point-x-y-is-inside-a-polygon-in-the-cartesian-coordinate-sy
        private static bool PointInPolyManifold(PointD Point, List<PointD> Poly)
        {
            double angle = 0;
            int size = Poly.Count;
            for (int i = 0; i < size; i++)
            {
                angle += Angle2D(Poly[i] - Point, Poly[(i + 1) % size] - Point);
            }

            return Math.Abs(angle) < Math.PI ? false : true;
        }

        private static double Angle2D(PointD Point1, PointD Point2)
        {
            double theta1 = Math.Atan2(Point1.Y, Point1.X);
            double theta2 = Math.Atan2(Point2.Y, Point2.X);
            double delta = theta2 - theta1;

            if (delta > Math.PI)
                delta -= Math.PI + Math.PI;
            else if (delta < -Math.PI)
                delta += Math.PI + Math.PI;

            return delta;
        }

        private static bool FilterRecord(string[] Fields, string FeatureClassFilter, string CountryFilter, List<List<PointD>> AreaFilter)
        {
            if(FeatureClassFilter.IndexOf(Fields[6],StringComparison.Ordinal) == -1)
                return false;

            if (CountryFilter.IndexOf(Fields[8], StringComparison.Ordinal) == -1)
                return false;

            PointD LatLng = new PointD(double.Parse(Fields[4]), double.Parse(Fields[5]));
            foreach(var poly in AreaFilter)
            {
                if (!PointInPolyManifold(LatLng, poly))
                    return false;
            }

            return true;
        }

        private static bool FilterRecord(string[] Fields, string FeatureClassFilter, string CountryFilter)
        {
            if (FeatureClassFilter.IndexOf(Fields[6], StringComparison.Ordinal) == -1)
                return false;

            if (CountryFilter.IndexOf(Fields[8], StringComparison.Ordinal) == -1)
                return false;

            return true;
        }

        public delegate bool Filter(string[] Fields);

        public static OptStats OptimizeDatabase(Stream Input, Stream Output, List<GeoFeatureClass> FeatureClassFilter, List<string> CountryFilter, List<List<PointD>> AreaFilter)
        {
            long RecordsIn = 0;
            long RecordsOut = 0;
            string CountryFilterCompiled = string.Join("|", CountryFilter.ToArray());
            StringBuilder sb = new StringBuilder();
            foreach (var f in FeatureClassFilter)
                sb.Append(GeoName.ClassToCode(f));

            string FeatureClassFilterCompiled = sb.ToString();

            using (StreamReader _in = new StreamReader(Input))
            {
                using (StreamWriter _out = new StreamWriter(Output))
                {
                    string Line;
                    while (!_in.EndOfStream && (Line = _in.ReadLine()) != null)
                    {
                        RecordsIn++;
                        if (FilterRecord(Line.Split('\t'), FeatureClassFilterCompiled, CountryFilterCompiled, AreaFilter))
                        {
                            RecordsOut++;
                            _out.WriteLine(Line);
                        }
                    }
                }
            }

            return new OptStats(RecordsIn, RecordsOut);
        }

        public static OptStats OptimizeDatabase(Stream Input, Stream Output, List<GeoFeatureClass> FeatureClassFilter, List<string> CountryFilter)
        {
            long RecordsIn = 0;
            long RecordsOut = 0;
            string CountryFilterCompiled = string.Join("|", CountryFilter.ToArray());
            StringBuilder sb = new StringBuilder();
            foreach (var f in FeatureClassFilter)
                sb.Append(GeoName.ClassToCode(f));

            string FeatureClassFilterCompiled = sb.ToString();

            using (StreamReader _in = new StreamReader(Input))
            {
                using (StreamWriter _out = new StreamWriter(Output))
                {
                    string Line;
                    while (!_in.EndOfStream && (Line = _in.ReadLine()) != null)
                    {
                        RecordsIn++;
                        if (FilterRecord(Line.Split('\t'), FeatureClassFilterCompiled, CountryFilterCompiled))
                        {
                            RecordsOut++;
                            _out.WriteLine(Line);
                        }
                    }
                }
            }

            return new OptStats(RecordsIn, RecordsOut);
        }

        public static OptStats OptimizeDatabase(Stream Input, Stream Output, Filter Filter)
        {
            long RecordsIn = 0;
            long RecordsOut = 0;

            using (StreamReader _in = new StreamReader(Input))
            {
                using (StreamWriter _out = new StreamWriter(Output))
                {
                    string Line;
                    while (!_in.EndOfStream && (Line = _in.ReadLine()) != null)
                    {
                        RecordsIn++;
                        if (Filter(Line.Split('\t')))
                        {
                            RecordsOut++;
                            _out.WriteLine(Line);
                        }
                    }
                }
            }

            return new OptStats(RecordsIn, RecordsOut);
        }
    }
}
