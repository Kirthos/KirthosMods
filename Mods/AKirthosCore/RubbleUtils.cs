using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Mods.TechTree;
using Eco.Shared.Math;
using Eco.Shared.Networking;
using System;
using System.Linq;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/29/2018
 */

namespace Kirthos.Mods.Utils
{
    public class RubbleUtils
    {
        /// <summary>
        /// Verify what carry the user and take the rubble the player carry or the nearest rubble if player hand is empty
        /// </summary>
        public static void PickUpRubble(User user, int range = 10, int qty = 20)
        {
            try
            {
                VerifyCarry(user.Inventory.Carried, user, range, qty);
            }
            catch(Exception)
            {

            }
        }

        /// <summary>
        /// Pick up the T rubble without verify what the user carry
        /// </summary>
        public static void PickRubble<T>(User user, int range, int qty) where T : RubbleObject
        {
            Type firstItemGet = null;
            Type itemType = null;
            int count = user.Inventory.Carried.Stacks.First<ItemStack>().Quantity;
            if (count >= qty)
                return;
            foreach (RubbleObject obj in NetObjectManager.GetObjectsOfType<T>())
            {
                if (obj.IsBreakable)
                {
                    continue;
                }
                if (Vector3.Distance(user.Position, obj.Position) < range)
                {
                    if (firstItemGet == null)
                    {
                        firstItemGet = obj.GetType();
                        if (firstItemGet.ToString().Contains("StoneRubbleSet"))
                        {
                            itemType = typeof(StoneItem);
                        }
                        else if (firstItemGet.ToString().Contains("CoalRubbleSet"))
                        {
                            itemType = typeof(CoalItem);
                        }
                        else if (firstItemGet.ToString().Contains("CopperOreRubbleSet"))
                        {
                            itemType = typeof(CopperOreItem);
                        }
                        else if (firstItemGet.ToString().Contains("GoldOreRubbleSet"))
                        {
                            itemType = typeof(GoldOreItem);
                        }
                        else if (firstItemGet.ToString().Contains("IronOreRubbleSet"))
                        {
                            itemType = typeof(IronOreItem);
                        }
                    }
                    if (itemType == typeof(StoneItem) && !obj.GetType().ToString().Contains("StoneRubbleSet"))
                    {
                        continue;
                    }
                    else if (itemType == typeof(CoalItem) && !obj.GetType().ToString().Contains("CoalRubbleSet"))
                    {
                        continue;
                    }
                    else if (itemType == typeof(CopperOreItem) && !obj.GetType().ToString().Contains("CopperOreRubbleSet"))
                    {
                        continue;
                    }
                    else if (itemType == typeof(GoldOreItem) && !obj.GetType().ToString().Contains("GoldOreRubbleSet"))
                    {
                        continue;
                    }
                    else if (itemType == typeof(IronOreItem) && !obj.GetType().ToString().Contains("IronOreRubbleSet"))
                    {
                        continue;
                    }
                    if (obj.AuthorizedToInteract(user))
                    {
                        if (obj.TryPickup(user.Inventory).IsSuccess)
                        {
                            count++;
                            if (count >= qty)
                                break;
                        }
                    }
                }
            }
        }

        private static void VerifyCarry(LimitedInventory carry, User user, int range, int qty)
        {
            if (carry.IsEmpty)
            {
                PickRubble<RubbleObject>(user, range, qty);
            }
            else
            {
                Item itemCarried = carry.Stacks.First<ItemStack>().Item;
                if (itemCarried is StoneItem)
                {
                    PickRubble<RubbleObject<StoneItem>>(user, range, qty);
                }
                else if (itemCarried is CoalItem)
                {
                    PickRubble<RubbleObject<CoalItem>>(user, range, qty);
                }
                else if (itemCarried is CopperOreItem)
                {
                    PickRubble<RubbleObject<CopperOreItem>>(user, range, qty);
                }
                else if (itemCarried is GoldOreItem)
                {
                    PickRubble<RubbleObject<GoldOreItem>>(user, range, qty);
                }
                else if (itemCarried is IronOreItem)
                {
                    PickRubble<RubbleObject<IronOreItem>>(user, range, qty);
                }
            }
        }

        public static void BreakBigRubble(Vector3i blockPosition, int percent)
        {
            Random rng = new Random();
            if (rng.Next(100) < percent)
            {
                foreach (RubbleObject obj in NetObjectManager.GetObjectsOfType<RubbleObject>())
                {
                    if (!obj.IsBreakable)
                    {
                        continue;
                    }
                    if (Vector3.Distance(blockPosition, obj.Position) < 1)
                    {
                        obj.Breakup();
                    }
                }
            }
        }
    }
}
