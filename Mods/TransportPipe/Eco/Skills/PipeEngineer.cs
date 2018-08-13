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
 * Created by Kirthos 04/12/2018
 */

namespace Kirthos.Mods.TransportPipe.Skills
{
    [Serialized]
    [RequiresSkill(typeof(AutomationSkill), 0)]
    public class PipeEngineerSkill : Skill
    {
        public override string FriendlyName { get { return "Pipes Engineer"; } }
        public override string Description { get { return ""; } }

        public static int[] SkillPointCost = { 1, 1, 1, 1, 1 };
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        public override int MaxLevel { get { return 1; } }
    }

    [Serialized]
    public class PipeEngineerSkillBook : SkillBook<PipeEngineerSkill, PipeEngineerSkillScroll>
    {
        public override string FriendlyName { get { return "Pipes Engineer Skill Book"; } }
    }

    [Serialized]
    public class PipeEngineerSkillScroll : SkillScroll<PipeEngineerSkill, PipeEngineerSkillBook>
    {
        public override string FriendlyName { get { return "Pipes Engineer Skill Scroll"; } }
    }

    [RequiresSkill(typeof(BasicEngineeringSkill), 0)]
    public class PipeEngineerSkillBookRecipe : Recipe
    {
        public PipeEngineerSkillBookRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<PipeEngineerSkillBook>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<IronIngotItem>(typeof(ResearchEfficiencySkill), 50, ResearchEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<GearItem>(typeof(ResearchEfficiencySkill), 50, ResearchEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<LumberItem>(typeof(ResearchEfficiencySkill), 75, ResearchEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = new ConstantValue(60);

            this.Initialize("Pipes Engineer Skill Book", typeof(PipeEngineerSkillBookRecipe));
            if (TransportPipePlugin.Conf.Get<bool>("CraftScrollInsteadOfBook") == false)
                CraftingComponent.AddRecipe(typeof(ResearchTableObject), this);
        }
    }

    [RequiresSkill(typeof(BasicEngineeringSkill), 0)]
    public class PipeEngineerSkillScrollRecipe : Recipe
    {
        public PipeEngineerSkillScrollRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<PipeEngineerSkillScroll>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<IronIngotItem>(typeof(ResearchEfficiencySkill), 50, ResearchEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<GearItem>(typeof(ResearchEfficiencySkill), 50, ResearchEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<LumberItem>(typeof(ResearchEfficiencySkill), 75, ResearchEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = new ConstantValue(60);

            this.Initialize("Pipes Engineer Skill Scroll", typeof(PipeEngineerSkillScrollRecipe));
            if (TransportPipePlugin.Conf.Get<bool>("CraftScrollInsteadOfBook"))
                CraftingComponent.AddRecipe(typeof(ResearchTableObject), this);
        }
    }
}
