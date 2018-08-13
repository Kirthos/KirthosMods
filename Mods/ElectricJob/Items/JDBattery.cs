namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Serialization;

    [RequiresSkill(typeof(BasicEWorkingSkill), 2)]
    public partial class JDBatteryRecipe : Recipe
    {
        public JDBatteryRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<JDBatteryItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<CopperWiringItem>(typeof(BasicEWorkingEfficiencySkill), 2, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<JDUnchargedBatteryItem>(typeof(BasicEWorkingEfficiencySkill), 1, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDBatteryRecipe), Item.Get<JDBatteryItem>().UILink(), 10f, typeof(BasicEWorkingSpeedSkill));
            this.Initialize("Battery", typeof(JDBatteryRecipe));

            CraftingComponent.AddRecipe(typeof(JDBatteryChargerObject), this);
        }
    }


    [Serialized]
    [Weight(1000)]
    [Fuel(5000)]
    [Currency]
    public partial class JDBatteryItem :
    Item
    {
        public override string FriendlyName { get { return "Battery"; } }
        public override string Description { get { return "Be careful or you will be shocked."; } }
    }
}