using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Gameplay.Property;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Mods.TechTree;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Kirthos.Mods.TransportPipe.Components;
using Kirthos.Mods.TransportPipe.Eco;
using Kirthos.Mods.TransportPipe.Items;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 06/22/2018
 */

namespace Kirthos.Mods.TransportPipe.Objects
{
    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(AutomaticCraftingComponent))]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(PowerConsumptionComponent))]
    [RequireComponent(typeof(PowerGridComponent))]
    public class PrinterObject : WorldObject
    {
        public override string FriendlyName { get { return "3D Printer"; } }

        static PrinterObject()
        {
            foreach(var r in CraftingComponent.RecipesOnWorldObject(typeof(WorkbenchObject)))
                CraftingComponent.AddRecipe(typeof(PrinterObject), r);

            WorldObject.AddOccupancy<PrinterObject>(new List<BlockOccupancy>(){
                new BlockOccupancy(new Vector3i(0, 0, -1), typeof(BeltSlotBlock)),
                new BlockOccupancy(new Vector3i(-1, 0, -1)),
                new BlockOccupancy(new Vector3i(1, 0, -1)),
                new BlockOccupancy(new Vector3i(0, 0, 0)),
                new BlockOccupancy(new Vector3i(-1, 0, 0)),
                new BlockOccupancy(new Vector3i(1, 0, 0), typeof(BeltSlotBlock)),
                new BlockOccupancy(new Vector3i(0, 0, 1), typeof(BeltSlotBlock)),
                new BlockOccupancy(new Vector3i(-1, 0, 1)),
                new BlockOccupancy(new Vector3i(1, 0, 1)),

                new BlockOccupancy(new Vector3i(0, 1, -1)),
                new BlockOccupancy(new Vector3i(-1, 1, -1)),
                new BlockOccupancy(new Vector3i(1, 1, -1)),
                new BlockOccupancy(new Vector3i(0, 1, 0)),
                new BlockOccupancy(new Vector3i(-1, 1, 0)),
                new BlockOccupancy(new Vector3i(1, 1, 0)),
                new BlockOccupancy(new Vector3i(0, 1, 1)),
                new BlockOccupancy(new Vector3i(-1, 1, 1)),
                new BlockOccupancy(new Vector3i(1, 1, 1)),

                new BlockOccupancy(new Vector3i(0, 2, -1)),
                new BlockOccupancy(new Vector3i(-1, 2, -1)),
                new BlockOccupancy(new Vector3i(1, 2, -1)),
                new BlockOccupancy(new Vector3i(0, 2, 0)),
                new BlockOccupancy(new Vector3i(-1, 2, 0)),
                new BlockOccupancy(new Vector3i(1, 2, 0)),
                new BlockOccupancy(new Vector3i(0, 2, 1)),
                new BlockOccupancy(new Vector3i(-1, 2, 1)),
                new BlockOccupancy(new Vector3i(1, 2, 1)),
            });

        }

        public List<PipeEntry> entries = new List<PipeEntry>();
        public PipeEntry MainEntry;

        protected override void Initialize()
        {
            entries.Add(new PipeEntry(new Vector3i(0, 0, -1), this));
            entries.Add(new PipeEntry(new Vector3i(0, 0, 1), this));
            entries.Add(new PipeEntry(new Vector3i(1, 0, 0), this));
            MainEntry = new PipeEntry(new Vector3i(0, 0, 0), this);
            entries.Add(MainEntry);

            GetComponent<PropertyAuthComponent>().Initialize(AuthModeType.Inherited);
            GetComponent<PowerConsumptionComponent>().Initialize(2000);
            GetComponent<PowerGridComponent>().Initialize(10, new ElectricPower());
        }

        public override InteractResult OnActInteract(InteractionContext context)
        {
            List<KeyValuePair<Type, int>> itemsList = new List<KeyValuePair<Type, int>>();

            itemsList = MainEntry.GetLinker().GetAllItemsInput(this);
            
            foreach(var kvp in itemsList)
            {
                Console.WriteLine($"{kvp.Value} : {kvp.Key.ToString().Split('.').Last()}");
            }
            return base.OnActInteract(context);
        }

        public override void Destroy()
        {
            foreach (var entry in entries)
                entry.Destroy();
            base.Destroy();
        }

        [Serialized] public bool State = false; // False means door is close

        public override InteractResult OnActRight(InteractionContext context)
        {
            State = !State;
            this.SetAnimatedState("Open", State); // This ask the client to open or close the door
            return base.OnActRight(context);
        }

    }

    [Serialized]
    [MaxStackSize(20)]
    [Weight(5000)]
    public class PrinterItem : WorldObjectItem<PrinterObject>
    {
        public override string FriendlyName { get { return "3D Printer"; } }
        public override string Description { get { return "A 3D printer capable of crafting simple recipe automatically"; } }
    }
}
