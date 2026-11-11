// Translated from AoWE.pas.
using Aow.Engine.Buffers;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Movement;

// map, status, player, mode, source, destination, map level, processed count, move limit, move
// table, processed fields, army, selection, move data, callback, field offsets, point offsets,
// gap endpoints, direct route, exploration mask, settings
public partial class MoveControl : IDisposable
{

    // MoveT.PAS:257
    public MoveControl(HSMap owner)
    {
        FMap = owner;
        FGapStarts = new MapCoordinateList();
        FGapEnds = new MapCoordinateList();
        FDirectRoute = new MapCoordinateList();
    }

    public HSMap FMap;
    public Set<StatusElement> FStatus;
    public sbyte FPlayer;
    public Set<MoveModeElement> FMode;
    public int FSourceX;
    public int FSourceY;
    public int FSourceL;
    public int FDestinationX;
    public int FDestinationY;
    public int FDestinationL;
    public MapLevel FMapLevel;
    public int FProcessedCount;
    public int FMovePointsLimit;
    public SByteArray16By16 FMovePointTable;
    public DelphiList FProcessedMapFields;
    public ArmyHexSprite FArmyHS;
    public int FSelection;
    public ArmyHexSprite FTransporterArmy;
    public MoveOnMovePointProc FOnMovePoint;
    public Int32Array8[] FFieldOffsets = new Int32Array8[2];
    public Int32Array8By2 FPointOffsets;
    public MapCoordinateList FGapStarts;
    public MapCoordinateList FGapEnds;
    public MapCoordinateList FDirectRoute;
    public int FExploredMask;
    public MoveControlSettings FSettings;

    // MoveT.PAS:266
    public virtual void Dispose()
    {
        FGapStarts?.Dispose();
        FGapEnds?.Dispose();
        FDirectRoute?.Dispose();
    }

    // MoveT.PAS:474
    public virtual bool SetupMove()
    {
        AoWMapField Field;
        PathFindingStep SubMove;
        FProcessedCount = 0;
        FSettings.ClearMovePoints();
        FSettings.FSubMoves.Clear();
        FSettings.FSubMoveIndex = 0;
        FSettings.FPath.Clear();
        if (FMode.Contains(MoveModeElement.LogProcessedMapFields))
        {
            FSettings.FProcessedMapFields.Count = 0;
            FSettings.FProcessedMapFields.Capacity = 4096;
        }

        // { Original bug: the Y bound also uses the map width. }
        if (!(FMapLevel != null && FSourceX >= 0 && FSourceX < FMapLevel.Width && FSourceY >= 0 && FSourceY < FMapLevel.Width))
        {
            // { Preserve the original assertion diagnostic. }
            Assert(false, "Invalid settings (TMoveControl)", "D:\\AoWDev\\AoWE\\inc\\MoveT.PAS", 32187);
        }

        if ((FSettings.FSourceX != FSettings.FDestinationX || FSettings.FSourceY != FSettings.FDestinationY || FSettings.FSourceL != FSettings.FDestinationL) && (FDestinationX == -1 || ValidLocation(FDestinationX, FDestinationY, FDestinationL)))
        {
            FSettings.SetupLevel(FSettings.FSourceL);
            if (FSettings.FDestinationL != -1)
            {
                FSettings.SetupLevel(FSettings.FDestinationL);
            }

            if (FSourceL == FDestinationL && FMovePointsLimit > 40 && FDestinationX != -1)
            {
                FMovePointsLimit = GetMovePointsCostFast(FSourceX, FSourceY, FDestinationX, FDestinationY);
                if (FMovePointsLimit == -1 || FMovePointsLimit > FSettings.FMovePointsMax)
                {
                    FMovePointsLimit = FSettings.FMovePointsMax;
                }

                AoWHSMap.Current.FAIExecuter.UseProcessTime(FMovePointsLimit);
            }

            FSettings.SetMovePoints(FSourceX, FSourceY, FSourceL, 0);
            Field = (AoWMapField)FMapLevel.GetField(FSourceX, FSourceY);
            if (FMode.Contains(MoveModeElement.LogProcessedMapFields))
            {
                FProcessedMapFields.Add(Field);
            }

            SubMove = SetupSubMove(Field, null);
            if ((Field.FFieldFlags & 32768) != 0)
            {
                Field.MoveExclusive(this);
            }

            SubMove.Owner = FSettings.FSubMoves;
            AoWHSMap.Current.FAIExecuter.UseProcessTime(100);
        }
        else
        {
            FSettings.SetupLevel(FSettings.FSourceL);
            FSettings.SetMovePoints(FSourceX, FSourceY, FSourceL, 0);
            FSettings.FStatus = MoveControlSettings.Status.Processed;
        }

        return true;
    }

