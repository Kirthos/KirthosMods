using Eco.Gameplay.Blocks;
using Eco.Gameplay.Components;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;
using Eco.World;
using Eco.World.Blocks;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/02/2018
 */

namespace Kirthos.Mods.ColorCube
{

    public class Colorable : BlockAttribute
    {
        public Colorable()
        { }
    }

    public class ColorCubeRecipe : Recipe
    {
        public ColorCubeRecipe()
        {
            ColorCubePlugin.init = "";
            this.Products = new CraftingElement[]
            {
                new CraftingElement<ColorCubeItem>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<ColorPlantItem>(1),
                new CraftingElement<PaperItem>(2),
            };
            this.CraftMinutes = new ConstantValue(1);
            this.Initialize("Color cube", typeof(ColorCubeRecipe));

            CraftingComponent.AddRecipe(typeof(WorkbenchObject), this);
        }
    }

    [Serialized]
    [MaxStackSize(50)]
    [Weight(5000)]
    public class ColorCubeItem : BlockItem<ColorCubeBlock>
    {
        public override string FriendlyName { get { return "Color cube"; } }
        public override string Description { get { return "A colored cube. You can change the color with a paint Sprayer."; } }
    }

    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    public class ColorCubeBlock : Block
    { }

    [Serialized]
    [Wall, Colorable, Solid, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("White", typeof(ColorCubeItem))]
    public class WhiteCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Black", typeof(ColorCubeItem))]
    public class BlackCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Gray", typeof(ColorCubeItem))]
    public class GrayCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Silver", typeof(ColorCubeItem))]
    public class SilverCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Maroon", typeof(ColorCubeItem))]
    public class MaroonCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Red", typeof(ColorCubeItem))]
    public class RedCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Olive", typeof(ColorCubeItem))]
    public class OliveCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Yellow", typeof(ColorCubeItem))]
    public class YellowCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Green", typeof(ColorCubeItem))]
    public class GreenCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Lime", typeof(ColorCubeItem))]
    public class LimeCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Teal", typeof(ColorCubeItem))]
    public class TealCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Aqua", typeof(ColorCubeItem))]
    public class AquaCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Navy", typeof(ColorCubeItem))]
    public class NavyCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Blue", typeof(ColorCubeItem))]
    public class BlueCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Purple", typeof(ColorCubeItem))]
    public class PurpleCubeBlock : Block { }
    //*
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Fuchsia", typeof(ColorCubeItem))]
    public class FuchsiaCubeBlock : Block { }
    //*/
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Orange", typeof(ColorCubeItem))]
    public class OrangeCubeBlock : Block { }
    //*/
    [Serialized]
    [Solid, Wall, Colorable, BuildRoomMaterialOption]
    [Tier(2)]
    [IsForm("Pink", typeof(ColorCubeItem))]
    public class PinkCubeBlock : Block { }
    //*/

}