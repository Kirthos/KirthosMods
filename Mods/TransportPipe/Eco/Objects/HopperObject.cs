using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Math;
using Eco.Shared.Networking;
using Eco.Shared.Serialization;
using Eco.World;
using Eco.World.Blocks;
using Kirthos.Mods.TransportPipe.Eco;
using Kirthos.Mods.TransportPipe.Items;
using Kirthos.Mods.TransportPipe.Skills;
using System;
using System.Collections.Generic;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/23/2018
 */

namespace Kirthos.Mods.TransportPipe.Objects
{
    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    public class HopperObject : WorldObject
    {
        static HopperObject()
        {
            WorldObject.AddOccupancy<HopperObject>(new List<BlockOccupancy>(){
                new BlockOccupancy(new Vector3i(0, 0, 1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(0, 0, -1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(1, 0, 1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(-1, 0, 1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(1, 0, -1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(-1, 0, -1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(0, 1, 0), typeof(BeltSlotBlock)),
                new BlockOccupancy(new Vector3i(0, 1, 1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(0, 1, -1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(1, 1, 0), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(-1, 1, 0), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(1, 1, 1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(-1, 1, 1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(1, 1, -1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(-1, 1, -1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(0, 2, 0), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(0, 2, 1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(0, 2, -1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(1, 2, 0), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(-1, 2, 0), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(1, 2, 1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(-1, 2, 1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(1, 2, -1), typeof(SolidWorldObjectBlock)),
                new BlockOccupancy(new Vector3i(-1, 2, -1), typeof(SolidWorldObjectBlock)),
                });
        }

        public override string FriendlyName { get { return "Hopper"; } }

        private PipeEntry entry;
        public int tick;

        protected override void Initialize()
        {
            GetComponent<PropertyAuthComponent>().Initialize();
            World.OnBlockChanged.Add(BlockChanged);
            entry = new PipeEntry(new Vector3i(0, 1, 0), this);
            
        }

        public override void Destroy()
        {
            entry.Destroy();
            base.Destroy();
        }

        private void BlockChanged(Vector3i pos)
        {
            if (entry.GetLinker() != null)
            {
                Vector3i startPosition = Position3i + Vector3i.Up * 3;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (pos == startPosition + new Vector3i(i, 0, j))
                        {
                            Type blockType = World.GetBlock(pos).GetType();
                            if (blockType == typeof(EmptyBlock)) continue;
                            BlockItem item = BlockItem.CreatingItem(blockType);
                            ConnectorObject obj = entry.GetLinker().GetFirstValidOutput(this, item.GetType());
                            if (obj == null) continue;
                            obj.Output.GetComponent<PublicStorageComponent>().Inventory.TryAddItem(item);
                            World.DeleteBlock(pos);
                        }
                    }
                }
            }
        }


        public override void Tick()
        {
            tick++;
            if (tick >= 10)
            {
                Vector3i startPosition = Position3i + Vector3i.Up * 3;
                foreach (RubbleObject obj in NetObjectManager.GetObjectsOfType<RubbleObject>())
                {
                    if (Vector3.Distance(obj.Position, Position + Vector3.Up * 1) < 1.5f)
                    {
                        if (obj.IsBreakable)
                            obj.Breakup();
                        else if (entry.GetLinker() != null)
                        {
                            ConnectorObject output = entry.GetLinker().GetFirstValidOutput(this, obj.GetType());
                            if (output != null)
                                obj.TryPickup(output.Output.GetComponent<PublicStorageComponent>().Inventory);
                        }
                    }
                }
                tick = 0;
            }
        }
    }

    [Serialized]
    [MaxStackSize(20)]
    [Weight(5000)]
    public class HopperItem : WorldObjectItem<HopperObject>
    {
        public override string FriendlyName { get { return "Hopper"; } }
        public override string Description { get { return "A hopper."; } }
    }

    //*
    [RequiresSkill(typeof(PipeCraftingSkill), 2)]
    public class HopperItemRecipe : Recipe
    {
        public HopperItemRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<HopperItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<FiberglassItem>(typeof(PipeCraftingEfficiencySkill), 25, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<SteelItem>(typeof(PipeCraftingEfficiencySkill), 30, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<GearItem>(typeof(PipeCraftingEfficiencySkill), 20, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<LubricantItem>(typeof(PipeCraftingEfficiencySkill), 2, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<CircuitItem>(typeof(PipeCraftingEfficiencySkill), 5, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(HopperItemRecipe), Item.Get<HopperItem>().UILinkContent(), 30, typeof(PipeCraftingSpeedSkill));
            this.Initialize("Hopper", typeof(HopperItemRecipe));

            CraftingComponent.AddRecipe(typeof(AssemblyLineObject), this);
        }
    }
    //*/
}
