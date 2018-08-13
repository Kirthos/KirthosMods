namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(BasicESmeltingSkill), 1)]
    public class JDRivetRecipe : Recipe
    {
        public JDRivetRecipe()
        {
            this.Products = new CraftingElement[]
            {
               new CraftingElement<RivetItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<IronIngotItem>(typeof(BasicESmeltingEfficiencySkill), 1, BasicESmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.Initialize("Rivet", typeof(JDRivetRecipe));
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDRivetRecipe), this.UILink(), 0.5f * JDElectricalPlugin.Conf.Get<float>("CraftingTimeMultiplierForElectricMachine"), typeof(BasicESmeltingSpeedSkill));
            CraftingComponent.AddRecipe(typeof(JDFurnaceObject), this);
        }
    }
}