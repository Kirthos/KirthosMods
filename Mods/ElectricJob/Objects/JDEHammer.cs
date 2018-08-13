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
    public class JDEHammerObject :
        WorldObject
    {
        public override string FriendlyName { get { return "Electric Hammer"; } }

        static JDEHammerObject()
        {
            WorldObject.AddOccupancy<JDEHammerObject>(new List<BlockOccupancy>(){
                new BlockOccupancy(new Vector3i(0, 0, -1)),
                new BlockOccupancy(new Vector3i(-1, 0, -1)),
                new BlockOccupancy(new Vector3i(0, 0, 0)),
                new BlockOccupancy(new Vector3i(-1, 0, 0)),

                new BlockOccupancy(new Vector3i(0, 1, -1)),
                new BlockOccupancy(new Vector3i(-1, 1, -1)),
                new BlockOccupancy(new Vector3i(0, 1, 0)),
                new BlockOccupancy(new Vector3i(-1, 1, 0)),

                new BlockOccupancy(new Vector3i(0, 2, -1)),
                new BlockOccupancy(new Vector3i(-1, 2, -1)),
                new BlockOccupancy(new Vector3i(0, 2, 0)),
                new BlockOccupancy(new Vector3i(-1, 2, 0)),

                new BlockOccupancy(new Vector3i(0, 3, -1)),
                new BlockOccupancy(new Vector3i(-1, 3, -1)),
                new BlockOccupancy(new Vector3i(0, 3, 0)),
                new BlockOccupancy(new Vector3i(-1, 3, 0)),
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
    public class JDEHammerItem : BrokenItem<JDEHammerObject>
    {
        public override string FriendlyName { get { return "Electric Hammer"; } }
        public override string Description { get { return "A Electric Hammer - " + (IsBroken ? "Machine is broken" : BrokenComponent.BreakChanceToString(BrokenState)); } }

    }


    [RequiresSkill(typeof(BasicEWorkingSkill), 2)]
    public class JDEHammerRecipe : Recipe
    {
        public JDEHammerRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<JDEHammerItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<SteelItem>(typeof(BasicEWorkingEfficiencySkill), 22, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
            };
            SkillModifiedValue value = new SkillModifiedValue(360f, BasicEWorkingSpeedSkill.MultiplicativeStrategy, typeof(BasicEWorkingSpeedSkill), Localizer.DoStr("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(JDEHammerRecipe), Item.Get<JDEHammerItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<JDEHammerItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Electric Hammer", typeof(JDEHammerRecipe));
            CraftingComponent.AddRecipe(typeof(AnvilObject), this);
        }
    }
}