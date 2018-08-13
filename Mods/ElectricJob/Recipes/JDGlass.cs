namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(BasicEWorkingSkill), 1)]
    public class JDGlassRecipe : Recipe
    {
        public JDGlassRecipe()
        {
            this.Products = new CraftingElement[]
            {
               new CraftingElement<GlassItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<SandItem>(typeof(BasicEWorkingEfficiencySkill), 4, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.Initialize("Glass", typeof(JDGlassRecipe));
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDGlassRecipe), this.UILink(), 1.0f * JDElectricalPlugin.Conf.Get<float>("CraftingTimeMultiplierForElectricMachine"), typeof(BasicEWorkingSpeedSkill));
            CraftingComponent.AddRecipe(typeof(JDKilnObject), this);
        }
    }
}