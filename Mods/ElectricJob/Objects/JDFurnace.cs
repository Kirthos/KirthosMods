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
    using Eco.Shared.Utils;
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
    public class JDFurnaceObject :
        WorldObject
    {
        public override string FriendlyName { get { return "Electric Furnace"; } }

        static JDFurnaceObject()
        {
            WorldObject.AddOccupancy<JDFurnaceObject>(new List<BlockOccupancy>(){
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

                new BlockOccupancy(new Vector3i(0, 3, -1)),
                new BlockOccupancy(new Vector3i(-1, 3, -1)),
                new BlockOccupancy(new Vector3i(1, 3, -1)),
                new BlockOccupancy(new Vector3i(0, 3, 0)),
                new BlockOccupancy(new Vector3i(-1, 3, 0)),
                new BlockOccupancy(new Vector3i(1, 3, 0)),
                new BlockOccupancy(new Vector3i(0, 3, 1)),
                new BlockOccupancy(new Vector3i(-1, 3, 1)),
                new BlockOccupancy(new Vector3i(1, 3, 1)),

                new BlockOccupancy(new Vector3i(0, 4, -1)),
                new BlockOccupancy(new Vector3i(-1, 4, -1)),
                new BlockOccupancy(new Vector3i(1, 4, -1)),
                new BlockOccupancy(new Vector3i(0, 4, 0)),
                new BlockOccupancy(new Vector3i(-1, 4, 0)),
                new BlockOccupancy(new Vector3i(1, 4, 0)),
                new BlockOccupancy(new Vector3i(0, 4, 1)),
                new BlockOccupancy(new Vector3i(-1, 4, 1)),
                new BlockOccupancy(new Vector3i(1, 4, 1)),
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
    public class JDFurnaceItem : BrokenItem<JDFurnaceObject>
    {
        public override string FriendlyName { get { return "Electric Furnace"; } }
        public override string Description { get { return "A Electric Furnace - " + (IsBroken ? "Machine is broken" : BrokenComponent.BreakChanceToString(BrokenState)); } }
    }


    [RequiresSkill(typeof(BasicESmeltingSkill), 3)]
    public class JDFurnaceRecipe : Recipe
    {
        public JDFurnaceRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<JDFurnaceItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<BrickItem>(typeof(BasicESmeltingEfficiencySkill), 30, BasicESmeltingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<IronIngotItem>(typeof(BasicESmeltingEfficiencySkill), 30, BasicESmeltingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<SteelItem>(typeof(BasicESmeltingEfficiencySkill), 30, BasicESmeltingEfficiencySkill.MultiplicativeStrategy),
            };
            SkillModifiedValue value = new SkillModifiedValue(360f, BasicESmeltingSpeedSkill.MultiplicativeStrategy, typeof(BasicESmeltingSpeedSkill), Localizer.DoStr("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(JDFurnaceRecipe), Item.Get<JDFurnaceItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<JDFurnaceItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Electric Furnace", typeof(JDFurnaceRecipe));
            CraftingComponent.AddRecipe(typeof(AnvilObject), this);
        }
    }
}