    // MoveT.PAS:1515
    public virtual void ProcessMove(MoveControlSettings Settings)
    {
        if (Settings.FSourceX != Settings.FDestinationX || Settings.FSourceY != Settings.FDestinationY || Settings.FSourceL != Settings.FDestinationL)
        {
            if (Activate(Settings))
            {
                Assert(GameTables.AoWEngine.FDemo || Settings.FStatus != MoveControlSettings.Status.Processed, "", "MoveT.PAS", 1541);
                do
                {
                }
                while (!!Process());
                if (FDestinationX != -1)
                {
                    CalculatePathEx(Settings.FPath, (sbyte)FDestinationX, (sbyte)FDestinationY, (sbyte)FDestinationL);
                    if (Settings.FPath.Count == 0)
                    {
                        Settings.FStatus = MoveControlSettings.Status.Failed;
                    }
                }

                Deactivate();
            }
        }
        else
        {
            Settings.FPath.Clear();
            Settings.FPath.AddXYL((sbyte)Settings.FSourceX, (sbyte)Settings.FSourceY, (sbyte)Settings.FSourceL);
            Settings.FStatus = MoveControlSettings.Status.Processed;
        }
    }

    // MoveT.PAS:274
    public void Clear()
    {
        Deactivate();
        FPlayer = -1;
        FMapLevel = null;
        FSourceX = -1;
        FSourceY = -1;
        FSourceL = -1;
        FDestinationX = -1;
        FDestinationY = -1;
        FDestinationL = -1;
        FProcessedCount = 0;
        FOnMovePoint = DefaultMoveOnMovePointProc;
        FProcessedMapFields = null;
    }

    // MoveT.PAS:299
    public int DefaultMoveOnMovePointProc(AoWMapField MapF, int MoveOverCode)
    {
        int Overlay;
        int Points;
        Overlay = MapF.OverlayType;
        Points = FMovePointTable[MapF.TerrainType][Overlay - -1];
        if ((MapF.FFieldFlags & 16384) != 0)
        {
            Points = MapF.CanMoveOn(this, Points, MoveOverCode);
        }

        return Points;
    }

    // MoveT.PAS:308
    public AoWMapField FindSubMoveOrigin(AoWMapField MapF)
    {
        int Best;
        int Points;
        int I;
        PathFindingStep SubMove;
        MoveControlSettings Settings;
        Best = 16777215;
        AoWMapField Result = null;
        I = 0;
        Settings = FSettings;
        while (I < Settings.FSubMoves.Count)
        {
            SubMove = Settings.FSubMoves.GetSubMove(I);
            if (MapF.Xhx == SubMove.FLocation.X && MapF.Yhx == SubMove.FLocation.Y && MapF.Lhx == SubMove.FLocation.L)
            {
                Points = Settings.GetMovePoints(SubMove.FOrigin.X, SubMove.FOrigin.Y, SubMove.FOrigin.L);
                if (Points <= Best)
                {
                    // { After pushing L, the original stack slot holds Self, not Result. }
                    Result = (AoWMapField)FMap.MapContainer.GetField(SubMove.FOrigin.X, SubMove.FOrigin.Y, SubMove.FOrigin.L);
                    Best = Points;
                }
            }

            I += 1;
        }

        return Result;
    }

