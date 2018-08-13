namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(BasicESmeltingSkill), 1)]
    public class JDCharcoalRecipe : Recipe
    {
        public JDCharcoalRecipe()
        {
            this.Products = new CraftingElement[]
            {
               new CraftingElement<CharcoalItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<LumberItem>(typeof(BasicESmeltingEfficiencySkill), 10, BasicESmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.Initialize("Charcoal", typeof(JDCharcoalRecipe));
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDCharcoalRecipe), this.UILink(), 1.0f * JDElectricalPlugin.Conf.Get<float>("CraftingTimeMultiplierForElectricMachine"), typeof(BasicESmeltingSpeedSkill));
            CraftingComponent.AddRecipe(typeof(JDFurnaceObject), this);
        }
    }
}