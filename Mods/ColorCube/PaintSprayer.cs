using Eco.Gameplay.Components;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.World;
using System;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/02/2018
 */

namespace Kirthos.Mods.ColorCube
{
    [RequiresSkill(typeof(MetalworkingSkill), 1)]
    [RepairRequiresSkill(typeof(MetalworkingSkill), 1)]
    public class PaintSprayerRecipe : Recipe
    {
        public PaintSprayerRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<PaintSprayerItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<ColorPlantItem>(typeof(MetalworkingEfficiencySkill), 5, MetalworkingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<IronIngotItem>(typeof(MetalworkingEfficiencySkill), 10, MetalworkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(PaintSprayerRecipe), Item.Get<PaintSprayerItem>().UILinkContent(), 5, typeof(MetalworkingSpeedSkill));
            this.Initialize("Paint sprayer", typeof(PaintSprayerRecipe));

            CraftingComponent.AddRecipe(typeof(AnvilObject), this);
        }
    }

    [Serialized]
    [CanMakeBlockForm(new[] { "White", "Black", "Gray", "Silver", "Maroon", "Red", "Olive", "Yellow", "Green", "Lime", "Teal", "Aqua", "Navy", "Blue", "Purple", "Fuchsia", "Orange", "Pink" })]
    public class PaintSprayerItem : ToolItem
    {
        public override string FriendlyName { get { return "Paint Sprayer"; } }
        public override string Description { get { return "Use this to choose the color when placing color cube"; } }

        public override ClientPredictedBlockAction LeftAction { get { return ClientPredictedBlockAction.PickupBlock; } }
        public override string LeftActionDescription { get { return "Pick up"; } }

        public override Item RepairItem { get { return Item.Get<IronIngotItem>(); } }
        public override int FullRepairAmount { get { return 10; } }

        private static SkillModifiedValue skilledRepairCost = new SkillModifiedValue(10, MetalworkingSkill.MultiplicativeStrategy, typeof(MetalworkingSkill), Localizer.DoStr("repair cost"));
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }

        static IDynamicValue caloriesBurn = new ConstantValue(5);
        public override IDynamicValue CaloriesBurn { get { return caloriesBurn; } }

        public override InteractResult OnActLeft(InteractionContext context)
        {
            if (context.HasBlock && context.Block.Is<Colorable>())
                return (InteractResult)PlayerDeleteBlock(context.BlockPosition.Value, context.Player, true, 1);
            return InteractResult.NoOp;
        }
        public override InteractResult OnActInteract(InteractionContext context)
        {
            // If the player press 'E' the cube is coloried from the actual selected color
            if (context.HasBlock && context.Block.Is<Colorable>())
            {
                Type blockToAdd = null;
                switch (context.Player.User.Inventory.Form)
                {
                    case "White":
                        blockToAdd = typeof(WhiteCubeBlock);
                        break;
                    case "Black":
                        blockToAdd = typeof(BlackCubeBlock);
                        break;
                    case "Gray":
                        blockToAdd = typeof(GrayCubeBlock);
                        break;
                    case "Silver":
                        blockToAdd = typeof(SilverCubeBlock);
                        break;
                    case "Maroon":
                        blockToAdd = typeof(MaroonCubeBlock);
                        break;
                    case "Red":
                        blockToAdd = typeof(RedCubeBlock);
                        break;
                    case "Olive":
                        blockToAdd = typeof(OliveCubeBlock);
                        break;
                    case "Yellow":
                        blockToAdd = typeof(YellowCubeBlock);
                        break;
                    case "Green":
                        blockToAdd = typeof(GreenCubeBlock);
                        break;
                    case "Lime":
                        blockToAdd = typeof(LimeCubeBlock);
                        break;
                    case "Teal":
                        blockToAdd = typeof(TealCubeBlock);
                        break;
                    case "Aqua":
                        blockToAdd = typeof(AquaCubeBlock);
                        break;
                    case "Navy":
                        blockToAdd = typeof(NavyCubeBlock);
                        break;
                    case "Blue":
                        blockToAdd = typeof(BlueCubeBlock);
                        break;
                    case "Purple":
                        blockToAdd = typeof(PurpleCubeBlock);
                        break;
                    case "Fuchsia":
                        blockToAdd = typeof(FuchsiaCubeBlock);
                        break;
                }
                World.SetBlock(blockToAdd, context.BlockPosition.Value);
                return InteractResult.Success;
            }
            return InteractResult.NoOp;
        }
        public override bool ShouldHighlight(Type block)
        {
            return Block.Is<Colorable>(block);
        }
    }
}
