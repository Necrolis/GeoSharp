
namespace GeoSharp.KDTree
{
    public class KDNode<T>
        where T : IKDComparator<T>
    {
        public KDNode<T> Left;
        public KDNode<T> Right;
        public T Location;

        public KDNode(KDNode<T> Left, KDNode<T> Right, T Location)
        {
            this.Left = Left;
            this.Right = Right;
            this.Location = Location;
        }
    }
}
