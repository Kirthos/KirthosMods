namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Skills;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;

    [Serialized]
    [RequiresSkill(typeof(JDElectricSkill), 1)]
    public partial class BasicESmeltingSkill : Skill
    {
        public override string FriendlyName { get { return "Electric Smelting"; } }
        public override string Description { get { return ""; } }

        public static int[] SkillPointCost = { 5, 10, 15, 20, 25 };
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        public override int MaxLevel { get { return 5; } }
    }
}
