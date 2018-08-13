using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Mods.TechTree;
using Eco.Shared.Math;
using Eco.World;
using Kirthos.Mods.TransportPipe.Eco;
using Kirthos.Mods.TransportPipe.Items;
using Kirthos.Mods.TransportPipe.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/12/2018
 */

namespace Kirthos.Mods.TransportPipe
{
    class Utils
    {
        public static List<TransportPipeInfo> GetNeighbourgPipesInfo(Vector3i position)
        {
            List<TransportPipeInfo> result = new List<TransportPipeInfo>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        if (i == 0 && j == 0 && k == 0)
                            continue;
                        if ((i == 0 && j == 0) || (i == 0 && k == 0) || (j == 0 && k == 0))
                        {
                            Vector3i newPos = MakeWorldMod(new Vector3i(position.x + i, position.y + j, position.z + k));
                            TransportPipeInfo info = null;
                            if (TransportPipeManager.pipesInfo.TryGetValue(newPos, out info))
                            {
                                result.Add(info);
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static List<ConnectorObject> SearchForNeighbourgBelt(Vector3i position)
        {
            List<ConnectorObject> result = new List<ConnectorObject>();
            foreach (WorldObject obj in WorldObjectManager.All)
            {
                if (obj is ConnectorObject)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            for (int k = -1; k <= 1; k++)
                            {
                                if (i == 0 && j == 0 && k == 0)
                                    continue;
                                if ((i == 0 && j == 0) || (i == 0 && k == 0) || (j == 0 && k == 0))
                                {
                                    Vector3i newPos = MakeWorldMod(new Vector3i(position.x + i, position.y + j, position.z + k));
                                    if (obj.Position3i == newPos)
                                    {
                                        result.Add(obj as ConnectorObject);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static void RecreateLinker(Vector3i pos, TransportPipeLinker linker)
        {

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        if (i == 0 && j == 0 && k == 0)
                        {
                            Vector3i newPos = MakeWorldMod(new Vector3i(pos.x + i, pos.y + j, pos.z + k));
                            TransportPipeInfo info = null;
                            if (TransportPipeManager.pipesInfo.TryGetValue(newPos, out info))
                            {
                                if (linker.ContainsConveyor(info) == false)
                                {
                                    linker.AddConveyor(info);
                                }
                            }
                            continue;
                        }
                        if ((i == 0 && j == 0) || (i == 0 && k == 0) || (j == 0 && k == 0))
                        {
                            Vector3i newPos = MakeWorldMod(new Vector3i(pos.x + i, pos.y + j, pos.z + k));
                            TransportPipeInfo info = null;
                            if (TransportPipeManager.pipesInfo.TryGetValue(newPos, out info))
                            {
                                if (info.beltLinker == null)
                                {
                                    info.beltLinker = linker;
                                    RecreateLinker(newPos, linker);
                                }
                            }
                            else if (World.GetBlock(newPos) is BaseTransportPipeBlock || World.GetBlock(newPos) is PipeSlotBlock)
                            {
                                info = new TransportPipeInfo();
                                info.pos = newPos;
                                info.beltLinker = linker;
                                info.type = World.GetBlock(newPos) is WoodenTransportPipeBlock ? PIPETYPE.Wooden : PIPETYPE.Electric;
                                TransportPipeManager.pipesInfo.TryAdd(newPos, info);
                                RecreateLinker(newPos, linker);
                            }
                        }
                    }
                }
            }
        }

        public static int Mod(int a, int n)
        {
            int result = a % n;
            if ((result < 0 && n > 0) || (result > 0 && n < 0))
            {
                result += n;
            }
            return result;
        }

        public static Vector3i MakeWorldMod(Vector3i vector3)
        {
            return new Vector3i(Mod(vector3.X, WorldArea.WholeWorld.Size.X), vector3.y, Mod(vector3.Z, WorldArea.WholeWorld.Size.Y));
        }

        public static bool VerifyInventoryPlace(Inventory inv, Type item)
        {
            foreach (ItemStack stack in inv.Stacks)
            {
                if (stack.Empty)
                    return true;
                if (stack.Item.GetType() == item && stack.Quantity < inv.GetMaxAccepted(stack.Item, stack.Quantity))
                    return true;
            }
            return false;
        }

        public static Type GetItemTypeFromRubbleType(Type rubbleType)
        {
            if (rubbleType.ToString().Contains("CoalRubbleSet"))
            {
                return typeof(CoalItem);
            }
            else if (rubbleType.ToString().Contains("CopperOreRubbleSet"))
            {
                return typeof(CopperOreItem);
            }
            else if (rubbleType.ToString().Contains("GoldOreRubbleSet"))
            {
                return typeof(GoldOreItem);
            }
            else if (rubbleType.ToString().Contains("IronOreRubbleSet"))
            {
                return typeof(IronOreItem);
            }
            else //if (rubbleType.ToString().Contains("StoneRubbleSet"))
            {
                return typeof(StoneItem);
            }
        }

        public static Inventory SearchInventoryFromObject(WorldObject obj)
        {
            if (obj != null)
            {
                PublicStorageComponent compStorage = obj.GetComponent<PublicStorageComponent>();
                if (compStorage != null)
                {
                    return compStorage.Inventory;
                }
            }
            if (obj != null)
            {
                FuelSupplyComponent compfuel = obj.GetComponent<FuelSupplyComponent>();
                if (compfuel != null)
                {
                    return compfuel.Inventory;
                }
            }
            return null;
        }

        public static Inventory SearchForConnectedInventory(Vector3i position)
        {
            return SearchInventoryFromObject(SearchForConnectedStorageObject(position));
            /*
            Block b = World.GetBlock(position + Vector3i.Left);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj is StockpileObject == false)
                {
                    Inventory inv = SearchInventoryFromObject(obj);
                    if (inv != null)
                        return inv;
                }
            }
            b = World.GetBlock(position + Vector3i.Forward);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj is StockpileObject == false)
                {
                    Inventory inv = SearchInventoryFromObject(obj);
                    if (inv != null)
                        return inv;
                }
            }
            b = World.GetBlock(position + Vector3i.Right);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj is StockpileObject == false)
                {
                    Inventory inv = SearchInventoryFromObject(obj);
                    if (inv != null)
                        return inv;
                }

            }
            b = World.GetBlock(position + Vector3i.Back);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj is StockpileObject == false)
                {
                    Inventory inv = SearchInventoryFromObject(obj);
                    if (inv != null)
                        return inv;
                }
            }
            b = World.GetBlock(position + Vector3i.Up);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj is StockpileObject == false)
                {
                    Inventory inv = SearchInventoryFromObject(obj);
                    if (inv != null)
                        return inv;
                }
            }
            b = World.GetBlock(position + Vector3i.Down);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj is StockpileObject == false)
                {
                    Inventory inv = SearchInventoryFromObject(obj);
                    if (inv != null)
                        return inv;
                }
            }
            foreach (WorldObject obj in WorldObjectManager.All)
            {
                if (obj.GetComponent<PublicStorageComponent>() != null)
                {
                    if (obj.Position3i == (position + (Vector3i.Left)) || obj.Position3i == (position + (Vector3i.Left * 3)))
                    {
                        Inventory inv = SearchInventoryFromObject(obj);
                        if (inv != null)
                            return inv;
                    }
                    if (obj.Position3i == (position + (Vector3i.Forward)) || obj.Position3i == (position + (Vector3i.Forward * 3)))
                    {
                        Inventory inv = SearchInventoryFromObject(obj);
                        if (inv != null)
                            return inv;
                    }
                    if (obj.Position3i == (position + (Vector3i.Right)) || obj.Position3i == (position + (Vector3i.Right * 3)))
                    {
                        Inventory inv = SearchInventoryFromObject(obj);
                        if (inv != null)
                            return inv;
                    }
                    if (obj.Position3i == (position + (Vector3i.Back)) || obj.Position3i == (position + (Vector3i.Back * 3)))
                    {
                        Inventory inv = SearchInventoryFromObject(obj);
                        if (inv != null)
                            return inv;
                    }
                }
            }
            return null;
            */
        }

        public static WorldObject SearchForConnectedStorageObject(Vector3i position)
        {
            Block b = World.GetBlock(position + Vector3i.Left);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj is StockpileObject == false)
                {
                    Inventory inv = SearchInventoryFromObject(obj);
                    if (inv != null)
                        return obj;
                }
            }
            b = World.GetBlock(position + Vector3i.Forward);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj is StockpileObject == false)
                {
                    Inventory inv = SearchInventoryFromObject(obj);
                    if (inv != null)
                        return obj;
                }
            }
            b = World.GetBlock(position + Vector3i.Right);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj is StockpileObject == false)
                {
                    Inventory inv = SearchInventoryFromObject(obj);
                    if (inv != null)
                        return obj;
                }
            }
            b = World.GetBlock(position + Vector3i.Back);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj is StockpileObject == false)
                {
                    Inventory inv = SearchInventoryFromObject(obj);
                    if (inv != null)
                        return obj;
                }
            }
            b = World.GetBlock(position + Vector3i.Up);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj is StockpileObject == false)
                {
                    Inventory inv = SearchInventoryFromObject(obj);
                    if (inv != null)
                        return obj;
                }
            }
            b = World.GetBlock(position + Vector3i.Down);
            if (b is WorldObjectBlock)
            {
                WorldObject obj = (b as WorldObjectBlock).WorldObjectHandle.Object;
                if (obj is StockpileObject == false)
                {
                    Inventory inv = SearchInventoryFromObject(obj);
                    if (inv != null)
                        return obj;
                }
            }
            foreach (WorldObject obj in WorldObjectManager.All)
            {
                if (obj.GetComponent<PublicStorageComponent>() != null)
                {
                    if (obj.Position3i == (position + (Vector3i.Left)) || obj.Position3i == (position + (Vector3i.Left * 3)))
                    {
                        Inventory inv = SearchInventoryFromObject(obj);
                        if (inv != null)
                            return obj;
                    }
                    if (obj.Position3i == (position + (Vector3i.Forward)) || obj.Position3i == (position + (Vector3i.Forward * 3)))
                    {
                        Inventory inv = SearchInventoryFromObject(obj);
                        if (inv != null)
                            return obj;
                    }
                    if (obj.Position3i == (position + (Vector3i.Right)) || obj.Position3i == (position + (Vector3i.Right * 3)))
                    {
                        Inventory inv = SearchInventoryFromObject(obj);
                        if (inv != null)
                            return obj;
                    }
                    if (obj.Position3i == (position + (Vector3i.Back)) || obj.Position3i == (position + (Vector3i.Back * 3)))
                    {
                        Inventory inv = SearchInventoryFromObject(obj);
                        if (inv != null)
                            return obj;
                    }
                }
            }
            return null;
        }

        public static bool MoveItemFromToInventory(Inventory input, Inventory output, int qty = 1, Filter InputFilter = null, Filter OutputFilter = null)
        {
            if (input == null)
                return false;
            if (output == null)
                return false;
            if (input == output)
                return false;
            try
            {
                foreach (ItemStack items in input.Stacks)
                {
                    if (items.Quantity >= qty)
                    {
                        //test the filter
                        if (InputFilter.CanAccept(items.Item.Type) == false)
                            continue;
                        if (OutputFilter.CanAccept(items.Item.Type) == false)
                            continue;
                        //try to move the item from source to dest
                        if (input.TryMoveItems<int>(items.Item.Type, qty, output))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                // Sometime that occurs when the inventory is destroy during the loop. It's just safe to don't move item this time.
            }
            return false;
        }

        public static List<KeyValuePair<Type, int>> MergeItemList(List<KeyValuePair<Type, int>> firstList, List<KeyValuePair<Type, int>> secondList)
        {
            List<KeyValuePair<Type, int>> result = new List<KeyValuePair<Type, int>>();
            foreach (var kvp1 in firstList)
            {
                int qty = kvp1.Value;
                foreach (var kvp2 in secondList)
                {
                    if (kvp1.Key == kvp2.Key)
                    {
                        qty += kvp2.Value;
                        break;
                    }
                }
                result.Add(new KeyValuePair<Type, int>(kvp1.Key, qty));
            }
            foreach (var kvp2 in secondList)
            {
                bool toAdd = true;
                foreach (var kvp1 in firstList)
                {
                    if (kvp1.Key == kvp2.Key)
                    {
                        toAdd = false;
                        break;
                    }
                }
                if (toAdd)
                    result.Add(new KeyValuePair<Type, int>(kvp2.Key, kvp2.Value));
            }
            return result;
        }

        public static bool IsAuthorizedToInteract(WorldObject obj, User user)
        {
            if (obj.OwnerUser == null && user == null)
                return true;
            if (user != null && obj.AuthorizedToInteract(user).Success)
                return true;
            return false;
        }

    }

    public class SkillStorage
    {
        public SkillBonusStorage[] Levels;

        public SkillStorage(SkillBonusStorage[] _Levels)
        {
            Levels = _Levels;
        }

        public float[] multiplicativeStrategy()
        {
            float[] result = new float[Levels.Length + 1];
            result[0] = 1f;
            for (int i = 0; i < Levels.Length; i++)
            {
                result[i + 1] = Levels[i].MultiplicativeStrategy;
            }
            return result;
        }

        public float[] additiveStrategy()
        {
            float[] result = new float[Levels.Length + 1];
            result[0] = 0.0f;
            for (int i = 0; i < Levels.Length; i++)
            {
                result[i + 1] = Levels[i].AdditiveStrategy;
            }
            return result;
        }

        public int[] skillPointCost()
        {
            int[] result = new int[Levels.Length];
            for (int i = 0; i < Levels.Length; i++)
            {
                result[i] = Levels[i].SkillPointCost;
            }
            return result;
        }
    }

    public class SkillBonusStorage
    {
        public float MultiplicativeStrategy;
        public float AdditiveStrategy;
        public int SkillPointCost;

        public SkillBonusStorage(float _MultiplicativeStrategy, float _AdditiveStrategy, int _SkillPointCost)
        {
            MultiplicativeStrategy = _MultiplicativeStrategy;
            AdditiveStrategy = _AdditiveStrategy;
            SkillPointCost = _SkillPointCost;
        }
    }
}
