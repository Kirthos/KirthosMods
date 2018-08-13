using Eco.Shared.Serialization;
using Eco.World;
using Eco.World.Blocks;

namespace Kirthos.Mods.TransportPipe.Eco
{
    public class TransportPipeAttr : BlockAttribute
    {
        public TransportPipeAttr()
        { }
    }

    [Serialized]
    [Solid, Constructed, TransportPipeAttr, Wall]
    public class BaseTransportPipeBlock : Block { }
}
