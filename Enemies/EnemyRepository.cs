namespace Modding
{
    public class EnemyRepository
    {
        public List<Enemy>[] enemyPrototypes;

        public void AddEnemyPrototype(Enemy e)
        {
            if (enemyPrototypes[e.Tier] is null)
                enemyPrototypes[e.Tier] = new List<Enemy>();
            enemyPrototypes[e.Tier].Add(e);
        }

        public Enemy? GetEnemy(int tier)
        {
            var list = enemyPrototypes[tier];
            if (list is null)
                return null;

            var e = list[RNGProvider.RNG.Next(list.Count)];
            return e.Clone();
        }
    }
}
