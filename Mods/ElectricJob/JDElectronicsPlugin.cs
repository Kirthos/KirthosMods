using Asphalt;
using Asphalt.Service;
using Asphalt.Storeable;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Players;
using Eco.Mods.TechTree;

[AsphaltPlugin("JDElectricJob")]
public class JDElectricalPlugin : IModKitPlugin, IInitializablePlugin
{
    #region Config
    public static bool f = false;
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

    public static KeyDefaultValue[] GetConfig()
    {
        return new KeyDefaultValue[]
        {
            new KeyDefaultValue("CraftScrollInsteadBook", false),
            new KeyDefaultValue("CraftingTimeMultiplierForElectricMachine", 1.0f),
            new KeyDefaultValue("BrokenChanceOverTime", 0.000001f),
            new KeyDefaultValue("CarefulSkillEfficencyPercentPerLevel", 19f),

        };
    }
    #endregion

    public JDElectricalPlugin() { }
    public override string ToString()
    {
        return "JD Electrical";
    }

    public string GetStatus()
    {
        return "Running...";
    }

    public void Initialize(TimedTask timer)
    {
        UserManager.OnUserLoggedIn.Add(u =>
        {
            if (!u.Skillset.HasSkill(typeof(JDElectricalSkill)))
                u.Skillset.LearnSkill(typeof(JDElectricalSkill));
        });
    }
};