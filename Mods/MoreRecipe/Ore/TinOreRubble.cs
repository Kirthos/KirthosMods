using Eco.Gameplay.Objects;
using Eco.Shared.Serialization;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/19/2018
 */

namespace Kirthos.Mods.MoreRecipe.Blocks
{
    [BecomesRubble(typeof(TinOreRubbleSet1Chunk1Object), typeof(TinOreRubbleSet1Chunk2Object), typeof(TinOreRubbleSet1Chunk3Object))]
    [BecomesRubble(typeof(TinOreRubbleSet2Chunk1Object), typeof(TinOreRubbleSet2Chunk2Object), typeof(TinOreRubbleSet2Chunk3Object), typeof(TinOreRubbleSet2Chunk4Object))]
    [BecomesRubble(typeof(TinOreRubbleSet3Chunk1Object), typeof(TinOreRubbleSet3Chunk2Object), typeof(TinOreRubbleSet3Chunk3Object))]
    [BecomesRubble(typeof(TinOreRubbleSet4Chunk1Object), typeof(TinOreRubbleSet4Chunk2Object), typeof(TinOreRubbleSet4Chunk3Object))]
    public partial class TinOreBlock
    { }

    [Serialized] public class TinOreRubbleSet1Chunk1Object : RubbleObject<TinOreItem> { }
    [Serialized] public class TinOreRubbleSet1Chunk2Object : RubbleObject<TinOreItem> { }

    [BecomesRubble(typeof(TinOreRubbleSet1Chunk3Split1Object), typeof(TinOreRubbleSet1Chunk3Split2Object))]
    [Serialized]
    public class TinOreRubbleSet1Chunk3Object : RubbleObject<TinOreItem> { }
    [Serialized] public class TinOreRubbleSet1Chunk3Split1Object : RubbleObject<TinOreItem> { }
    [Serialized] public class TinOreRubbleSet1Chunk3Split2Object : RubbleObject<TinOreItem> { }

    [Serialized] public class TinOreRubbleSet2Chunk1Object : RubbleObject<TinOreItem> { }
    [Serialized] public class TinOreRubbleSet2Chunk2Object : RubbleObject<TinOreItem> { }
    [Serialized] public class TinOreRubbleSet2Chunk3Object : RubbleObject<TinOreItem> { }
    [Serialized] public class TinOreRubbleSet2Chunk4Object : RubbleObject<TinOreItem> { }

    [Serialized] public class TinOreRubbleSet3Chunk1Object : RubbleObject<TinOreItem> { }
    [Serialized] public class TinOreRubbleSet3Chunk2Object : RubbleObject<TinOreItem> { }
    [BecomesRubble(typeof(TinOreRubbleSet3Chunk3Split1Object), typeof(TinOreRubbleSet3Chunk3Split2Object))]
    [Serialized]
    public class TinOreRubbleSet3Chunk3Object : RubbleObject<TinOreItem> { }
    [Serialized] public class TinOreRubbleSet3Chunk3Split1Object : RubbleObject<TinOreItem> { }
    [Serialized] public class TinOreRubbleSet3Chunk3Split2Object : RubbleObject<TinOreItem> { }

    [BecomesRubble(typeof(TinOreRubbleSet4Chunk1Split1Object), typeof(TinOreRubbleSet4Chunk1Split2Object))]
    [Serialized]
    public class TinOreRubbleSet4Chunk1Object : RubbleObject<TinOreItem> { }
    [Serialized] public class TinOreRubbleSet4Chunk1Split1Object : RubbleObject<TinOreItem> { }
    [Serialized] public class TinOreRubbleSet4Chunk1Split2Object : RubbleObject<TinOreItem> { }
    [Serialized] public class TinOreRubbleSet4Chunk2Object : RubbleObject<TinOreItem> { }
    [Serialized] public class TinOreRubbleSet4Chunk3Object : RubbleObject<TinOreItem> { }
}
