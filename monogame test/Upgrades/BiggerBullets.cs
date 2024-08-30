using Modding;

namespace UserScripts
{
    internal class BiggerBullets : UpgradeSpawner
    {
        public override Upgrade GetUpgradePrototype()
        {
            var bud = new BulletUpgradeData() { SizeMult = 10f };
            var bu = new BulletUpgrade()
            {
                BulletUpgradeData = bud,
            };


            return new Upgrade()
            {
                Tier = 0,
                BulletUpgrade = bu
            };
        }
    }
}
