using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesLibrary
{
    public class BoxHeight_Y: IComparable<BoxHeight_Y>
    {
        public double Height { get; set; }
        public int Count { get; set; }
        internal DateLinkedList<TimeData>.Node TimeNodeRef  { get; set; }

        public BoxHeight_Y(double y, int count)
        {
            Height = y;
            Count = count;
        }

        public BoxHeight_Y(double y)
        {
            Height = y;
        }
        public int CompareTo(BoxHeight_Y other)
        {
            return Height.CompareTo(other.Height);
        }

    }
}
