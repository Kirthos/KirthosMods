using Eco.Gameplay.Components;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Math;
using Eco.Shared.Networking;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/12/2018
 */

namespace Kirthos.Mods.KirthosExplosive
{
    [Serialized]
    public class DynamiteObject : PhysicsWorldObject
    {
        private bool ignited = false;
        private double ignitedTime;
        private double timeToIgnite = 3.8;
        private bool hasExplode = false;
        private Player igniter;

        public override string FriendlyName { get { return "Mining Dynamite"; } }

        protected override void Initialize()
        {
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        public override InteractResult OnActRight(InteractionContext context)
        {
            Ignite(timeToIgnite, context.Player);
            return InteractResult.Success;
        }

        public override InteractResult OnActInteract(InteractionContext context)
        {
            Ignite(timeToIgnite, context.Player);
            return base.OnActInteract(context);
        }

        public void Explode()
        {
            if (hasExplode == false)
            {
                ignitedTime = TimeUtil.Seconds;
                SetAnimatedState("Explode", true);
                hasExplode = true;
                Utils.Explosion(igniter, Position3i, 3, 25);
            }
        }

        public void Ignite(double igniteTime, Player playerIgniter)
        {
            if (ignited == false)
            {
                igniter = playerIgniter;
                timeToIgnite = igniteTime;
                ignited = true;
                ignitedTime = TimeUtil.Seconds;
                SetAnimatedState("Ignite", true);
            }
        }

        public override void Tick()
        {
            if (ignited)
            {
                if (hasExplode == false && ignitedTime + timeToIgnite < TimeUtil.Seconds)
                {
                    Explode();
                }
            }
            if (hasExplode && ignitedTime + 4.5 < TimeUtil.Seconds)
            {
                Destroy();
            }
            base.Tick();
        }
    }

    [Serialized]
    public class DynamiteItem :
    WorldObjectItem<DynamiteObject>
    {
        public override string FriendlyName { get { return "Mining dynamite"; } }
        public override string Description { get { return "A explosive dynamite. Placed dynamite can be actived with right click or when interract. 25% of mined block became rubble."; } }

        static DynamiteItem()
        {

        }
    }

    [Serialized]
    public class GunpowderItem : Item
    {
        public override string FriendlyName { get { return "Gunpowder"; } }
        public override string Description { get { return "A black powder used to make dynamite."; } }

        static GunpowderItem()
        {

        }
    }

    [RequiresSkill(typeof(MortarProductionSkill), 4)]
    public partial class GunpowderRecipe : Recipe
    {
        public GunpowderRecipe()
        {
            this.Products = new CraftingElement[] { new CraftingElement<GunpowderItem>() };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<CopperOreItem>(typeof(MortarProductionEfficiencySkill), 2, MortarProductionEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<SandItem>(typeof(MortarProductionEfficiencySkill), 5, MortarProductionEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<StoneItem>(typeof(MortarProductionEfficiencySkill), 5, MortarProductionEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(GunpowderRecipe), Item.Get<GunpowderItem>().UILinkContent(), 5, typeof(MortarProductionSpeedSkill));
            this.Initialize("Gunpowder", typeof(GunpowderRecipe));
            CraftingComponent.AddRecipe(typeof(MasonryTableObject), this);
        }
    }

    [RequiresSkill(typeof(PrimitiveMechanicsSkill), 2)]
    public partial class DynamiteRecipe : Recipe
    {
        public DynamiteRecipe()
        {
            this.Products = new CraftingElement[] { new CraftingElement<DynamiteItem>() };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<GunpowderItem>(typeof(PrimitiveMechanicsEfficiencySkill), 5, PrimitiveMechanicsEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<PhosphateFertilizerItem>(typeof(PrimitiveMechanicsEfficiencySkill), 2, PrimitiveMechanicsEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<PaperItem>(typeof(PrimitiveMechanicsEfficiencySkill), 2, PrimitiveMechanicsEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(GunpowderRecipe), Item.Get<GunpowderItem>().UILinkContent(), 5, typeof(PrimitiveMechanicsSpeedSkill));
            this.Initialize("Dynamite", typeof(DynamiteRecipe));
            CraftingComponent.AddRecipe(typeof(MachinistTableObject), this);
        }
    }
}
