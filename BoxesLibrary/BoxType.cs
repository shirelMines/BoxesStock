using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesLibrary
{
    public class BoxType
    {
        public double Bottom { get; set; }
        public double Height { get; set; }
        public BoxHeight_Y DataY { get; set; }
        public BoxBottom_X BoxX { get; set; }

        public BoxType( BoxBottom_X boxX, BoxHeight_Y dataY)
        {
            BoxX = boxX;
            Bottom = boxX.Bottom;
            Height = dataY.Height;
            DataY = dataY;
        }
    }
}
