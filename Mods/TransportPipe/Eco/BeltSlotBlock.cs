using Eco.Gameplay.Objects;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.World.Blocks;
using System.Collections.Generic;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/12/2018
 */

namespace Kirthos.Mods.TransportPipe.Eco
{
    public class PipeEntry
    {
        private Vector3i pos;
        public PipeEntry(Vector3i offset, WorldObject obj)
        {
            TransportPipeInfo info = new TransportPipeInfo();
            info.beltLinker = null;
            info.type = PIPETYPE.Electric;
            pos = Utils.MakeWorldMod(obj.Position3i + offset);
            List<TransportPipeInfo> neighbourgs = Utils.GetNeighbourgPipesInfo(pos);
            if (neighbourgs.Count > 0)
            {
                foreach (TransportPipeInfo neighbourg in neighbourgs)
                {
                    if (info.beltLinker == null)
                        info.beltLinker = neighbourg.beltLinker;
                    else if (neighbourg.beltLinker != null && info.beltLinker != neighbourg.beltLinker)
                        info.beltLinker.AbsorbLinker(neighbourg.beltLinker);
                }
                if (info.beltLinker == null)
                {
                    info.beltLinker = new TransportPipeLinker();
                }
                info.beltLinker.AddConveyor(info);
            }
            else
            {
                info.beltLinker = new TransportPipeLinker();
                info.beltLinker.AddConveyor(info);
            }
            info.pos = pos;
            TransportPipeManager.pipesInfo[pos] = info;
        }

        public void Destroy()
        {
            TransportPipeInfo info = null;
            if (TransportPipeManager.pipesInfo.TryGetValue(pos, out info))
            {
                info.beltLinker.RemoveConveyor(info);
                TransportPipeManager.pipesInfo.TryRemove(pos, out info);
                info.beltLinker.RebuildLinker();
            }
        }

        public TransportPipeLinker GetLinker()
        {
            TransportPipeInfo info = null;
            if (TransportPipeManager.pipesInfo.TryGetValue(pos, out info))
            {
                return info.beltLinker;
            }
            return null;
        }
    }

    [Serialized]
    [Solid]
    [Transient]
    public class BeltSlotBlock : WorldObjectBlock
    {
        public BeltSlotBlock(WorldObject obj) : base(obj)
        {}
        protected BeltSlotBlock()
        { }
    }
}
