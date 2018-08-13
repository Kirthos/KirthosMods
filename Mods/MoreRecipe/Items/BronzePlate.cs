using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;

namespace Kirthos.Mods.MoreRecipe.Items
{
    [RequiresSkill(typeof(SteelworkingSkill), 4)]
    public partial class BronzePlateRecipe : Recipe
    {
        public BronzePlateRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<BronzePlateItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<BronzeIngotItem>(typeof(SteelworkingEfficiencySkill), 10, SteelworkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(BronzePlateRecipe), Item.Get<BronzePlateItem>().UILinkContent(), 15f, typeof(SteelworkingSpeedSkill));
            this.Initialize("Bronze Plate", typeof(BronzePlateRecipe));

            CraftingComponent.AddRecipe(typeof(ElectricStampingPressObject), this);
        }
    }

    [Serialized]
    [Weight(10000)]
    [Currency]
    public partial class BronzePlateItem :
    Item
    {
        public override string FriendlyName { get { return "Bronze Plate"; } }
        public override string Description { get { return "A plate made of Bronze"; } }
    }
}
