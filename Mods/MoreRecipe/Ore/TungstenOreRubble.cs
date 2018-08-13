using Eco.Gameplay.Objects;
using Eco.Shared.Serialization;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/19/2018
 */

namespace Kirthos.Mods.MoreRecipe.Blocks
{
    [BecomesRubble(typeof(TungstenOreRubbleSet1Chunk1Object), typeof(TungstenOreRubbleSet1Chunk2Object), typeof(TungstenOreRubbleSet1Chunk3Object))]
    [BecomesRubble(typeof(TungstenOreRubbleSet2Chunk1Object), typeof(TungstenOreRubbleSet2Chunk2Object), typeof(TungstenOreRubbleSet2Chunk3Object), typeof(TungstenOreRubbleSet2Chunk4Object))]
    [BecomesRubble(typeof(TungstenOreRubbleSet3Chunk1Object), typeof(TungstenOreRubbleSet3Chunk2Object), typeof(TungstenOreRubbleSet3Chunk3Object))]
    [BecomesRubble(typeof(TungstenOreRubbleSet4Chunk1Object), typeof(TungstenOreRubbleSet4Chunk2Object), typeof(TungstenOreRubbleSet4Chunk3Object))]
    public partial class TungstenOreBlock
    { }

    [Serialized] public class TungstenOreRubbleSet1Chunk1Object : RubbleObject<TungstenOreItem> { }
    [Serialized] public class TungstenOreRubbleSet1Chunk2Object : RubbleObject<TungstenOreItem> { }

    [BecomesRubble(typeof(TungstenOreRubbleSet1Chunk3Split1Object), typeof(TungstenOreRubbleSet1Chunk3Split2Object))]
    [Serialized]
    public class TungstenOreRubbleSet1Chunk3Object : RubbleObject<TungstenOreItem> { }
    [Serialized] public class TungstenOreRubbleSet1Chunk3Split1Object : RubbleObject<TungstenOreItem> { }
    [Serialized] public class TungstenOreRubbleSet1Chunk3Split2Object : RubbleObject<TungstenOreItem> { }

    [Serialized] public class TungstenOreRubbleSet2Chunk1Object : RubbleObject<TungstenOreItem> { }
    [Serialized] public class TungstenOreRubbleSet2Chunk2Object : RubbleObject<TungstenOreItem> { }
    [Serialized] public class TungstenOreRubbleSet2Chunk3Object : RubbleObject<TungstenOreItem> { }
    [Serialized] public class TungstenOreRubbleSet2Chunk4Object : RubbleObject<TungstenOreItem> { }

    [Serialized] public class TungstenOreRubbleSet3Chunk1Object : RubbleObject<TungstenOreItem> { }
    [Serialized] public class TungstenOreRubbleSet3Chunk2Object : RubbleObject<TungstenOreItem> { }
    [BecomesRubble(typeof(TungstenOreRubbleSet3Chunk3Split1Object), typeof(TungstenOreRubbleSet3Chunk3Split2Object))]
    [Serialized]
    public class TungstenOreRubbleSet3Chunk3Object : RubbleObject<TungstenOreItem> { }
    [Serialized] public class TungstenOreRubbleSet3Chunk3Split1Object : RubbleObject<TungstenOreItem> { }
    [Serialized] public class TungstenOreRubbleSet3Chunk3Split2Object : RubbleObject<TungstenOreItem> { }

    [BecomesRubble(typeof(TungstenOreRubbleSet4Chunk1Split1Object), typeof(TungstenOreRubbleSet4Chunk1Split2Object))]
    [Serialized]
    public class TungstenOreRubbleSet4Chunk1Object : RubbleObject<TungstenOreItem> { }
    [Serialized] public class TungstenOreRubbleSet4Chunk1Split1Object : RubbleObject<TungstenOreItem> { }
    [Serialized] public class TungstenOreRubbleSet4Chunk1Split2Object : RubbleObject<TungstenOreItem> { }
    [Serialized] public class TungstenOreRubbleSet4Chunk2Object : RubbleObject<TungstenOreItem> { }
    [Serialized] public class TungstenOreRubbleSet4Chunk3Object : RubbleObject<TungstenOreItem> { }
}
