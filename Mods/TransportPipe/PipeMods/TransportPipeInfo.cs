using Eco.Shared.Math;
using Eco.Shared.Serialization;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/12/2018
 */

namespace Kirthos.Mods.TransportPipe
{
    [Serialized]
    public enum PIPETYPE
    {
        Electric,
        Wooden,
    }

    [Serialized]
    public class TransportPipeInfo
    {
        public TransportPipeInfo() { }
        public TransportPipeLinker beltLinker;
        [Serialized] public Vector3i pos;
        [Serialized] public PIPETYPE type;

        public override string ToString()
        {
            return "Linker: " + beltLinker + "  Pos: " + pos;
        }
    }
}
