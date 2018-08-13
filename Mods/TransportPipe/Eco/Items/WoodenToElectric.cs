using Eco.Gameplay.Components;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.World;
using Kirthos.Mods.TransportPipe.Eco;
using Kirthos.Mods.TransportPipe.Objects;
using Kirthos.Mods.TransportPipe.Skills;
using System;
using System.ComponentModel;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/20/2018
 */

namespace Kirthos.Mods.TransportPipe.Items
{
    [RequiresSkill(typeof(PipeCraftingSkill), 3)]
    public class WoodenToElectricRecipe : Recipe
    {
        public WoodenToElectricRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<WoodenToElectricItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<FiberglassItem>(typeof(PipeCraftingEfficiencySkill), 5, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<SteelItem>(typeof(PipeCraftingEfficiencySkill), 10, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<LubricantItem>(typeof(PipeCraftingEfficiencySkill), 1, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<CircuitItem>(typeof(PipeCraftingEfficiencySkill), 1, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(WoodenToElectricRecipe), Item.Get<WoodenToElectricItem>().UILinkContent(), 15, typeof(PipeCraftingSpeedSkill));
            this.Initialize("Wooden to electric pipe upgrader", typeof(WoodenToElectricRecipe));

            CraftingComponent.AddRecipe(typeof(AssemblyLineObject), this);
        }
    }

    [Serialized]
    [Weight(500)]
    public class WoodenToElectricItem : Item, IInteractingItem
    {
        public override string Description { get { return "Wooden to electric pipe upgrader item"; } }
        public override string FriendlyName { get { return "Electric pipe upgrader"; } }

        public float InteractDistance => DefaultInteractDistance.Tool;

        public override InteractResult OnActLeft(InteractionContext context)
        {
            if (context.HasBlock && context.Block is WoodenTransportPipeBlock)
            {
                TransportPipeInfo info = null;
                if (TransportPipeManager.pipesInfo.TryGetValue(context.BlockPosition.Value, out info))
                {
                    if (context.Player.User.Inventory.TryRemoveItems(GetType(), 1))
                    {
                        info.type = PIPETYPE.Electric;
                        info.beltLinker.UpdateInfo(info);
                        World.SetBlock(typeof(ElectricTransportPipeBlock), context.BlockPosition.Value);
                        return InteractResult.Success;
                    }
                }
            }
            else if (context.HasTarget && context.Target is ConnectorObject)
            {
                TransportPipeInfo info = null;
                if (TransportPipeManager.pipesInfo.TryGetValue((context.Target as ConnectorObject).Position3i, out info))
                {
                    if (info.type == PIPETYPE.Wooden)
                    {
                        if (context.Player.User.Inventory.TryRemoveItems(GetType(), 1))
                        {
                            info.type = PIPETYPE.Electric;
                            info.beltLinker.UpdateInfo(info);
                            return InteractResult.Success;
                        }
                    }
                }
            }
            return InteractResult.NoOp;
        }

        public override InteractResult OnActRight(InteractionContext context)
        {
            if (context.HasBlock && context.Block is WoodenTransportPipeBlock)
            {
                TransportPipeInfo info = null;
                if (TransportPipeManager.pipesInfo.TryGetValue(context.BlockPosition.Value, out info))
                {
                    if (context.Player.User.Inventory.TryRemoveItems(GetType(), 1))
                    {
                        info.type = PIPETYPE.Electric;
                        info.beltLinker.UpdateInfo(info);
                        World.SetBlock(typeof(ElectricTransportPipeBlock), context.BlockPosition.Value);
                        return InteractResult.Success;
                    }
                }
            }
            else if (context.HasTarget && context.Target is ConnectorObject)
            {
                TransportPipeInfo info = null;
                if (TransportPipeManager.pipesInfo.TryGetValue((context.Target as ConnectorObject).Position3i, out info))
                {
                    if (context.Player.User.Inventory.TryRemoveItems(GetType(), 1))
                    {
                        info.type = PIPETYPE.Electric;
                        info.beltLinker.UpdateInfo(info);
                        return InteractResult.Success;
                    }
                }
            }
            return InteractResult.NoOp;
        }

        public override InteractResult OnActInteract(InteractionContext context)
        {
            return InteractResult.NoOp;
        }

        public bool ShouldHighlight(Type block)
        {
            return Block.Is<TransportPipeAttr>(block);
        }
    }
}
