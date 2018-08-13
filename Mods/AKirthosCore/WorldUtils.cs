using Eco.Gameplay.Interactions;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Shared.Items;
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
    public class WorldUtils
    {
        /// <summary>
        /// Return a list of world object where position are one block away from the position given
        /// </summary>
        public static List<WorldObject> GetNeightbourgWorldObject<T>(Vector3i position) where T : WorldObject
        {
            List<WorldObject> objects = new List<WorldObject>();
            foreach (WorldObject obj in WorldObjectManager.All)
            {
                if (obj is T)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            for (int k = -1; k <= 1; k++)
                            {
                                Vector3i newPos = new Vector3i(position.x + i, position.y + j, position.z + k);
                                if (obj.Position3i == newPos)
                                {
                                    objects.Add(obj);
                                }
                            }
                        }
                    }
                }
            }
            return objects;
        }

        /// <summary>
        /// Get a list of top Block that match the type T. Usefull for getting tree debris or plants
        /// </summary>
        public static List<T> GetTopBlockAroundPoint<T>(User user, Vector3i position, int range) where T : Block
        {
            try
            {
                List<T> blockLists = new List<T>();
                for (int i = -range; i < range; i++)
                {
                    for (int j = -range; j < range; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        Vector3i positionAbove = World.GetTopPos(new Vector2i(position.x + i, position.z + j)) + Vector3i.Up;
                        Block blockAbove = World.GetBlockProbablyTop(positionAbove);
                        if (blockAbove is T)
                        {
                            if (positionAbove != position && Vector3i.Distance(positionAbove, position) < range)
                            {
                                InteractionInfo info = new InteractionInfo();
                                info.Method = InteractionMethod.Right;
                                info.BlockPosition = positionAbove;
                                InteractionContext context = info.MakeContext(user.Player);
                                if (context.Authed())
                                {
                                    blockLists.Add(blockAbove as T);
                                }
                            }
                        }
                    }
                }
                return blockLists;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get a list of position of top Block that match the type T. Usefull when wanted to destroy plants or tree debris
        /// </summary>
        public static List<Vector3i> GetTopBlockPositionAroundPoint<T>(User user, Vector3i position, int range) where T : Block
        {
            try
            {
                List<Vector3i> blockLists = new List<Vector3i>();
                for (int i = -range; i < range; i++)
                {
                    for (int j = -range; j < range; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        Vector3i positionAbove = World.GetTopPos(new Vector2i(position.x + i, position.z + j)) + Vector3i.Up;
                        Block blockAbove = World.GetBlockProbablyTop(positionAbove);
                        if (blockAbove is T)
                        {
                            if (positionAbove != position && Vector3i.Distance(positionAbove, position) < range)
                            {
                                InteractionInfo info = new InteractionInfo();
                                info.Method = InteractionMethod.Right;
                                info.BlockPosition = positionAbove;
                                InteractionContext context = info.MakeContext(user.Player);
                                if (context.Authed())
                                {
                                    blockLists.Add(positionAbove);
                                }
                            }
                        }
                    }
                }
                return blockLists;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get the position of all block in a sphere radius
        /// </summary>
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
    }
}
