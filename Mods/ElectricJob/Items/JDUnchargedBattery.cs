namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Serialization;

    [RequiresSkill(typeof(BasicEWorkingSkill), 1)]
    public partial class JDUnchargedBatteryRecipe : Recipe
    {
        public JDUnchargedBatteryRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<JDUnchargedBatteryItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<CopperWiringItem>(typeof(BasicEWorkingEfficiencySkill), 5, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<IronIngotItem>(typeof(BasicEWorkingEfficiencySkill), 1, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDUnchargedBatteryRecipe), Item.Get<JDUnchargedBatteryItem>().UILink(), 10f, typeof(BasicEWorkingSpeedSkill));
            this.Initialize("Uncharged Battery", typeof(JDUnchargedBatteryRecipe));

            CraftingComponent.AddRecipe(typeof(ElectronicsAssemblyObject), this);
        }
    }


    [Serialized]
    [Weight(500)]      
    [Currency]
    public partial class JDUnchargedBatteryItem :
    Item
    {
        public override string FriendlyName { get { return "Uncharged Battery"; } }
        public override string Description { get { return "A battery not charged."; } }

    }

}