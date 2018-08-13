using Eco.Gameplay.Objects;
using Eco.Shared.Math;
using Eco.World;
using Eco.World.Blocks;
using System;
using System.Collections.Generic;
using Eco.Shared.Networking;
using Eco.Gameplay.Players;
using Eco.Shared.Items;
using Eco.Gameplay.Interactions;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/10/2018
 */

namespace Kirthos.Mods.KirthosExplosive
{
    public class Utils
    {
        public static List<Vector3i> GetSphereBlock(Vector3i centerPosition, int radius)
        {
            List<Vector3i> blocks = new List<Vector3i>();

            int size = radius * 2 + 1;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        double distance = (radius - i) * (radius - i)
                                  + (radius - j) * (radius - j)
                                  + (radius - k) * (radius - k);

                        if (distance < (radius * radius))
                        {
                            Vector3i pos = centerPosition + new Vector3i(i, j, k) - new Vector3i(radius, radius, radius);
                            if (pos.x < 0 || pos.y < 0 || pos.z < 0)
                                continue;
                            blocks.Add(pos);
                        }
                    }
                }
            }
            return blocks;
        }

        public static void Explosion(Player player, Vector3i centerPosition, int radius, int percentBlockDrop)
        {
            Random rng = new Random();
            List<DynamiteObject> tnts = new List<DynamiteObject>();
            foreach (WorldObject obj in WorldObjectManager.All) { if (obj is DynamiteObject) tnts.Add(obj as DynamiteObject); }
            foreach (Vector3i pos in GetSphereBlock(centerPosition, radius))
            {
                Block block = World.GetBlock(pos);
                InteractionInfo info = new InteractionInfo();
                info.Method = InteractionMethod.None;
                info.BlockPosition = pos;
                InteractionContext context = info.MakeContext(player);
                if (context.Authed())
                {
                    foreach (DynamiteObject tnt in tnts)
                    {
                        if (centerPosition != pos && tnt.Position3i == pos)
                            tnt.Ignite(1, player);
                    }
                    if (block is ImpenetrableStoneBlock || block is EmptyBlock)
                        continue;
                    if (block is TreeBlock)
                    {
                        foreach (TreeEntity tree in NetObjectManager.GetObjectsOfType<TreeEntity>())
                        {
                            if (tree.Position.Ceiling == pos)
                            {
                                tree.Destroy();
                            }
                        }
                    }
                    if (block is WorldObjectBlock)
                    {
                        (block as WorldObjectBlock).WorldObjectHandle.Object.Destroy();
                    }
                    if (rng.Next(100) < percentBlockDrop)
                    {
                        if (block.Is<Minable>())
                            RubbleObject.TrySpawnFromBlock(block.GetType(), pos);
                    }
                    World.DeleteBlock(pos);
                }
            }
        }
    }
}
