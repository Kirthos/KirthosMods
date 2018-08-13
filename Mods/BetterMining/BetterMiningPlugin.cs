using Asphalt;
using Asphalt.Api.Event;
using Asphalt.Service;
using Asphalt.Storeable;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/01/2018
 */

namespace Kirthos.Mods.BetterMining
{
    [AsphaltPlugin("KirthosMods/BetterMining")]
    public class BetterMiningPlugin : IModKitPlugin, IInitializablePlugin
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

        public BetterMiningPlugin() {}
        public override string ToString()
        {
            return "Better Mining";
        }

        public string GetStatus()
        {
            return "Version 1.6.0";
        }

        public void Initialize(TimedTask timer)
        {
            BetterMiningEventListener listener = new BetterMiningEventListener();
            EventManager.RegisterListener(listener);
        }

        public static KeyDefaultValue[] GetConfig()
        {
            return new KeyDefaultValue[]
            {
                new KeyDefaultValue("PickupRubbleBySkillLevel", 4),
                new KeyDefaultValue("MiningPickupAmountSkill.enabled", true),
                new KeyDefaultValue("MiningPickupAmountSkill.SkillPointCost.1", 5),
                new KeyDefaultValue("MiningPickupAmountSkill.SkillPointCost.2", 7),
                new KeyDefaultValue("MiningPickupAmountSkill.SkillPointCost.3", 10),
                new KeyDefaultValue("MiningPickupAmountSkill.SkillPointCost.4", 15),
                new KeyDefaultValue("MiningPickupAmountSkill.SkillPointCost.5", 20),
                new KeyDefaultValue("MiningPickupRangeSkill.enabled", true),
                new KeyDefaultValue("MiningPickupRangeSkill.SkillPointCost.1", 5),
                new KeyDefaultValue("MiningPickupRangeSkill.SkillPointCost.2", 7),
                new KeyDefaultValue("MiningPickupRangeSkill.SkillPointCost.3", 10),
                new KeyDefaultValue("MiningPickupRangeSkill.SkillPointCost.4", 15),
                new KeyDefaultValue("MiningPickupRangeSkill.SkillPointCost.5", 20),
                new KeyDefaultValue("StrongMiningSkill.enabled", true),
                new KeyDefaultValue("StrongMiningSkill.SkillPointCost.1", 5),
                new KeyDefaultValue("StrongMiningSkill.SkillPointCost.2", 10),
                new KeyDefaultValue("StrongMiningSkill.SkillPointCost.3", 15),
                new KeyDefaultValue("StrongMiningSkill.SkillPointCost.4", 30),
                new KeyDefaultValue("StrongMiningSkill.SkillPointCost.5", 50),
                
            };
        }
    }

}
