using Eco.Core.Plugins;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;
using Kirthos.Mods.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/10/2018
 */

namespace Kirthos.Mods.MolecularStorage
{
    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(PowerConsumptionComponent))]
    [RequireComponent(typeof(PowerGridComponent))]
    public class MolecularStorageObject : WorldObject
    {

       [Serialized] private Type itemTypeStored;
       [Serialized] private int itemStoredAmount;

        private const int MAX_QUANTITY = 5000;
        private User molecularUser;

        private Inventory storage;

        public override string FriendlyName { get { return "Molecular Storage"; } }

        protected override void Initialize()
        {
            GetComponent<PropertyAuthComponent>().Initialize();
            GetComponent<PowerConsumptionComponent>().Initialize(500);
            GetComponent<PowerGridComponent>().Initialize(10, new ElectricPower());

            if (itemTypeStored == null)
                itemTypeStored = typeof(Item);
            this.GetComponent<PublicStorageComponent>().Initialize(2);
            molecularUser = User.CreateUser("MolecularUser", "MolecularSteamId", "MolecularSlgId", StorageManager.CreateStorageHandle("Users", $"MolecularUser"));
            storage = this.GetComponent<PublicStorageComponent>().Storage;
            storage.AddInvRestriction(new MolecularStorageRestriction(itemTypeStored, MAX_QUANTITY - itemStoredAmount, Operating));
            storage.OnChangedDetailed.Add(InventoryChanged);
            this.SetAnimatedState("setText", $"{itemStoredAmount}");

            string storedItemName = "Empty";
            if (itemTypeStored != typeof(Item))
            {
                storedItemName = Item.Get(itemTypeStored).FriendlyName;
            }
            this.SetAnimatedState("setItem", $"{storedItemName}");
        }

        public override InteractResult OnActInteract(InteractionContext context)
        {
            storage.ClearRestrictions();
            storage.AddInvRestriction(new MolecularStorageRestriction(itemTypeStored, MAX_QUANTITY - itemStoredAmount, Operating));
            if (itemTypeStored != typeof(Item) && itemStoredAmount > 0)
            {
                if (storage.IsEmpty && Operating)
                {
                    int maxSize = Item.Get(itemTypeStored).MaxStackSize;
                    storage.AddItems(itemTypeStored, itemStoredAmount > maxSize ? maxSize : itemStoredAmount, molecularUser);
                }
            }
            return base.OnActInteract(context);
        }

        private void InventoryChanged(User user, IEnumerable<KeyValuePair<Type, int>> changes, Dictionary<ItemStack, ChangedStack> changedStack)
        {
            if (user != null && user.Name == "MolecularUser")
                return;
            storage.ClearRestrictions();
            var change = changes.First();
            if (change.Value > 0)
            {
                if (itemTypeStored == typeof(Item))
                    itemTypeStored = change.Key;
                itemStoredAmount += change.Value;
                if (itemStoredAmount > Item.Get(change.Key).MaxStackSize)
                    storage.TryRemoveItems(change.Key, change.Value, molecularUser);
            }
            else if (change.Value < 0)
            {
                itemStoredAmount += change.Value;
                if (itemStoredAmount > -change.Value)
                {
                    if (Operating)
                      storage.TryAddItems(change.Key, -change.Value, molecularUser);
                }
                else if (itemStoredAmount > 0)
                {
                    if (Operating)
                      storage.TryAddItems(change.Key, itemStoredAmount, molecularUser);
                }
                else
                {
                    itemTypeStored = typeof(Item);
                }
            }
            storage.AddInvRestriction(new MolecularStorageRestriction(itemTypeStored, MAX_QUANTITY - itemStoredAmount, Operating));
            string storedItemName = "Empty";
            if (itemTypeStored == typeof(Item))
            {
                if (user != null)
                  user.Player.SendTemporaryMessage($"Empty");
            }
            else
            {
                storedItemName = Item.Get(itemTypeStored).FriendlyName;
                if (user != null)
                 user.Player.SendTemporaryMessage($"Stored: {itemStoredAmount} {Item.Get(itemTypeStored).UILink()}");
            }
            this.SetAnimatedState("setText", $"{itemStoredAmount}");
            this.SetAnimatedState("setItem", $"{storedItemName}");
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }

    [Serialized]
    [MaxStackSize(20)]
    [Weight(5000)]
    public class MolecularStorageItem : WorldObjectItem<MolecularStorageObject>
    {
        public override string FriendlyName { get { return "Molecular Storage"; } }
        public override string Description { get { return "A molecular storage box that can store a huge amount of one items."; } }
    }

    [ModRequiresSkill("MachineryCraftingSkill", typeof(IndustrialEngineeringSkill), 1)]
    public class MolecularStorageRecipe : Recipe
    {
        public MolecularStorageRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<MolecularStorageItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {

            };
            if (ModsUtil.IsModInstalled("TransportPipe"))
                this.CraftMinutes = CreateCraftTimeValue(typeof(MolecularStorageRecipe), Item.Get<MolecularStorageItem>().UILinkContent(), 5, ModsUtil.GetTypeFromMod("MachineryCraftingSpeedSkill"));
            else
                this.CraftMinutes = CreateCraftTimeValue(typeof(MolecularStorageRecipe), Item.Get<MolecularStorageItem>().UILinkContent(), 5, typeof(IndustrialEngineeringSpeedSkill));
            this.Initialize("Molecular Storage", typeof(MolecularStorageRecipe));

            CraftingComponent.AddRecipe(typeof(AssemblyLineObject), this);
        }
    }
}
