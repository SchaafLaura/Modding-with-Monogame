using Microsoft.Xna.Framework;
namespace Modding
{
    public static class ColorExtensions
    {
        public static Color RandomColor()
        {
            return new Color(RNGProvider.RNG.Next(256), RNGProvider.RNG.Next(256), RNGProvider.RNG.Next(256));
        }
    }
}
