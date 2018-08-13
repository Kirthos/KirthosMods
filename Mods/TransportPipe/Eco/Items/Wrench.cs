using Eco.Gameplay.Components;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
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
 * Created by Kirthos 04/12/2018
 */

namespace Kirthos.Mods.TransportPipe.Items
{
    /*
    [RequiresSkill(typeof(PipeCraftingSkill), 2)]
    [RepairRequiresSkill(typeof(PipeCraftingSkill), 1)]
    public class WrenchRecipe : Recipe
    {
        public WrenchRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<WrenchItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<SteelItem>(typeof(PipeCraftingEfficiencySkill), 20, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<IronIngotItem>(typeof(PipeCraftingEfficiencySkill), 20, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<GearItem>(typeof(PipeCraftingEfficiencySkill), 10, PipeCraftingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(WrenchRecipe), Item.Get<WrenchItem>().UILinkContent(), 15, typeof(PipeCraftingSpeedSkill));
            this.Initialize("Wrench", typeof(WrenchRecipe));

            CraftingComponent.AddRecipe(typeof(AnvilObject), this);
        }
    }
    */
    [Serialized]
    [Weight(1000)]
    [Category("Tool")]
    public class WrenchItem : ToolItem
    {
        public override string Description { get { return "Wrench item"; } }
        public override string FriendlyName { get { return "Wrench"; } }

        public override ClientPredictedBlockAction LeftAction { get { return ClientPredictedBlockAction.None; } }
        public override string LeftActionDescription { get { return "Config"; } }

        private static SkillModifiedValue caloriesBurn = CreateCalorieValue(40, typeof(PipeCraftingEfficiencySkill), typeof(WrenchItem), new WrenchItem().UILinkContent());
        public override IDynamicValue CaloriesBurn { get { return caloriesBurn; } }

        private static SkillModifiedValue skilledRepairCost = new SkillModifiedValue(10, PipeCraftingSkill.MultiplicativeStrategy, typeof(PipeCraftingSkill), Localizer.DoStr("repair cost"));
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }

        public override float DurabilityRate { get { return DurabilityMax / 100; } }

        public override Item RepairItem { get { return Item.Get<SteelItem>(); } }
        public override int FullRepairAmount { get { return 10; } }

        public override InteractResult OnActLeft(InteractionContext context)
        {
            // if I target a pipe with the wrench, I transform it to a connector set to input
            if (context.HasBlock && context.Block is BaseTransportPipeBlock && Utils.SearchForConnectedInventory(context.BlockPosition.Value) != null)
            {
                //Removing the block and add the connector
                World.DeleteBlock(context.BlockPosition.Value);
                ConnectorObject connector = WorldObjectManager.TryToAdd<ConnectorObject>(context.Player.User, context.BlockPosition.Value, Quaternion.Identity);
                // If the conenctor can't be added just reset the action
                if (connector == null)
                {
                    World.SetBlock(context.Block.GetType(), context.BlockPosition.Value);
                    return InteractResult.NoOp;
                }
                // I instanciate the connector info with the info from the pipe
                TransportPipeInfo info = null;
                if (TransportPipeManager.pipesInfo.TryGetValue(context.BlockPosition.Value, out info))
                {
                    connector.info = info;
                    context.Player.SendTemporaryMessage($"Set to input");
                    connector.mode = CONNECTOR_MODE.Input;

                    BurnCalories(context.Player);
                    UseDurability(1.0f, context.Player);

                    return InteractResult.Success;
                }
                else
                {
                    // If the pipe don't contains any info I recreate the linker from the pipe and reset the action 
                    connector.Destroy();
                    World.SetBlock(context.Block.GetType(), context.BlockPosition.Value);
                    Utils.RecreateLinker(context.BlockPosition.Value, new TransportPipeLinker());
                }
            }
            // If I use the wrench on a connector
            if (context.HasTarget && context.Target is ConnectorObject)
            {
                // I process the config inside the connector class
                (context.Target as ConnectorObject).ProcessConfig(context);
                BurnCalories(context.Player);
                UseDurability(1.0f, context.Player);
                return InteractResult.Success;
            }
            return InteractResult.NoOp;
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
