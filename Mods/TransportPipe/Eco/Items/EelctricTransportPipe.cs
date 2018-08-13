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
    [RequiresSkill(typeof(PipeCraftingSkill), 3)]
    public class ElectricTransportPipeRecipe : Recipe
    {
        public ElectricTransportPipeRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<ElectricTransportPipeItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<FiberglassItem>(typeof(PipeCraftingEfficiencySkill), 5, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<SteelItem>(typeof(PipeCraftingEfficiencySkill), 10, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<GearItem>(typeof(PipeCraftingEfficiencySkill), 5, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<LubricantItem>(typeof(PipeCraftingEfficiencySkill), 1, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<CircuitItem>(typeof(PipeCraftingEfficiencySkill), 1, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(ElectricTransportPipeRecipe), Item.Get<ElectricTransportPipeItem>().UILinkContent(), 3, typeof(PipeCraftingSpeedSkill));
            this.Initialize("Electric Transport Pipe", typeof(ElectricTransportPipeRecipe));

            CraftingComponent.AddRecipe(typeof(AssemblyLineObject), this);
        }
    }

    [Serialized]
    [IsForm("Floor", typeof(ElectricTransportPipeItem))]
    public class ElectricTransportPipeBlock : BaseTransportPipeBlock { }

    [Serialized]
    [MaxStackSize(20)]
    [Weight(2000)]
    public class ElectricTransportPipeItem : BlockItem<BaseTransportPipeBlock>
    {
        public override string FriendlyName { get { return "Electric transport pipe"; } }
        public override string Description { get { return "A electric pipe that transport items. (1 item per second)"; } }

        private static Type[] blockTypes = new Type[] {
            typeof(ElectricTransportPipeStacked1Block)
        };
        public override Type[] BlockTypes { get { return blockTypes; } }
    }

    [Serialized, Solid] public class ElectricTransportPipeStacked1Block : PickupableBlock { }
    //[Serialized, Solid] public class TransportPipeStacked2Block : PickupableBlock { }
}
