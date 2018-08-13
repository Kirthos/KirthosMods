using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kirthos.Mods.BigShovel
{
    public class ToolStorage
    {
        public float durability;
        public float calories;
        public CraftIngredients repair;
        public CraftingStorage craft;

        public ToolStorage(float _durability, float _calories, CraftIngredients _repair, CraftingStorage _craft)
        {
            durability = _durability;
            calories = _calories;
            repair = _repair;
            craft = _craft;
        }
    }

    public class CraftingStorage
    {
        public bool enable;
        public string table;
        public float craftMinutesTime;
        public CraftIngredients[] ingredients;

        public CraftingStorage(bool _enable, string _table, float _craftMinutesTime, CraftIngredients[] _craftCost)
        {
            enable = _enable;
            table = _table;
            craftMinutesTime = _craftMinutesTime;
            ingredients = _craftCost;
        }
    }

    public class CraftIngredients
    {
        public string item;
        public float quantity;

        public CraftIngredients(string ingredient, float cost)
        {
            item = ingredient;
            quantity = cost;
        }

        public override string ToString()
        {
            return item + " : " + quantity;
        }
    }

    public class SkillStorage
    {
        public string skill;
        public int level;

        public SkillStorage(string _skill, int _level)
        {
            skill = _skill;
            level = _level;
        }
    }
}
