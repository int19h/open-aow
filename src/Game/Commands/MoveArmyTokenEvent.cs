// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.AI.Groups;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// mode, unit IDs, selection, army sprite, moving/target player, target, destination, animate
public partial class MoveArmyTokenEvent : MoveTokenEvent
{
    public new const int ClassID = 135424;

    public Set<ArmyMoveModeElement> FMode;
    public Int32List FUnitIDs;
    public byte FSelection;
    public ArmyHexSprite FArmyHS;
    public sbyte FPlayer;
    public sbyte FTargetPlayer;
    public sbyte FTargetX;
    public sbyte FTargetY;
    public sbyte FTargetL;
    public sbyte FDestinationX;
    public sbyte FDestinationY;
    public sbyte FDestinationL;
    public bool FAnimate;

    // MoveT.PAS:2066
    public override void Dispose()
    {
        FUnitIDs?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:2093
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(30, FUnitIDs.LoadFromStream, FUnitIDs.SaveToStream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 31);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FTargetX), 32);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FTargetY), 33);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FTargetL), 34);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FTargetPlayer), 35);
        RwSet(ref FMode, 1, 36, Stream.rwData, 0);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationX), 37);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationY), 38);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationL), 39);
    }

    // MoveT.PAS:2060
    public MoveArmyTokenEvent(ECustomNode owner) : base(owner)
    {
        FUnitIDs = new Int32List();
    }

    // MoveT.PAS:2137
    public override bool Validate()
    {
        int I;
        int UnitIndex;
        MoveControlSettings Settings;
        sbyte TargetPlayer;
        I = FPath.Count - 1;
        bool Result = true;
        if (I < 1)
        {
            Result = false;
        }
        else
        {
            FArmyHS = (ArmyHexSprite)GetMapF(FPath[I]).FindOwnedChild(131607);
            if (FArmyHS == null)
            {
                Result = false;
            }
            else
            {
                I = FUnitIDs.Count;
                FSelection = 0;
                while (I != 0 && Result)
                {
                    I -= 1;
                    var item = FUnitIDs[I];
                    UnitIndex = FArmyHS.FArmy.IndexOfID(item);
                    if (UnitIndex != -1)
                    {
                        FSelection = (byte)(FSelection | (1 << UnitIndex));
                    }

                    var fUnitIDsItem = FUnitIDs[I];
                    Result = FArmyHS.FArmy.IndexOfID(fUnitIDsItem) != -1;
                }

                Settings = new MoveControlSettings(null);
                Settings.SetupArmy(FArmyHS, FSelection);
                Settings.FMode = Settings.FMode.With(MoveModeElement.AI);
                if (AoWHSMap.Current.FPlayers.GetPlayers(FArmyHS.FArmy.FPlayer).FPlayerType == PlayerType.Human && AoWHSMap.Current.FExplorationEnabled)
                {
                    Settings.FMode = Settings.FMode.With(MoveModeElement.ExploredTerrainOnly);
                }

                Result = Result && AoWHSMap.Current.FMoveControl.ValidPath(Settings, FPath);
                Result = Result && FArmyHS.FArmy.FPlayer == FPlayer;
                TargetPlayer = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(FTargetX, FTargetY, FTargetL)).GetPlayer();
                Result = Result && (TargetPlayer == FTargetPlayer || TargetPlayer == FArmyHS.FArmy.FPlayer);
                if (FMode.Contains(ArmyMoveModeElement.BuildRoads))
                {
                    bool condition = Result;
                    if (condition)
                    {
                        var movePoints = FArmyHS.FArmy.MovePoints(FSelection);
                        condition = AbilityRules.RoadConstructionCost(FPlayer, FPath, movePoints) <= AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FGems;
                    }

                    Result = condition;
                }

                Settings.Release();
            }
        }

        if (!Result && AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType != PlayerType.Human && AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FAIControl.FMoveExecuter.FStatus == AIMoveExecutor.Status.Busy)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FAIControl.FMoveExecuter.InvalidateMove(FArmyHS, FSelection);
        }

        return Result;
    }

    // MoveT.PAS:2183
    public override void InitializeMove()
    {
        int Last;
        ArmyHexSprite ArmyHS;
        base.InitializeMove();
        FAnimate = false;
        Last = FPath.Count - 1;
        FSelection = FArmyHS.FArmy.ValidateSelection(FSelection);
        var validateSelection = FArmyHS.FArmy.ValidateSelection(255);
        if (FSelection != validateSelection)
        {
            ArmyHS = new ArmyHexSprite(null);
            ArmyHS.FArmy.SetPlayer(FArmyHS.FArmy.FPlayer);
            ArmyHS.FArmy.AddUnitSelection(FArmyHS.FArmy, FSelection);
            var item = FPath[Last];
            var fPathItem = FPath[Last];
            var item2 = FPath[Last];
            ArmyHS.PlacePrivate(AoWHSMap.Current.MapContainer, item2.X, fPathItem.Y, item.L);
            FArmyHS.FArmy.ResetDestinationPos();
            if (FArmyHS.GetSelected())
            {
                AoWHSMap.Current.Selection.UnselectLocation();
                AoWHSMap.Current.Selection.SelectLocationHS(ArmyHS);
                AoWHSMap.Current.FSelectedArmy.DeactivateMoveSelection();
            }

            FArmyHS = ArmyHS;
            FSelection = 255;
        }
        else
        {
            if (FArmyHS.GetSelected())
            {
                AoWHSMap.Current.FSelectedArmy.DeactivateMoveSelection();
            }
        }

        var army = FArmyHS.FArmy;
        army.FMode = army.FMode.With(ArmyModeElement.Moving);
        army.FDestinationX = this.FDestinationX;
        army.FDestinationY = this.FDestinationY;
        army.FDestinationL = this.FDestinationL;
    }

    // MoveT.PAS:2221
    public override void FinishMove()
    {
        bool Selected;
        MapField Field;
        ArmyHexSprite ArmyHS;
        base.FinishMove();
        if (FArmyHS != null)
        {
            Selected = FArmyHS.GetSelected();
            Field = (MapField)FArmyHS.Owner;
            FArmyHS.SetOwner(null);
            ArmyHS = GetNoneMovingArmyHS(Field);
            FArmyHS.SetOwner(Field);
            if (ArmyHS != null)
            {
                FArmyHS.FArmy.FUserMode = FArmyHS.FArmy.FUserMode & ArmyHS.FArmy.FUserMode;
                ArmyHS.FArmy.AssignSettings(FArmyHS.FArmy);
                ArmyHS.FArmy.AddUnitSelection(FArmyHS.FArmy, 255);
                FArmyHS = ArmyHS;
                if (Selected)
                {
                    AoWHSMap.Current.FSelectedArmy.Select(ArmyHS);
                    AoWHSMap.Current.FSelectedArmy.DeactivateMoveSelection();
                }
            }

            FArmyHS.FArmy.FMode = FArmyHS.FArmy.FMode - Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
            FArmyHS.CancelMove();
            if (Selected)
            {
                AoWHSMap.Current.FSelectedArmy.ActivateMoveSelection();
                AoWHSMap.Current.FSelectedArmy.Update();
                if (FMode.Contains(ArmyMoveModeElement.SelectEndLocation))
                {
                    var lhx = FArmyHS.GetLhx();
                    var yhx = FArmyHS.GetYhx();
                    var xhx = FArmyHS.GetXhx();
                    AoWHSMap.Current.Selection.SelectLocationHX(xhx, yhx, lhx);
                }
            }
        }
    }

    // MoveT.PAS:2368
    public override MoveStatus DefaultMove()
    {
        MoveStatus Result;
        if (!FArmyHS.Moving())
        {
            if (CanMoveNext())
            {
                SetupNextMove();
                if (!Animating())
                {
                    Result = ExecuteMove();
                }
                else
                {
                    Result = MoveStatus.Moving;
                }
            }
            else
            {
                Result = MoveStatus.Failed;
            }
        }
        else
        {
            if (Animating())
            {
                UpdateAnimation();
                bool condition = FArmyHS.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer;
                if (!condition)
                {
                    var lhx = FArmyHS.GetLhx();
                    var yhx = FArmyHS.GetYhx();
                    var xhx = FArmyHS.GetXhx();
                    condition = AoWHSMap.Current.Watching(xhx, yhx, lhx, 0);
                }

                if (condition)
                {
                    FArmyHS.PlayMoveSFX();
                }

                if (!Animating())
                {
                    Result = ExecuteMove();
                }
                else
                {
                    Result = MoveStatus.Moving;
                }
            }
            else
            {
                Result = MoveStatus.Failed;
            }
        }

        return Result;
    }

    // MoveT.PAS:2304
    public virtual bool Animating() => FArmyHS.GetMoveStatus() < 100 && FArmyHS.GetMoveStatus() != -1;

    // MoveT.PAS:2309
    public virtual void UpdateAnimation()
    {
        FArmyHS.MoveNext();
    }

    // MoveT.PAS:2342
    public virtual void SetupNextMove()
    {
        int Direction;
        bool Instant;
        AoWMapField Field;
        if ((MoveVisible(FMoveIndex) || FAnimate) && (FPath[FMoveIndex].L == AoWHSMap.Current.ViewL || AoWHSMap.Current.FShowIndependentMoves))
        {
            FAnimate = true;
            Direction = GetDirection(FMoveIndex);
            if (Direction == 0)
            {
                Direction = GetHT(FMoveIndex);
                Instant = true;
            }
            else
            {
                Instant = false;
            }

            FArmyHS.MoveFirst(Direction, Instant);
            if (ObserveMove(FMoveIndex - 1))
            {
                Field = GetMapF(FPath[FMoveIndex - 1]);
                AoWHSMap.Current.ValidateObservation(FArmyHS.FArmy.FPlayer, Field.Xhx, Field.Yhx, Field.Lhx, 0);
            }
        }
    }

    // MoveT.PAS:2312
    public virtual bool CanMoveNext()
    {
        bool Stop;
        int I;
        AoWMapField Field;
        ArmyHexSprite ArmyHS;
        bool Result = false;
        Stop = false;
        I = FMoveIndex;
        while (I != 0 && !(Stop || Result))
        {
            I -= 1;
            Field = GetMapF(FPath[I]);
            Result = Field.GetPlayer() == -1;
            if (!Result)
            {
                ArmyHS = GetNoneMovingArmyHS(Field);
                if (ArmyHS != null)
                {
                    if (ArmyHS.FArmy.FPlayer == FArmyHS.FArmy.FPlayer)
                    {
                        Result = FArmyHS.FArmy.CanCombine(ArmyHS.FArmy, 255);
                    }
                    else
                    {
                        // { Original queries the moving player toward the other army. }
                        if (AoWHSMap.Current.FPlayers.GetDiplomaticRelation(FArmyHS.FArmy.FPlayer, ArmyHS.FArmy.FPlayer) != DiplomaticRelation.Alliance)
                        {
                            Stop = true;
                        }
                    }
                }
                else
                {
                    Result = Field.GetPlayer() == FArmyHS.FArmy.FPlayer;
                }
            }
        }

        return Result;
    }

    // MoveT.PAS:2279
    public virtual MoveStatus ExecuteMove()
    {
        MoveStatus Result;
        AoWMapField Field;
        int Direction;
        if (FMode.Contains(ArmyMoveModeElement.BuildRoads))
        {
            var mapF = GetMapF(FPath[FMoveIndex]);
            ConstructRoad(FArmyHS.FArmy.FPlayer, mapF);
        }

        FMoveIndex -= 1;
        Field = GetMapF(FPath[FMoveIndex]);
        var item = FPath[FMoveIndex];
        FArmyHS.MoveTo(Field, item.Flag);
        if (Field.FindChildIndex(FArmyHS) != -1)
        {
            Direction = GetDirection(FMoveIndex + 1);
            if (Direction == 0)
            {
                Direction = GetHT(FMoveIndex + 1);
            }

            if (Direction > 0 && Direction <= 6)
            {
                FArmyHS.SetDirection((byte)Direction);
            }
            else
            {
                FArmyHS.SetDirection(0);
            }

            if (FMode.Contains(ArmyMoveModeElement.BuildRoads))
            {
                ConstructRoad(FArmyHS.FArmy.FPlayer, Field);
            }

            Result = ProcessMove();
        }
        else
        {
            FArmyHS = null;
            Result = MoveStatus.Done;
        }

        return Result;
    }

    // MoveT.PAS:2108
    public virtual bool Setup(ArmyHexSprite ArmyHS, byte Selection, MovePath Path, Set<ArmyMoveModeElement> Mode)
    {
        bool Result;
        int First;
        int Last;
        FPath.Clear();
        First = Path.MovePointsToPos(ArmyHS.FArmy.MovePoints(Selection));
        Last = Path.Count - 1;
        if (Last > First)
        {
            for (int I = First; I <= Last; I++)
            {
                var item = Path[I];
                FPath.Add(item);
            }

            FArmyHS = ArmyHS;
            FPlayer = ArmyHS.FArmy.FPlayer;
            FMode = Mode;
            FTargetPlayer = GetMapF(Path[0]).GetPlayer();
            FTargetX = Path[0].X;
            FTargetY = Path[0].Y;
            FTargetL = Path[0].L;
            FSelection = ArmyHS.FArmy.ValidateSelection(Selection);
            for (int I = 0, last = ArmyHS.FArmy.Count - 1; I <= last; I++)
            {
                if (((1 << I) & Selection) != 0)
                {
                    var unit = ArmyHS.FArmy.GetUnit(I);
                    FUnitIDs.Add(unit.FUnitID);
                }
            }

            FDestinationX = FArmyHS.FArmy.FDestinationX;
            FDestinationY = FArmyHS.FArmy.FDestinationY;
            FDestinationL = FArmyHS.FArmy.FDestinationL;
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // MoveT.PAS:2259
    public bool ObserveMove(int Index)
    {
        bool Visible;
        MultiMoveTokenEvent Event;
        Visible = MoveVisible(Index);
        bool Result = Visible && AoWHSMap.Current.FShowIndependentMoves && (AoWHSMap.Current.FSeatedPlayer != FArmyHS.FArmy.FPlayer || AoWHSMap.Current.FPlayers.GetPlayers((sbyte)(byte)AoWHSMap.Current.FSeatedPlayer).FPlayerType != PlayerType.Human);
        if (Result && AoWHSMap.Current.FTokenManager.TokenExecuter.GetExecutingTokenEvent() is MultiMoveTokenEvent && ((MultiMoveTokenEvent)AoWHSMap.Current.FTokenManager.TokenExecuter.GetExecutingTokenEvent()).FSimultaneous)
        {
            Event = (MultiMoveTokenEvent)AoWHSMap.Current.FTokenManager.TokenExecuter.GetExecutingTokenEvent();
            Visible = this == Event.FEvents[0];
            Result = Visible;
        }

        return Result;
    }

    // MoveT.PAS:2269
    public bool MoveVisible(int Index)
    {
        bool Result;
        if (AoWHSMap.Current.FSeatedPlayer != -1)
        {
            var item = FPath[Index];
            var fPathItem = FPath[Index];
            var item2 = FPath[Index];
            Result = AoWHSMap.Current.Visible(item2.X, fPathItem.Y, item.L, 0);
            if (Result && FArmyHS.FArmy.FStatus.Contains(ArmyStatusElement.FullyConcealed))
            {
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // intf_P5_07.inc:816
    public ArmyHexSprite GetNoneMovingArmyHS(MapField field)
    {
        for (int i = field.Count - 1; i >= 0; i--)
        {
            var sprite = field.HSList[i];
            if (sprite.EClass.ClassID == 0x20217 && sprite.Owner == field)
            {
                var army = (ArmyHexSprite)sprite;
                if (!army.FArmy.FMode.Contains(ArmyModeElement.Moving)) return army;
            }
        }
        return null;
    }

    private static bool ConstructRoad(sbyte Player, AoWMapField Field)
    {
        return false;
    }
}
