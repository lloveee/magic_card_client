using SpacetimeDB.Types;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData
{
    [CreateAssetMenu(fileName = "TestHero2SO", menuName = "GamePlay/HeroData/TestHero2", order = 0)]
    public class TestHero2SO : HeroCardSO
    {
        private TestHero2Stats Stats;
        public uint Heal = 10;
        public override HeroCard TryUpdateData()
        {
            Stats = new TestHero2Stats
            {
                BaseStats = new PlayerStats(100, 100, 100, 0),
                Heal = this.Heal
            };
            HeroCard data = new HeroCard
            {
                CardName = cardName,
                CardDescription = description,
                Stats = new StatsUnion.Hero2(Stats)
            };
            return data;
        }
    }
}