using Eco.Gameplay.Components;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 09/29/2018
 * Updated by Kirthos 10/19/2018
 */

namespace Kirthos.Mods.Utils
{
    public class RecipeRemover
    {
        public static void RemoveRecipe(Type targetRecipeType)
        {
            Console.Write("Removing" + string.Concat(targetRecipeType.ToString().Split('.').Last().Select(x => Char.IsUpper(x) ? " " + x : x.ToString())));

            // Get all the existing recipe
            Dictionary<Type, Recipe[]> staticRecipes = (Dictionary<Type, Recipe[]>)typeof(CraftingComponent).GetFields(BindingFlags.Static | BindingFlags.NonPublic).First(x => x.Name.Contains("staticRecipes")).GetValue(Activator.CreateInstance(typeof(CraftingComponent)));

            // Get all the recipe to table dicationary
            Dictionary<Type, List<Type>> recipeToTable = (Dictionary<Type, List<Type>>)typeof(CraftingComponent).GetFields(BindingFlags.Static | BindingFlags.NonPublic).First(x => x.Name.Contains("recipeToTable")).GetValue(Activator.CreateInstance(typeof(CraftingComponent)));

            // Get all item to recipe dictionary
            Dictionary<Type, List<Recipe>> itemToRecipe = (Dictionary<Type, List<Recipe>>)typeof(CraftingComponent).GetFields(BindingFlags.Static | BindingFlags.NonPublic).First(x => x.Name.Contains("itemToRecipe")).GetValue(Activator.CreateInstance(typeof(CraftingComponent)));

            lock (staticRecipes)
            {
                Type targetTable = CraftingComponent.TablesForRecipe(targetRecipeType).First();

                Console.WriteLine(" from" + string.Concat(targetTable.ToString().Split('.').Last().Select(x => Char.IsUpper(x) ? " " + x : x.ToString())));
                Recipe targetRecipe = null;
                Recipe[] recipes;

                // Get all the recipe inside the table
                if (staticRecipes.TryGetValue(targetTable, out recipes))
                {
                    // Get the recipe
                    targetRecipe = recipes.First(x => x.GetType() == targetRecipeType);

                    // Remove the target recipe from the recipe list
                    recipes = recipes.Where(x => x.GetType() != targetRecipeType).ToArray();
                }
                // Set back the recipe inside the static recipe dictionnary
                staticRecipes[targetTable] = recipes;

                // Remove the recipe from the recipe to table dictionary
                recipeToTable[targetRecipeType].Remove(targetTable);

                // Remove the recipe from the item to recipe dictionary
                targetRecipe?.Products.ForEach(product => itemToRecipe[product.Item.Type].Remove(targetRecipe));

                // Remove the table from the list of table
                // Only if there is no more recipe inside the table
                if (recipes.Length == 0)
                    CraftingComponent.AllTableWorldObjects.Remove(targetTable);

                // After removing the recipe we need to update the skill unlock

                // Get all the skillunlock tooltips
                Dictionary<Type, Dictionary<int, List<LocString>>> skillUnlocksTooltips = (Dictionary<Type, Dictionary<int, List<LocString>>>)typeof(Skill).GetFields(BindingFlags.Static | BindingFlags.NonPublic).First(x => x.Name.Contains("skillUnlocksTooltips")).GetValue(null);

                // Get all the skillbenefit tooltips
                Dictionary<Type, Dictionary<LocString, List<SkillModifiedValue>>> skillBenefitsTooltips = (Dictionary<Type, Dictionary<LocString, List<SkillModifiedValue>>>)typeof(SkillModifiedValueManager).GetFields(BindingFlags.Static | BindingFlags.NonPublic).First(x => x.Name.Contains("skillBenefits")).GetValue(null);

                // Get the skill that unlock the recipe
                Type skillType = RequiresSkillAttribute.Cache.Get(targetRecipeType).FirstOrDefault()?.SkillItem.Type;

                // Get the level that unlock the recipe
                int? recipeUnlockLevel = RequiresSkillAttribute.Cache.Get(targetRecipeType).FirstOrDefault()?.Level;

                // Get the loc string for the recipe
                LocString locStr = new LocString(Text.Indent(targetRecipe.UILink()));

                // If there is a require skill
                if (skillType != null)
                {
                    // Get the list of unlock for the skill for the unlock level
                    List<LocString> unlocks = skillUnlocksTooltips[skillType][recipeUnlockLevel.Value];

                    // Search the correct unlock
                    for (int i = 0; i < unlocks.Count; i++)
                    {
                        if (unlocks[i] == locStr)
                        {
                            // remove the loc string from the list
                            unlocks.RemoveAt(i);
                            break;
                        }
                    }
                    // Set the new unlock list
                    skillUnlocksTooltips[skillType][recipeUnlockLevel.Value] = unlocks;

                    // For testing
                    //List<Type> KeyA = new List<Type>();
                    //List<LocString> KeyB = new List<LocString>();

                    // Search all skill that benefit by the removed recipe
                    foreach (var benefit in skillBenefitsTooltips)
                    {
                        for (int i = 0; i < benefit.Value.Count; i++)
                        {
                            var b = benefit.Value.ElementAt(i);

                            // if the benefit is for a recipe
                            if (b.Key == targetRecipe.UILink())
                            {
                                //KeyA.Add(benefit.Key);
                                //KeyB.Add(b.Key);
                                b.Value.Clear();
                                // Update benefit list
                                benefit.Value[b.Key] = b.Value;
                                continue;
                            }

                            //if not we try to found benefit for an item
                            foreach (var item in targetRecipe.Products)
                            {
                                LocString itemLocString = item.Item.UILink();

                                // if this benefit is benefit for an item on the recipe product
                                if (b.Key == itemLocString)
                                {
                                    //KeyA.Add(benefit.Key);
                                    //KeyB.Add(b.Key);

                                    // clear all the benefit for this recipe
                                    b.Value.Clear();
                                    // Update benefit list
                                    benefit.Value[b.Key] = b.Value;
                                    break;
                                }
                            }
                        }
                    }
                    /*
                    for (int i = 0; i < KeyA.Count; i++)
                    {
                        Console.WriteLine(KeyA[i]);
                        Console.WriteLine(KeyA[i]);
                        skillBenefitsTooltips[KeyA[i]][KeyB[i]].ForEach(x => Console.WriteLine(x));
                    }
                    */
                }

            }
        }

    }
}