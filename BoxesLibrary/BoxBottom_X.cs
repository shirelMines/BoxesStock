using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesLibrary
{
   public class BoxBottom_X: IComparable<BoxBottom_X>
    {
        public double Bottom { get; set; }
        public BinarySearchTree<BoxHeight_Y> YTree { get; set; }

        public BoxBottom_X(double x)
        {
            Bottom= x;
        }

        public BoxBottom_X(double x,double y,int count)
        {
            Bottom = x;
            YTree = new BinarySearchTree<BoxHeight_Y>();
            YTree.Add(new BoxHeight_Y(y, count));
        }

        public BoxBottom_X(double x, BoxHeight_Y boxHeightY)
        {
            Bottom = x;
            YTree = new BinarySearchTree<BoxHeight_Y>();
            YTree.Add(boxHeightY);
        }

        public int CompareTo(BoxBottom_X other)
        {
            return Bottom.CompareTo(other.Bottom);
        }

    }
}
