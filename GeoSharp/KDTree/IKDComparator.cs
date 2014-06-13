using System.Collections.Generic;

namespace GeoSharp.KDTree
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    public interface IKDComparator<T>
    {
        IComparer<T> Comparator(Axis Axis);
    
        // Return squared distance between current and other
        double SquaredDistance(T Other);
    
        // Return squared distance between one axis only
        double AxisSquaredDistance(T Other, Axis Axis);
    }
}
