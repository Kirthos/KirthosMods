using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
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
    [RequireComponent(typeof(OnOffComponent))]
    public class QuarryObject : WorldObject
    {
        static QuarryObject()
        {
            WorldObject.AddOccupancy<QuarryObject>(new List<BlockOccupancy>(){
                new BlockOccupancy(new Vector3i(0, 0, 0), typeof(BeltSlotBlock))
                });
        }

        public override string FriendlyName { get { return "Quarry"; } }

        private PipeEntry entry;

        public const int MINING_SIZE = 5;
        public const float MINING_TIME = 0;
        private float actualMiningTime = 0;

        [Serialized] public Vector3i blockMinedPos;

        protected override void Initialize()
        {
            GetComponent<PropertyAuthComponent>().Initialize();
            if (blockMinedPos == Vector3i.Zero)
                blockMinedPos = Rotation.Forward.Floor - (Rotation.Right.Floor * (int)(((float)MINING_SIZE / 2f) + 0.5f));
            entry = new PipeEntry(new Vector3i(0, 0, 0), this);
        }

        public override void Destroy()
        {
            entry.Destroy();
            base.Destroy();
        }

        public override void Tick()
        {
            actualMiningTime += WorldObjectManager.TickDeltaTime;
            if (actualMiningTime >= MINING_TIME && Operating)
            {
                Vector3i previousMiningBlock = blockMinedPos;
                blockMinedPos += Rotation.Right.Floor;
                int testPos = 0;
                if (Rotation.Right.Floor.x < 0)
                    testPos = -blockMinedPos.x;
                else if (Rotation.Right.Floor.x > 0)
                    testPos = blockMinedPos.x;
                else if (Rotation.Right.Floor.z < 0)
                    testPos = -blockMinedPos.z;
                else if (Rotation.Right.Floor.z > 0)
                    testPos = blockMinedPos.z;
                if (testPos == (MINING_SIZE / 2) + 1)
                {
                    blockMinedPos += Rotation.Forward.Floor;
                    blockMinedPos += -Rotation.Right.Floor * MINING_SIZE;
                }

                if (Rotation.Forward.Floor.x < 0)
                    testPos = -blockMinedPos.x;
                else if (Rotation.Forward.Floor.x > 0)
                    testPos = blockMinedPos.x;
                else if (Rotation.Forward.Floor.z < 0)
                    testPos = -blockMinedPos.z;
                else if (Rotation.Forward.Floor.z > 0)
                    testPos = blockMinedPos.z;

                if (testPos == MINING_SIZE + 1)
                {
                    blockMinedPos += -Rotation.Up.Floor;
                    blockMinedPos += -Rotation.Forward.Floor * MINING_SIZE;
                }
                Block minedBlock = World.GetBlock(blockMinedPos + Position3i);
                if (minedBlock.Is<Impenetrable>())
                {
                    GetComponent<OnOffComponent>().SetOnOff(Creator.Player, false);
                    blockMinedPos = previousMiningBlock;
                    return;
                }
                Item minedItem = null;
                if (minedBlock.Is<Diggable>())
                {
                    minedItem = BlockItem.CreatingItem(minedBlock.GetType());
                }
                if (minedBlock.Is<Minable>())
                {
                    if (minedBlock is StoneBlock)
                        minedItem = Item.Create(typeof(StoneItem));
                    else if (minedBlock is CoalBlock)
                        minedItem = Item.Create(typeof(CoalItem));
                    else if (minedBlock is CopperOreBlock)
                        minedItem = Item.Create(typeof(CopperOreItem));
                    else if (minedBlock is IronOreBlock)
                        minedItem = Item.Create(typeof(IronOreItem));
                    else if (minedBlock is GoldOreBlock)
                        minedItem = Item.Create(typeof(GoldOreItem));
                }
                if (minedItem != null && entry.GetLinker() != null)
                {
                    //, minedBlock.Is<Minable>() ? 4 : 1
                    ConnectorObject obj = entry.GetLinker().GetFirstValidOutput(this, minedItem.GetType());
                    if (obj == null)
                    {
                        blockMinedPos = previousMiningBlock;
                        return;
                    }
                    obj.Output.GetComponent<PublicStorageComponent>().Inventory.TryAddItems(minedItem.Type, minedBlock.Is<Minable>() ? 4 : 1);
                }
                World.DeleteBlock(blockMinedPos + Position3i);
                actualMiningTime = 0;
            }
        }
    }

    [Serialized]
    [MaxStackSize(20)]
    [Weight(5000)]
    public class QuarryItem : WorldObjectItem<QuarryObject>
    {
        public override string FriendlyName { get { return "Quarry"; } }
        public override string Description { get { return "A quarry."; } }
    }

    /*
    [RequiresSkill(typeof(PipeCraftingSkill), 3)]
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

            CraftingComponent.AddRecipe(typeof(FactoryObject), this);
        }
    }
    //*/
}
