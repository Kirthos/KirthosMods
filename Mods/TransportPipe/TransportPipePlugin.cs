using Asphalt;
using Asphalt.Api.Event;
using Asphalt.Service;
using Asphalt.Storeable;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Players;
using Kirthos.Mods.TransportPipe.Skills;
using System;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/12/2018
 */

namespace Kirthos.Mods.TransportPipe
{
    [AsphaltPlugin("KirthosMods/TransportPipe")]
    public class TransportPipePlugin : IModKitPlugin, IInitializablePlugin
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
                    try
                    {
                        ServiceHelper.InjectValues();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    return ConfigStorage;
                }
            }
        }

        public static KeyDefaultValue[] GetConfig()
        {
            return new KeyDefaultValue[]
            {
                new KeyDefaultValue("ElectricPipePowerNeed", 25.0f),
                new KeyDefaultValue("WoodenPipePowerNeed", 10.0f),
                new KeyDefaultValue("ElectricPipeTimeMove", 1.0f),
                new KeyDefaultValue("WoodenPipeTimeMove", 5.0f),
                new KeyDefaultValue("CraftScrollInsteadOfBook", false),
                new KeyDefaultValue("Advanced Petrol Refining", new SkillStorage(
                    new SkillBonusStorage[] {
                        new SkillBonusStorage(0.8f, 0.2f, 20),
                    })
                ),
                new KeyDefaultValue("Advanced Petrol Refining Efficiency", new SkillStorage(
                    new SkillBonusStorage[] {
                        new SkillBonusStorage(0.8f, 0.2f, 10),
                        new SkillBonusStorage(0.65f, 0.35f, 20),
                        new SkillBonusStorage(0.5f, 0.5f, 30),
                        new SkillBonusStorage(0.35f, 0.65f, 40),
                        new SkillBonusStorage(0.2f, 0.8f, 50),
                    })
                ),
                new KeyDefaultValue("Advanced Petrol Refining Speed", new SkillStorage(
                    new SkillBonusStorage[] {
                        new SkillBonusStorage(0.8f, 0.2f, 10),
                        new SkillBonusStorage(0.65f, 0.35f, 10),
                        new SkillBonusStorage(0.5f, 0.5f, 10),
                        new SkillBonusStorage(0.35f, 0.65f, 10),
                        new SkillBonusStorage(0.2f, 0.8f, 10),
                    })
                ),
                new KeyDefaultValue("Pipes Crafting", new SkillStorage(
                    new SkillBonusStorage[] {
                        new SkillBonusStorage(0.8f, 0.2f, 25),
                        new SkillBonusStorage(0.65f, 0.35f, 35),
                        new SkillBonusStorage(0.5f, 0.5f, 50),
                    })
                ),
                new KeyDefaultValue("Pipes Crafting Efficiency", new SkillStorage(
                    new SkillBonusStorage[] {
                        new SkillBonusStorage(0.8f, 0.2f, 15),
                        new SkillBonusStorage(0.65f, 0.35f, 25),
                        new SkillBonusStorage(0.5f, 0.5f, 35),
                        new SkillBonusStorage(0.35f, 0.65f, 45),
                        new SkillBonusStorage(0.2f, 0.8f, 50),
                    })
                ),
                new KeyDefaultValue("Pipes Crafting Speed", new SkillStorage(
                    new SkillBonusStorage[] {
                        new SkillBonusStorage(0.8f, 0.2f, 15),
                        new SkillBonusStorage(0.65f, 0.35f, 15),
                        new SkillBonusStorage(0.5f, 0.5f, 15),
                        new SkillBonusStorage(0.35f, 0.65f, 15),
                        new SkillBonusStorage(0.2f, 0.8f, 15),
                    })
                ),
                new KeyDefaultValue("Machinery Crafting", new SkillStorage(
                    new SkillBonusStorage[] {
                        new SkillBonusStorage(0.8f, 0.2f, 25),
                        new SkillBonusStorage(0.65f, 0.35f, 35),
                        new SkillBonusStorage(0.5f, 0.5f, 50),
                    })
                ),
                new KeyDefaultValue("Machinery Crafting Efficiency", new SkillStorage(
                    new SkillBonusStorage[] {
                        new SkillBonusStorage(0.8f, 0.2f, 15),
                        new SkillBonusStorage(0.65f, 0.35f, 25),
                        new SkillBonusStorage(0.5f, 0.5f, 35),
                        new SkillBonusStorage(0.35f, 0.65f, 45),
                        new SkillBonusStorage(0.2f, 0.8f, 50),
                    })
                ),
                new KeyDefaultValue("Machinery Crafting Speed", new SkillStorage(
                    new SkillBonusStorage[] {
                        new SkillBonusStorage(0.8f, 0.2f, 15),
                        new SkillBonusStorage(0.65f, 0.35f, 15),
                        new SkillBonusStorage(0.5f, 0.5f, 15),
                        new SkillBonusStorage(0.35f, 0.65f, 15),
                        new SkillBonusStorage(0.2f, 0.8f, 15),
                    })
                ),
            };
        }

        public TransportPipePlugin() { }

        static TransportPipePlugin()
        {
            PreInit();
        }

        public override string ToString()
        {
            return "Transport pipe";
        }

        public string GetStatus()
        {
            return "Version 1.3.0 - Found " + TransportPipeManager.pipesInfo.Count + " pipes in " + TransportPipeManager.GetNumberOfLinker() + " systems !";
        }

        public static void PreInit()
        {
        }

        public void Initialize(TimedTask timer)
        {
            ModsUtil.ReadInstalledMod();
            TransportPipeEventListener listener = new TransportPipeEventListener();
            EventManager.RegisterListener(listener);
            UserManager.OnUserLoggedIn.Add(u =>
            {
                if (!u.Skillset.HasSkill(typeof(AutomationSkill)))
                    u.Skillset.LearnSkill(typeof(AutomationSkill));
            });
        }
    }

}
