using Asphalt;
using Asphalt.Api.Event;
using Asphalt.Service;
using Asphalt.Storeable;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 06/13/2018
 */


namespace Kirthos.Mods.ActiveCraft
{
    [AsphaltPlugin("KirthosMods/ActiveCraft")]
    public class ActiveCraftPlugin : IModKitPlugin, IInitializablePlugin
    {
        public ActiveCraftPlugin() { }
        public override string ToString()
        {
            return "Active Craft";
        }

        public string GetStatus()
        {
            return "Version 1.0.0";
        }

        public void Initialize(TimedTask timer)
        {
            ActiveCraftEventListener listener = new ActiveCraftEventListener();
            EventManager.RegisterListener(listener);
        }
    }
}
