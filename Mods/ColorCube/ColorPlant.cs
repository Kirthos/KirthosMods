using Eco.Gameplay.Components;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
using Eco.Gameplay.Plants;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/02/2018
 */

namespace Kirthos.Mods.ColorCube
{
    [Serialized]
    public class ColorPlantBlock : InteractablePlantBlock
    {
        protected ColorPlantBlock() { }
        public ColorPlantBlock(WorldPosition3i position) : base(position) { }
    }


    [Serialized]
    [Yield(typeof(ColorPlantSeedItem), typeof(GrasslandGathererSkill), new float[] { 1f, 1.2f, 1.4f, 1.6f, 1.8f, 2f })]
    [Weight(10)]
    public partial class ColorPlantSeedItem : SeedItem
    {
        static ColorPlantSeedItem() { }

        private static Nutrients nutrition = new Nutrients() { Carbs = 0, Fat = 0, Protein = 0, Vitamins = 0 };

        public override string FriendlyName { get { return "Color plant seed"; } }
        public override string Description { get { return "Plant to grow color plant."; } }
        public override string SpeciesName { get { return "ColorPlant"; } }

        public override float Calories { get { return 0; } }
        public override Nutrients Nutrition { get { return nutrition; } }
    }

    [Serialized]
    [Weight(10)]
    [Yield(typeof(ColorPlantItem), typeof(GrasslandGathererSkill), new float[] { 1f, 1.2f, 1.4f, 1.6f, 1.8f, 2f })]
    public partial class ColorPlantItem :
    Item
    {
        public override string FriendlyName { get { return "Color plant"; } }
        public override string FriendlyNamePlural { get { return "Color plant"; } }
        public override string Description { get { return "A plant colored in all color. Useful for crafting color cube."; } }
    }

    [RequiresSkill(typeof(SeedProductionSkill), 4)]
    public class ColorPlantSeedRecipe : Recipe
    {
        public ColorPlantSeedRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<ColorPlantSeedItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<ColorPlantItem>(typeof(SeedProductionEfficiencySkill), 2, SeedProductionEfficiencySkill.MultiplicativeStrategy),
            };
            SkillModifiedValue value = new SkillModifiedValue(2, SeedProductionSpeedSkill.MultiplicativeStrategy, typeof(SeedProductionSpeedSkill), Localizer.DoStr("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(ColorPlantSeedRecipe), Item.Get<ColorPlantItem>().UILinkContent(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<ColorPlantItem>().UILinkContent(), value);
            this.CraftMinutes = value;

            this.Initialize("Color plant seed", typeof(ColorPlantSeedRecipe));
            CraftingComponent.AddRecipe(typeof(FarmersTableObject), this);
        }
    }
}
