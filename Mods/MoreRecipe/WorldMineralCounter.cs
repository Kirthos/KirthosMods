using Eco.Mods.TechTree;
using Eco.Shared.Math;
using Eco.World;
using Eco.World.Blocks;
using Kirthos.Mods.MoreRecipe.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/21/2018
 */


namespace Kirthos.Mods.MoreRecipe
{
    public class WorldMineralCounter
    {
        public static void ScanWorld()
        {
            new Thread(() =>
                {
                    float stone = 0;
                    float iron = 0;
                    float copper = 0;
                    float gold = 0;
                    float coal = 0;
                    float dirt = 0;
                    float tungsten = 0;
                    float tin = 0;
                    float lead = 0;
                    for (int i = 0; i < WorldArea.WholeWorld.Size.X; i++)
                    {
                        for (int j = 0; j < WorldArea.WholeWorld.Size.Y; j++)
                        {
                            for (int k = 0; k < 100; k++)
                            {
                                Block block = World.GetBlock(new Vector3i(i, k, j));
                                if (block is StoneBlock)
                                    stone++;
                                else if (block is DirtBlock)
                                    dirt++;
                                else if (block is IronOreBlock)
                                    iron++;
                                else if (block is CopperOreBlock)
                                    copper++;
                                else if (block is GoldOreBlock)
                                    gold++;
                                else if (block is CoalBlock)
                                    coal++;
                                else if (block is TungstenOreBlock)
                                    tungsten++;
                                else if (block is TinOreBlock)
                                    tin++;
                                else if (block is LeadOreBlock)
                                    lead++;
                            }
                        }
                        if (i % 50 == 0)
                            Console.WriteLine("Scan progress: " + (int)((float)i / WorldArea.WholeWorld.Size.X * 100f) + "%");
                    }
                    float totalBlock = stone + dirt + iron + coal + copper + gold + tin + lead + tungsten;
                    Console.WriteLine();
                    Console.WriteLine("Scan complete");
                    Console.WriteLine();
                    Console.WriteLine("World size " + WorldArea.WholeWorld.Size.X + "/" + WorldArea.WholeWorld.Size.Y);
                    Console.WriteLine("Total underground block: " + ConvertNum(totalBlock));
                    Console.WriteLine("Dirt     " + ConvertNum(dirt).PadRight(12) + (dirt / totalBlock * 100) + "%");
                    Console.WriteLine("Stone    " + ConvertNum(stone).PadRight(12) + (stone / totalBlock * 100) + "%");
                    Console.WriteLine("Coal     " + ConvertNum(coal).PadRight(12) + (coal / totalBlock * 100) + "%");
                    Console.WriteLine("Iron     " + ConvertNum(iron).PadRight(12) + (iron / totalBlock * 100) + "%");
                    Console.WriteLine("Copper   " + ConvertNum(copper).PadRight(12) + (copper / totalBlock * 100) + "%");
                    Console.WriteLine("Gold     " + ConvertNum(gold).PadRight(12) + (gold / totalBlock * 100) + "%");
                    Console.WriteLine("Tungsten " + ConvertNum(tungsten).PadRight(12) + (tungsten / totalBlock * 100) + "%");
                    Console.WriteLine("Tin      " + ConvertNum(tin).PadRight(12) + (tin / totalBlock * 100) + "%");
                    Console.WriteLine("Lead     " + ConvertNum(lead).PadRight(12) + (lead / totalBlock * 100) + "%");
                }).Start();
        }

        public static string ConvertNum(float number)
        {
            string result = "";
            int million = (int)number / 1000000;
            if (million > 0)
                result += million + ",";
            int millier = (int)number % 1000000 / 1000;
            if (millier > 0)
                result += millier + ",";
            result += number % 1000;
            return result;
        }
    }
}
