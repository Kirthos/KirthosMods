namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Economy;
    using Eco.Gameplay.Housing;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Minimap;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Pipes.LiquidComponents;
    using Eco.Gameplay.Pipes.Gases;
    using Eco.Gameplay.Systems.Tooltip;
    using Eco.Shared;
    using Eco.Shared.Math;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.View;
    using Eco.Shared.Items;
    using Eco.Gameplay.Pipes;
    using Eco.World.Blocks;

    [Serialized]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    public partial class GatewayObject :
        WorldObject
    {
        [Serialized] public bool State = false; // false -> door closed
        public override InteractResult OnActRight(InteractionContext context)
        {
            State = !State;
            this.SetAnimatedState("Open", State);
            return base.OnActRight(context)
        }

        public override string FriendlyName { get { return "Large Lumber Door"; } }


        protected override void Initialize()
        {


        }

        public override void Destroy()
        {
            base.Destroy();
        }

    }

    [Serialized]
    public partial class GatewayItem :
        WorldObjectItem<GatewayObject>
    {
        public override string FriendlyName { get { return "Large Lumber Door"; } }
        public override string Description { get { return "A large door."; } }

        static GatewayItem()
        {

        }

    }


    public partial class GatewayRecipe : Recipe
    {
        public GatewayRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<GatewayItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<LumberItem>(typeof(LumberWoodworkingEfficiencySkill), 20, LumberWoodworkingEfficiencySkill.MultiplicativeStrategy),
            };
            SkillModifiedValue value = new SkillModifiedValue(20, LumberWoodworkingSpeedSkill.MultiplicativeStrategy, typeof(LumberWoodworkingSpeedSkill), Localizer.DoStr("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(GatewayRecipe), Item.Get<GatewayItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<GatewayItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Large Lumber Door", typeof(GatewayRecipe));
            CraftingComponent.AddRecipe(typeof(SawmillObject), this);
        }
    }
}