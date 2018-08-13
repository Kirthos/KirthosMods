using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using System;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/29/2018
 */

namespace Kirthos.Mods.Utils
{
    public class SkillsUtil
    {

        /// <summary>
        /// Return true if the user has the level of the skillm return false if the user don't have the skill
        /// </summary>
        public static bool HasSkillLevel(User user, Type skillType, int level)
        {
            foreach(Skill skill in user.Skillset.Skills)
            {
                if (skill.Type == skillType)
                {
                    if (skill.Level >= level)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Return the level of the skillType for the user
        /// </summary>
        public static int GetSkillLevel(User user, Type skillType)
        {
            foreach (Skill skill in user.Skillset.Skills)
            {
                if (skill.Type == skillType)
                {
                    return skill.Level;
                }
            }
            return 0;
        }
    }
}
