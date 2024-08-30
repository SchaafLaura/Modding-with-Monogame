using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Modding
{
    public class Player
    {
        public Vector2 Position     { get; set; } = Vector2.Zero;
        public Point Size           { get; set; } = new Point(30, 30);
        public float Speed          { get; set; } = 5;


        List<Upgrade> upgrades = new List<Upgrade>();
        TimeSpan bulletCooldown = TimeSpan.FromSeconds(0.001);
        TimeSpan lastBulletFired = TimeSpan.Zero;
        Bullet prototype = new();
        float bulletSpeed = 20;
        Texture2D texture;

        public void AddUpgrade(Upgrade u)
        {
            // TODO: verify some shit
            upgrades.Add(u);
            if (u.BulletUpgrade is not null)
                MakeBulletPrototype();
        }

        private void MakeBulletPrototype()
        {
            var bullet = new Bullet();
            foreach(var u in upgrades)
            {
                var x = bullet.Size.X;
                var y = bullet.Size.Y;
                var f = u.BulletUpgrade!.BulletUpgradeData!.Value.SizeMult;
                bullet.Size = new Point((int)(x * f), (int)(y * f));
            }
            prototype = bullet.Clone();
        }

        public void MoveAtMaxSpeedInDirection(Vector2 direction)
        {
            if (direction.X == 0 && direction.Y == 0)
                return;
            direction = direction.OfMag(Speed);
            Position += direction;
        }

        public Bullet TryFireBulletAt(GameTime t, Vector2 p)
        {
            if (t.TotalGameTime - lastBulletFired < bulletCooldown)
                return null;
            lastBulletFired = t.TotalGameTime;

            var bulletSpawnPosition = Position + new Vector2((30 - 5) / 2, 0);
            var d = p - bulletSpawnPosition;
            d = d.OfMag(bulletSpeed);
            var bulletVelocity = d;

            var bullet = prototype.Clone();
            bullet.Position = bulletSpawnPosition;
            bullet.Velocity = bulletVelocity;

            return bullet;
        }

        public void SetTexture(GraphicsDevice gd)
        {
            texture = new Texture2D(gd, 1, 1);
            texture.SetData(new Color[] { Color.HotPink });
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle(Position.ToPoint(), Size), Color.White);
        }
    }
}
