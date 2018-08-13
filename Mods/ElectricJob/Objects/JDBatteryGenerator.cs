namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Housing;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Systems.Tooltip;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Serialization;

    [Serialized]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(FuelSupplyComponent))]
    [RequireComponent(typeof(FuelConsumptionComponent))]
    [RequireComponent(typeof(PowerGridComponent))]
    [RequireComponent(typeof(PowerGeneratorComponent))]
    [RequireComponent(typeof(HousingComponent))]
    //[RequireComponent(typeof(SolidGroundComponent))]            
    public partial class JDBatteryGeneratorObject :
        WorldObject
    {
        public override string FriendlyName { get { return "Electric Generator"; } }

        private static Type[] fuelTypeList = new Type[]
        {
            typeof(JDBatteryItem),
            typeof(JDBigBatteryItem),
        };

        static JDBatteryGeneratorObject()
        {
            WorldObject.AddOccupancy<JDBatteryGeneratorObject>(new List<BlockOccupancy>(){
                new BlockOccupancy(new Vector3i(0, 0, -1)),
                new BlockOccupancy(new Vector3i(0, 0, 0)),
                new BlockOccupancy(new Vector3i(0, 0, 1)),
            });
        }

        protected override void Initialize()
        {
            this.GetComponent<MinimapComponent>().Initialize("Power");
            this.GetComponent<FuelSupplyComponent>().Initialize(2, fuelTypeList);
            this.GetComponent<FuelConsumptionComponent>().Initialize(100);
            this.GetComponent<PowerGridComponent>().Initialize(30, new ElectricPower());
            this.GetComponent<PowerGeneratorComponent>().Initialize(1500);
            this.GetComponent<HousingComponent>().Set(JDBatteryGeneratorItem.HousingVal);
        }

        public override void Destroy()
        {
            base.Destroy();
        }

    }

    [Serialized]
    public partial class JDBatteryGeneratorItem : WorldObjectItem<JDBatteryGeneratorObject>
    {
        public override string FriendlyName { get { return "Electric Generator"; } }
        public override string Description { get { return "Use battery to produce power"; } }

        [TooltipChildren] public HousingValue HousingTooltip { get { return HousingVal; } }
        [TooltipChildren]
        public static HousingValue HousingVal
        {
            get
            {
                return new HousingValue()
                {
                    Category = "Industrial",
                    TypeForRoomLimit = "",
                };
            }
        }
    }


    [RequiresSkill(typeof(MechanicalEngineeringSkill), 3)]
    public partial class JDBatteryGeneratorRecipe : Recipe
    {
        public JDBatteryGeneratorRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<JDBatteryGeneratorItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<IronIngotItem>(typeof(MechanicsAssemblyEfficiencySkill), 20, MechanicsAssemblyEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<PistonItem>(typeof(MechanicsAssemblyEfficiencySkill), 5, MechanicsAssemblyEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<CombustionEngineItem>(typeof(MechanicsAssemblyEfficiencySkill), 1, MechanicsAssemblyEfficiencySkill.MultiplicativeStrategy),
            };
            SkillModifiedValue value = new SkillModifiedValue(150, MechanicsAssemblySpeedSkill.MultiplicativeStrategy, typeof(MechanicsAssemblySpeedSkill), Localizer.DoStr("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(JDBatteryGeneratorRecipe), Item.Get<JDBatteryGeneratorItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<JDBatteryGeneratorItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Electric Generator", typeof(JDBatteryGeneratorRecipe));
            CraftingComponent.AddRecipe(typeof(AssemblyLineObject), this);
        }
    }
}