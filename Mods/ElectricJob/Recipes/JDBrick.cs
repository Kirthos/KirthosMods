namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(BasicEWorkingSkill), 1)]
    public class JDBrickRecipe : Recipe
    {
        public JDBrickRecipe()
        {
            this.Products = new CraftingElement[]
            {
               new CraftingElement<BrickItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<StoneItem>(typeof(BasicEWorkingEfficiencySkill), 20, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<PitchItem>(typeof(BasicEWorkingEfficiencySkill), 5, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.Initialize("Brick", typeof(JDBrickRecipe));
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDBrickRecipe), this.UILink(), 1.0f * JDElectricalPlugin.Conf.Get<float>("CraftingTimeMultiplierForElectricMachine"), typeof(BasicEWorkingSpeedSkill));
            CraftingComponent.AddRecipe(typeof(JDKilnObject), this);
        }
    }
}