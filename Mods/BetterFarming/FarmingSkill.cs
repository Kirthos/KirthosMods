using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/23/2018
 */

namespace Kirthos.Mods.BetterFarming
{
    [Serialized]
    [RequiresSkill(typeof(ScytheEfficiencySkill), 1)]
    public partial class ScythePickupRangeSkill : Skill
    {
        private bool enabled = BetterFarmingPlugin.Conf.Get<bool>("ScytheRadiusSkill.enabled");

        public override string FriendlyName { get { return "Scythe Pickup Range"; } }
        public override string Description { get { return enabled ? "Scythe can reaping around plant with right click. Pickup only plant at least 90% mature. Level increase radius." : "This skill has been disabled by the server."; } }

        public static int[] SkillPointCost = {
            BetterFarmingPlugin.Conf.GetInt("ScytheRadiusSkill.SkillPointCost.1"),
            BetterFarmingPlugin.Conf.GetInt("ScytheRadiusSkill.SkillPointCost.2"),
            BetterFarmingPlugin.Conf.GetInt("ScytheRadiusSkill.SkillPointCost.3"),
        };
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        public override int MaxLevel { get { return enabled ? 3 : 0; } }
    }

    [Serialized]
    [RequiresSkill(typeof(GatheringSkill), 1)]
    public partial class HoePickupRangeSkill : Skill
    {
        private bool enabled = BetterFarmingPlugin.Conf.Get<bool>("HoeRadiusSkill.enabled");

        public override string FriendlyName { get { return "Hoe Pickup Range"; } }
        public override string Description { get { return enabled ? "Hoe can pickup around plant with right click. Pickup only plant at least 90% mature. Level increase radius." : "This skill has been disabled by the server."; } }

        public static int[] SkillPointCost = {
            BetterFarmingPlugin.Conf.GetInt("HoeRadiusSkill.SkillPointCost.1"),
            BetterFarmingPlugin.Conf.GetInt("HoeRadiusSkill.SkillPointCost.2"),
            BetterFarmingPlugin.Conf.GetInt("HoeRadiusSkill.SkillPointCost.3"), };
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        public override int MaxLevel { get { return enabled ? 3 : 0; } }
    }

}
