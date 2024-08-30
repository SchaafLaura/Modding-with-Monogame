using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class EnemySpawner
{
    public abstract Enemy GetEnemyPrototype();
}

public sealed class Enemy
{
    public Point Size { get; set; } = new Point(20, 20);
    public int Tier { get; set; }
    public Color Color;
    public Vector2 Position;
    public Action<Enemy, Vector2> Move;

    Texture2D texture;

    public Enemy Clone()
    {
        Enemy enemy = new Enemy()
        {
            Tier = Tier,
            Color = Color,
            Position = Position,
            Move = Move
        };
        return enemy;
    }

    public void SetTexture(GraphicsDevice gd)
    {
        texture = new Texture2D(gd, 1, 1);
        texture.SetData(new Color[] { Color });
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(texture, new Rectangle(Position.ToPoint(), Size), Color.White);
    }
}