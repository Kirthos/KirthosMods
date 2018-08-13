using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;
using Eco.World;
using Eco.World.Blocks;
using Kirthos.Mods.TransportPipe.Eco;
using Kirthos.Mods.TransportPipe.Skills;
using System;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/12/2018
 */

namespace Kirthos.Mods.TransportPipe.Items
{
    [RequiresSkill(typeof(PipeCraftingSkill), 1)]
    public class WoodenTransportPipeRecipe : Recipe
    {
        public WoodenTransportPipeRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<WoodenTransportPipeItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<LumberItem>(typeof(PipeCraftingEfficiencySkill), 5, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<IronIngotItem>(typeof(PipeCraftingEfficiencySkill), 5, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<WoodenGearItem>(typeof(PipeCraftingEfficiencySkill), 5, PipeCraftingEfficiencySkill.MultiplicativeStrategy)
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(WoodenTransportPipeRecipe), Item.Get<WoodenTransportPipeItem>().UILinkContent(), 1, typeof(PipeCraftingSpeedSkill));
            this.Initialize("Wooden Transport Pipe", typeof(WoodenTransportPipeRecipe));

            CraftingComponent.AddRecipe(typeof(WainwrightTableObject), this);
        }
    }

    [Serialized]
    [Solid, Constructed, Wall]
    [IsForm("Floor", typeof(WoodenTransportPipeItem))]
    public class WoodenTransportPipeBlock : BaseTransportPipeBlock { }

    [Serialized]
    [MaxStackSize(20)]
    [Weight(2000)]
    public class WoodenTransportPipeItem : BlockItem<BaseTransportPipeBlock>
    {
        public override string FriendlyName { get { return "Wooden transport pipe"; } }
        public override string Description { get { return "A wooden pipe that use mechanical energy to transport items. (1 item every 5 second)"; } }

        private static Type[] blockTypes = new Type[] {
            typeof(WoodenTransportPipeStacked1Block)
        };
        public override Type[] BlockTypes { get { return blockTypes; } }
    }

    [Serialized, Solid] public class WoodenTransportPipeStacked1Block : PickupableBlock { }
    //[Serialized, Solid] public class TransportPipeStacked2Block : PickupableBlock { }
}
