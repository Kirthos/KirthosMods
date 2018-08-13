using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;
using System;
using System.Collections.Generic;
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
    [RequiresSkill(typeof(PrimitiveMechanicsSkill), 2)]
    public partial class WoodenGearRecipe : Recipe
    {
        public WoodenGearRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<WoodenGearItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<BoardItem>(typeof(PrimitiveMechanicsEfficiencySkill), 5, PrimitiveMechanicsEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(WoodenGearRecipe), Item.Get<WoodenGearItem>().UILinkContent(), 0.75f, typeof(PrimitiveMechanicsSpeedSkill));
            this.Initialize("Wooden Gear", typeof(WoodenGearRecipe));

            CraftingComponent.AddRecipe(typeof(WainwrightTableObject), this);
        }
    }


    [Serialized]
    [Weight(500)]
    [Currency]
    public partial class WoodenGearItem :
    Item
    {
        public override string FriendlyName { get { return "Wooden Gear"; } }
        public override string Description { get { return "A wooden toothed machine part that interlocks with others."; } }

    }
}
