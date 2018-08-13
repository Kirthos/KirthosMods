namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(BasicESmeltingSkill), 1)]
    public class JDRebarRecipe : Recipe
    {
        public JDRebarRecipe()
        {
            this.Products = new CraftingElement[]
            {
               new CraftingElement<RebarItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<SteelItem>(typeof(BasicESmeltingEfficiencySkill), 5, BasicESmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.Initialize("Rebar", typeof(JDRebarRecipe));
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDRebarRecipe), this.UILink(), 0.5f * JDElectricalPlugin.Conf.Get<float>("CraftingTimeMultiplierForElectricMachine"), typeof(BasicESmeltingSpeedSkill));
            CraftingComponent.AddRecipe(typeof(JDFurnaceObject), this);
        }
    }
}