    // MoveT.PAS:430
    public bool ValidLocation(int X, int Y, int L)
    {
        AoWMapField Field;
        Field = (AoWMapField)FMap.MapContainer.GetField(X, Y, L);
        return FOnMovePoint(Field, 1) >= 0;
    }

    // MoveT.PAS:437
    public void ExecuteExternalMove(AoWMapField Source, AoWMapField Target, int MovePoints)
    {
        int Points = default;
        int Previous = default;
        int Index = default;
        PathFindingStep SubMove = default;
        var canMoveOver = Source.CanMoveOver(this);
        Points = FOnMovePoint(Target, canMoveOver);
        if (Points >= 0)
        {
            FSettings.SetupLevel(Target.Lhx);
            if (MovePoints == -1)
            {
                Points += FSettings.GetMovePoints(Source.Xhx, Source.Yhx, Source.Lhx);
            }
            else
            {
                Points = FSettings.GetMovePoints(Source.Xhx, Source.Yhx, Source.Lhx) + MovePoints;
            }

            Previous = FSettings.GetMovePoints(Target.Xhx, Target.Yhx, Target.Lhx);
            if (Points < Previous && Points <= FMovePointsLimit)
            {
                var xyl = Target.GetXYL();
                Index = FSettings.FSubMoves.IndexOfXYL(xyl);
                if (Index > FSettings.FSubMoveIndex)
                {
                    FSettings.FSubMoves.Delete(Index);
                }

                if (FMode.Contains(MoveModeElement.LogProcessedMapFields) && Previous == 16777215)
                {
                    FProcessedMapFields.Add(Target);
                }

                FSettings.SetMovePoints(Target.Xhx, Target.Yhx, Target.Lhx, Points);
                if (Target.Xhx == FDestinationX && Target.Yhx == FDestinationY && Target.Lhx == FDestinationL)
                {
                    FMovePointsLimit = Points;
                }

                if ((Target.FFieldFlags & 8192) == 0 || Target.CanMoveOver(this) != 0)
                {
                    SubMove = SetupSubMove(Target, Source);
                    if ((Target.FFieldFlags & 32768) != 0)
                    {
                        Target.MoveExclusive(this);
                    }

                    SubMove.Owner = FSettings.FSubMoves;
                }
                else
                {
                    SubMove = new PathFindingStep(null);
                    var targetXYL = Target.GetXYL();
                    SubMove.FLocation.Packed = targetXYL;
                    var sourceXYL = Source.GetXYL();
                    SubMove.FOrigin.Packed = sourceXYL;
                    SubMove.Owner = FSettings.FSubMoves;
                }
            }
        }
    }

    // MoveT.PAS:1257
    public PathFindingStep SetupSubMove(AoWMapField MapF, AoWMapField Origin)
    {
        PathFindingStep SubMove;
        int OverCode;
        int Direction;
        int Count;
        int HT;
        SubMove = new PathFindingStep(null);
        var xyl = MapF.GetXYL();
        SubMove.FLocation.Packed = xyl;
        if (Origin != null)
        {
            var originXYL = Origin.GetXYL();
            SubMove.FOrigin.Packed = originXYL;
            if ((MapF.FFieldFlags & 8192) == 0)
            {
                OverCode = 1;
            }
            else
            {
                OverCode = MapF.CanMoveOver(this);
            }
        }
        else
        {
            OverCode = 1;
        }

        if (OverCode != 0)
        {
            if (FSettings.FDestinationX != -1)
            {
                Direction = HexGeometry.dHXtoHT(MapF.Xhx, MapF.Yhx, FSettings.FDestinationX, FSettings.FDestinationY);
                if (Direction == 0)
                {
                    Direction = 1;
                }
            }
            else
            {
                Direction = 1;
            }

            for (int I = 0; I <= 2; I++)
            {
                HT = P5OddDirections[Direction - 1][I];
                Count = CalculateMovePointLine(MapF, HT, OverCode);
                if (Count != 0)
                {
                    SubMove.AddMovePointLine(HT, Count);
                }
            }

            for (int I = 0; I <= 2; I++)
            {
                HT = P5EvenDirections[Direction - 1][I];
                Count = CalculateMovePointLine(MapF, HT, OverCode);
                if (Count != 0)
                {
                    SubMove.AddMovePointLine(HT, Count);
                }
            }
        }

        return SubMove;
    }

