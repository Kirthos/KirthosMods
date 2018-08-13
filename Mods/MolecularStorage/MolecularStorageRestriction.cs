using Eco.Gameplay.Items;
using Eco.Gameplay.Systems.TextLinks;
using System;
using System.Linq;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 06/22/2018
 */

namespace Kirthos.Mods.MolecularStorage
{
    public class MolecularStorageRestriction : InventoryRestriction
    {
        private Type storedItem;
        private int amountLeft;
        private bool operating;

        private string message = "";
        public override string Message => this.message;
        public override bool SurpassStackSize => true;

        public MolecularStorageRestriction(Type storedItem, int amountLeft, bool operating)
        {
            this.storedItem = storedItem;
            this.amountLeft = amountLeft;
            this.operating = operating;
        }

        public override int MaxAccepted(Item item, int currentQuantity)
        {
            if (operating == false)
            {
                message = "The storage need power to compact item.";
                return 0;
            }
            if (storedItem != typeof(Item) && storedItem != item.Type)
            {
                message = "You can only store " + Item.Get(storedItem).UILink();
                return 0;
            }
            if (item.MaxStackSize > amountLeft)
            {
                message = "The molecular storage is full";
                return amountLeft;
            }
            return amountLeft;
        }
    }
}
