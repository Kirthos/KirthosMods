using Eco.Gameplay.Components;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Kirthos.Mods.TransportPipe.Skills;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 06/16/2018
 */

namespace Kirthos.Mods.TransportPipe.Items
{
    //*
    [RequiresSkill(typeof(PipeCraftingSkill), 3)]
    [RepairRequiresSkill(typeof(PipeCraftingSkill), 1)]
    public class SteelWrenchRecipe : Recipe
    {
        public SteelWrenchRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<SteelWrenchItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<SteelItem>(typeof(PipeCraftingEfficiencySkill), 20, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<IronIngotItem>(typeof(PipeCraftingEfficiencySkill), 20, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<GearItem>(typeof(PipeCraftingEfficiencySkill), 10, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(SteelWrenchRecipe), Item.Get<SteelWrenchItem>().UILinkContent(), 15f, typeof(PipeCraftingSpeedSkill));
            this.Initialize("Steel Wrench", typeof(SteelWrenchRecipe));

            CraftingComponent.AddRecipe(typeof(AnvilObject), this);
        }
    }
    //*/

    [Serialized]
    [Weight(1000)]
    [Category("Tool")]
    public class SteelWrenchItem : WrenchItem
    {
        public override string Description { get { return "Steel Wrench used to configure input and output for pipe."; } }
        public override string FriendlyName { get { return "Steel Wrench"; } }

        private static SkillModifiedValue caloriesBurn = CreateCalorieValue(20, typeof(PipeCraftingEfficiencySkill), typeof(SteelWrenchItem), new SteelWrenchItem().UILinkContent());
        public override IDynamicValue CaloriesBurn { get { return caloriesBurn; } }

        private static SkillModifiedValue skilledRepairCost = new SkillModifiedValue(20, PipeCraftingSkill.MultiplicativeStrategy, typeof(PipeCraftingSkill), Localizer.DoStr("repair cost"));
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }

        public override float DurabilityRate { get { return DurabilityMax / 250; } }

        public override Item RepairItem { get { return Item.Get<SteelItem>(); } }
        public override int FullRepairAmount { get { return 20; } }
    }
}