    // MoveT.PAS:1295
    public bool ProcessSubMove(PathFindingStep SubMove)
    {
        int X = default;
        int Y = default;
        int Count;
        int Direction;
        int Processed;
        if (SubMove.FMovePointLines.Count != 0)
        {
            FSettings.SetupLevel(SubMove.FLocation.L);
            SetMapL(FMap.MapContainer.GetMapLevel(SubMove.FLocation.L));
            Direction = SubMove.FMovePointLines[0].A;
            Count = SubMove.FMovePointLines[0].B;
            HexGeometry.CenterHNtoHX(SubMove.FLocation.X, SubMove.FLocation.Y, Direction, ref X, ref Y);
            SubMove.FMovePointLines.Delete(0);
            Processed = FProcessedCount;
            ProcessHexagon((AoWMapField)FMapLevel.GetField(X, Y), Direction, Count - 1);
            Processed = FProcessedCount - Processed;
            AoWHSMap.Current.FAIExecuter.UseProcessTime(Round((double)((double)Processed / 430) * 1000));
        }

        return SubMove.FMovePointLines.Count == 0;
    }

    // MoveT.PAS:1315
    public bool Process()
    {
        Assert(FStatus.Contains(StatusElement.Activated), "", "MoveT.PAS", 1339);
        switch (FSettings.FStatus)
        {
            case MoveControlSettings.Status.Idle:
                SetupMove();
                FSettings.FStatus = MoveControlSettings.Status.Processing;
                break;
            case MoveControlSettings.Status.Processing:
                var count = FSettings.FSubMoves.Count;
                if (FSettings.FSubMoveIndex < count)
                {
                    if (ProcessSubMove(FSettings.FSubMoves.GetSubMove(FSettings.FSubMoveIndex)))
                    {
                        FSettings.FSubMoveIndex += 1;
                    }
                }

                var fSubMovesCount = FSettings.FSubMoves.Count;
                if (FSettings.FSubMoveIndex >= fSubMovesCount)
                {
                    FSettings.FStatus = MoveControlSettings.Status.Processed;
                }

                break;
        }

        return FSettings.FStatus != MoveControlSettings.Status.Processed && FSettings.FStatus != MoveControlSettings.Status.Failed;
    }

    // MoveT.PAS:1436
    public void CalculatePath(MoveControlSettings Settings, MovePath Path, sbyte X, sbyte Y, sbyte L)
    {
        Activate(Settings);
        CalculatePathEx(Path, X, Y, L);
        Deactivate();
    }

    // MoveT.PAS:1443
    public int ExploredOnlyMoveOnMovePointProc(AoWMapField MapF, int MoveOverCode)
    {
        int Points;
        if ((MapF.FFieldFlags & FExploredMask) != 0)
        {
            Points = FMovePointTable[MapF.TerrainType][MapF.OverlayType - -1];
            if ((MapF.FFieldFlags & 16384) != 0)
            {
                Points = MapF.CanMoveOn(this, Points, MoveOverCode);
            }
        }
        else
        {
            Points = -1;
        }

        return Points;
    }

    // MoveT.PAS:1504
    public void Deactivate()
    {
        if (FStatus.Contains(StatusElement.Activated))
        {
            FStatus = FStatus - Set<StatusElement>.Of(StatusElement.Activated);
            if (FSettings.FStatus == MoveControlSettings.Status.Processing)
            {
                FSettings.FMovePointsLimit = FMovePointsLimit;
            }

            FSettings.Release();
            FSettings = null;
        }
    }

