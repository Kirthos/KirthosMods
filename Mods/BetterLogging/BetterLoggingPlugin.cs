using Asphalt;
using Asphalt.Api.Event;
using Asphalt.Service;
using Asphalt.Storeable;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/29/2018
 */

namespace Kirthos.Mods.BetterLogging
{
    [AsphaltPlugin("KirthosMods/BetterLogging")]
    public class BetterLoggingPlugin : IModKitPlugin, IInitializablePlugin
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


        public BetterLoggingPlugin() { }

        public override string ToString()
        {
            return "Better Logging";
        }

        public string GetStatus()
        {
            return "Version 1.5.0";
        }

        public static KeyDefaultValue[] GetConfig()
        {
            return new KeyDefaultValue[]
            {
                new KeyDefaultValue("WoodPulpCleanerSkill.enabled", true),
                new KeyDefaultValue("WoodPulpCleanerSkill.SkillPointCost.1", 5),
                new KeyDefaultValue("WoodPulpCleanerSkill.SkillPointCost.2", 7),
                new KeyDefaultValue("WoodPulpCleanerSkill.SkillPointCost.3", 10),
                new KeyDefaultValue("WoodPulpCleanerSkill.SkillPointCost.4", 15),
                new KeyDefaultValue("WoodPulpCleanerSkill.SkillPointCost.5", 20),
                new KeyDefaultValue("StumpCleanerSkill.enabled", true),
                new KeyDefaultValue("StumpCleanerSkill.SkillPointCost.1", 25),
                new KeyDefaultValue("ExpertLumbererSkill.enabled", true),
                new KeyDefaultValue("ExpertLumbererSkill.SkillPointCost.1", 50),

            };
        }

        public void Initialize(TimedTask timer)
        {
            BetterLoggingEventListener listener = new BetterLoggingEventListener();
            EventManager.RegisterListener(listener);
        }
    }

}
