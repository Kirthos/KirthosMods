using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/12/2018
 */

namespace Kirthos.Mods.TransportPipe.Skills
{
    [Serialized]
    [RequiresSkill(typeof(OilDrillingSkill), 1)]    
    public class AdvancedPetrolRefiningSkill : Skill
    {
        public override string FriendlyName { get { return "Advanced Petrol Refining"; } }
        public override string Description { get { return ""; } }

        public static ModificationStrategy MultiplicativeStrategy = 
            new MultiplicativeStrategy(TransportPipePlugin.Conf.Get<SkillStorage>("Advanced Petrol Refining").multiplicativeStrategy());
        public static ModificationStrategy AdditiveStrategy =
            new AdditiveStrategy(TransportPipePlugin.Conf.Get<SkillStorage>("Advanced Petrol Refining").additiveStrategy());
        public static int[] SkillPointCost = TransportPipePlugin.Conf.Get<SkillStorage>("Advanced Petrol Refining").skillPointCost();
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        private int _MaxLevel = TransportPipePlugin.Conf.Get<SkillStorage>("Advanced Petrol Refining").Levels.Length;
        public override int MaxLevel { get { return _MaxLevel; } }
    }

}
