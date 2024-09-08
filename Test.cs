using Shapez2;
class Test
{
    public static void Main(string[] args)
    {
        var (shape1, shape2) = (Shape.EmptyShape, Shape.EmptyShape);

        Console.WriteLine(Shape.IsShapeCodeValidWithLog("CySrWmWg:P-RucuWu:CbCcGrRw"));

        shape1 = "CuCrCgCb";
        Console.WriteLine(Shape.Rotate(shape1, Variant.Rotate.hundredEighty));

        Shape.CurrentMode = GameMode.Hexagon;
        shape1 = "HrFgGbHcFmGy";
        Console.WriteLine(Shape.Rotate(shape1));
        Shape.CurrentMode = GameMode.Quarter;

        shape1 = "crcrcrcr:CuCuCuCu:cr------";
        Console.WriteLine(Shape.Cut(shape1));

        shape1 = "CucrcrCu:CuCucrcr:----CuCu";
        shape2 = "CucgCucg";
        Console.WriteLine(Shape.Cut(shape1, shape2, Variant.Cut.Swap));

        shape1 = "crcr----:crcrcrcr:Cucrcrcr:crcrcrcr";
        Console.WriteLine(Shape.PinPush(shape1));

        shape1 = "CuCu----:CuCu----:Cu------:Cu------";
        shape2 = "--cr----:--Cu----:--Cu----:CuCuCuCu";
        Console.WriteLine(Shape.Stack(shape1, shape2));

        Shape.MaxFloorEachShape = 5;
        shape1 = "CuCuCu--:CuCu----:Cu------:--------:P-CrCr--";
        shape2 = "------cr:------Cu:------Cu";
        Console.WriteLine(Shape.Stack(shape1, shape2));
        Shape.MaxFloorEachShape = 4;

        shape1 = "Cu------:P-------";
        Console.WriteLine(Shape.CrystalGenerate(shape1, Color.Red));
        
        shape1 = "CuCrWuWu:P-cuCuWu";
        Console.WriteLine(Shape.Paint(shape1, Color.Red));

        Console.WriteLine(Shape.ColorMix(Color.Blue, Color.Magenta));
        Console.WriteLine(Shape.ColorMix(Color.Yellow, Color.Magenta));
    }
}
