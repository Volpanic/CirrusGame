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

        public static float SinWave(float from, float to, float duration, float offset, float timer)
        {
            float a4 = (to - from) * 0.5f;

            float si = from + a4 + (float)Math.Sin((((timer * 0.01f) + duration * offset) / duration) * ((float)Math.PI * 2.0f)) * a4;

            return si;
        }

        public static float WrapValue(float value, float min, float max)
        {
            float mod = (value - min) % (max - min);
            if (mod < 0.0f)
            {
                return mod + max;
            }
            else
            {
                return mod + min;
            }
        }

        public static int WrapValue(int value, int min, int max)
        {
            int mod = (value - min) % (max - min);
            if (mod < 0.0f)
            {
                return mod + max;
            }
            else
            {
                return mod + min;
            }
        }
    }
}
