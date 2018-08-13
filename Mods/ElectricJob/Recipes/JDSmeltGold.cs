namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(BasicESmeltingSkill), 1)]
    public class JDSmeltingGoldRecipe : Recipe
    {
        public JDSmeltingGoldRecipe()
        {
            this.Products = new CraftingElement[]
            {
               new CraftingElement<GoldIngotItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<GoldOreItem>(typeof(BasicESmeltingEfficiencySkill), 4, BasicESmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.Initialize("Smelt Gold", typeof(JDSmeltingGoldRecipe));
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDSmeltingGoldRecipe), this.UILink(), 0.5f * JDElectricalPlugin.Conf.Get<float>("CraftingTimeMultiplierForElectricMachine"), typeof(BasicESmeltingSpeedSkill));
            CraftingComponent.AddRecipe(typeof(JDFurnaceObject), this);
        }
    }
}