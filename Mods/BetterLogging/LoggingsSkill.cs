using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;
using Kirthos.Mods.BetterLogging;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/29/2018
 */

namespace Kirthos.Mods
{
    [Serialized]
    [RequiresSkill(typeof(LoggingSkill), 1)]
    public partial class WoodPulpCleanerSkill : Skill
    {
        private bool enabled = BetterLoggingPlugin.Conf.Get<bool>("WoodPulpCleanerSkill.enabled");

        public override string FriendlyName { get { return "Wood pulp cleaner"; } }
        public override string Description { get { return enabled ? "When break a wood pulp, break others wood pulp around. Levels increase range." : "This skill has been disabled by the server."; } }

        public static int[] SkillPointCost = {
            BetterLoggingPlugin.Conf.GetInt("WoodPulpCleanerSkill.SkillPointCost.1"),
            BetterLoggingPlugin.Conf.GetInt("WoodPulpCleanerSkill.SkillPointCost.2"),
            BetterLoggingPlugin.Conf.GetInt("WoodPulpCleanerSkill.SkillPointCost.3"),
            BetterLoggingPlugin.Conf.GetInt("WoodPulpCleanerSkill.SkillPointCost.4"),
            BetterLoggingPlugin.Conf.GetInt("WoodPulpCleanerSkill.SkillPointCost.5")
        };
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        public override int MaxLevel { get { return enabled ? 5 : 0; } }
    }
    //*
    [Serialized]
    [RequiresSkill(typeof(WoodPulpCleanerSkill), 1)]
    public partial class StumpCleanerSkill : Skill
    {
        private bool enabled = BetterLoggingPlugin.Conf.Get<bool>("StumpCleanerSkill.enabled");

        public override string FriendlyName { get { return "Stump cleaner"; } }
        public override string Description { get { return enabled ? "Break tree stump in one hit." : "This skill has been disabled by the server."; } }

        public static int[] SkillPointCost = { BetterLoggingPlugin.Conf.GetInt("StumpCleanerSkill.SkillPointCost.1") };
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        public override int MaxLevel { get { return enabled ? 1 : 0; } }
    }
    //*/
    [Serialized]
    [RequiresSkill(typeof(LoggingSkill), 1)]
    public partial class ExpertLumbererSkill : Skill
    {
        private bool enabled = BetterLoggingPlugin.Conf.Get<bool>("ExpertLumbererSkill.enabled");

        public override string FriendlyName { get { return "Expert lumberer"; } }
        public override string Description { get { return enabled ? "Cut the tree trunk, ready to harvest." : "This skill has been disabled by the server."; } }

        public static int[] SkillPointCost = { BetterLoggingPlugin.Conf.GetInt("ExpertLumbererSkill.SkillPointCost.1") };
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        public override int MaxLevel { get { return enabled ? 1 : 0; } }
    }

}
