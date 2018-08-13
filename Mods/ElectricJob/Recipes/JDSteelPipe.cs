namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(BasicESmeltingSkill), 1)]
    public class JDSteelPipeRecipe : Recipe
    {
        public JDSteelPipeRecipe()
        {
            this.Products = new CraftingElement[]
            {
               new CraftingElement<SteelPipeItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<SteelItem>(typeof(BasicESmeltingEfficiencySkill), 2, BasicESmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.Initialize("Steel Pipe", typeof(JDSteelPipeRecipe));
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDSteelPipeRecipe), this.UILink(), 4.0f * JDElectricalPlugin.Conf.Get<float>("CraftingTimeMultiplierForElectricMachine"), typeof(BasicESmeltingSpeedSkill));
            CraftingComponent.AddRecipe(typeof(JDFurnaceObject), this);
        }
    }
}