namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Systems.Tooltip;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Serialization;
    using JDElectricJob.Components;
    using System.Collections.Generic;

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(CraftingComponent))]
    [RequireComponent(typeof(PowerGridComponent))]
    [RequireComponent(typeof(PowerConsumptionComponent))]
    [RequireComponent(typeof(BrokenComponent))]
    //[RequireComponent(typeof(SolidGroundComponent))]                  
    public class JDKilnObject :
        WorldObject
    {
        public override string FriendlyName { get { return "Electric Kiln"; } }

        static JDKilnObject()
        {
            WorldObject.AddOccupancy<JDKilnObject>(new List<BlockOccupancy>(){
                new BlockOccupancy(new Vector3i(0, 0, -1)),
                new BlockOccupancy(new Vector3i(-1, 0, -1)),
                new BlockOccupancy(new Vector3i(1, 0, -1)),
                new BlockOccupancy(new Vector3i(0, 0, 0)),
                new BlockOccupancy(new Vector3i(-1, 0, 0)),
                new BlockOccupancy(new Vector3i(1, 0, 0)),
                new BlockOccupancy(new Vector3i(0, 0, 1)),
                new BlockOccupancy(new Vector3i(-1, 0, 1)),
                new BlockOccupancy(new Vector3i(1, 0, 1)),

                new BlockOccupancy(new Vector3i(0, 1, -1)),
                new BlockOccupancy(new Vector3i(-1, 1, -1)),
                new BlockOccupancy(new Vector3i(1, 1, -1)),
                new BlockOccupancy(new Vector3i(0, 1, 0)),
                new BlockOccupancy(new Vector3i(-1, 1, 0)),
                new BlockOccupancy(new Vector3i(1, 1, 0)),
                new BlockOccupancy(new Vector3i(0, 1, 1)),
                new BlockOccupancy(new Vector3i(-1, 1, 1)),
                new BlockOccupancy(new Vector3i(1, 1, 1)),

                new BlockOccupancy(new Vector3i(0, 2, -1)),
                new BlockOccupancy(new Vector3i(-1, 2, -1)),
                new BlockOccupancy(new Vector3i(1, 2, -1)),
                new BlockOccupancy(new Vector3i(0, 2, 0)),
                new BlockOccupancy(new Vector3i(-1, 2, 0)),
                new BlockOccupancy(new Vector3i(1, 2, 0)),
                new BlockOccupancy(new Vector3i(0, 2, 1)),
                new BlockOccupancy(new Vector3i(-1, 2, 1)),
                new BlockOccupancy(new Vector3i(1, 2, 1)),
            });
        }

        protected override void Initialize()
        {
            this.GetComponent<MinimapComponent>().Initialize("Crafting");
            this.GetComponent<PowerConsumptionComponent>().Initialize(1600);
            this.GetComponent<PowerGridComponent>().Initialize(10, new ElectricPower());
        }

        public override void Destroy()
        {
            base.Destroy();
        }

    }

    [Serialized]
    public class JDKilnItem : BrokenItem<JDKilnObject>
    {
        public override string FriendlyName { get { return "Electric Kiln"; } }
        public override string Description { get { return "A Electric Kiln - " + (IsBroken ? "Machine is broken" : BrokenComponent.BreakChanceToString(BrokenState)); } }

    }


    [RequiresSkill(typeof(BasicEWorkingSkill), 1)]
    public class JDKilnRecipe : Recipe
    {
        public JDKilnRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<JDKilnItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<SteelItem>(typeof(BasicEWorkingEfficiencySkill), 25, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<IronIngotItem>(typeof(BasicEWorkingEfficiencySkill), 20, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
            };
            SkillModifiedValue value = new SkillModifiedValue(360f, BasicEWorkingSpeedSkill.MultiplicativeStrategy, typeof(BasicEWorkingSpeedSkill), Localizer.DoStr("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(JDKilnRecipe), Item.Get<JDKilnItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<JDKilnItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Electric Kiln", typeof(JDKilnRecipe));
            CraftingComponent.AddRecipe(typeof(AnvilObject), this);
        }
    }
}