    // MoveT.PAS:1559
    public int CanMove(MoveControlSettings Settings, Set<CanMoveModeElement> Mode)
    {
        int Points;
        if (Settings.FSourceX != Settings.FDestinationX || Settings.FSourceY != Settings.FDestinationY || Settings.FSourceL != Settings.FDestinationL)
        {
            Settings.ResetState();
            if (Activate(Settings))
            {
                Points = -1;
                if (FDestinationX == -1 || ValidLocation(FDestinationX, FDestinationY, FDestinationL))
                {
                    if (FSourceL == FDestinationL && Mode.Contains(CanMoveModeElement.Fast))
                    {
                        Points = GetMovePointsCostFast(FSourceX, FSourceY, FDestinationX, FDestinationY);
                    }

                    if (Points == -1 && Mode.Contains(CanMoveModeElement.Exact))
                    {
                        do
                        {
                        }
                        while (!!Process());
                        Points = FSettings.GetMovePoints(FDestinationX, FDestinationY, FDestinationL);
                        if (Points == 16777215)
                        {
                            Points = -1;
                        }
                    }
                }

                Deactivate();
            }
            else
            {
                Points = -1;
            }
        }
        else
        {
            Points = 0;
        }

        return Points;
    }

    // MoveT.PAS:1589
    public bool ValidPath(MoveControlSettings Settings, MovePath Path)
    {
        bool Valid;
        int I;
        int Points;
        Valid = true;
        I = Path.Count - 2;
        Points = 0;
        Settings.FMovePointsMax = Path.PosToMovePoints(0);
        ProcessMove(Settings);
        while (I > 0 && Valid)
        {
            Points += Path[I].Flag;
            var item = Path[I];
            var pathItem = Path[I];
            Valid = Points == Settings.GetMovePoints(Path[I].X, pathItem.Y, item.L);
            I -= 1;
        }

        return Valid;
    }

    // MoveT.PAS:1608
    public void CalculateGaps(int SourceX, int SourceY, int DestinationX, int DestinationY)
    {
        int X;
        int Y;
        int Direction;
        int OverCode;
        AoWMapField Field;
        bool Clear;
        int Points = default;
        Clear = true;
        FGapStarts.Count = 0;
        FGapEnds.Count = 0;
        FDirectRoute.Count = 0;
        OverCode = 1;
        FDirectRoute.AddXYL((sbyte)SourceX, (sbyte)SourceY, 0);
        X = 0;
        Y = 0;
        Direction = 0;
        while (SourceX != DestinationX || SourceY != DestinationY)
        {
            if (SourceX == DestinationX)
            {
                if (SourceY > DestinationY)
                {
                    Direction = 1;
                    X = SourceX;
                    Y = SourceY - 1;
                }
                else
                {
                    if (SourceY < DestinationY)
                    {
                        Direction = 4;
                        X = SourceX;
                        Y = SourceY + 1;
                    }
                }
            }
            else
            {
                if ((SourceX & 1) == 0)
                {
                    if (SourceX > DestinationX)
                    {
                        if (SourceY > DestinationY)
                        {
                            Direction = 6;
                            X = SourceX - 1;
                            Y = SourceY - 1;
                        }
                        else
                        {
                            Direction = 5;
                            X = SourceX - 1;
                            Y = SourceY;
                        }
                    }
                    else
                    {
                        if (SourceY > DestinationY)
                        {
                            Direction = 2;
                            X = SourceX + 1;
                            Y = SourceY - 1;
                        }
                        else
                        {
                            Direction = 3;
                            X = SourceX + 1;
                            Y = SourceY;
                        }
                    }
                }
                else
                {
                    if (SourceX > DestinationX)
                    {
                        if (SourceY < DestinationY)
                        {
                            Direction = 5;
                            X = SourceX - 1;
                            Y = SourceY + 1;
                        }
                        else
                        {
                            Direction = 6;
                            X = SourceX - 1;
                            Y = SourceY;
                        }
                    }
                    else
                    {
                        if (SourceY < DestinationY)
                        {
                            Direction = 3;
                            X = SourceX + 1;
                            Y = SourceY + 1;
                        }
                        else
                        {
                            Direction = 2;
                            X = SourceX + 1;
                            Y = SourceY;
                        }
                    }
                }
            }

            Field = (AoWMapField)FMapLevel.GetField(X, Y);
            Points = FOnMovePoint(Field, OverCode);
            if (Points >= 0)
            {
                if ((Field.FFieldFlags & 8192) != 0 && (X != DestinationX || Y != DestinationY))
                {
                    OverCode = Field.CanMoveOver(this);
                    if (OverCode == 0)
                    {
                        Points = -1;
                        OverCode = 1;
                    }
                }
                else
                {
                    OverCode = 1;
                }
            }

            if (Points < 0)
            {
                if (Clear)
                {
                    FGapStarts.AddXYL((sbyte)SourceX, (sbyte)SourceY, (sbyte)Direction);
                    Clear = false;
                    OverCode = 1;
                }
            }
            else
            {
                if (!Clear)
                {
                    FGapEnds.AddXYL((sbyte)X, (sbyte)Y, -1);
                    Clear = true;
                }
            }

            SourceX = X;
            SourceY = Y;
            FDirectRoute.AddXYL((sbyte)SourceX, (sbyte)SourceY, (sbyte)Points);
        }

        if (!Clear)
        {
            FGapEnds.AddXYL((sbyte)DestinationX, (sbyte)DestinationY, -1);
        }
    }

