using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Chat;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.World;
using System;
using System.Threading;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/01/2018
 */

namespace Kirthos.Mods.Utils
{
    public class TransportPipeCommand : IChatCommandHandler
    {
        [ChatCommand("Count the number of block by name", ChatAuthorizationLevel.Developer)]
        public static void countBlock(User user, string typeName)
        {
            Type t = BlockManager.FromTypeName(typeName);
            new Thread(() =>
            {
                int count = 0;
                for (int i = 0; i < WorldArea.WholeWorld.Size.X; i++)
                {
                    for (int j = 0; j < WorldArea.WholeWorld.Size.Y; j++)
                    {
                        for (int k = 0; k < 100; k++)
                        {
                            if (World.GetBlock(new Vector3i(i, k, j)).GetType() == t)
                            {
                                count++;                
                            }
                        }
                    }
                    Console.WriteLine(((float)i/ WorldArea.WholeWorld.Size.X * 100f) + "% " + count);
                }
                Console.WriteLine("Count " + count + " " + typeName);
                user.Player.SendTemporaryMessage(new LocString("Count" + count + " " + typeName));
            }).Start();
        }
    }
}

