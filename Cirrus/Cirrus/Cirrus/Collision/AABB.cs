using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus.Cirrus.Collision
{
    public class AABB
    {
        public float X = 0;
        public float Y = 0; 

        public float width = 1;
        public float height = 1;

        public float Right { get{ return X + width;} }
        public float Bottem { get { return Y + height; } }

        public AABB(Vector2 topLeft, Vector2 size)
        {
            X = topLeft.X;
            Y = topLeft.Y;

            width = size.X;
            height = size.Y;
        }

        public AABB(float _X, float _Y, float _width, float _height)
        {
            X = _X;
            Y = _Y;

            width = _width;
            height = _height;
        }

        public AABB(Rectangle re) // i made my own class for the floats man
        {
            X = re.X;
            Y = re.Y;

            width = re.Width;
            height = re.Height;
        }

        public static explicit operator Rectangle(AABB box) => new Rectangle((int)box.X, (int)box.Y, (int)box.width, (int)box.height);

        public bool CollidingWith(AABB other)
        {
            return (X < other.Right && Right > other.X && Y < other.Bottem && Bottem > other.Y);
        }

        public bool PointInRect(Vector2 point)
        {
            return ((point.X > X && point.X < Right) && (point.Y > Y && point.Y < Bottem));
        }

        public static bool RectCollision(AABB rect1, AABB rect2)
        {
            return rect1.CollidingWith(rect2);
        }
    }
}
