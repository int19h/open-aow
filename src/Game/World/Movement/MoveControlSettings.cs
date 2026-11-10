// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Graphics;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Movement;

public partial class MoveControlSettings : EObject
{

    // MoveT.PAS:73
    public MoveControlSettings(ECustomNode owner) : base(owner)
    {
        FRefCount = 1;
        FProcessedMapFields = new DelphiList();
        FPath = new Movepath();
        FSubMoves = new SubMoveList(null);
        FUnits = new UnitUserList(null);
        Reset();
    }

    public int FRefCount;
    public int FMovePointsLimit;
    public SubMoveList FSubMoves;
    public int FSubMoveIndex;
    public DelphiList FProcessedMapFields;
    public int[] FRowOffsets = [];
    public int[][] FLevelData = [];
    public FixedArray_int_8 FEvenOffsets;
    public FixedArray_int_8 FOddOffsets;
    public bool FMovePointsClear;
    public Status FStatus;
    public Set<MoveModeElement> FMode;
    public Movepath FPath;
    public sbyte FPlayer;
    public int FWidth;
    public int FHeight;
    public int FLevelCount;
    public int FSourceX;
    public int FSourceY;
    public int FSourceL;
    public int FDestinationX;
    public int FDestinationY;
    public int FDestinationL;
    public int FMovePointsMax;
    public UnitHS FArmyHS;
    public int FSelection;
    public ArmyHS FTransporterArmy;
    public UnitUserList FUnits;
    // AddUnitsEx
    public FixedArray_Aow_Units_FixedArray_sbyte_16_16 FMovePointTable;

    // MoveT.PAS:84
    public override void Dispose()
    {
        Clear();
        FProcessedMapFields?.Dispose();
        FSubMoves?.Dispose();
        FPath?.Dispose();
        FUnits?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:107
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // MoveT.PAS:113
    public override int Release()
    {
        FRefCount -= 1;
        int Result = FRefCount;
        if (FRefCount == 0)
        {
            Dispose();
        }

        return Result;
    }

    // MoveT.PAS:94
    public override void Clear()
    {
        FPath.Clear();
        FProcessedMapFields.Clear();
        for (int I = 0, last = FLevelCount - 1; I <= last; I++)
        {
            Array.Resize(ref FLevelData[I], 0);
        }

        Array.Resize(ref FRowOffsets, 0);
        Array.Resize(ref FLevelData, 0);
        FWidth = 0;
        FHeight = 0;
        FLevelCount = 0;
    }

    // MoveT.PAS:120
    public void ResetState()
    {
        FStatus = Status.Idle;
    }

    // MoveT.PAS:125
    public void Reset()
    {
        FPath.Clear();
        FSubMoves.Clear();
        FProcessedMapFields.Clear();
        FMovePointsMax = 4095;
        FillBytes(Bytes.Of(ref FMovePointTable, 256), -1);
        for (int I = 0, last = FLevelCount - 1; I <= last; I++)
        {
            if (FLevelData[I] is not null)
            {
                FillInt(MemoryMarshal.AsBytes(FLevelData[I].AsSpan(0, FWidth * FHeight * 4 / 4)), FWidth * FHeight, 16777215);
            }
        }

        FMode = Set<MoveModeElement>.Of();
        FPlayer = -1;
        FSourceX = -1;
        FSourceY = -1;
        FSourceL = -1;
        FDestinationX = -1;
        FDestinationY = -1;
        FDestinationL = -1;
        FStatus = Status.Idle;
        FUnits.Clear();
    }

    // MoveT.PAS:148
    public void SetSourcePos(int X, int Y, int L)
    {
        FSourceX = X;
        FSourceY = Y;
        FSourceL = L;
    }

    // MoveT.PAS:155
    public void SetDestinationPos(int X, int Y, int L)
    {
        FDestinationX = X;
        FDestinationY = Y;
        FDestinationL = L;
    }

    // MoveT.PAS:162
    public void SetupArmy(ArmyHS Army, byte Selection)
    {
        FArmyHS = Army;
        FSelection = Selection;
        FUnits.AddUnitsEx(Army.FArmy, Selection);
        FSourceX = Army.Xhx;
        FSourceY = Army.Yhx;
        FSourceL = Army.Lhx;
        FPlayer = Army.FArmy.FPlayer;
        Army.FArmy.CreateMovePointTable(Selection, Bytes.Of(ref FMovePointTable));
        if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType != PlayerType.Human)
        {
            FMode = FMode + Set<MoveModeElement>.Of(MoveModeElement.AI);
        }
    }

    // MoveT.PAS:175
    public void SetupLevel(int Level)
    {
        if (FLevelData[Level] is null)
        {
            Array.Resize(ref FLevelData[Level], FWidth * FHeight);
            FillInt(MemoryMarshal.AsBytes(FLevelData[Level].AsSpan(0, FWidth * FHeight * 4 / 4)), FWidth * FHeight, 16777215);
        }
    }

    // MoveT.PAS:184
    public void SetupMovePointData(int Width, int Height, int LevelCount)
    {
        int RowSize;
        int Offset;
        if (Width != FWidth || Height != FHeight || LevelCount != FLevelCount)
        {
            Clear();
            FWidth = Width;
            FHeight = Height;
            FLevelCount = LevelCount;
            FMovePointsClear = true;
            Offset = 0;
            RowSize = Width * 4;
            Array.Resize(ref FRowOffsets, Height);
            for (int I = 0; I <= Height - 1; I++)
            {
                FRowOffsets[I] = Offset;
                Offset += RowSize;
            }

            Array.Resize(ref FLevelData, LevelCount);
            Array.Clear(FLevelData);
            for (int I = 1; I <= 6; I++)
            {
                FEvenOffsets[I] = HexGeometry.HNtoXYTable[0][I].X * 4 + HexGeometry.HNtoXYTable[0][I].Y * (Width * 4);
                FOddOffsets[I] = HexGeometry.HNtoXYTable[1][I].X * 4 + HexGeometry.HNtoXYTable[1][I].Y * (Width * 4);
            }
        }
        else
        {
            ClearMovePoints();
        }
    }

    // MoveT.PAS:214
    public void ClearMovePoints()
    {
        if (!FMovePointsClear)
        {
            for (int I = 0, last = FLevelCount - 1; I <= last; I++)
            {
                if (FLevelData[I] is not null)
                {
                    FillInt(MemoryMarshal.AsBytes(FLevelData[I].AsSpan(0, FWidth * FHeight * 4 / 4)), FWidth * FHeight, 16777215);
                }
            }

            FMovePointsClear = true;
        }
    }

    // MoveT.PAS:238
    public int[] GetMovePointLevelData(int Level) => FLevelData[Level];

    internal static void FillInt(Span<byte> destination, int count, int value)
    {
        Blit.FillInt(destination, count, value);
    }

    public enum Status : byte
    {
        Idle = 0, Processing = 1, Processed = 2, Failed = 3
    }

    // intf_P5_01.inc:63

    // intf_P5_01.inc:63

}
