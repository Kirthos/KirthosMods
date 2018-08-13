using Eco.Gameplay.Objects;
using Eco.Shared.Serialization;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/19/2018
 */

namespace Kirthos.Mods.MoreRecipe.Blocks
{
    [BecomesRubble(typeof(LeadOreRubbleSet1Chunk1Object), typeof(LeadOreRubbleSet1Chunk2Object), typeof(LeadOreRubbleSet1Chunk3Object))]
    [BecomesRubble(typeof(LeadOreRubbleSet2Chunk1Object), typeof(LeadOreRubbleSet2Chunk2Object), typeof(LeadOreRubbleSet2Chunk3Object), typeof(LeadOreRubbleSet2Chunk4Object))]
    [BecomesRubble(typeof(LeadOreRubbleSet3Chunk1Object), typeof(LeadOreRubbleSet3Chunk2Object), typeof(LeadOreRubbleSet3Chunk3Object))]
    [BecomesRubble(typeof(LeadOreRubbleSet4Chunk1Object), typeof(LeadOreRubbleSet4Chunk2Object), typeof(LeadOreRubbleSet4Chunk3Object))]
    public partial class LeadOreBlock
    { }

    [Serialized] public class LeadOreRubbleSet1Chunk1Object : RubbleObject<LeadOreItem> { }
    [Serialized] public class LeadOreRubbleSet1Chunk2Object : RubbleObject<LeadOreItem> { }

    [BecomesRubble(typeof(LeadOreRubbleSet1Chunk3Split1Object), typeof(LeadOreRubbleSet1Chunk3Split2Object))]
    [Serialized]
    public class LeadOreRubbleSet1Chunk3Object : RubbleObject<LeadOreItem> { }
    [Serialized] public class LeadOreRubbleSet1Chunk3Split1Object : RubbleObject<LeadOreItem> { }
    [Serialized] public class LeadOreRubbleSet1Chunk3Split2Object : RubbleObject<LeadOreItem> { }

    [Serialized] public class LeadOreRubbleSet2Chunk1Object : RubbleObject<LeadOreItem> { }
    [Serialized] public class LeadOreRubbleSet2Chunk2Object : RubbleObject<LeadOreItem> { }
    [Serialized] public class LeadOreRubbleSet2Chunk3Object : RubbleObject<LeadOreItem> { }
    [Serialized] public class LeadOreRubbleSet2Chunk4Object : RubbleObject<LeadOreItem> { }

    [Serialized] public class LeadOreRubbleSet3Chunk1Object : RubbleObject<LeadOreItem> { }
    [Serialized] public class LeadOreRubbleSet3Chunk2Object : RubbleObject<LeadOreItem> { }
    [BecomesRubble(typeof(LeadOreRubbleSet3Chunk3Split1Object), typeof(LeadOreRubbleSet3Chunk3Split2Object))]
    [Serialized]
    public class LeadOreRubbleSet3Chunk3Object : RubbleObject<LeadOreItem> { }
    [Serialized] public class LeadOreRubbleSet3Chunk3Split1Object : RubbleObject<LeadOreItem> { }
    [Serialized] public class LeadOreRubbleSet3Chunk3Split2Object : RubbleObject<LeadOreItem> { }

    [BecomesRubble(typeof(LeadOreRubbleSet4Chunk1Split1Object), typeof(LeadOreRubbleSet4Chunk1Split2Object))]
    [Serialized]
    public class LeadOreRubbleSet4Chunk1Object : RubbleObject<LeadOreItem> { }
    [Serialized] public class LeadOreRubbleSet4Chunk1Split1Object : RubbleObject<LeadOreItem> { }
    [Serialized] public class LeadOreRubbleSet4Chunk1Split2Object : RubbleObject<LeadOreItem> { }
    [Serialized] public class LeadOreRubbleSet4Chunk2Object : RubbleObject<LeadOreItem> { }
    [Serialized] public class LeadOreRubbleSet4Chunk3Object : RubbleObject<LeadOreItem> { }
}
