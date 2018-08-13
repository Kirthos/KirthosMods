using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;
using Eco.World;
using Eco.World.Blocks;
using Kirthos.Mods.TransportPipe.Skills;
using System;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/12/2018
 */

namespace Kirthos.Mods.TransportPipe.Items
{
    [RequiresSkill(typeof(AdvancedPetrolRefiningSkill), 1)]
    public class LubricantRecipe : Recipe
    {
        public LubricantRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<LubricantItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<PetroleumItem>(typeof(AdvancedPetrolRefiningEfficiencySkill), 10, AdvancedPetrolRefiningEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(LubricantRecipe), Item.Get<LubricantItem>().UILinkContent(), 5, typeof(AdvancedPetrolRefiningSpeedSkill));
            this.Initialize("Lubricant", typeof(LubricantRecipe));

            CraftingComponent.AddRecipe(typeof(OilRefineryObject), this);
        }
    }

    [Serialized]
    [Solid]
    public class LubricantBarrelBlock : PickupableBlock { }

    /*
    [Serialized]
    [Solid]
    public class LubricantBarrelPickupableBlock : PickupableBlock { }
    //*/

    [Serialized]
    [MaxStackSize(10)]
    [Weight(10000)]
    public class LubricantItem :
    BlockItem<LubricantBarrelBlock>
    {
        public override string FriendlyName { get { return "Lubricant"; } }
        public override string FriendlyNamePlural { get { return "Lubricant"; } }
        public override string Description { get { return "An extremely useful material used for transport pipe."; } }

        /*
        private static Type[] blockTypes = new Type[] {
            typeof(LubricantBarrelPickupableBlock)
        };
        public override Type[] BlockTypes { get { return blockTypes; } }
        //*/
    }
}
