using Asphalt.Api.Event;
using Asphalt.Api.Event.PlayerEvents;
using Asphalt.Api.Event.RpcEvents;
using Asphalt.Util;
using Asphalt.Utils;
using Eco.Gameplay.Interactions;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using Eco.Shared.Serialization;
using Eco.World.Blocks;
using System;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/01/2018
 */

namespace Kirthos.Mods.BetterMining
{
    public class BetterMiningEventListener
    {
        public BetterMiningEventListener() { }

        [EventHandler]
        public void OnPlayerInterract(PlayerInteractEvent evt)
        {
            evt.SetCancelled(ContextOnInterraction(evt.Context));
        }

        private bool ContextOnInterraction(InteractionContext context)
        {
            if (context.Method == InteractionMethod.Right)
            {
                if (context.SelectedItem is PickaxeItem)
                {
                    if (context.HasBlock == false || context.Player.User.Inventory.Carried.IsEmpty)
                    {
                        if (SkillsUtil.HasSkillLevel(context.Player.User, typeof(MiningPickupAmountSkill), 1))
                        {
                            RubbleUtils.PickUpRubble(context.Player.User, 2 + (2 * SkillsUtil.GetSkillLevel(context.Player.User, typeof(MiningPickupRangeSkill))), (BetterMiningPlugin.ConfigStorage.GetInt("PickupRubbleBySkillLevel") * SkillsUtil.GetSkillLevel(context.Player.User, typeof(MiningPickupAmountSkill))));
                        }
                    }
                }
            }
            else if (context.Method == InteractionMethod.Left)
            {
                if (context.SelectedItem is PickaxeItem && context.HasBlock && context.Block.Is<Minable>())
                {
                    if (context.SelectedItem.OnActLeft(context).IsSuccess)
                    {
                        RubbleUtils.BreakBigRubble(context.BlockPosition.Value, 20 * SkillsUtil.GetSkillLevel(context.Player.User, typeof(StrongMiningSkill)));
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
