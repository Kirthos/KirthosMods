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
    //*
    [RequiresSkill(typeof(MetalworkingSkill), 3)]
    [RepairRequiresSkill(typeof(MetalworkingSkill), 1)]
    public class IronBigShovelRecipe : Recipe
    {
        public IronBigShovelRecipe()
        {
            try
            {
                CraftingStorage storage = BigShovelPlugin.Conf.Get<ToolStorage>("IronBigShovel").craft;
                this.Products = new CraftingElement[] { new CraftingElement<IronBigShovelItem>() };

                this.Ingredients = new CraftingElement[storage.ingredients.Length];
                for (int i = 0; i < storage.ingredients.Length; i++)
                {
                    CraftIngredients current = storage.ingredients[i];
                    var genericListType = typeof(CraftingElement<>);
                    var specificListType = genericListType.MakeGenericType(Item.GetType(current.item));
                    var craftElem = Activator.CreateInstance(specificListType, new object[] { typeof(MetalworkingEfficiencySkill), current.quantity, MetalworkingEfficiencySkill.MultiplicativeStrategy });
                    this.Ingredients[i] = craftElem as CraftingElement;
                }
                if (storage.enable)
                {
                    CraftingComponent.AddRecipe(BigShovelPlugin.GetTypeFromString(storage.table), this);
                }
                this.CraftMinutes = CreateCraftTimeValue(typeof(IronBigShovelRecipe), Item.Get<IronBigShovelItem>().UILink(), storage.craftMinutesTime, typeof(MetalworkingSpeedSkill));
                this.Initialize("Iron Big Shovel", typeof(IronBigShovelRecipe));
            }
            catch (Exception e)
            {
                Console.WriteLine("[BigShovel] - A error occured when setting IronBigShovelCraft - " + e.Message);
                return;
            }

        }
    }
    //*/

    [Serialized]
    [Weight(1000)]
    [Category("Tool")]
    public class IronBigShovelItem : ShovelItem
    {
        //*
        public override string FriendlyName { get { return "Iron Big Shovel"; } }
        private static SkillModifiedValue caloriesBurn = CreateCalorieValue(BigShovelPlugin.Conf.Get<ToolStorage>("IronBigShovel").calories, typeof(ShovelEfficiencySkill), typeof(IronBigShovelItem), new IronBigShovelItem().UILink());
        public override IDynamicValue CaloriesBurn { get { return caloriesBurn; } }

        public override float DurabilityRate { get { return DurabilityMax / BigShovelPlugin.Conf.Get<ToolStorage>("IronBigShovel").durability; } }

        private static SkillModifiedValue skilledRepairCost = new SkillModifiedValue(BigShovelPlugin.Conf.Get<ToolStorage>("IronBigShovel").repair.quantity, MetalworkingSkill.MultiplicativeStrategy, typeof(MetalworkingSkill), Localizer.DoStr("repair cost"));
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }

        public override Item RepairItem { get { return Item.Get(BigShovelPlugin.Conf.Get<ToolStorage>("IronBigShovel").repair.item); } }
        public override int FullRepairAmount { get { return (int)BigShovelPlugin.Conf.Get<ToolStorage>("IronBigShovel").repair.quantity; } }
        //*/
    }
}