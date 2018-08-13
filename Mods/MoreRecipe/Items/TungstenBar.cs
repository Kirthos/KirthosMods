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
    public partial class TungstenBarRecipe : Recipe
    {
        public TungstenBarRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<TungstenBarItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<TungstenOreItem>(typeof(AlloySmeltingEfficiencySkill), 10, AlloySmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(TungstenBarRecipe), Item.Get<TungstenBarItem>().UILinkContent(), 10f, typeof(AlloySmeltingSpeedSkill));
            this.Initialize("Tungsten Bar", typeof(TungstenBarRecipe));

            CraftingComponent.AddRecipe(typeof(BlastFurnaceObject), this);
        }
    }


    [Serialized]
    [Weight(10000)]
    [Currency]
    public partial class TungstenBarItem :
    Item
    {
        public override string FriendlyName { get { return "Tungsten Bar"; } }
        public override string Description { get { return "A rigid and solid tungsten bar"; } }
    }
}
