using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/19/2018
 */

namespace Kirthos.Mods.MoreRecipe.Items
{
    [RequiresSkill(typeof(SteelworkingSkill), 4)]
    public partial class LeadPlateRecipe : Recipe
    {
        public LeadPlateRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<LeadIngotItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<LeadIngotItem>(typeof(SteelworkingEfficiencySkill), 10, SteelworkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(LeadPlateRecipe), Item.Get<LeadPlateItem>().UILinkContent(), 15f, typeof(SteelworkingSpeedSkill));
            this.Initialize("Lead Plate", typeof(LeadPlateRecipe));

            CraftingComponent.AddRecipe(typeof(ElectricStampingPressObject), this);
        }
    }


    [Serialized]
    [Weight(10000)]
    [Currency]
    public partial class LeadPlateItem :
    Item
    {
        public override string FriendlyName { get { return "Lead Plate"; } }
        public override string Description { get { return "A plate made of Lead"; } }
    }
}
