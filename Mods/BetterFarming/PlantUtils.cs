using Eco.Core.Utils;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Plants;
using Eco.Mods.TechTree;
using Eco.Shared.Math;
using Eco.Shared.Utils;
using Eco.Simulation;
using Eco.Simulation.Agents;
using Eco.World;
using System;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 05/23/2018
 */

namespace Kirthos.Mods
{
    class PlantUtils
    {
        public static void GetReapableBlockAroundPoint(InteractionContext context, Vector3i position, int range)
        {
            try
            {
                for (int i = -range; i < range; i++)
                {
                    for (int j = -range; j < range; j++)
                    {
                        Vector3i positionAbove = World.GetTopPos(new Vector2i(position.x + i, position.z + j)) + Vector3i.Up;
                        Block blockAbove = World.GetBlockProbablyTop(positionAbove);
                        if (blockAbove.Is<Reapable>())
                        {
                            Plant plant = EcoSim.PlantSim.GetPlant(positionAbove);
                            if (plant == null) continue;
                            if (plant.GrowthPercent >= 0.9f)
                            {
                                Result result = (plant as IHarvestable).TryHarvest(context.Player, true);
                                if (result.Success)
                                {
                                    context.Player.SpawnBlockEffect(positionAbove, blockAbove.GetType(), BlockEffect.Harvest);
                                    (context.SelectedItem as ScytheItem).UseDurability(1, context.Player);
                                    context.Player.User.Stomach.UseCalories((context.SelectedItem as ScytheItem).CaloriesBurn.GetCurrentValue(context.Player.User));
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception)
            {

            }
        }

        public static void GetPlantBlockAroundPoint(InteractionContext context, Vector3i position, int range)
        {
            try
            {
                for (int i = -range; i < range; i++)
                {
                    for (int j = -range; j < range; j++)
                    {
                        Vector3i positionAbove = World.GetTopPos(new Vector2i(position.x + i, position.z + j)) + Vector3i.Up;
                        Block blockAbove = World.GetBlockProbablyTop(positionAbove);
                        if (blockAbove is InteractablePlantBlock || blockAbove is CamasBlock)
                        {
                            Plant plant = EcoSim.PlantSim.GetPlant(positionAbove);
                            if (plant.GrowthPercent >= 0.9f)
                            {
                                Result result = (plant as IHarvestable).TryHarvest(context.Player, true);
                                if (result.Success)
                                {
                                    context.Player.SpawnBlockEffect(positionAbove, blockAbove.GetType(), BlockEffect.Harvest);
                                    (context.SelectedItem as HoeItem).UseDurability(1, context.Player);
                                    //context.Player.User.Stomach.UseCalories((context.SelectedItem as HoeItem).CaloriesBurn.GetCurrentValue(context.Player.User));
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception)
            {

            }
        }
    }
}
