namespace Shapez2
{
    public class Shape : ICloneable
    {
        /// <remarks>
        /// If you wang to change PieceEachFloor independently, please use UnsafeSetPieceEachFloor(), CurrentMode follows suit.
        /// </remarks>
        public static GameMode CurrentMode
        {
            get{ return _CurrentMode; } 
            set
            {
                if (value == GameMode.Quarter)
                    PieceEachFloor = 4;
                else if(value == GameMode.Hexagon)
                    PieceEachFloor = 6;
                else
                    throw new ArgumentException("Unknown game mode.", nameof(CurrentMode));
                _CurrentMode = value;
            }
        }
        private static GameMode _CurrentMode = GameMode.Quarter;
        public static void UnsafeSetCurrentMode(GameMode gameMode)
        {
            _CurrentMode = gameMode;
        }
        /// <remarks>
        /// If you wang to change PieceEachFloor independently, please use UnsafeSetPieceEachFloor(), CurrentMode follows suit.
        /// </remarks>
        public static int PieceEachFloor 
        {
            get{ return _PieceEachFloor; } 
            private set{ _PieceEachFloor = value; }
        }
        private static int _PieceEachFloor = 4;
        public static void UnsafeSetPieceEachFloor(int pieceEachFloor)
        {
            _PieceEachFloor = pieceEachFloor;
        }
        public static int MaxFloorEachShape = 4;

