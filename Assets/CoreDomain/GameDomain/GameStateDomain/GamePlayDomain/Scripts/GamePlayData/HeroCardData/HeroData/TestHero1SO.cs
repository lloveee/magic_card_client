using SpacetimeDB.Types;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData
{
    [CreateAssetMenu(fileName = "TestHero1SO", menuName = "GamePlay/HeroData/TestHero1", order = 0)]
    public class TestHero1SO : HeroCardSO
    {
        private TestHero1Stats Stats;
        public uint Heal = 0;
        public override HeroCard TryUpdateData()
        {
            Stats = new TestHero1Stats
            {
                BaseStats = new PlayerStats(100, 100, 100, 0),
                Heal = this.Heal
            };
            HeroCard data = new HeroCard
            {
                CardName = cardName,
                CardDescription = description,
                Stats = new StatsUnion.Hero1(Stats)
            };
            return data;
        }
    }
}