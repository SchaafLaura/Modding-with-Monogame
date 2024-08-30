using Modding;
using Microsoft.Xna.Framework;
namespace UserScripts
{
    internal class MyOtherEnemy : EnemySpawner
    {
        public override Enemy GetEnemyPrototype()
        {
            Enemy e = new Enemy();
            e.Color = new Color(1.0f, 0.0f, 0.0f);

            e.Move = (self, playerPosition) =>
            {
                self.Position += new Vector2(RNGProvider.RNG.Next(-2, 3), RNGProvider.RNG.Next(-2, 3));
            };

            e.Tier = 0;
            return e;
        }
    }
}