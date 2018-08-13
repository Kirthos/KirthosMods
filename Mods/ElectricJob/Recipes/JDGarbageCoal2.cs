namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Gameplay.Systems.TextLinks;

    [RequiresSkill(typeof(BasicESmeltingSkill), 1)]
    public class JDGarbageCoalRecipe : Recipe
    {
        public JDGarbageCoalRecipe()
        {
            this.Products = new CraftingElement[]
            {
               new CraftingElement<CoalItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<GarbageItem>(typeof(BasicESmeltingEfficiencySkill), 2, BasicESmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.Initialize("JDGarbageCoal", typeof(JDGarbageCoalRecipe));
            this.CraftMinutes = CreateCraftTimeValue(typeof(JDGarbageCoalRecipe), this.UILink(), 8.0f, typeof(BasicESmeltingSpeedSkill));
            CraftingComponent.AddRecipe(typeof(JDFurnaceObject), this);
        }
    }
}