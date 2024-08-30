using Microsoft.Xna.Framework;

namespace Modding
{
    public static class Vector2Extensions
    {
        public static float Mag(this Vector2 v)
        {
            var sq = v.MagSq();
            var m = Math.Sqrt(sq);
            return (float)m;
        }
        public static float MagSq(this Vector2 v)
        {
            return v.X * v.X + v.Y * v.Y;
        }
        public static Vector2 OfMag(this Vector2 v, float mag)
        {
            v.Normalize();
            v.X *= mag;
            v.Y *= mag;
            return v;
        }

        public static Point ToPoint(this Vector2 v)
        {
            return new Point((int)v.X, (int)v.Y);
        }
    }
}
