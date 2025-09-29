using SpacetimeDB.Types;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData
{
    public abstract class HeroCardSO : ScriptableObject
    {
        public string cardName;
        public string description;

        public abstract HeroCard TryUpdateData();
    }
}