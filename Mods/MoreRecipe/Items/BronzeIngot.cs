using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;

namespace Kirthos.Mods.MoreRecipe.Items
{
    [RequiresSkill(typeof(AlloySmeltingSkill), 4)]
    public partial class BronzeIngotRecipe : Recipe
    {
        public BronzeIngotRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<BronzeIngotItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<CopperIngotItem>(typeof(AlloySmeltingEfficiencySkill), 10, AlloySmeltingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<TinIngotItem>(typeof(AlloySmeltingEfficiencySkill), 2, AlloySmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(BronzeIngotRecipe), Item.Get<BronzeIngotItem>().UILinkContent(), 10f, typeof(AlloySmeltingSpeedSkill));
            this.Initialize("Bronze Ingot", typeof(BronzeIngotRecipe));

            CraftingComponent.AddRecipe(typeof(BlastFurnaceObject), this);
        }
    }

    [Serialized]
    [Weight(10000)]
    [Currency]
    public partial class BronzeIngotItem :
    Item
    {
        public override string FriendlyName { get { return "Bronze Ingot"; } }
        public override string Description { get { return "A bronze ingot made of copper and tin"; } }
    }
}
