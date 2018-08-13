using Eco.Gameplay.Components;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Kirthos.Mods.TransportPipe.Items;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/10/2018
 */

namespace Kirthos.Mods.TransportPipe.Skills
{
    [Serialized]
    [RequiresSkill(typeof(AutomationSkill), 0)]
    public class AdvancedMachinerySkill : Skill
    {
        public override string FriendlyName { get { return "Advanced Machinery"; } }
        public override string Description { get { return ""; } }

        public static int[] SkillPointCost = { 1, 1, 1, 1, 1 };
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        public override int MaxLevel { get { return 1; } }
    }

    [Serialized]
    public class AdvancedMachinerySkillBook : SkillBook<AdvancedMachinerySkill, AdvancedMachinerySkillScroll>
    {
        public override string FriendlyName { get { return "Advanced Machinery Skill Book"; } }
    }

    [Serialized]
    public class AdvancedMachinerySkillScroll : SkillScroll<AdvancedMachinerySkill, AdvancedMachinerySkillBook>
    {
        public override string FriendlyName { get { return "Advanced Machinery Skill Scroll"; } }
    }

    [RequiresSkill(typeof(IndustrySkill), 0)]
    public class AdvancedMachinerySkillBookRecipe : Recipe
    {
        public AdvancedMachinerySkillBookRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<AdvancedMachinerySkillBook>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<IronIngotItem>(typeof(ResearchEfficiencySkill), 50, ResearchEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<GearItem>(typeof(ResearchEfficiencySkill), 50, ResearchEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<LumberItem>(typeof(ResearchEfficiencySkill), 75, ResearchEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = new ConstantValue(60);

            this.Initialize("Advanced Machinery Skill Book", typeof(AdvancedMachinerySkillBookRecipe));
            CraftingComponent.AddRecipe(typeof(ResearchTableObject), this);
        }
    }

}