        public static Shape EmptyShape
        {
            get
            {
                if (MaxFloorEachShape == 4 && CurrentMode == GameMode.Quarter)
                    return _EmptyShape44;
                else if (MaxFloorEachShape == 5 && CurrentMode == GameMode.Quarter)
                    return _EmptyShape54;
                else if (MaxFloorEachShape == 4 && CurrentMode == GameMode.Hexagon)
                    return _EmptyShape46;
                else if (MaxFloorEachShape == 5 && CurrentMode == GameMode.Hexagon)
                    return _EmptyShape56;
                else
                {
                    if (CurrentMode == GameMode.Quarter)
                        return GenerateEmptyShape(MaxFloorEachShape, 4);
                    if (CurrentMode == GameMode.Hexagon)
                        return GenerateEmptyShape(MaxFloorEachShape, 6);
                    throw new ArgumentException("Unknown game mode.", nameof(CurrentMode));
                }
            }
        }
        private static Shape _EmptyShape44 = GenerateEmptyShape(4, 4);
        private static Shape _EmptyShape54 = GenerateEmptyShape(5, 4);
        private static Shape _EmptyShape46 = GenerateEmptyShape(4, 6);
        private static Shape _EmptyShape56 = GenerateEmptyShape(5, 6);
        private static Shape GenerateEmptyShape(int maxFloorEachShape, int pieceEachFloor)
        {
            var tempMaxFloor = MaxFloorEachShape;
            var tempPieceEachFloor = PieceEachFloor;
            MaxFloorEachShape = maxFloorEachShape;
            PieceEachFloor = pieceEachFloor;
            Shape shape = string.Concat(Enumerable.Repeat("--",PieceEachFloor));
            MaxFloorEachShape = tempMaxFloor;
            PieceEachFloor = tempPieceEachFloor;
            return shape;
        }
        public static bool IsShapeCodeValid(string? shapeCode)
        {
            try
            {
                IsShapeCodeValidWithException(shapeCode);
            }
            catch (Exception e)
            {
                _ = e;
                return false;
            }
            return true;
        }
        /// <returns>
        /// null if shape code valid else log.
        /// </returns>
        public static string? IsShapeCodeValidWithLog(string? shapeCode)
        {
            try
            {
                IsShapeCodeValidWithException(shapeCode);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }
        public static void IsShapeCodeValidWithException(string? shapeCode)
        {
            ArgumentNullException.ThrowIfNull(shapeCode);

            string[] floorCode = shapeCode.Split(':');

            if (floorCode.Length > MaxFloorEachShape)
                throw new ArgumentException($"Too many floors at \"{shapeCode}\".", nameof(shapeCode));

            foreach (var floor in floorCode)
                if (floor.Length != PieceEachFloor * 2)
                    throw new ArgumentException($"Too many or too few pieces at \"{floor}\".", nameof(shapeCode));

            for (int i = 0; i < shapeCode.Length; i += 2)
            {
                if (shapeCode[i] == ':')
                    i++;

                Piece.IsPieceCodeValidWithExcepition(shapeCode[i..(i + 2)]);
            }
        }
        /// <remarks>
        /// Notice:Must ensure parameter validity, or it will cause UB.
        /// </remarks>
        private static Shape CodeToShape(string shapeCode)
        {
            Shape shape = new();
            for (int i = 0; i < MaxFloorEachShape; i++)
            {
                for (int j = 0; j < PieceEachFloor; j++)
                {
                    shape.ShapeArray[i, j] = Piece.EmptyPiece;
                }
            }

            var floorCount = (shapeCode.Length + 1) / (PieceEachFloor * 2 + 1);
            for (int i = 0; i < floorCount; i++)
            {
                for (int j = 0; j < PieceEachFloor; j++)
                {
                    int index = (PieceEachFloor * 2 + 1) * i + j * 2;
                    shape.ShapeArray[i, j] = shapeCode[index..(index+2)];
                }
            }
            return shape;
        }
        public override string ToString()
        {
            return ToShapeCode();
        }
        /// <remarks>
        /// Notice:Empty shape presented by "--------", not null.
        /// </remarks>
        public string ToShapeCode()
        {
            if(this == EmptyShape)
            {
                if (CurrentMode == GameMode.Quarter)
                    return "--------";
                else if (CurrentMode == GameMode.Hexagon)
                    return "------------";
                else
                    throw new ArgumentException("Unknown game mode.", nameof(CurrentMode));
            }

            string code = "";
            for (int i = 0; i < MaxFloorEachShape; i++)
            {
                for (int j = 0; j < PieceEachFloor; j++)
                    code += ShapeArray[i, j];
                
                code += ":";
            }
            for (int i = 0; i < MaxFloorEachShape; i++)
                if (code.EndsWith(EmptyShape + ":"))
                    code = code[..^(PieceEachFloor * 2 + 1)];

            if (code == "")
                return EmptyShape;

            return code[..^1];
        }
        public static implicit operator string(Shape shape)
        {
            return shape.ToShapeCode();
        }
        public static implicit operator Shape(string shapeCode)
        {
            return new Shape(shapeCode);
        }
        /// <summary>
        /// Used to compare content equality rather than reference equality.
        /// </summary>
        public static bool operator ==(Shape shape1, Shape shape2)
        {
            return shape1.Equals(shape2);
        }
        /// <summary>
        /// Used to compare content equality rather than reference equality.
        /// </summary>
        public static bool operator !=(Shape shape1, Shape shape2)
        {
            return !(shape1 == shape2);
        }
        /// <summary>
        /// Used to compare content equality rather than reference equality.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if(obj is null && this is null)
                return true;
        
            if (obj is not null && this is null || obj is null && this is not null)
                return false;

            try
            {
                for (int i = 0; i < MaxFloorEachShape; i++)               
                    for (int j = 0; j < PieceEachFloor; j++)
                        if (this.ShapeArray[i, j] != ((Shape)obj!).ShapeArray[i, j])
                            return false;
            }
            catch (Exception e)
            {
                _ = e;
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            int hash = 17;

            for (int i = 0; i < MaxFloorEachShape; i++)
                for (int j = 0; j < PieceEachFloor; j++)
                    hash = HashCode.Combine(hash, ShapeArray[i, j]);

            return hash;
        }
        public object Clone()
        {
            var shapeClone = new Shape();
            shapeClone.ShapeArray = ShapeArray;
            return shapeClone;
        }
        
        public Piece[,] ShapeArray;

        private Shape() 
        {
            ShapeArray = new Piece[MaxFloorEachShape, PieceEachFloor]; 
        }
        public Shape(string shapeCode)
        {
            IsShapeCodeValidWithException(shapeCode);

            this.ShapeArray = CodeToShape(shapeCode!).ShapeArray;
        }

        public static Shape Rotate(Shape shape, Variant.Rotate variant = Variant.Rotate.ninetyCW)
        {
            shape = (Shape)shape.Clone();
            shape.Rotate(variant);
            return shape;
        }
        /// <returns>
        /// "this" will be changed if not want this use the static version.
        /// </returns>
        public void Rotate(Variant.Rotate variant = Variant.Rotate.ninetyCW)
        {
            var tempArray = new Piece[MaxFloorEachShape, PieceEachFloor];
            Array.Copy(ShapeArray, tempArray, ShapeArray.Length);

            for (int i = 0; i < MaxFloorEachShape; i++)
            {
                for (int j = 0; j < PieceEachFloor; j++)
                {
                    if (variant == Variant.Rotate.ninetyCW)
                        ShapeArray[i, j] = tempArray[i, j.CCW()];
                    else if (variant == Variant.Rotate.ninetyCCW)
                        ShapeArray[i, j] = tempArray[i, j.CW()];
                    else if (variant == Variant.Rotate.hundredEighty)
                        ShapeArray[i, j] = tempArray[i, j.CW(PieceEachFloor / 2)];
                    else
                        throw new ArgumentException("Unknown variant.", nameof(variant));
                }
            }
        }

        public static (Shape, Shape) Cut(Shape shape, Variant.Cut variant = Variant.Cut.HalfDestroy)
        {
            shape = (Shape)shape.Clone();
            if (variant == Variant.Cut.HalfDestroy)
            {
                shape.Cut();
                return (shape, EmptyShape);
            }
            else if(variant == Variant.Cut.Separate)
            {
                var sepShape = shape.Cut(Variant.Cut.Separate);
                return (shape, sepShape);
            }
            else
                throw new ArgumentException("Unknown variant or the wrong overload was used.", nameof(variant));

        }
        public static (Shape, Shape) Cut(Shape shape1, Shape shape2, Variant.Cut variant)
        {
            shape1 = (Shape)shape1.Clone();
            shape2 = (Shape)shape2.Clone();
            if (variant == Variant.Cut.Swap)
            {
                shape1.Cut(shape2, Variant.Cut.Swap);
                return (shape1, shape2);
            }
            else
                throw new ArgumentException("Unknown variant or the wrong overload was used.", nameof(variant));
        }
        /// <returns>
        /// "this" will be changed if not want this use the static version.
        /// </returns>
        public Shape Cut(Variant.Cut variant = Variant.Cut.HalfDestroy)
        {
            if(variant == Variant.Cut.HalfDestroy)
            {
                for (int i = 0; i < MaxFloorEachShape; i++)
                {
                    if(CurrentMode == GameMode.Quarter)
                    {
                        Destroy(i, 2);
                        Destroy(i, 3);
                    }
                    else if(CurrentMode == GameMode.Hexagon)
                    {
                        Destroy(i, 3);
                        Destroy(i, 4);
                        Destroy(i, 5);
                    }
                }
                Fall();
                
                return EmptyShape;
            }
            else if(variant == Variant.Cut.Separate)
            {
                Shape shapeCopy = (Shape)this.Clone();
                this.Cut();
                shapeCopy.Rotate(Variant.Rotate.hundredEighty);
                shapeCopy.Cut();
                shapeCopy.Rotate(Variant.Rotate.hundredEighty);
                
                return shapeCopy;
            }
            else
                throw new ArgumentException("Unknown variant or the wrong overload was used.", nameof(variant));
        }
        /// <returns>
        /// "this" and "shape2" will be changed if not want this use the static version.
        /// </returns>
        public Shape Cut(Shape shape2, Variant.Cut? variant)//Nullable should not have been used here, but omitting would cause error.
        {
            if(variant != Variant.Cut.Swap)
                throw new ArgumentException("Unknown variant or the wrong overload was used.", nameof(variant));

            if (ReferenceEquals(this, shape2))
                shape2 = (Shape)shape2.Clone();

            var WestShape = this.Cut(Variant.Cut.Separate);
            var WestShape2 = shape2.Cut(Variant.Cut.Separate);

            for (int i = 0; i < MaxFloorEachShape; i++)
            {
                if (CurrentMode == GameMode.Quarter)
                {
                    this.ShapeArray[i, 2] = WestShape2.ShapeArray[i, 2];
                    this.ShapeArray[i, 3] = WestShape2.ShapeArray[i, 3];
                }
                else if (CurrentMode == GameMode.Hexagon)
                {
                    this.ShapeArray[i, 3] = WestShape2.ShapeArray[i, 3];
                    this.ShapeArray[i, 4] = WestShape2.ShapeArray[i, 4];
                    this.ShapeArray[i, 5] = WestShape2.ShapeArray[i, 5];
                }
                else
                    throw new ArgumentException("Unknown game mode.", nameof(CurrentMode));
            }
            for (int i = 0; i < MaxFloorEachShape; i++)
            {
                if (CurrentMode == GameMode.Quarter)
                {
                    shape2.ShapeArray[i, 2] = WestShape.ShapeArray[i, 2];
                    shape2.ShapeArray[i, 3] = WestShape.ShapeArray[i, 3];
                }
                else if (CurrentMode == GameMode.Hexagon)
                {
                    shape2.ShapeArray[i, 3] = WestShape.ShapeArray[i, 3];
                    shape2.ShapeArray[i, 4] = WestShape.ShapeArray[i, 4];
                    shape2.ShapeArray[i, 5] = WestShape.ShapeArray[i, 5];
                }
                else
                    throw new ArgumentException("Unknown game mode.", nameof(CurrentMode));
            }

            return shape2;
        }

        public static Shape Stack(Shape shapeBelow, Shape shapeAbove)
        {
            shapeBelow = (Shape)shapeBelow.Clone();
            shapeBelow.Stack(shapeAbove);
            return shapeBelow;
        }
        /// <returns>
        /// "this" will be changed if not want this use the static version.
        /// </returns>
        public void Stack(Shape shapeAbove)
        {
            if (ReferenceEquals(shapeAbove, this))
                shapeAbove = (Shape)shapeAbove.Clone();

            Piece[,] bigShapeArray = new Piece[MaxFloorEachShape * 2, PieceEachFloor];

            for (int i = 0; i < MaxFloorEachShape; i++)
                for (int j = 0; j < PieceEachFloor; j++)
                    bigShapeArray[i, j] = this.ShapeArray[i, j];

            for (int i = 0; i < MaxFloorEachShape; i++)
                for (int j = 0; j < PieceEachFloor; j++)
                    if (shapeAbove.ShapeArray[i, j].Type == Type.Crystal)
                        bigShapeArray[i + MaxFloorEachShape, j] = Piece.EmptyPiece;
                    else
                        bigShapeArray[i + MaxFloorEachShape, j] = shapeAbove.ShapeArray[i, j];
            
            bigShapeArray = FallExtraFloor(bigShapeArray, MaxFloorEachShape * 2);

            for (int i = 0; i < MaxFloorEachShape; i++)
                for (int j = 0; j < PieceEachFloor; j++)
                    this.ShapeArray[i, j] = bigShapeArray[i, j];
        }

        public static Shape PinPush(Shape shape)
        {
            shape = (Shape)shape.Clone();
            shape.PinPush();
            return shape;
        }
        /// <returns>
        /// "this" will be changed if not want this use the static version.
        /// </returns>
        public void PinPush()
        {
            bool[] needPush = new bool[PieceEachFloor];
            for (int j = 0; j < PieceEachFloor; j++)
                if(ShapeArray[0,j].Type != Type.Empty)
                    needPush[j] = true;
            
            for (int j = 0; j < PieceEachFloor; j++)
                Destroy(3,j);
            
            for (int i = MaxFloorEachShape - 1; i >= 1; i--)
                for (int j = 0; j < PieceEachFloor; j++)
                    ShapeArray[i,j] = ShapeArray[i - 1,j];
            for (int j = 0; j < PieceEachFloor; j++)
                ShapeArray[0, j] = Piece.EmptyPiece;
            
            for (int j = 0; j < PieceEachFloor; j++)
                if (needPush[j])
                    ShapeArray[0, j] = Piece.PinPiece;
            
            Fall();
        }
        
        public static Shape Paint(Shape shape, Color color)
        {
            shape = (Shape)shape.Clone();
            shape.Paint(color);
            return shape;
        }
        /// <returns>
        /// "this" will be changed if not want this use the static version.
        /// </returns>
        public void Paint(Color color)
        {
            if (color == Color.Empty)
                throw new ArgumentException("Color can not be Empty in the context, do you mean Uncolored?", nameof(color));

            int top = GetTopOfShape(ShapeArray);
            if (top == -1)
                return;
         
            for (int j = 0; j < PieceEachFloor; j++)
            {
                if (ShapeArray[top, j].Type != Type.Empty &&
                    ShapeArray[top, j].Type != Type.Pin &&
                    ShapeArray[top, j].Type != Type.Crystal)
                    { ShapeArray[top, j] = new Piece(ShapeArray[top, j].Type, color); }
            }
        }

        public static Color ColorMix(Color color1, Color color2)
        {
            if (color1 == Color.Empty)
                throw new ArgumentException("Color can not be Empty in the context, do you mean Uncolored?", nameof(color1));
            if (color2 == Color.Empty)
                throw new ArgumentException("Color can not be Empty in the context, do you mean Uncolored?", nameof(color2));
            return Piece.ColorMixTable[(int)color1, (int)color2];
        }

        public static Shape CrystalGenerate(Shape shape, Color color)
        {
            shape = (Shape)shape.Clone();
            shape.CrystalGenerate(color);
            return shape;
        }
        /// <returns>
        /// "this" will be changed if not want this use the static version.
        /// </returns>
        public void CrystalGenerate(Color color)
        {
            if (color == Color.Empty)
                throw new ArgumentException("Color can not be Empty in the context, do you mean Uncolored?", nameof(color));

            int top = GetTopOfShape(ShapeArray);

            // AUTO DO THIS STEP

            // if (FloorFillTo == -1)
            // {
            //     this.ShapeArray = EmptyShape.ShapeArray;
            //     return;
            // }

            for (int i = 0; i <= top; i++)
                for (int j = 0; j < PieceEachFloor; j++)
                    if (ShapeArray[i, j].Type == Type.Empty || ShapeArray[i, j].Type == Type.Pin)
                        ShapeArray[i, j] = new Piece(Type.Crystal, color);
        }

        private void Destroy(int floor,int piece)
        {
            
            if (ShapeArray[floor, piece].Type != Type.Crystal)
            {
                ShapeArray[floor, piece] = Piece.EmptyPiece;
                return;
            }
            
            ShapeArray[floor, piece] = Piece.EmptyPiece;

            if (ShapeArray[floor, piece.CW()].Type == Type.Crystal)
                Destroy(floor, piece.CW());
           
            if (ShapeArray[floor, piece.CCW()].Type == Type.Crystal)
                Destroy(floor, piece.CCW());

            if(floor + 1 < MaxFloorEachShape)
                if (ShapeArray[floor + 1, piece].Type == Type.Crystal)
                    Destroy(floor + 1, piece);

            if (floor - 1 >= 0)
                if (ShapeArray[floor - 1, piece].Type == Type.Crystal)
                    Destroy(floor - 1, piece);
        }
        private static bool IsAllSupported(Piece[,] shapeArray, int maxFloor, bool[,] isSupported)
        {
            for (int i = 0; i < maxFloor; i++)
            {
                for (int j = 0; j < PieceEachFloor; j++)
                {
                    if (shapeArray[i, j].Type != Type.Empty && isSupported[i, j] == false)
                        return false;
                }
            }
            return true;
        }
        private static bool[,] GetSupportedArray(Piece[,] shapeArray, int maxFloor)
        {
            bool[,] isSupported = new bool[maxFloor, PieceEachFloor];

            for (int j = 0; j < PieceEachFloor; j++)
                if (shapeArray[0, j].Type != Type.Empty)
                    isSupported[0, j] = true;

            for (int i = 1; i < maxFloor; i++)
            {
                for (int j = 0; j < PieceEachFloor; j++)
                {
                    if (isSupported[i - 1, j] == true && shapeArray[i, j].Type != Type.Empty)
                    {
                        isSupported[i, j] = true;
                        for (int count = 1; count <= PieceEachFloor / 2; count++)
                            if (shapeArray[i, j.CW(count)].Type != Type.Empty &&
                                shapeArray[i, j].Type != Type.Pin && shapeArray[i, j.CW(count)].Type != Type.Pin)
                                { isSupported[i, j.CW(count)] = true; }
                            else break;

                        for (int count = 1; count <= PieceEachFloor / 2; count++)
                            if (shapeArray[i, j.CCW(count)].Type != Type.Empty &&
                                shapeArray[i, j].Type != Type.Pin && shapeArray[i, j.CCW(count)].Type != Type.Pin)
                                { isSupported[i, j.CCW(count)] = true; }
                            else break;
                    }
                }
            }
            return isSupported;
        }
        private void Fall()
        {
            this.ShapeArray = FallExtraFloor(ShapeArray, MaxFloorEachShape);
        }
        private static Piece[,] FallExtraFloor(Piece[,] shapeArray, int maxFloor)
        {
            shapeArray = (Piece[,])shapeArray.Clone();
            
            bool[,] isSupported = GetSupportedArray(shapeArray, maxFloor);
            if (IsAllSupported(shapeArray, maxFloor, isSupported))
                return shapeArray;

            HashSet<int> currentFallingGroup;

            for (int i = 1; i < maxFloor; i++)
            {
                for (int j = 0; j < PieceEachFloor; j++)
                {
                    if (isSupported[i, j] == false && shapeArray[i, j].Type != Type.Empty)
                    {
                        currentFallingGroup = [];

                        currentFallingGroup.Add(j);

                        for (int count = 1; count <= PieceEachFloor / 2; count++)
                        {
                            if (shapeArray[i, j.CW(count)].Type == Type.Empty ||
                                shapeArray[i, j].Type == Type.Pin || shapeArray[i, j.CW(count)].Type == Type.Pin)
                                break;
                            else
                                currentFallingGroup.Add(j.CW(count));
                        }
                        for (int count = 1; count <= PieceEachFloor / 2; count++)
                        {
                            if (shapeArray[i, j.CCW(count)].Type == Type.Empty ||
                                shapeArray[i, j].Type == Type.Pin || shapeArray[i, j.CCW(count)].Type == Type.Pin)
                                break;
                            else
                                currentFallingGroup.Add(j.CCW(count));
                        }

                        int FloorFallTo = i - 1;
                        if (i - 2 >= 0)
                            for (; FloorFallTo >= 1; FloorFallTo--)
                                foreach (int count in currentFallingGroup)
                                    if (shapeArray[FloorFallTo - 1, count].Type != Type.Empty)
                                        goto exit;
                        exit:

                        foreach (int count in currentFallingGroup)
                        {
                            shapeArray[FloorFallTo, count] = shapeArray[i, count];
                            shapeArray[i, count] = Piece.EmptyPiece;
                        }

                        isSupported = GetSupportedArray(shapeArray, maxFloor);

                        // DEBUG ONLY

                        // foreach (int count in currentFallingGroup)
                        //     Console.Write($"({i},{count})");
                        // Console.WriteLine();
                        // Console.WriteLine("shapeArrayLength:"+shapeArray.GetLength(0));
                        // for (int _i = 0; _i < maxFloor; _i++)
                        // {
                        //     for (int _j = 0; _j < PieceEachFloor; _j++)
                        //     {
                        //         Console.Write(isSupported[_i, _j] ? "T" : "F");
                        //     }
                        //     Console.Write(":");
                        // }
                        // Console.WriteLine();
                        // var (_low, _high) = (new Shape(), new Shape()); 
                        // for (int _i = 0; _i < MaxFloorEachShape; _i++)
                        //     for (int _j = 0; _j < PieceEachFloor; _j++)
                        //         _low.ShapeArray[_i,_j] = shapeArray[_i,_j];
                        // Console.WriteLine(_low);
                        // if (shapeArray.GetLength(0) != MaxFloorEachShape)
                        // {
                        //     for (int _i = 0; _i < MaxFloorEachShape; _i++)
                        //         for (int _j = 0; _j < PieceEachFloor; _j++)
                        //             _high.ShapeArray[_i, _j] = shapeArray[_i + MaxFloorEachShape, _j];
                        //     Console.WriteLine("high:" + _high);
                        // }
                        // Console.WriteLine(IsAllSupported(shapeArray, maxFloor, isSupported));

                        if (IsAllSupported(shapeArray, maxFloor, isSupported))
                            return shapeArray;
                    }
                }
            }
            throw new Exception("Fall function error.");
        }
        private static int GetTopOfShape(Piece[,] shapeArray)
        {
            for (int top = MaxFloorEachShape - 1; top >= 0; top--)
                for (int j = 0; j < PieceEachFloor; j++)
                    if (shapeArray[top, j].Type != Type.Empty)
                        return top;

            return -1;
        }
    }
    public class Piece
    {
        private const string ValidType = "-PcCRSWHFG";
        private const string ValidType4 = "-PcCRSW";
        private const string ValidType6 = "-PcHFG";
        private const string ValidColor = "-urgbcmyw";
        public static readonly Piece EmptyPiece = "--";
        public static readonly Piece PinPiece = "P-";
        public static readonly Color[,] ColorMixTable =
        {
            {Color.Empty,    Color.Empty,    Color.Empty,    Color.Empty,    Color.Empty,    Color.Empty,    Color.Empty,    Color.Empty,    Color.Empty    },
            {Color.Empty,    Color.Uncolored,Color.Red,      Color.Green,    Color.Blue,     Color.Cyan,     Color.Magenta,  Color.Yellow,   Color.White    },
            {Color.Empty,    Color.Red,      Color.Red,      Color.Yellow,   Color.Magenta,  Color.White,    Color.Magenta,  Color.Yellow,   Color.White    },
            {Color.Empty,    Color.Green,    Color.Yellow,   Color.Green,    Color.Cyan,     Color.Cyan,     Color.White,    Color.Yellow,   Color.White    },
            {Color.Empty,    Color.Blue,     Color.Magenta,  Color.Cyan,     Color.Blue,     Color.Cyan,     Color.Magenta,  Color.White,    Color.White    },
            {Color.Empty,    Color.Cyan,     Color.White,    Color.Cyan,     Color.Cyan,     Color.Cyan,     Color.White,    Color.White,    Color.White    },
            {Color.Empty,    Color.Magenta,  Color.Magenta,  Color.White,    Color.Magenta,  Color.White,    Color.Magenta,  Color.White,    Color.White    },
            {Color.Empty,    Color.Yellow,   Color.Yellow,   Color.Yellow,   Color.White,    Color.White,    Color.White,    Color.Yellow,   Color.White    },
            {Color.Empty,    Color.White,    Color.White,    Color.White,    Color.White,    Color.White,    Color.White,    Color.White,    Color.White    },
        };
        public Type Type;
        public Color Color;
        public Piece(Type type, Color color)
        {
            this.Type = type;
            this.Color = color;
        }
        public Piece(string pieceCode)
        {
            IsPieceCodeValidWithExcepition(pieceCode);

            this.Type = CodeToPiece(pieceCode!).Type;
            this.Color = CodeToPiece(pieceCode!).Color;
        }
        public static bool IsPieceCodeValid(string? pieceCode)
        {
            try
            {
                IsPieceCodeValidWithExcepition(pieceCode);
            }
            catch (Exception e)
            {
                _ = e;
                return false;
            }
            return true;
        }
        /// <returns>
        /// null if piece code valid else log.
        /// </returns>
        public static string? IsPieceCodeValidWithLog(string? pieceCode)
        {
            try
            {
                IsPieceCodeValidWithExcepition(pieceCode);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }
        public static void IsPieceCodeValidWithExcepition(string? pieceCode)
        {
            ArgumentNullException.ThrowIfNull(pieceCode);

            if (pieceCode.Length != 2)
                throw new ArgumentException($"Piece code \"{pieceCode}\" length invalid.", nameof(pieceCode));

            if (Shape.CurrentMode == GameMode.Quarter)
            {
                if (!ValidType4.Contains(pieceCode[0]))
                    throw new ArgumentException($"Unknown piece type \"{pieceCode[0]}\" or not applicable in the current mode.", nameof(pieceCode));
            }
            else if (Shape.CurrentMode == GameMode.Hexagon)
            {
                if (!ValidType6.Contains(pieceCode[0]))
                    throw new ArgumentException($"Unknown piece type \"{pieceCode[0]}\" or not applicable in the current mode.", nameof(pieceCode));
            }
            else
                throw new ArgumentException($"Unknown game mode \"{Shape.CurrentMode}\".", nameof(Shape.CurrentMode));

            if (!ValidColor.Contains(pieceCode[1]))
                throw new ArgumentException($"Unknown piece color \"{pieceCode[1]}\".", nameof(pieceCode));

            if (pieceCode[0] == '-' && pieceCode[1] != '-')
                throw new ArgumentException("Empty type can not have color.", nameof(pieceCode));
            if (pieceCode[0] == 'P' && pieceCode[1] != '-')
                throw new ArgumentException("Pin type can not have color.", nameof(pieceCode));
            if (pieceCode[0] == 'c' && pieceCode[1] == '-')
                throw new ArgumentException("Crystal type must have color.", nameof(pieceCode));
            if (!"-Pc".Contains(pieceCode[0]) && pieceCode[1] == '-')
                throw new ArgumentException("Regular type must have color.", nameof(pieceCode));
        }
        /// <remarks>
        /// Notice:Must ensure parameter validity, or it will cause UB.
        /// </remarks>
        private static Piece CodeToPiece(string pieceCode)
        {
            return new Piece((Type)ValidType.IndexOf(pieceCode[0]), (Color)ValidColor.IndexOf(pieceCode[1]));
        }
        public override string ToString()
        {
            return ToPieceCode();
        }
        public string ToPieceCode()
        {
            return ValidType[(int)Type] + "" + ValidColor[(int)Color];
        }
        public static implicit operator string(Piece piece)
        {
            return piece.ToPieceCode();
        }
        public static implicit operator Piece(string pieceCode)
        {
            return new Piece(pieceCode);
        }
        /// <summary>
        /// Used to compare content equality rather than reference equality.
        /// </summary>
        public static bool operator ==(Piece piece1, Piece piece2)
        {
            return piece1.Equals(piece2);
        }
        /// <summary>
        /// Used to compare content equality rather than reference equality.
        /// </summary>
        public static bool operator !=(Piece piece1, Piece piece2)
        {
            return !(piece1 == piece2);
        }
        /// <summary>
        /// Used to compare content equality rather than reference equality.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is null && this is null)
                return true;
            if (obj is not null && this is null || obj is null && this is not null)
                return false;
            try
            {
                if (this.Type != ((Piece)obj!).Type)
                    return false;
                if (this.Color != ((Piece)obj).Color)
                    return false;
            }
            catch (Exception e)
            {
                _ = e;
                return false;
            }
           
            return true;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Color);
        }
    }
    public enum GameMode
    {
        Quarter = 4,
        Hexagon = 6,
    }
    public enum Type
    {
        Empty,
        Pin,
        Crystal,
        Circle,
        Square,
        Star,
        Diamond,
        Hexagon,
        Flower,
        Gear,
    }
    public enum Color
    {
        Empty,
        Uncolored,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow,
        White,
    }
    public class Variant
    {
        public enum Rotate
        {
            ninetyCW,
            ninetyCCW,
            hundredEighty,
        }
        public enum Cut
        {
            HalfDestroy,
            Separate,
            Swap,
        }
    }
    public static class Extension
    {
        public static int CW(this int i)
        {
            return (i + 1) % Shape.PieceEachFloor;
        }
        public static int CCW(this int i)
        {
            return (i - 1 + Shape.PieceEachFloor) % Shape.PieceEachFloor;
        }
        public static int CW(this int i, int count)
        {
            return (i + count) % Shape.PieceEachFloor;
        }
        public static int CCW(this int i, int count)
        {
            return (i - count + count * Shape.PieceEachFloor) % Shape.PieceEachFloor;
        }
        public static Shape ToShape(this string code)
        {
            return new Shape(code);
        }
        public static Piece ToPiece(this string code)
        {
            return new Piece(code);
        }
    }
}