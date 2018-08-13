using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Shared.Math;
using Eco.World;
using System;
using System.Collections.Generic;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/04/2018
 */

namespace Kirthos.Mods.Utils
{
    public class InvUtils
    {
        /// <summary>
        /// Move items from a worldObjetc iwht a inventory to a world object with a inventory
        /// </summary>
        /// <param name="input">Input world object as chest or stockpile</param>
        /// <param name="output">Output world object as chest or stockpile</param>
        /// <param name="qty">The quantity moved (can't move more than a stack)</param>
        /// <param name="filteredItemInput">List of item type that can be taken in input (null means take all type)</param>
        /// <param name="filteredItemOutput">List of item type that can be donne to output(null means take all type)</param>
        /// <returns></returns>
        public static bool MoveItemFromToInventory(WorldObject input, WorldObject output, int qty = 1, List<Type> filteredItemInput = null, List<Type> filteredItemOutput = null)
        {
            if (input.GetComponent<PublicStorageComponent>() == null)
                return false;
            if (output.GetComponent<PublicStorageComponent>() == null)
                return false;
            Inventory invInput = input.GetComponent<PublicStorageComponent>().Inventory;
            Inventory invOutput = output.GetComponent<PublicStorageComponent>().Inventory;

            if (invInput == null || invOutput == null)
                return false;
            foreach (ItemStack items in invInput.Stacks)
            {
                if (items.Quantity >= qty)
                {
                    if (filteredItemInput != null && filteredItemInput.Count > 0 && filteredItemInput.Contains(items.Item.Type) == false)
                        continue;
                    if (filteredItemOutput != null && filteredItemOutput.Count > 0 && filteredItemOutput.Contains(items.Item.Type) == false)
                        continue;
                    if (invOutput.TryAddItems(items.Item.Type, qty))
                    {
                        invInput.RemoveItems(items.Item.Type, qty);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Search for a connected storage around the position, can out the orientation
        /// </summary>
        /// <param name="position"></param>
        /// <param name="orientation">0-N 1-E 2-S 3-W not working for up and down</param>
        /// <returns></returns>
        public static WorldObject SearchForConnectedStorage(Vector3i position, out int orientation)
        {
            orientation = 0;
            Block b = World.GetBlock(position + Vector3i.Left);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj.GetComponent<PublicStorageComponent>() != null)
                {
                    return obj;
                }
            }
            orientation = 1;
            b = World.GetBlock(position + Vector3i.Forward);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj.GetComponent<PublicStorageComponent>() != null)
                {
                    return obj;
                }
            }
            orientation = 2;
            b = World.GetBlock(position + Vector3i.Right);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj.GetComponent<PublicStorageComponent>() != null)
                {
                    return obj;
                }
            }
            orientation = 3;
            b = World.GetBlock(position + Vector3i.Back);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj.GetComponent<PublicStorageComponent>() != null)
                {
                    return obj;
                }
            }
            foreach (WorldObject obj in WorldObjectManager.All)
            {
                if (obj.GetComponent<PublicStorageComponent>() != null)
                {
                    orientation = 0;
                    if (obj.Position3i == (position + (Vector3i.Left)) || obj.Position3i == (position + (Vector3i.Left * 3)))
                    {
                        return obj;
                    }
                    orientation = 1;
                    if (obj.Position3i == (position + (Vector3i.Forward)) || obj.Position3i == (position + (Vector3i.Forward * 3)))
                    {
                        return obj;
                    }
                    orientation = 2;
                    if (obj.Position3i == (position + (Vector3i.Right)) || obj.Position3i == (position + (Vector3i.Right * 3)))
                    {
                        return obj;
                    }
                    orientation = 3;
                    if (obj.Position3i == (position + (Vector3i.Back)) || obj.Position3i == (position + (Vector3i.Back * 3)))
                    {
                        return obj;
                    }
                }
            }
            return null;
        }
    }
}