    // MoveT.PAS:1815
    public int GetMovePointsCostFast(int SourceX, int SourceY, int DestinationX, int DestinationY)
    {
        int I;
        int Points;
        int GapIndex;
        int Cost;
        I = 1;
        GapIndex = 0;
        Points = 0;
        CalculateGaps(SourceX, SourceY, DestinationX, DestinationY);
        while (I < FDirectRoute.Count && Points != -1)
        {
            Cost = FDirectRoute[I].L;
            if (Cost < 0)
            {
                var item = FDirectRoute[I - 1];
                var fDirectRouteItem = FDirectRoute[I - 1];
                GapIndex = FGapStarts.IndexOfXY(fDirectRouteItem.X, item.Y);
                Cost = CalculateTurnAroundMovePoints(ref GapIndex);
                if (Cost != -1)
                {
                    Points += Cost;
                    var fGapEndsItem = FGapEnds[GapIndex];
                    var item2 = FGapEnds[GapIndex];
                    I = FDirectRoute.IndexOfXY(item2.X, fGapEndsItem.Y);
                }
                else
                {
                    Points = -1;
                }
            }
            else
            {
                Points += Cost;
            }

            I += 1;
        }

        return Points;
    }

    private static Int32Array7 P5OppositeDirections = Int32Array7.From([0, 4, 5, 6, 1, 2, 3]);
    private static Int32Array2By7 P5SideDirections = Int32Array2By7.From([Int32Array2.From([-1, -1]), Int32Array2.From([6, 2]), Int32Array2.From([1, 3]), Int32Array2.From([2, 4]), Int32Array2.From([3, 5]), Int32Array2.From([4, 6]), Int32Array2.From([5, 1])]);
    private static ByteArray3By6 P5OddDirections = ByteArray3By6.From([ByteArray3.From([1, 3, 5]), ByteArray3.From([3, 1, 5]), ByteArray3.From([3, 5, 1]), ByteArray3.From([5, 3, 1]), ByteArray3.From([5, 1, 3]), ByteArray3.From([1, 5, 3])]);
    private static ByteArray3By6 P5EvenDirections = ByteArray3By6.From([ByteArray3.From([2, 4, 6]), ByteArray3.From([4, 2, 6]), ByteArray3.From([4, 6, 2]), ByteArray3.From([6, 4, 2]), ByteArray3.From([6, 2, 4]), ByteArray3.From([2, 6, 4])]);

    public enum StatusElement : byte
    {
        Activated = 0
    }

    public delegate int MoveOnMovePointProc(AoWMapField MapF, int MoveOverCode);

    // intf_P5_01.inc:120

    // intf_P5_01.inc:120

}
