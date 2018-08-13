using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/01/2018
 */

namespace Kirthos.Mods.BetterMining
{
    [Serialized]
    [RequiresSkill(typeof(MiningSkill), 1)]
    public partial class MiningPickupAmountSkill : Skill
    {
        private bool enabled = BetterMiningPlugin.Conf.Get<bool>("MiningPickupAmountSkill.enabled");

        public override string FriendlyName { get { return "Mining Pickup amount"; } }
        public override string Description { get { return enabled ? "Pickup near rubble when right-click rubble with pickaxe. Levels increase max amount." : "This skill has been disabled by the server."; } }

        public static int[] SkillPointCost = {//0,0,0,0,0
            //*
            BetterMiningPlugin.Conf.GetInt("MiningPickupAmountSkill.SkillPointCost.1"),
            BetterMiningPlugin.Conf.GetInt("MiningPickupAmountSkill.SkillPointCost.2"),
            BetterMiningPlugin.Conf.GetInt("MiningPickupAmountSkill.SkillPointCost.3"),
            BetterMiningPlugin.Conf.GetInt("MiningPickupAmountSkill.SkillPointCost.4"),
            BetterMiningPlugin.Conf.GetInt("MiningPickupAmountSkill.SkillPointCost.5")
            //*/
        };
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        public override int MaxLevel { get { return enabled ? 5 : 0; } }
    }

    [Serialized]
    [RequiresSkill(typeof(MiningPickupAmountSkill), 1)]
    public partial class MiningPickupRangeSkill : Skill
    {
        private bool enabled = BetterMiningPlugin.Conf.Get<bool>("MiningPickupAmountSkill.enabled");

        public override string FriendlyName { get { return "Mining Pickup range"; } }
        public override string Description { get { return enabled ? "Increase the range of the mining pickup." : "This skill has been disabled by the server."; } }

        public static int[] SkillPointCost = {//0,0,0,0,0
            //*
            BetterMiningPlugin.ConfigStorage.GetInt("MiningPickupRangeSkill.SkillPointCost.1"),
            BetterMiningPlugin.ConfigStorage.GetInt("MiningPickupRangeSkill.SkillPointCost.2"),
            BetterMiningPlugin.ConfigStorage.GetInt("MiningPickupRangeSkill.SkillPointCost.3"),
            BetterMiningPlugin.ConfigStorage.GetInt("MiningPickupRangeSkill.SkillPointCost.4"),
            BetterMiningPlugin.ConfigStorage.GetInt("MiningPickupRangeSkill.SkillPointCost.5")
            //*/
        };
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        public override int MaxLevel { get { return enabled ? 5 : 0; } }
    }

    [Serialized]
    [RequiresSkill(typeof(MiningSkill), 1)]
    public partial class StrongMiningSkill : Skill
    {
        private bool enabled = BetterMiningPlugin.Conf.Get<bool>("StrongMiningSkill.enabled");

        public override string FriendlyName { get { return "Strong Mining"; } }
        public override string Description { get { return enabled ? "Have chance to break directly big rubble when mine blocks. (20% chance per level)" : "This skill has been disabled by the server."; } }

        public static int[] SkillPointCost = {//0,0,0,0,0
            //*
            BetterMiningPlugin.ConfigStorage.GetInt("StrongMiningSkill.SkillPointCost.1"),
            BetterMiningPlugin.ConfigStorage.GetInt("StrongMiningSkill.SkillPointCost.2"),
            BetterMiningPlugin.ConfigStorage.GetInt("StrongMiningSkill.SkillPointCost.3"),
            BetterMiningPlugin.ConfigStorage.GetInt("StrongMiningSkill.SkillPointCost.4"),
            BetterMiningPlugin.ConfigStorage.GetInt("StrongMiningSkill.SkillPointCost.5")
            //*/
        };
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        public override int MaxLevel { get { return enabled ? 5 : 0; } }
    }
}
