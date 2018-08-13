using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;
using Kirthos.Mods.MoreRecipe.Blocks;

namespace Kirthos.Mods.MoreRecipe.Items
{
    [RequiresSkill(typeof(AlloySmeltingSkill), 2)]
    public partial class TinIngotRecipe : Recipe
    {
        public TinIngotRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<TinIngotItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<TinOreItem>(typeof(AlloySmeltingEfficiencySkill), 10, AlloySmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(TinIngotRecipe), Item.Get<TinIngotItem>().UILinkContent(), 10f, typeof(AlloySmeltingSpeedSkill));
            this.Initialize("Tin Ingot", typeof(TinIngotRecipe));

            CraftingComponent.AddRecipe(typeof(BlastFurnaceObject), this);
        }
    }

    [Serialized]
    [Weight(10000)]
    [Currency]
    public partial class TinIngotItem :
    Item
    {
        public override string FriendlyName { get { return "Tin Ingot"; } }
        public override string Description { get { return "An ingot made of tin"; } }
    }
}
