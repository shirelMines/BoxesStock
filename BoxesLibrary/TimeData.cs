using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesLibrary
{
    class TimeData
    {
        public double Bottom { get; set; }
        public double Height { get; set; }
        public DateTime DateLastPurchase { get; set; }

        public TimeData(double width_x, double height_y)
        {
            Bottom= width_x;
            Height= height_y;
            DateLastPurchase = DateTime.Now;
        } 

    }
}
