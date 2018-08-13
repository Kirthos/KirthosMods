namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(BasicEWorkingSkill), 1)]
    public class JDFramedGlassRecipe : Recipe
    {
        public JDFramedGlassRecipe()
        {
            this.Products = new CraftingElement[]
            {
               new CraftingElement<FramedGlassItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<GlassItem>(typeof(BasicEWorkingEfficiencySkill), 10, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<SteelItem>(typeof(BasicEWorkingEfficiencySkill), 6, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.Initialize("Framed Glass", typeof(JDFramedGlassRecipe));
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDFramedGlassRecipe), this.UILink(), 1.0f * JDElectricalPlugin.Conf.Get<float>("CraftingTimeMultiplierForElectricMachine"), typeof(BasicEWorkingSpeedSkill));
            CraftingComponent.AddRecipe(typeof(JDKilnObject), this);
        }
    }
}