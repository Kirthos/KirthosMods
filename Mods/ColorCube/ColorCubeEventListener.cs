using Asphalt.Api.Event;
using Asphalt.Events.WorldObjectEvent;
using Eco.Core.Utils;
using Eco.Core.Utils.Items;
using Eco.Gameplay.Plants;
using Eco.Shared.Math;
using Eco.Simulation;
using Eco.Simulation.Types;
using Eco.Simulation.WorldLayers;
using Eco.World;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 04/12/2018
 */

namespace Kirthos.Mods.ColorCube
{
    public class ColorCubeEventListener
    {
        public ColorCubeEventListener() { }

        [EventHandler]
        public void PluginConfigPostLoadEvent(PluginConfigPostLoadEvent evt)
        {
            if (evt.PluginConfig is EcoSim && (evt.PluginConfig as EcoSim).EcoDef.Species.Where(x => x.Name == "ColorPlant").Count() == 0)
            {
                PlantSpecies ColorPlant = new PlantSpecies();
                ColorPlant.CaloriesPerVoxelColumnPerDensity = 10f;
                ColorPlant.ResourceConstraintsByLayer = new Lazy<Dictionary<string, PlantSpecies.ResourceConstraint>>();
                ColorPlant.CapacityConstraintsByLayer = new Lazy<Dictionary<string, PlantSpecies.CapacityConstraint>>();
                ColorPlant.PlantType = new PlantType() { Type = typeof(PlantEntity) };
                ColorPlant.SeedDropChance = 0.5f;
                ColorPlant.SeedsAtGrowth = 0.6f;
                ColorPlant.SeedsBonusAtGrowth = 0.9f;
                ColorPlant.SeedRange = new Range(1, 3);
                ColorPlant.SeedItem = new ItemType(typeof(ColorPlantSeedItem));
                ColorPlant.BlockType = new BlockType(typeof(ColorPlantBlock));
                ColorPlant.MaxGrowthRate = 0.01f;
                ColorPlant.MaxDeathRate = 0.005f;
                ColorPlant.SpreadRate = 0.001f;
                ColorPlant.ResourceConstraints = new SerializedSynchronizedCollection<PlantSpecies.ResourceConstraint>()
                {
                    new PlantSpecies.ResourceConstraint()
                    {
                        LayerName = LayerNames.Nitrogen,
                        HalfSpeedConcentration = 0.4f,
                        MaxResourceContent = 0.4f
                    },
                    new PlantSpecies.ResourceConstraint()
                    {
                        LayerName = LayerNames.Phosphorus,
                        HalfSpeedConcentration = 0.4f,
                        MaxResourceContent = 0.4f
                    },
                    new PlantSpecies.ResourceConstraint()
                    {
                        LayerName = LayerNames.Potassium,
                        HalfSpeedConcentration = 0.1f,
                        MaxResourceContent = 0.1f
                    },
                    new PlantSpecies.ResourceConstraint()
                    {
                        LayerName = LayerNames.SoilMoisture,
                        HalfSpeedConcentration = 0.1f,
                        MaxResourceContent = 0.1f
                    }
                };
                ColorPlant.CapacityConstraints = new SerializedSynchronizedCollection<PlantSpecies.CapacityConstraint>()
                {
                    new PlantSpecies.CapacityConstraint()
                    {
                        CapacityLayerName = LayerNames.FertileGround,
                        ConsumedCapacityPerPop = 1f
                    },
                    new PlantSpecies.CapacityConstraint()
                    {
                        CapacityLayerName = LayerNames.ShrubSpace,
                        ConsumedCapacityPerPop = 3f
                    }
                };
                ColorPlant.IdealTemperatureRange = new Range(14.7f, 15f);
                ColorPlant.IdealMoistureRange = new Range(0.29f, 0.33f);
                ColorPlant.TemperatureExtremes = new Range(12.5f, 16.9f);
                ColorPlant.MoistureExtremes = new Range(0.2f, 0.49f);
                ColorPlant.MaxPollutionDensity = 0.7f;
                ColorPlant.PollutionDensityTolerance = 0.1f;
                ColorPlant.VoxelsPerEntry = 5;
                ColorPlant.Name = "ColorPlant";
                ColorPlant.DisplayName = "Color plant";
                ColorPlant.MaturityAgeDays = 0.8f;
                ColorPlant.CalorieValue = 4.0f;
                ColorPlant.ResourceItem = new ItemType(typeof(ColorPlantItem));
                ColorPlant.ResourceRange = new Range(1f, 6f);
                ColorPlant.ResourceBonusAtGrowth = 0.9f;
                ColorPlant.ReleasesCO2ppmPerDay = -0.001f;
                (evt.PluginConfig as EcoSim).EcoDef.Species.Add(ColorPlant);
            }
        }

        [EventHandler]
        public void PluginConfigPreSaveEvent(PluginConfigPreSaveEvent evt)
        {
            if (evt.PluginConfig is EcoSim)
            {

            }
        }
    }
}
