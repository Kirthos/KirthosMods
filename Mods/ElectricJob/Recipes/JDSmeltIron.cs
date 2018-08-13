namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(BasicESmeltingSkill), 1)]
    public class JDSmeltingIronRecipe : Recipe
    {
        public JDSmeltingIronRecipe()
        {
            this.Products = new CraftingElement[]
            {
               new CraftingElement<IronIngotItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<IronOreItem>(typeof(BasicESmeltingEfficiencySkill), 4, BasicESmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.Initialize("Smelt Iron", typeof(JDSmeltingIronRecipe));
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDSmeltingIronRecipe), this.UILink(), 0.5f * JDElectricalPlugin.Conf.Get<float>("CraftingTimeMultiplierForElectricMachine"), typeof(BasicESmeltingSpeedSkill));
            CraftingComponent.AddRecipe(typeof(JDFurnaceObject), this);
        }
    }
}