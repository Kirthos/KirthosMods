using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Skills;
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
    [RequiresSkill(typeof(PipeEngineerSkill), 1)]
    public class PipeCraftingSkill : Skill
    {
        public override string FriendlyName { get { return "Pipes Crafting"; } }
        public override string Description { get { return ""; } }

        public static ModificationStrategy MultiplicativeStrategy =
            new MultiplicativeStrategy(TransportPipePlugin.Conf.Get<SkillStorage>("Pipes Crafting").multiplicativeStrategy());
        public static ModificationStrategy AdditiveStrategy =
            new AdditiveStrategy(TransportPipePlugin.Conf.Get<SkillStorage>("Pipes Crafting").additiveStrategy());
        public static int[] SkillPointCost = TransportPipePlugin.Conf.Get<SkillStorage>("Pipes Crafting").skillPointCost();
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        private int _MaxLevel = TransportPipePlugin.Conf.Get<SkillStorage>("Pipes Crafting").Levels.Length;
        public override int MaxLevel { get { return _MaxLevel; } }
    }
}
