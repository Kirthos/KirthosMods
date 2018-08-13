using Eco.Shared.Math;
using System.Collections.Concurrent;
using System.Collections.Generic;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/12/2018
 */

namespace Kirthos.Mods.TransportPipe
{
    public class TransportPipeManager
    {
        public static ConcurrentDictionary<Vector3i, TransportPipeInfo> pipesInfo = new ConcurrentDictionary<Vector3i, TransportPipeInfo>();
        public static int idLinker = 0;

        public static TransportPipeManager Obj;

        public static int GetNumberOfLinker()
        {
            List<TransportPipeLinker> linkerList = new List<TransportPipeLinker>();
            foreach(TransportPipeInfo info in pipesInfo.Values)
            {
                if (linkerList.Contains(info.beltLinker) == false)
                {
                    linkerList.Add(info.beltLinker);
                }
            }
            return linkerList.Count;
        }
    }
}
