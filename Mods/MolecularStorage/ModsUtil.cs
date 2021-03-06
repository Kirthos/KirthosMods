﻿using Eco.Core.Utils;
using Eco.Gameplay.Skills;
using System;
using System.Linq;
using System.Reflection;

namespace Kirthos.Mods.Utils
{
    public class ModRequiresSkillAttribute : RequiresSkillAttribute
    {
        public ModRequiresSkillAttribute(string modSkillName, Type vanillaSkillType, int requiredSkillLevel) : base(GetSkillType(modSkillName, vanillaSkillType), requiredSkillLevel)
        {}

        public static Type GetSkillType(string modSkillName, Type vanillaSkill)
        {
            Type modType = ModsUtil.GetTypeFromMod(modSkillName);
            if (modType != null)
                return modType;
            return vanillaSkill;
        }
    }

    public class ModsUtil
    {
        public static ThreadSafeList<Assembly> installedMod = new ThreadSafeList<Assembly>();

        public static void ReadInstalledMod()
        {
            Console.WriteLine("Get installed mod:");
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.FullName == "Anonymously Hosted DynamicMethods Assembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
                    continue;
                if (asm.FullName == "HarmonySharedState, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
                    continue;
                if (asm.Location.IsEmpty() && asm.FullName.Contains("Eco") == false && asm.FullName.Contains("System") == false && asm.FullName.Contains("Lidgren") == false
                     && asm.FullName.Contains("SharpNoise") == false && asm.FullName.Split(',').First() != "C5" && asm.FullName.Contains("NodeGraphControl") == false
                      && asm.FullName.Contains("Newtonsoft") == false && asm.FullName.Contains("Priority Queue") == false && asm.FullName.Contains("NLog") == false
                       && asm.FullName.Contains("Humanizer") == false)
                {
                    installedMod.Add(asm);
                    Console.WriteLine("     - " + asm.FullName.Split(',').First() + " " + asm.FullName.Split(',')[1].Split('=').Last());
                }

            }
        }

        public static bool IsModInstalled(string assemblyModName)
        {
            if (installedMod.Count == 0)
                ReadInstalledMod();
            return installedMod.Where(x => x.FullName.Split(',').First() == assemblyModName).Count() > 0;
        }

        public static Type GetTypeFromMod(string typeName)
        {
            if (installedMod.Count == 0)
                ReadInstalledMod();
            foreach (Assembly asm in installedMod)
            {
                foreach (Type t in asm.GetExportedTypes())
                {
                    if (t.ToString().Contains(typeName))
                    {
                        return t;
                    }
                }
            }
            return null;
        }
    }
}
