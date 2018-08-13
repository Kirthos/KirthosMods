namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Serialization;

    [RequiresSkill(typeof(BasicEWorkingSkill), 3)]
    public partial class JDBigBatteryRecipe : Recipe
    {
        public JDBigBatteryRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<JDBigBatteryItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<CopperWiringItem>(typeof(BasicEWorkingEfficiencySkill), 10, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<JDUnchargedBatteryItem>(typeof(BasicEWorkingEfficiencySkill), 2, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDBigBatteryRecipe), Item.Get<JDBigBatteryItem>().UILink(), 20f, typeof(BasicEWorkingSpeedSkill));
            this.Initialize("Big Battery", typeof(JDBigBatteryRecipe));

            CraftingComponent.AddRecipe(typeof(JDBatteryChargerObject), this);
        }
    }


    [Serialized]
    [Weight(2000)]
    [Fuel(15000)]
    [Currency]
    public partial class JDBigBatteryItem :
    Item
    {
        public override string FriendlyName { get { return "Big Battery"; } }
        public override string Description { get { return "Be careful or you will be shocked."; } }

    }

}