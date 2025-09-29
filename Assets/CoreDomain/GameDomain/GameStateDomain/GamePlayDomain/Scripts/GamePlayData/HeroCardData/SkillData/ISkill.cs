namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.SkillData
{
    public interface ISkill
    {
        string SkillId { get; }
        void Activate(GamePlayContext context);
    }
}