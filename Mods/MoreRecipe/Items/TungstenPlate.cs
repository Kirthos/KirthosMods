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
    public partial class TungstenPlateRecipe : Recipe
    {
        public TungstenPlateRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<TungstenBarItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<TungstenBarItem>(typeof(SteelworkingEfficiencySkill), 10, SteelworkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(TungstenPlateRecipe), Item.Get<TungstenPlateItem>().UILinkContent(), 15f, typeof(SteelworkingSpeedSkill));
            this.Initialize("Tungsten Plate", typeof(TungstenPlateRecipe));

            CraftingComponent.AddRecipe(typeof(ElectricStampingPressObject), this);
        }
    }


    [Serialized]
    [Weight(10000)]
    [Currency]
    public partial class TungstenPlateItem :
    Item
    {
        public override string FriendlyName { get { return "Tungsten Plate"; } }
        public override string Description { get { return "A rigid and solid plate made of tungsten"; } }
    }
}
