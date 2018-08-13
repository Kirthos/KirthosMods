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
    [RequiresSkill(typeof(PipeCraftingSkill), 2)]
    [RepairRequiresSkill(typeof(PipeCraftingSkill), 1)]
    public class WoodenWrenchRecipe : Recipe
    {
        public WoodenWrenchRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<WoodenWrenchItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<LumberItem>(typeof(PipeCraftingEfficiencySkill), 10, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<BoardItem>(typeof(PipeCraftingEfficiencySkill), 20, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<WoodenGearItem>(typeof(PipeCraftingEfficiencySkill), 10, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(WoodenWrenchRecipe), Item.Get<WoodenWrenchItem>().UILinkContent(), 5, typeof(PipeCraftingSpeedSkill));
            this.Initialize("Wooden Wrench", typeof(WoodenWrenchRecipe));

            CraftingComponent.AddRecipe(typeof(WainwrightTableObject), this);
        }
    }

    [Serialized]
    [Weight(1000)]
    [Category("Tool")]
    public class WoodenWrenchItem : WrenchItem
    {
        public override string Description { get { return "Wooden Wrench used to configure input and output for pipe."; } }
        public override string FriendlyName { get { return "Wooden Wrench"; } }

        private static SkillModifiedValue caloriesBurn = CreateCalorieValue(40, typeof(PipeCraftingEfficiencySkill), typeof(WoodenWrenchItem), new WoodenWrenchItem().UILinkContent());
        public override IDynamicValue CaloriesBurn { get { return caloriesBurn; } }

        private static SkillModifiedValue skilledRepairCost = new SkillModifiedValue(10, PipeCraftingSkill.MultiplicativeStrategy, typeof(PipeCraftingSkill), Localizer.DoStr("repair cost"));
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }

        public override float DurabilityRate { get { return DurabilityMax / 100; } }

        public override Item RepairItem { get { return Item.Get<LumberItem>(); } }
        public override int FullRepairAmount { get { return 10; } }
    }
}
