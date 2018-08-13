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
    public partial class TinOreBlock :
        Block
    { }

    [Serialized]
    [MaxStackSize(20)]
    [Weight(30000)]
    [ResourcePile]
    public class TinOreItem :
    BlockItem<TinOreBlock>
    {
        public override string FriendlyName { get { return "Tin Ore"; } }
        public override string FriendlyNamePlural { get { return "Tin Ore"; } }
        public override string Description { get { return "Unrefined ore with traces of Tin."; } }

        public override bool CanStickToWalls { get { return false; } }

        private static Type[] blockTypes = new Type[] {
            typeof(TinOreStacked1Block),
            typeof(TinOreStacked2Block),
            typeof(TinOreStacked3Block),
            typeof(TinOreStacked4Block)
        };
        public override Type[] BlockTypes { get { return blockTypes; } }
    }

    [Serialized, Solid] public class TinOreStacked1Block : PickupableBlock { }
    [Serialized, Solid] public class TinOreStacked2Block : PickupableBlock { }
    [Serialized, Solid] public class TinOreStacked3Block : PickupableBlock { }
    [Serialized, Solid, Wall] public class TinOreStacked4Block : PickupableBlock { }
}
