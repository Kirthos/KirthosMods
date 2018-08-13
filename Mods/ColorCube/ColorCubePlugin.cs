using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/02/2018
 */

namespace Kirthos.Mods.ColorCube
{
    public class ColorCubePlugin : IModKitPlugin, IInitializablePlugin
    {
        public static string init = "";

        public ColorCubePlugin() { }
        public override string ToString()
        {
            return "Color Cube";
        }

        public string GetStatus()
        {
            return "Version 1.0.1";
        }

        public void Initialize(TimedTask timer)
        {
            //ColorCubeEventListener listener = new ColorCubeEventListener();
            //EventManager.RegisterListener(listener);
        }
    }
}
