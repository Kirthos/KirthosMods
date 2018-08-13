using System;
using Eco.Gameplay.Components;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using Eco.Shared.Serialization;
using JDElectricJob.Components;

namespace JDElectricJob.Items
{
    [RequiresSkill(typeof(BasicEWorkingSkill), 3)]
    public partial class RepairToolKitRecipe : Recipe
    {
        public RepairToolKitRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<RepairToolKitItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<CopperWiringItem>(typeof(BasicEWorkingEfficiencySkill), 10, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<IronIngotItem>(typeof(BasicEWorkingEfficiencySkill), 5, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<SteelItem>(typeof(BasicEWorkingEfficiencySkill), 5, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(RepairToolKitRecipe), Item.Get<RepairToolKitItem>().UILink(), 10f, typeof(BasicEWorkingSpeedSkill));
            this.Initialize("Repair ToolKit", typeof(RepairToolKitRecipe));

            CraftingComponent.AddRecipe(typeof(AnvilObject), this);
        }
    }


    [Serialized]
    [Weight(1000)]
    public partial class RepairToolKitItem :
    Item, IInteractingItem
    {
        public override string FriendlyName { get { return "Repair ToolKit"; } }
        public override string Description { get { return "Used to repair a broken machine."; } }

        public float InteractDistance => DefaultInteractDistance.Tool;

        public override InteractResult OnActLeft(InteractionContext context)
        {
            if (context.HasTarget && context.Target is WorldObject)
            {
                BrokenComponent target = (context.Target as WorldObject).GetComponent<BrokenComponent>();
                if (target != null && target.Enabled == false)
                {
                    if (context.Player.User.Inventory.TryRemoveItems(GetType(), 1))
                    {
                        target.Repair();
                        return InteractResult.Success;
                    }
                }
            }
            return InteractResult.NoOp;
        }

        public override InteractResult OnActRight(InteractionContext context)
        {
            return OnActLeft(context);
        }

        public override InteractResult OnActInteract(InteractionContext context)
        {
            return InteractResult.NoOp;
        }

        public bool ShouldHighlight(Type block)
        {
            return false;
        }
    }
}
