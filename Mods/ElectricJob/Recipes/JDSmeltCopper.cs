namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(BasicESmeltingSkill), 1)]
    public class JDSmeltCopperRecipe : Recipe
    {
        public JDSmeltCopperRecipe()
        {
            this.Products = new CraftingElement[]
            {
               new CraftingElement<CopperIngotItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<CopperOreItem>(typeof(BasicESmeltingEfficiencySkill), 4, BasicESmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.Initialize("Smelt Copper", typeof(JDSmeltCopperRecipe));
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDSmeltCopperRecipe), this.UILink(), 0.5f * JDElectricalPlugin.Conf.Get<float>("CraftingTimeMultiplierForElectricMachine"), typeof(BasicESmeltingSpeedSkill));
            CraftingComponent.AddRecipe(typeof(JDFurnaceObject), this);
        }
    }
}