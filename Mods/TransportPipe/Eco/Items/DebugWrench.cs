using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Systems.Chat;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.World;
using Kirthos.Mods.TransportPipe.Eco;
using Kirthos.Mods.TransportPipe.Objects;
using Kirthos.Mods.TransportPipe.Skills;
using System;
using System.ComponentModel;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/09/2018
 */

namespace Kirthos.Mods.TransportPipe.Items
{

    [Serialized]
    [Weight(1000)]
    [Category("Tool")]
    public class DebugWrenchItem : ToolItem
    {
        public override string Description { get { return "Debug wrench item"; } }
        public override string FriendlyName { get { return "Debug wrench"; } }

        public override ClientPredictedBlockAction LeftAction { get { return ClientPredictedBlockAction.None; } }
        public override string LeftActionDescription { get { return "Debug"; } }

        private static SkillModifiedValue skilledRepairCost = new SkillModifiedValue(0, PipeCraftingSkill.MultiplicativeStrategy, typeof(PipeCraftingSkill), Localizer.DoStr("repair cost"));
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }


        public override InteractResult OnActLeft(InteractionContext context)
        {
            TransportPipeInfo info = null;
            if (context.HasBlock && context.Block is BaseTransportPipeBlock)
                info = TransportPipeManager.pipesInfo[Utils.MakeWorldMod(context.BlockPosition.Value)];
            if (context.HasTarget && context.Target is ConnectorObject)
                info = TransportPipeManager.pipesInfo[Utils.MakeWorldMod((context.Target as ConnectorObject).Position3i)];
            ChatManager.ServerMessageToAll($"{info}", false);
            return InteractResult.Success;
        }

        public override InteractResult OnActRight(InteractionContext context)
        {
            return InteractResult.NoOp;
        }

        public override InteractResult OnActInteract(InteractionContext context)
        {
            return InteractResult.NoOp;
        }

        public override bool ShouldHighlight(Type block)
        {
            return Block.Is<TransportPipeAttr>(block);
        }
    }
}
