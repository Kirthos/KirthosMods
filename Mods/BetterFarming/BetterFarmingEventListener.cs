using Asphalt.Api.Event;
using Asphalt.Api.Event.PlayerEvents;
using Asphalt.Util;
using Eco.Gameplay.Interactions;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using System;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/23/2018
 */

namespace Kirthos.Mods.BetterFarming
{
    public class BetterFarmingEventListener
    {
        public BetterFarmingEventListener() { }

        [EventHandler]
        public void OnPlayerInterract(PlayerInteractEvent evt)
        {
            evt.SetCancelled(ContextOnInterraction(evt.Context));
        }

        private bool ContextOnInterraction(InteractionContext context)
        {
            if (context.Method == InteractionMethod.Right)
            {
                if (context.SelectedItem is ScytheItem && context.HasBlock)
                {
                    if (SkillsUtil.HasSkillLevel(context.Player.User, typeof(ScythePickupRangeSkill), 1))
                    {
                        PlantUtils.GetReapableBlockAroundPoint(context, context.BlockPosition.Value, SkillsUtil.GetSkillLevel(context.Player.User, typeof(ScythePickupRangeSkill)));
                    }
                }
                else if (context.SelectedItem is HoeItem && context.HasBlock)
                {
                    if (SkillsUtil.HasSkillLevel(context.Player.User, typeof(HoePickupRangeSkill), 1))
                    {
                        PlantUtils.GetPlantBlockAroundPoint(context, context.BlockPosition.Value, SkillsUtil.GetSkillLevel(context.Player.User, typeof(HoePickupRangeSkill)));
                    }
                }
            }
            return false;
        }
    }
}
