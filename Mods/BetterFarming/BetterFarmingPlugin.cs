using Asphalt;
using Asphalt.Api.Event;
using Asphalt.Service;
using Asphalt.Storeable;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/23/2018
 */

namespace Kirthos.Mods.BetterFarming
{
    [AsphaltPlugin("KirthosMods/BetterFarming")]
    public class BetterFarmingPlugin : IModKitPlugin, IInitializablePlugin
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

        public BetterFarmingPlugin() {}
        public override string ToString()
        {
            return "Better Farming";
        }

        public string GetStatus()
        {
            return "Version 1.0.3";
        }

        public void Initialize(TimedTask timer)
        {
            BetterFarmingEventListener listener = new BetterFarmingEventListener();
            EventManager.RegisterListener(listener);
        }

        public static KeyDefaultValue[] GetConfig()
        {
            return new KeyDefaultValue[]
            {
                new KeyDefaultValue("ScytheRadiusSkill.enabled", true),
                new KeyDefaultValue("ScytheRadiusSkill.SkillPointCost.1", 5),
                new KeyDefaultValue("ScytheRadiusSkill.SkillPointCost.2", 10),
                new KeyDefaultValue("ScytheRadiusSkill.SkillPointCost.3", 25),
                new KeyDefaultValue("HoeRadiusSkill.enabled", true),
                new KeyDefaultValue("HoeRadiusSkill.SkillPointCost.1", 5),
                new KeyDefaultValue("HoeRadiusSkill.SkillPointCost.2", 10),
                new KeyDefaultValue("HoeRadiusSkill.SkillPointCost.3", 25),
                
            };
        }
    }

}
