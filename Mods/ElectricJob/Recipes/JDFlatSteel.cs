namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(SteelworkingSkill), 3)]
    public partial class JDFlatSteelRecipe : Recipe
    {
        public JDFlatSteelRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<FlatSteelItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<SteelItem>(typeof(SteelworkingEfficiencySkill), 2, SteelworkingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<EpoxyItem>(typeof(SteelworkingEfficiencySkill), 1, SteelworkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDFlatSteelRecipe), Item.Get<FlatSteelItem>().UILink(), 5 * JDElectricalPlugin.Conf.Get<float>("CraftingTimeMultiplierForElectricMachine"), typeof(SteelworkingSpeedSkill));
            this.Initialize("Flat Steel", typeof(JDFlatSteelRecipe));

            CraftingComponent.AddRecipe(typeof(JDEHammerObject), this);
        }
    }
}