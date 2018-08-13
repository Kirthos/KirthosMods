namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(BasicESmeltingSkill), 1)]
    public class JDSteelItemRecipe : Recipe
    {
        public JDSteelItemRecipe()
        {
            this.Products = new CraftingElement[]
            {
               new CraftingElement<SteelItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<IronIngotItem>(typeof(BasicESmeltingEfficiencySkill), 4, BasicESmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.Initialize("Steel", typeof(JDSteelItemRecipe));
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDSteelItemRecipe), this.UILink(), 5.0f * JDElectricalPlugin.Conf.Get<float>("CraftingTimeMultiplierForElectricMachine"), typeof(BasicESmeltingSpeedSkill));
            CraftingComponent.AddRecipe(typeof(JDFurnaceObject), this);
        }
    }
}