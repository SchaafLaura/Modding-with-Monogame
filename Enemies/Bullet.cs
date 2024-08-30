using Vector2 = Microsoft.Xna.Framework.Vector2;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Microsoft.Xna.Framework.Graphics;

namespace Modding
{
    public class Bullet
    {
        public Vector2 Position { get; set; }
        public Vector2 LastPosition { get; private set; }
        public Vector2 Velocity { get; set; }
        public Point Size { get; set; } = new Point(5, 5);

        Texture2D texture;

        public Bullet Clone()
        {
            return new Bullet()
            {
                Position = Position,
                LastPosition = LastPosition,
                Velocity = Velocity,
                Size = Size
            };
        }

        public void Update()
        {
            LastPosition = Position;
            Position += Velocity;
        }

        public void SetTexture(GraphicsDevice gd)
        {
            texture = new Texture2D(gd, 1, 1);
            texture.SetData(new Color[] { new Color(0.6f, 0.1f, 0.3f) });
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle(Position.ToPoint(), Size), Color.White);
        }

        public bool HitRect(Rectangle r)
        {
            var stepSize = Size.X * 0.25f;
            var numSteps = (int)(Velocity.Mag() / stepSize);
            var step = Velocity.OfMag(stepSize);

            var hitbox = new Rectangle(LastPosition.ToPoint(), Size);
            var currentHitbox = new Rectangle(LastPosition.ToPoint(), Size);
            int i = 0;
            do
            {
                if (currentHitbox.Intersects(r))
                    return true;
                hitbox.Offset(step * i);
                currentHitbox = new Rectangle(hitbox.Location, hitbox.Size);
                hitbox.Offset(-step * i);
            }
            while (i++ < numSteps);
            return false;
        }

    }
}
