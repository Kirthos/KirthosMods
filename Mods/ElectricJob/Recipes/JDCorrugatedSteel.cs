namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(SteelworkingSkill), 2)]
    public partial class JDCorrugatedSteelRecipe : Recipe
    {
        public JDCorrugatedSteelRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<CorrugatedSteelItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<SteelItem>(typeof(SteelworkingEfficiencySkill), 2, SteelworkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDCorrugatedSteelRecipe), Item.Get<CorrugatedSteelItem>().UILink(), 2 * JDElectricalPlugin.Conf.Get<float>("CraftingTimeMultiplierForElectricMachine"), typeof(SteelworkingSpeedSkill));
            this.Initialize("Corrugated Steel", typeof(JDCorrugatedSteelRecipe));

            CraftingComponent.AddRecipe(typeof(JDEHammerObject), this);
        }
    }
}