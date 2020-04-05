using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus.Cirrus.Helpers
{
    public static class MathMore
    {
        public static bool PointInRectangle(Point pt, Rectangle rect)
        {
            return ((pt.X >= rect.X && pt.X <= rect.X + rect.Width) && (pt.Y >= rect.Y && pt.Y <= rect.Y + rect.Height));
  
        }

        public static bool PointInRectangle(Vector2 pt, Rectangle rect)
        {
            return ((pt.X >= rect.X && pt.X <= rect.X + rect.Width) && (pt.Y >= rect.Y && pt.Y <= rect.Y + rect.Height));

        }

        public static float Approach(float value, float target, float amount)
        {
            if (value < target)
            {
                value = Math.Min(value + amount, target);
            }
            else
            {
                value = Math.Max(value - amount, target);
            }
            return value;
        }
    }
}
