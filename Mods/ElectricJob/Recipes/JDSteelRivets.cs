namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(BasicESmeltingSkill), 1)]
    public class JDSteelRivetsRecipe : Recipe
    {
        public JDSteelRivetsRecipe()
        {
            this.Products = new CraftingElement[]
            {
               new CraftingElement<RivetItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<SteelItem>(typeof(BasicESmeltingEfficiencySkill), 1, BasicESmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.Initialize("Steel Rivets", typeof(JDSteelRivetsRecipe));
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDSteelRivetsRecipe), this.UILink(), 2.0f * JDElectricalPlugin.Conf.Get<float>("CraftingTimeMultiplierForElectricMachine"), typeof(BasicESmeltingSpeedSkill));
            CraftingComponent.AddRecipe(typeof(JDFurnaceObject), this);
        }
    }
}