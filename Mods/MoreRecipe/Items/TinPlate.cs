using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;

namespace Kirthos.Mods.MoreRecipe.Items
{
    [RequiresSkill(typeof(SteelworkingSkill), 4)]
    public partial class TinPlateRecipe : Recipe
    {
        public TinPlateRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<TinPlateItem>(1),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<TinIngotItem>(typeof(SteelworkingEfficiencySkill), 10, SteelworkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(TinPlateRecipe), Item.Get<TinPlateItem>().UILinkContent(), 15f, typeof(SteelworkingSpeedSkill));
            this.Initialize("Tin Plate", typeof(TinPlateRecipe));

            CraftingComponent.AddRecipe(typeof(ElectricStampingPressObject), this);
        }
    }

    [Serialized]
    [Weight(10000)]
    [Currency]
    public partial class TinPlateItem :
    Item
    {
        public override string FriendlyName { get { return "Tin Plate"; } }
        public override string Description { get { return "A plate made of tin"; } }
    }
}
