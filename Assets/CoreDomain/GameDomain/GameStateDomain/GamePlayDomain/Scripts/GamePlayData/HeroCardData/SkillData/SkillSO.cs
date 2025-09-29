using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.SkillData
{
    public abstract class SkillSO : ScriptableObject, ISkill
    {
        public abstract string SkillId { get; }
        public abstract void Activate(GamePlayContext context);
    }
}