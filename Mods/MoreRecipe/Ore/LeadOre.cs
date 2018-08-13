using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Shared.Serialization;
using Eco.World;
using Eco.World.Blocks;
using System;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/19/2018
 */

namespace Kirthos.Mods.MoreRecipe.Blocks
{

    [Serialized]
    [Minable, Solid, Wall]
    public partial class LeadOreBlock :
        Block
    { }

    [Serialized]
    [MaxStackSize(20)]
    [Weight(30000)]
    [ResourcePile]
    public class LeadOreItem :
    BlockItem<LeadOreBlock>
    {
        public override string FriendlyName { get { return "Lead Ore"; } }
        public override string FriendlyNamePlural { get { return "Lead Ore"; } }
        public override string Description { get { return "Unrefined ore with traces of lead."; } }

        public override bool CanStickToWalls { get { return false; } }

        private static Type[] blockTypes = new Type[] {
            typeof(LeadOreStacked1Block),
            typeof(LeadOreStacked2Block),
            typeof(LeadOreStacked3Block),
            typeof(LeadOreStacked4Block)
        };
        public override Type[] BlockTypes { get { return blockTypes; } }
    }

    [Serialized, Solid] public class LeadOreStacked1Block : PickupableBlock { }
    [Serialized, Solid] public class LeadOreStacked2Block : PickupableBlock { }
    [Serialized, Solid] public class LeadOreStacked3Block : PickupableBlock { }
    [Serialized, Solid, Wall] public class LeadOreStacked4Block : PickupableBlock { }
}
