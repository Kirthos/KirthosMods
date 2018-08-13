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
    using System.Collections.Generic;

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(CraftingComponent))]
    [RequireComponent(typeof(PowerGridComponent))]
    [RequireComponent(typeof(PowerConsumptionComponent))]
    //[RequireComponent(typeof(SolidGroundComponent))]                  
    public partial class JDBatteryChargerObject :
        WorldObject
    {
        public override string FriendlyName { get { return "Battery Charger"; } }

        static JDBatteryChargerObject()
        {
            WorldObject.AddOccupancy<JDBatteryChargerObject>(new List<BlockOccupancy>(){
                new BlockOccupancy(new Vector3i(0, 0, -1)),
                new BlockOccupancy(new Vector3i(0, 0, 0)),
                new BlockOccupancy(new Vector3i(0, 0, 1)),

                new BlockOccupancy(new Vector3i(0, 1, -1)),
                new BlockOccupancy(new Vector3i(0, 1, 0)),
                new BlockOccupancy(new Vector3i(0, 1, 1)),

                new BlockOccupancy(new Vector3i(0, 2, -1)),
                new BlockOccupancy(new Vector3i(0, 2, 0)),
                new BlockOccupancy(new Vector3i(0, 2, 1)),
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
    public partial class JDBatteryChargerItem : WorldObjectItem<JDBatteryChargerObject>
    {
        public override string FriendlyName { get { return "Battery Charger"; } }
        public override string Description { get { return "Charge uncharged battery"; } }
    }


    [RequiresSkill(typeof(BasicEWorkingSkill), 1)]
    public partial class JDBatteryChargerRecipe : Recipe
    {
        public JDBatteryChargerRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<JDBatteryChargerItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<SteelItem>(typeof(BasicEWorkingEfficiencySkill), 25, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<IronIngotItem>(typeof(BasicEWorkingEfficiencySkill), 20, BasicEWorkingEfficiencySkill.MultiplicativeStrategy),
            };
            SkillModifiedValue value = new SkillModifiedValue(360f, BasicEWorkingSpeedSkill.MultiplicativeStrategy, typeof(BasicEWorkingSpeedSkill), Localizer.DoStr("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(JDBatteryChargerRecipe), Item.Get<JDBatteryChargerItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<JDBatteryChargerItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Battery Charger", typeof(JDBatteryChargerRecipe));
            CraftingComponent.AddRecipe(typeof(AssemblyLineObject), this);
        }
    }
}