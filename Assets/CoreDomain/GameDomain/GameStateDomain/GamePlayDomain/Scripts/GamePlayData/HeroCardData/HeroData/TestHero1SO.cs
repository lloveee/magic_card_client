using CoreDomain.Scripts.Utils;
using SpacetimeDB.Types;
using Unity.Properties;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData
{
    [CreateAssetMenu(fileName = "TestHero1SO", menuName = "GamePlay/HeroData/TestHero1", order = 0)]
    public class TestHero1SO : HeroCardSO
    {
        private TestHero1Stats Stats;
        [CreateProperty]
        public uint Heal = 0;
        public override HeroCard GetHeroCardData()
        {
            if (Stats == null)
            {
                Stats = new TestHero1Stats
                {
                    BaseStats = new PlayerStats(MaxHealth, CurrentHealth, MaxMana, CurrentMana),
                    Heal = this.Heal
                };
            }
            
            HeroCard data = new HeroCard
            {
                CardName = cardName,
                CardDescription = description,
                Stats = new StatsUnion.Hero1(Stats)
            };
            return data;
        }

        public override void SetHeroCardData(HeroCard heroCardData)
        {
            var heroStats = heroCardData.Stats.TryGet<TestHero1Stats>();
            if (heroStats != null)
            {
                Stats = heroStats;
                Debug.Log("[new] => "+ Stats.Heal);
            }
            cardName = heroCardData.CardName;
            description = heroCardData.CardDescription;
        }
    }
}