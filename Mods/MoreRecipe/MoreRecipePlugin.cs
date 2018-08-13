using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/21/2018
 */

namespace Kirthos.Mods.MoreRecipe
{
    public class MoreRecipePlugin : IModKitPlugin, IInitializablePlugin
    {

        public MoreRecipePlugin() { }

        public override string ToString()
        {
            return "More Recipe";
        }

        public string GetStatus()
        {
            return "Version 1.0.0 !";
        }

        public void Initialize(TimedTask timer)
        {
            WorldMineralCounter.ScanWorld();
        }
    }

}
