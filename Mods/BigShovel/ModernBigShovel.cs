using Asphalt;
using Asphalt.Service;
using Asphalt.Storeable;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Serialization;
using Eco.Gameplay.Components;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Gameplay.Utils;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/13/2018
 */

namespace Kirthos.Mods.BigShovel
{

    [RequiresSkill(typeof(SteelworkingSkill), 4)]
    [RepairRequiresSkill(typeof(SteelworkingSkill), 3)]
    public class ModernBigShovelRecipe : Recipe
    {
        public ModernBigShovelRecipe()
        {
            try
            {
                CraftingStorage storage = BigShovelPlugin.Conf.Get<ToolStorage>("ModernBigShovel").craft;
                this.Products = new CraftingElement[] { new CraftingElement<ModernBigShovelItem>() };

                this.Ingredients = new CraftingElement[storage.ingredients.Length];
                for (int i = 0; i < storage.ingredients.Length; i++)
                {
                    CraftIngredients current = storage.ingredients[i];
                    var genericListType = typeof(CraftingElement<>);
                    var specificListType = genericListType.MakeGenericType(Item.GetType(current.item));
                    var craftElem = Activator.CreateInstance(specificListType, new object[] { typeof(SteelworkingEfficiencySkill), current.quantity, SteelworkingEfficiencySkill.MultiplicativeStrategy });
                    this.Ingredients[i] = craftElem as CraftingElement;
                }
                if (storage.enable)
                    CraftingComponent.AddRecipe(BigShovelPlugin.GetTypeFromString(storage.table), this);
                this.CraftMinutes = CreateCraftTimeValue(typeof(ModernBigShovelRecipe), Item.Get<ModernBigShovelItem>().UILink(), storage.craftMinutesTime, typeof(SteelworkingSpeedSkill));
                this.Initialize("Modern Big Shovel", typeof(ModernBigShovelRecipe));
            }
            catch (Exception e)
            {
                Console.WriteLine("[BigShovel] - A error occured when setting ModernBigShovelCraft - " + e.Message);
                return;
            }

        }
    }

    [Serialized]
    [Weight(1000)]
    [Category("Tool")]
    public partial class ModernBigShovelItem : ShovelItem
    {
        public override string FriendlyName { get { return "Modern Big Shovel"; } }
        private static SkillModifiedValue caloriesBurn = CreateCalorieValue(BigShovelPlugin.Conf.Get<ToolStorage>("ModernBigShovel").calories, typeof(ShovelEfficiencySkill), typeof(ModernBigShovelItem), new ModernBigShovelItem().UILink());
        public override IDynamicValue CaloriesBurn { get { return caloriesBurn; } }

        public override float DurabilityRate { get { return DurabilityMax / BigShovelPlugin.Conf.Get<ToolStorage>("ModernBigShovel").durability; } }

        private static SkillModifiedValue skilledRepairCost = new SkillModifiedValue(BigShovelPlugin.Conf.Get<ToolStorage>("ModernBigShovel").repair.quantity, SteelworkingSkill.MultiplicativeStrategy, typeof(SteelworkingSkill), Localizer.DoStr("repair cost"));
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }

        public override Item RepairItem { get { return Item.Get(BigShovelPlugin.Conf.Get<ToolStorage>("ModernBigShovel").repair.item); } }
        public override int FullRepairAmount { get { return (int)BigShovelPlugin.Conf.Get<ToolStorage>("ModernBigShovel").repair.quantity; } }
    }
}