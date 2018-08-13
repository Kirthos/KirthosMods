namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;

    [Serialized]
    public partial class JDElectricalSkill : Skill
    {
        public override string FriendlyName { get { return "Electrical"; } }
        public override string Description { get { return ""; } }

        public static int[] SkillPointCost = { 1, 1, 1, 1, 1 };
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        public override int MaxLevel { get { return 1; } }
    }

    [Serialized]
    public partial class JDElectricSkillBook : SkillBook<JDElectricSkill, JDElectricSkillScroll>
    {
        public override string FriendlyName { get { return "Electric Book"; } }
    }

    [Serialized]
    public partial class JDElectricSkillScroll : SkillScroll<JDElectricSkill, JDElectricSkillBook>
    {
        public override string FriendlyName { get { return "Electric Scroll"; } }
    }

    //[RequiresSkill(typeof(JDResearcherSkill), 2)]
    public partial class JDElectricSkillScrollRecipe : Recipe
    {
        public JDElectricSkillScrollRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<JDElectricSkillScroll>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<CopperWiringItem>(typeof(ResearchEfficiencySkill), 25, ResearchEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<IronIngotItem>(typeof(ResearchEfficiencySkill), 50, ResearchEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<PaperItem>(typeof(ResearchEfficiencySkill), 300, ResearchEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = new ConstantValue(360);
            this.Initialize("Electric Scroll", typeof(JDElectricSkillScrollRecipe));
            if (JDElectricalPlugin.Conf.Get<bool>("CraftScrollInsteadBook"))
                CraftingComponent.AddRecipe(typeof(ResearchTableObject), this);
        }
    }

    //[RequiresSkill(typeof(FarmingSkill), 0)] 
    public partial class JDElectricSkillBookRecipe : Recipe
    {
        public JDElectricSkillBookRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<JDElectricSkillBook>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<CopperWiringItem>(typeof(ResearchEfficiencySkill), 25, ResearchEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<IronIngotItem>(typeof(ResearchEfficiencySkill), 50, ResearchEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<PaperItem>(typeof(ResearchEfficiencySkill), 300, ResearchEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = new ConstantValue(360);

            this.Initialize("Electric Book", typeof(JDElectricSkillBookRecipe));
            if (JDElectricalPlugin.Conf.Get<bool>("CraftScrollInsteadBook") == false)
                CraftingComponent.AddRecipe(typeof(ResearchTableObject), this);
        }
    }
}
