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
    public partial class TungstenOreBlock :
        Block
    { }

    [Serialized]
    [MaxStackSize(20)]
    [Weight(30000)]
    [ResourcePile]
    public class TungstenOreItem :
    BlockItem<TungstenOreBlock>
    {
        public override string FriendlyName { get { return "Tungsten Ore"; } }
        public override string FriendlyNamePlural { get { return "Tungsten Ore"; } }
        public override string Description { get { return "Unrefined ore with traces of tungsten."; } }

        public override bool CanStickToWalls { get { return false; } }

        private static Type[] blockTypes = new Type[] {
            typeof(TungstenOreStacked1Block),
            typeof(TungstenOreStacked2Block),
            typeof(TungstenOreStacked3Block),
            typeof(TungstenOreStacked4Block)
        };
        public override Type[] BlockTypes { get { return blockTypes; } }
    }

    [Serialized, Solid] public class TungstenOreStacked1Block : PickupableBlock { }
    [Serialized, Solid] public class TungstenOreStacked2Block : PickupableBlock { }
    [Serialized, Solid] public class TungstenOreStacked3Block : PickupableBlock { }
    [Serialized, Solid, Wall] public class TungstenOreStacked4Block : PickupableBlock { }
}
