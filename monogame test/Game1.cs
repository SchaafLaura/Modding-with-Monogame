using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Modding;

namespace monogame_test
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player player = new();

        List<Bullet> bullets = new();

        List<Enemy> enemies = new();

        EnemyRepository enemyRepo;
        UpgradeRepository upgradeRepo;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;

            Window.Title = "The Best Game of All Time";

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            enemyRepo = new EnemyRepository();
            enemyRepo.Init();

            upgradeRepo = new UpgradeRepository();
            upgradeRepo.Init();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            player.SetTexture(GraphicsDevice);

            int n = 15;
            int m = 25;
            for (int j = 0; j < m; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    var e = enemyRepo.GetEnemy(tier: 0);
                    e.Position = new Vector2(100 + i * 100, 100 + j * 30);
                    e.SetTexture(GraphicsDevice);
                    enemies.Add(e);
                }
            }

            var u = upgradeRepo.GetUpgrade(tier: 0);
            player.AddUpgrade(u);
        }

        protected override void Update(GameTime gameTime)
        {
            // exit if escape pressed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // move player
            player.MoveAtMaxSpeedInDirection(GetMovementDirection());

            // pewpew
            var ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed)
            {
                var b = player.TryFireBulletAt(gameTime, new Vector2(ms.Position.X, ms.Position.Y));
                if (b is not null)
                {
                    b.SetTexture(GraphicsDevice);
                    bullets.Add(b);
                }
            }

            // update bullets
            foreach (var b in bullets)
                b.Update();

            
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                // delete offscreen bullets
                if (!Inbounds(bullets[i].Position, 100))
                {
                    bullets.RemoveAt(i);
                    continue;
                }
                    
                // murder
                var b = bullets[i];
                var count = enemies.Count;
                for(int j = count - 1; j >= 0; j--)
                {
                    var re = new Rectangle(enemies[j].Position.ToPoint(), enemies[j].Size);
                    if (b.HitRect(re))
                    {
                        bullets.RemoveAt(i);
                        enemies.RemoveAt(j);
                        goto NextBullet;
                    }
                }
            NextBullet:;
            }
            
            // update enemies
            foreach(var e in enemies)
                e.Move(e, player.Position);

            // :shrug:
            base.Update(gameTime);
        }

        private Vector2 GetMovementDirection()
        {
            var dir = new Vector2(0, 0);
            var ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.S))
                dir.Y += 1;
            if (ks.IsKeyDown(Keys.W))
                dir.Y -= 1;
            if (ks.IsKeyDown(Keys.D))
                dir.X += 1;
            if (ks.IsKeyDown(Keys.A))
                dir.X -= 1;
            return dir;
        }

        private bool Inbounds(Vector2 v, float margin = 0)
        {
            return !(
                v.X < -margin ||
                v.Y < -margin ||
                v.X > _graphics.PreferredBackBufferWidth  + margin ||
                v.Y > _graphics.PreferredBackBufferHeight + margin
            );
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            player.Draw(_spriteBatch);

            foreach (var b in bullets)
                b.Draw(_spriteBatch);
            
            foreach(var e in enemies)
                e.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
