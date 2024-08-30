using Microsoft.Xna.Framework;
namespace Modding
{
    public class UpgradeRepository
    {
        public List<Upgrade>[] upgradePrototypes;
        public void AddUpgradePrototype(Upgrade u)
        {
            if (upgradePrototypes[u.Tier] is null)
                upgradePrototypes[u.Tier] = [];
            upgradePrototypes[u.Tier].Add(u);
        }

        public Upgrade? GetUpgrade(int tier)
        {
            var list = upgradePrototypes[tier];
            if (list is null)
                return null;

            var u = list[RNGProvider.RNG.Next(list.Count)];
            return u.Clone();
        }
    }

    public abstract class UpgradeSpawner
    {
        public abstract Upgrade GetUpgradePrototype();
    }

    public readonly record struct PlayerUpgradeData
    {
        public PlayerUpgradeData()              { }
        public readonly float MaxHealthAdd      { get; init; } = 0;
        public readonly float MaxHealthMult     { get; init; } = 1;
        public readonly float SpeedAdd          { get; init; } = 0;
        public readonly float SpeedMult         { get; init; } = 1;
    }

    public readonly record struct BulletUpgradeData
    {
        public BulletUpgradeData()              { }
        public readonly float VelocityAdd       { get; init; } = 0;
        public readonly float VelocityMult      { get; init; } = 1;
        public readonly float SizeAdd           { get; init; } = 0;
        public readonly float SizeMult          { get; init; } = 1;
        public readonly float DamageAdd         { get; init; } = 0;
        public readonly float DamageMult        { get; init; } = 1;
    }
    
    public sealed class BulletUpgrade
    {
        public Action<Bullet>? OnBulletUpdate           { get; init; }
        public Action<Enemy>? OnBulletHit               { get; init; }
        public BulletUpgradeData? BulletUpgradeData     { get; init; }
        public BulletUpgrade Clone()
        {
            return new BulletUpgrade()
            {
                OnBulletUpdate      = OnBulletUpdate,
                OnBulletHit         = OnBulletHit,
                BulletUpgradeData   = BulletUpgradeData,
            };
        }
    }

    public sealed class PlayerUpgrade
    {
        public Action<Player>? OnBulletFired            { get; init; }
        public Action<Player>? OnPlayerWasHit           { get; init; }
        public PlayerUpgradeData? PlayerUpgradeData     { get; init; }
        public PlayerUpgrade Clone()
        {
            return new PlayerUpgrade()
            {
                OnBulletFired       = OnBulletFired,
                OnPlayerWasHit      = OnPlayerWasHit,
                PlayerUpgradeData   = PlayerUpgradeData,
            };
        }
    }

    public sealed class Upgrade
    {
        public int Tier                         { get; init; }
        public BulletUpgrade? BulletUpgrade     { get; init; }
        public PlayerUpgrade? PlayerUpgrade     { get; init; }

        public Upgrade Clone()
        {
            return new Upgrade()
            {
                Tier            = Tier,
                BulletUpgrade   = BulletUpgrade?.Clone(),
                PlayerUpgrade   = PlayerUpgrade?.Clone()
            };
        }

        public void bla()
        {
            var bud = new BulletUpgradeData() { SizeMult = 1.5f };
            var pud = new PlayerUpgradeData() { SpeedMult = 0.8f };

            var bu = new BulletUpgrade()
            {
                BulletUpgradeData = bud,
                OnBulletHit = (self) => { self.Color = ColorExtensions.RandomColor(); }
            };

            var pu = new PlayerUpgrade()
            {
                PlayerUpgradeData = pud
            };

            var upgrade = new Upgrade()
            {
                Tier = 2,
                PlayerUpgrade = pu,
                BulletUpgrade = bu
            };
        }
    }
}