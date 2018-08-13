using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;
using Kirthos.Mods.MoreRecipe.Blocks;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/19/2018
 */

namespace Kirthos.Mods.MoreRecipe.Items
{
    [RequiresSkill(typeof(AlloySmeltingSkill), 2)]
    public partial class LeadIngotRecipe : Recipe
    {
        public LeadIngotRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<LeadIngotItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<LeadOreItem>(typeof(AlloySmeltingEfficiencySkill), 10, AlloySmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(LeadIngotRecipe), Item.Get<LeadIngotItem>().UILinkContent(), 10f, typeof(AlloySmeltingSpeedSkill));
            this.Initialize("Lead Ingot", typeof(LeadIngotRecipe));

            CraftingComponent.AddRecipe(typeof(BlastFurnaceObject), this);
        }
    }


    [Serialized]
    [Weight(10000)]
    [Currency]
    public partial class LeadIngotItem :
    Item
    {
        public override string FriendlyName { get { return "Lead Ingot"; } }
        public override string Description { get { return "An ingot made of lead"; } }
    }
}
