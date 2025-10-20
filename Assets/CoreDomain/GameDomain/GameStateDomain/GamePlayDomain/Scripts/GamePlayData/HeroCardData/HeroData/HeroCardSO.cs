using System.Collections.Generic;
using SpacetimeDB.Types;
using Unity.Properties;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData
{
    public abstract class HeroCardSO : ScriptableObject
    {
        [CreateProperty]
        public string cardName;
        [CreateProperty]
        public string description;
        [CreateProperty]
        public uint MaxHealth = 100;
        [CreateProperty]
        public uint CurrentHealth = 100;
        [CreateProperty]
        public uint MaxMana = 100;
        [CreateProperty]
        public uint CurrentMana = 0;

        public abstract HeroCard GetHeroCardData();
        public abstract void SetHeroCardData(HeroCard heroCardData);
        public abstract List<string> GetSkillDescription();
    }
}