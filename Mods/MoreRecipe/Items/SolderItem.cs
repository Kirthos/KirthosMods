using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;

namespace Kirthos.Mods.MoreRecipe.Items
{
    [RequiresSkill(typeof(AlloySmeltingSkill), 3)]
    public partial class SolderItemRecipe : Recipe
    {
        public SolderItemRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<SolderItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<TinIngotItem>(typeof(AlloySmeltingEfficiencySkill), 5, AlloySmeltingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<LeadIngotItem>(typeof(AlloySmeltingEfficiencySkill), 4, AlloySmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(SolderItemRecipe), Item.Get<SolderItem>().UILinkContent(), 10f, typeof(AlloySmeltingSpeedSkill));
            this.Initialize("Solder", typeof(SolderItemRecipe));

            CraftingComponent.AddRecipe(typeof(BlastFurnaceObject), this);
        }
    }

    [Serialized]
    [Weight(10000)]
    [Currency]
    public partial class SolderItem :
    Item
    {
        public override string FriendlyName { get { return "Solder"; } }
        public override string Description { get { return "Solder are used in electronics"; } }
    }
}
