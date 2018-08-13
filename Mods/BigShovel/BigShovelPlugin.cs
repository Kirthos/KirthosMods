using Asphalt;
using Asphalt.Service;
using Asphalt.Storeable;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using System;
using System.Reflection;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 06/12/2018
 */

namespace Kirthos.Mods.BigShovel
{
    [AsphaltPlugin("KirthosMods/BigShovel")]
    public class BigShovelPlugin : IModKitPlugin, IInitializablePlugin
    {

        private static bool f = false;

        [Inject]
        [StorageLocation("Config")]
        [DefaultValues(nameof(GetConfig))]
        public static IStorage ConfigStorage;

        public static IStorage Conf
        {
            get
            {
                if (f)
                    return ConfigStorage;
                else
                {
                    f = true;
                    ServiceHelper.InjectValues();
                    return ConfigStorage;
                }
            }
        }

        public BigShovelPlugin() { }
        public override string ToString()
        {
            return "Big Shovel";
        }

        public string GetStatus()
        {
            return "Version 1.6.1";
        }

        public static KeyDefaultValue[] GetConfig()
        {
            return new KeyDefaultValue[]
            {
                new KeyDefaultValue("IronBigShovel", new ToolStorage(200f, 20f, new CraftIngredients("IronIngotItem", 20), new CraftingStorage(
                    true,
                    "AnvilObject",
                    10f,
                    new CraftIngredients[] {
                        new CraftIngredients("BoardItem", 50),
                        new CraftIngredients("IronIngotItem", 100),
                    }
                ))),
                new KeyDefaultValue("SteelBigShovel", new ToolStorage(500f, 15f, new CraftIngredients("SteelItem", 20), new CraftingStorage(
                    true,
                    "AnvilObject",
                    20f,
                    new CraftIngredients[] {
                        new CraftIngredients("BoardItem", 50),
                        new CraftIngredients("SteelItem", 100),
                    }
                ))),
                new KeyDefaultValue("ModernBigShovel", new ToolStorage(1000f, 10f, new CraftIngredients("FiberglassItem", 20), new CraftingStorage(
                    true,
                    "AssemblyLineObject",
                    30f,
                    new CraftIngredients[] {
                        new CraftIngredients("FiberglassItem", 50),
                        new CraftIngredients("SteelItem", 100),
                    }
                ))),
            };
        }

        public static Type GetTypeFromString(string typename)
        {
      //      Console.WriteLine("Assembly loaded: ");
            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (ass.FullName.Contains("Version=0.0.0.0") && ass.FullName.Contains("Eco.Mods") == false)
                {
                   // Console.WriteLine("NOT LOADED: " + ass.FullName);
                    continue;
                }
              //  Console.WriteLine(ass.FullName);
                foreach (Type t in ass.GetExportedTypes())
                {
                    if (t.ToString().Contains(typename))
                    {
                    //    Console.WriteLine("Return " + t);
                        return t;
                    }
                }
            }
    //        Console.WriteLine("Return null");
            return null;
        }

        public void Initialize(TimedTask timer)
        {
            
        }
    }
}
