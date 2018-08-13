using Asphalt.Api.Event;
using Asphalt.Api.Event.PlayerEvents;
using Asphalt.Util;
using Eco.Gameplay.Interactions;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using Eco.Shared.Math;
using Eco.Simulation.Agents;
using Eco.World;
using Eco.World.Blocks;
using System;
using System.Linq;
using System.Reflection;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/29/2018
 */

namespace Kirthos.Mods.BetterLogging
{
    public class BetterLoggingEventListener
    {
        public BetterLoggingEventListener() { }

        [EventHandler]
        public void OnPlayerInterract(PlayerInteractEvent evt)
        {
            evt.SetCancelled(ContextOnInterraction(evt.Context));
        }

        private bool ContextOnInterraction(InteractionContext context)
        {
            if (context.Method == InteractionMethod.Left)
            {
                if (context.SelectedItem is AxeItem)
                {
                    if (context.HasBlock && context.Authed())
                    {
                        var block = World.GetBlock(context.BlockPosition.Value);
                        if (block.Is<TreeDebris>())
                        {
                            foreach (Vector3i pos in WorldUtils.GetTopBlockPositionAroundPoint<TreeDebrisBlock>(context.Player.User, context.BlockPosition.Value, 1 + SkillsUtil.GetSkillLevel(context.Player.User, typeof(WoodPulpCleanerSkill))))
                            {
                                if (context.Player.User.Inventory.TryAddItems<WoodPulpItem>(5))
                                    World.DeleteBlock(pos);
                            }
                        }
                    }
                    if (context.HasTarget && context.Target is TreeEntity)
                    {
                        TreeEntity tree = context.Target as TreeEntity;
                        if (context.Parameters != null && context.Parameters.ContainsKey("stump"))
                        {
                            if (SkillsUtil.HasSkillLevel(context.Player.User, typeof(StumpCleanerSkill), 1))
                            {
                                tree.TryApplyDamage(context.Player, 500, context);
                            }
                        }
                        if (context.Parameters != null && context.Parameters.ContainsKey("slice"))
                        {
                            if (SkillsUtil.HasSkillLevel(context.Player.User, typeof(ExpertLumbererSkill), 1))
                            {
                                TreeBranch[] branches = typeof(Tree).GetField("branches", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(tree) as TreeBranch[];
                                if (!branches.Where(branch => branch != null).Select(branch => branch.Health).Where(health => health != 0).Any())
                                {
                                    for (int i = 1; i < 20; i++)
                                    {
                                        typeof(TreeEntity).GetMethod("TrySliceTrunk", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(tree, new object[] { context.Player, 0.05f * (float)i });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
