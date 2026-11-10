// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Ai;

using Aow.Game.Combat;

using Aow.Game.Commands;

using Aow.Game.Draw.Animations;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// FArmy. FUnitIndex. FUnitYOffset. FMoveSteps
public partial class ArmyHS : UnitHS
{

    public new const int ClassID = 131607;

    public Army FArmy;
    public byte FUnitIndex;
    public sbyte FUnitYOffset;
    public sbyte FMoveSteps;

    // Army.PAS:2654
    public override void Dispose()
    {
        FArmy.FOnUpdate = null;
        base.Dispose();
        FArmy?.Dispose();
        FArmy = null;
    }

    // Army.PAS:3054
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FArmy, 10);
    }

    // Army.PAS:2648
    public ArmyHS(ECustomNode owner) : base(owner)
    {
        FArmy = new Army(null);
    }

    // Army.PAS:2684
    public override void Clear()
    {
        base.Clear();
        FArmy.Clear();
    }

    // Army.PAS:3414
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if ((Msg.MsgID & 1073741824) == 0 && FArmy != null)
        {
            FArmy.MsgProc(ref Msg, Sender);
        }

        if (Msg.MsgID == 537001989)
        {
            UpdateFlagID();
        }
    }

    // Army.PAS:2740
    public override bool GetSelected() => FArmy.FStatus.Contains(ArmyStatusElement.Selected);

    // Army.PAS:3060
    public override bool CanChangeTerrain(in HSSenderInfo Info)
    {
        bool Result;
        if (Info.HN == 0)
        {
            Result = Info.MapField.TerrainType == 0 && Info.Sender != null && (((EObject)Info.Sender).EClass.ClassID == 135742 || ((EObject)Info.Sender).EClass.ClassID == 131436) || FArmy.ValidTerrain(Info.MapField.TerrainType, Info.MapField.OverlayType);
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Army.PAS:3069
    public override void TerrainChanged(in HSTerrainInfo Info)
    {
        base.TerrainChanged(Info);
        if (Info.HN == 0 && FArmy.FStatus.Contains(ArmyStatusElement.Activated))
        {
            FArmy.BeginUpdate();
            FArmy.ValidateTerrain(Info.TerrainType, Info.OverlayType);
            FArmy.EndUpdate();
        }
    }

    // Army.PAS:2690
    public override bool PlaceOnMap()
    {
        MapFieldMsg Msg = default;
        bool Result = base.PlaceOnMap();
        if (Result)
        {
            FArmy.BeginUpdate();
            var mapField = (MapField)Owner;
            FArmy.SetXYL(mapField.Xhx, mapField.Yhx, mapField.Lhx);

            FArmy.UpdateUnfog();
            FArmy.UpdateExploration();
            Msg.MsgID = 4354;
            Msg.Param[0] = this;
            ((MapField)Owner).SendMapFieldMsg(ref Msg);
            FArmy.EndUpdate();
        }

        return Result;
    }

    // Army.PAS:2707
    public override void RemoveFromMap()
    {
        MapField Field;
        MapFieldMsg Msg = default;
        Field = (MapField)Owner;
        if (Owner != null)
        {
            FArmy.TurnUnFogOff();
        }

        base.RemoveFromMap();
        if (Field != null && AoWHSMap.Current != null && !AoWHSMap.Current.Status.Contains(HSMapStatusElement.Destroying))
        {
            Msg.MsgID = 4355;
            Msg.Param[0] = this;
            Field.SendMapFieldMsg(ref Msg);
        }
    }

    // Army.PAS:3421
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.HN == 0)
        {
            if (Msg.MsgID == 4364)
            {
                if (!FArmy.FStatus.Contains(ArmyStatusElement.FullyConcealed) && (!AoWHSMap.Current.FFogEnabled || Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((AoWMapField)Owner).FFieldFlags).Hi).Lo != 0))
                {
                    if (FArmy.FPlayer != 0)
                    {
                        var players = AoWHSMap.Current.FPlayers.GetPlayers(FArmy.FPlayer);
                        Msg.Param[0] = Msg.Param[0].Int | (players.FPlayerRace + 1);
                    }
                    else
                    {
                        Msg.Param[0] = Msg.Param[0].Int | 13;
                    }
                }
            }
            else
            {
                if (Msg.MsgID == 4608)
                {
                    FArmy.UpdateAITarget(Msg.Param[0].As<AITargetQuery>().Info, ref Msg.Param[0].As<AITargetQuery>().Data);
                    if (Msg.Param[0].As<AITargetQuery>().Data.Priority < 0)
                    {
                        Msg.Result = 1;
                    }
                }
                else
                {
                    if (Msg.MsgID == 4360)
                    {
                        FArmy.ReturnBattleFieldInfo(ref Msg.Param[0].As<BattleFieldQuery>().Info);
                    }
                    else
                    {
                        if (Msg.MsgID == 4353)
                        {
                            var moveAction = FArmy.GetMoveAction((sbyte)Msg.Param[0].Int);
                            Msg.Param[1] = (int)moveAction;
                        }
                        else
                        {
                            if (Msg.MsgID == 4352)
                            {
                                Msg.Result = FArmy.FPlayer;
                            }
                            else
                            {
                                if (Msg.MsgID == 4389)
                                {
                                    if (FArmy.FPlayer == 0)
                                    {
                                        Msg.Result = FArmy.FIndependentRelation;
                                    }
                                }
                                else
                                {
                                    if (Msg.MsgID == 4400)
                                    {
                                        if (Msg.Param[1].Int == 1)
                                        {
                                            Msg.Result = FArmy.FPlayer;
                                        }
                                        else
                                        {
                                            if (FArmy.FConcealedUnits != AbilityRules.ArmySelectionMasks[FArmy.FChildren.Count])
                                            {
                                                Msg.Result = FArmy.FPlayer;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Msg.MsgID == 4362)
                                        {
                                            if (FArmy.FPlayer == Msg.Param[1].Int || AoWHSMap.Current.FPlayers.GetPlayers(FArmy.FPlayer).FDiplomaticRelations.GetDiplomaticRelation((sbyte)Msg.Param[1].Int) == DiplomaticRelation.Alliance)
                                            {
                                                var visibilityRange = FArmy.VisibilityRange();
                                                if (Msg.Param[0].Int <= visibilityRange)
                                                {
                                                    Msg.Result = 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Msg.MsgID == 4363)
                                            {
                                                if (FArmy.FPlayer == Msg.Param[1].Int || AoWHSMap.Current.FPlayers.GetPlayers(FArmy.FPlayer).FDiplomaticRelations.GetDiplomaticRelation((sbyte)Msg.Param[1].Int) == DiplomaticRelation.Alliance)
                                                {
                                                    var trueVisionRange = FArmy.TrueVisionRange();
                                                    if (Msg.Param[0].Int <= trueVisionRange)
                                                    {
                                                        Msg.Result = 1;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Msg.MsgID == 4365)
                                                {
                                                    if (Msg.Param[0].Int <= 8)
                                                    {
                                                        FArmy.UpdateConcealment();
                                                    }

                                                    bool condition = FArmy.FPlayer > 0;
                                                    if (condition)
                                                    {
                                                        var visibilityRange = FArmy.VisibilityRange();
                                                        condition = Msg.Param[0].Int <= visibilityRange;
                                                    }

                                                    if (condition)
                                                    {
                                                        var trueVisionRange = FArmy.TrueVisionRange();
                                                        AoWHSMap.Current.UpdatePlayerSeen(FArmy.FPlayer, Msg.MapField.Xhx, Msg.MapField.Yhx, Msg.MapField.Lhx, Msg.Param[0].Int, trueVisionRange);
                                                    }
                                                }
                                                else
                                                {
                                                    if (Msg.MsgID == 4366)
                                                    {
                                                        if (FArmy.IncommingNegotiateRequest(Msg.Param[0].As<NegotiateRequestEventLog>()))
                                                        {
                                                            Msg.Param[1] = 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Msg.MsgID == 4367)
                                                        {
                                                            if (FArmy.IncommingNegotiateOffer(Msg.Param[0].As<NegotiateTE>()))
                                                            {
                                                                Msg.Result = 1;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (Msg.MsgID == 4432 && !((AoWMapField)Msg.MapField).GetMagicShieldActive())
                                                            {
                                                                FArmy.IncommingStorm((StormType)Msg.Param[0].Int, Msg.Param[1].Int, (sbyte)Msg.Param[2].Int);
                                                                return;
                                                            }
                                                            else
                                                            {
                                                                if (Msg.MsgID == 4433 && !((AoWMapField)Msg.MapField).GetMagicShieldActive())
                                                                {
                                                                    FArmy.RetrieveStormDamageInfo((StormType)Msg.Param[0].Int, Msg.Param[1].Int, ref Msg.Param[2].As<StormDamageQuery>().Info);
                                                                    return;
                                                                }
                                                                else
                                                                {
                                                                    if (Msg.MsgID == 4357)
                                                                    {
                                                                        FArmy.TriggerFireDamage();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Army.PAS:3114
    public override void CanMoveOn(in HSMoveOnInfo Info, ref HSMoveOnResult MoveResult)
    {
        MoveControl Control;
        if (Info.HN == 0)
        {
            Control = (MoveControl)Info.Sender;
            if (FArmy.FPlayer == Control.FPlayer)
            {
                if (!(Control.FMode.Contains(MoveModeElement.Surround) && Info.MoveOverCode != 1 && ((MapField)Owner).Xhx == Control.FDestinationX && ((MapField)Owner).Yhx == Control.FDestinationY && ((MapField)Owner).Lhx == Control.FDestinationL && !FArmy.CanCombine(Control.FArmyHS.FArmy, (byte)Control.FSelection)))
                {
                    if (Control.FArmyHS != null && FArmy.IndexOfTransporter(255) != -1 && Control.FArmyHS.FArmy.Transporter((byte)Control.FSelection) == null)
                    {
                        MoveResult.Code = 4;
                        MoveResult.Done = true;
                    }
                }
                else
                {
                    MoveResult.Code = -1;
                    MoveResult.Done = true;
                }
            }
            else
            {
                if (Info.MoveOverCode != 1)
                {
                    MoveResult.Code = -1;
                    MoveResult.Done = true;
                }
            }
        }
    }

    // Army.PAS:3080
    public override int CanMoveOver(in HSSenderInfo Info)
    {
        int Result;
        if (Info.HN == 0)
        {
            if (!((MoveControl)Info.Sender).FMode.Contains(MoveModeElement.AI) && (GetFogEnabled() || FArmy.FStatus.Contains(ArmyStatusElement.FullyConcealed) || ((MoveControl)Info.Sender).FMode.Contains(MoveModeElement.IgnoreArmies)))
            {
                return 1;
            }

            if (((MoveControl)Info.Sender).FPlayer == FArmy.FPlayer)
            {
                if (!((MoveControl)Info.Sender).FMode.Contains(MoveModeElement.AvoidArmies) || this == ((MoveControl)Info.Sender).FTransporterArmy)
                {
                    if (FArmy.FPlayer != 0)
                    {
                        if (((MoveControl)Info.Sender).FMode.Contains(MoveModeElement.AI) && FArmy.IndexOfTransporter(255) != -1)
                        {
                            return 2;
                        }

                        if (((MoveControl)Info.Sender).FArmyHS != null && FArmy.CanCombine(((MoveControl)Info.Sender).FArmyHS.FArmy, (byte)((MoveControl)Info.Sender).FSelection))
                        {
                            Result = 1;
                        }
                        else
                        {
                            Result = 2;
                        }
                    }
                    else
                    {
                        Result = 2;
                    }
                }
                else
                {
                    Result = 0;
                }
            }
            else
            {
                if (AoWHSMap.Current.FPlayers.GetDiplomaticRelation(((MoveControl)Info.Sender).FPlayer, FArmy.FPlayer) == DiplomaticRelation.Alliance)
                {
                    if (Info.MapField.OverlayType != 7 || Info.MapField.FindNoneTransparentHS(typeof(PlayerStructure)) == null)
                    {
                        Result = 2;
                    }
                    else
                    {
                        Result = 0;
                    }
                }
                else
                {
                    Result = 0;
                }
            }
        }
        else
        {
            Result = 1;
        }

        return Result;
    }

    // Army.PAS:3030
    public override void Activate()
    {
        base.Activate();
        var lhx = GetLhx();
        var yhx = GetYhx();
        var xhx = GetXhx();
        FArmy.SetXYL(xhx, yhx, lhx);
        FArmy.Activate();
        FArmy.FOnUpdate = ArmyUpdated;
        if (this == ((AoWMapField)Owner).GetArmyHS())
        {
            if (FArmy.Count != 0)
            {
                UpdateUnitGFXResource();
            }
            else
            {
                if (!AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
                {
                    Dispose();
                    return;
                }
            }
        }
        else
        {
            Dispose();
        }
    }

    // Army.PAS:3048
    public override void Deactivate()
    {
        base.Deactivate();
        FArmy.Deactivate();
    }

    // Army.PAS:3135
    public override void UpdateMapField(in HSMapFieldInfo Info)
    {
        if (Info.HN == 0)
        {
            ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 8192 | 4096 | 16777216 | 16384 | 33554432 | 67108864 | 134217728;
        }
    }

    // Army.PAS:2734
    public override bool CanSelect(int Xhp, int Yhp)
    {
        bool Result;
        if (!GetFogEnabled() && AoWHSMap.Current.FSeatedPlayer != -1)
        {
            if (FArmy.FStatus.Contains(ArmyStatusElement.FullyConcealed) || !base.CanSelect(Xhp, Yhp))
            {
                Result = false;
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Army.PAS:2721
    public override void Select()
    {
        FArmy.Select();
        if (this != AoWHSMap.Current.FSelectedArmy.GetArmyHS())
        {
            AoWHSMap.Current.FSelectedArmy.SetArmyHS(this);
        }
    }

    // Army.PAS:2727
    public override void Unselect()
    {
        base.Unselect();
        FArmy.Unselect();
        if (this == AoWHSMap.Current.FSelectedArmy.GetArmyHS())
        {
            AoWHSMap.Current.FSelectedArmy.SetArmyHS(null);
        }
    }

    // Army.PAS:2679
    public override string EditName() => AbilityRules.TranslateRStr(P2ArmyRStr);

    // Army.PAS:3140
    public override bool MoveTo(MapField Field, int MoveCost)
    {
        bool Result;
        MapField OldField;
        MapFieldMsg Msg = default;
        FArmy.BeginUpdate();
        OldField = (MapField)Owner;
        if (OldField != null)
        {
            Msg.MsgID = 4416;
            Msg.Param[0] = this;
            Msg.Param[1] = Field;
            Msg.Result = 0;
            OldField.SendMapFieldMsg(ref Msg);
        }

        if (OldField == null || Msg.Result.Int == 0)
        {
            Msg.MsgID = 4417;
            Msg.Param[0] = this;
            Msg.Param[1] = OldField;
            Msg.Result = 0;
            Field.SendMapFieldMsg(ref Msg);
            if (Msg.Result.Int == 0)
            {
                base.MoveTo(Field, MoveCost);
                if (OldField != null && (Field.Lhx != OldField.Lhx || !ConnectingHX(OldField.Xhx, OldField.Yhx, Field.Xhx, Field.Yhx)))
                {
                    AoWHSMap.Current.NotifyPlayerChanged(OldField.Xhx, OldField.Yhx, OldField.Lhx);
                    AoWHSMap.Current.NotifyPlayerChanged(Field.Xhx, Field.Yhx, Field.Lhx);
                }
                else
                {
                    AoWHSMap.Current.NotifyPlayerChangedEx(Field.Xhx, Field.Yhx, Field.Lhx, 13);
                }

                FArmy.Validate();
                FArmy.MovedTo(OldField, Field, MoveCost);
                Msg.MsgID = 4418;
                Msg.Param[0] = this;
                Msg.Param[1] = OldField;
                Msg.Result = 0;
                Field.SendMapFieldMsg(ref Msg);
                if (FArmy.FPlayer > 0)
                {
                    var visibilityRange = FArmy.VisibilityRange();
                    var trueVisionRange = FArmy.TrueVisionRange();
                    AoWHSMap.Current.UpdatePlayerSeen(FArmy.FPlayer, Field.Xhx, Field.Yhx, Field.Lhx, visibilityRange, trueVisionRange);
                }

                Result = true;
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        FArmy.EndUpdate();
        return Result;
    }

    // Army.PAS:3187
    public override bool PlaceHX(MapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        bool Result;
        MapField Field;
        EObject Army;
        if (FArmy.CanPlace(Xhx, Yhx, Lhx))
        {
            Field = Map.GetField(Xhx, Yhx, Lhx);
            Army = Field.FindOwnedChild(131607);
            if (Army == null || GetEditMode())
            {
                Result = base.PlaceHX(Map, Xhx, Yhx, Lhx);
            }
            else
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

    // Army.PAS:2760
    public override void UpdateFlagID()
    {
        if (FArmy.FPlayer > 0)
        {
            FFlagID = (byte)AoWHSMap.Current.FPlayers.GetPlayers(FArmy.FPlayer).FPlayerRace;
            return;
        }

        FFlagID = 32;
    }

    // Army.PAS:3517
    public override sbyte MoveFirst(int Direction, bool Instant)
    {
        sbyte[] Result__cell = new sbyte[1];
        ref sbyte Result = ref Result__cell[0];
        MapLevel Level;
        int X = default;
        int Y = default;
        int DX = default;
        int DY = default;
        ImageSequenceAnimation Animation;
        Result = base.MoveFirst(Direction, Instant);
        switch (GameTables.AoWEngine.FRegistry.GetMovementSpeed())
        {
            case AoWRegistry.MovementSpeed.Normal:
                FMoveSteps = (sbyte)base.GetMoveSteps();
                break;
            case AoWRegistry.MovementSpeed.Fast:
                FMoveSteps = (sbyte)base.GetMoveSteps();
                if (FMoveSteps > 8)
                {
                    FMoveSteps = (sbyte)(FMoveSteps - 4);
                }
                else
                {
                    FMoveSteps = 4;
                }

                break;
            case AoWRegistry.MovementSpeed.Turbo:
                FMoveSteps = 2;
                break;
        }

        if (AoWHSMap.Current.FFPS > 0)
        {
            if (AoWHSMap.Current.FFPS < 15)
            {
                FMoveSteps = (sbyte)(FMoveSteps / 2);
            }
            else
            {
                if (AoWHSMap.Current.FFPS < 25)
                {
                    FMoveSteps = (sbyte)(FMoveSteps - 2);
                }
            }

            if (FMoveSteps < 2)
            {
                FMoveSteps = 2;
            }
        }

        if (GetLhx() > 0)
        {
            Level = (MapLevel)Owner.Owner;
            var yhx = GetYhx();
            HexGeometry.CenterHNtoHX(GetXhx(), yhx, Direction, ref X, ref Y);
            if (X >= 0 && Y >= 0 && X < Level.Width && Y < Level.Height)
            {
                if (Level.GetField(X, Y).TerrainType == 7)
                {
                    if (AoWHSSet.Current.FAbilityControl.GetAbility(42).FImageSequences.GetImageSequence(0) != null)
                    {
                        Animation = new ImageSequenceAnimation(null);
                        Animation.SetImageSequence(AoWHSSet.Current.FAbilityControl.GetAbility(42).FImageSequences.GetImageSequence(0));
                        Animation.FAnimationLevel = 27;
                        HexGeometry.HNtoHP(Direction, ref DX, ref DY);
                        Animation.SetXhpOffset(DX / 2);
                        Animation.SetYhpOffset(DY / 2);
                        var lhx = GetLhx();
                        var yhx2 = GetYhx();
                        var xhx = GetXhx();
                        AoWHSMap.Current.PlaceHX(Animation, xhx, yhx2, lhx);
                    }
                }
            }
        }

        return Result;
    }

    // Army.PAS:3555
    public override int GetMoveSteps()
    {
        if (Moving())
        {
            return FMoveSteps;
        }

        return base.GetMoveSteps();
    }

    // Army.PAS:2667
    public bool GetFogEnabled() => AoWHSMap.Current.FFogEnabled && Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((AoWMapField)Owner).FFieldFlags).Hi).Lo == 0;

    // Army.PAS:2673
    public bool VisibleForSeatedPlayer() => (!AoWHSMap.Current.FFogEnabled || Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((AoWMapField)Owner).FFieldFlags).Hi).Lo != 0) && !FArmy.GetFullyConcealed();

    // Army.PAS:2745
    public void ArmyUpdated(object Sender)
    {
        MapFieldMsg Msg = default;
        if (FArmy.Count != 0)
        {
            Msg.MsgID = 4388;
            Msg.Param[0] = this;
            ((MapField)Owner).SendMapFieldMsg(ref Msg);
            UpdateUnitGFXResource();
            if (FArmy.FPlayer != 0)
            {
                FFlagID = (byte)AoWHSMap.Current.FPlayers.GetPlayers(FArmy.FPlayer).FPlayerRace;
            }
            else
            {
                FFlagID = 32;
            }
        }
        else
        {
            if (Sender == FArmy)
            {
                Dispose();
            }
        }
    }

    // Army.PAS:2770
    public void UpdateUnitGFXResource()
    {
        int I;
        if (FArmy.FStatus.Contains(ArmyStatusElement.Concealed))
        {
            I = 0;
            while ((((int)(((uint)FArmy.FConcealedUnits) >> I)) & 1) == 1)
            {
                I += 1;
            }

            if (I >= FArmy.Count)
            {
                I = 0;
            }
        }
        else
        {
            I = 0;
        }

        FUnitIndex = (byte)I;
        SetUnitGFXResourceIndex(FArmy.GetUnit(I).GetUnitGFXResourceIndex());
        if (FArmy.GetUnit(I).GetMoveTypes().Contains(MoveType.Flying))
        {
            FUnitYOffset = -8;
        }
        else
        {
            FUnitYOffset = 0;
        }

        Assert(FUnitGFXResource != null, "", "Army.PAS", 3027);
    }

    // Army.PAS:3200
    public bool PlacePrivate(MapContainer Map, sbyte X, sbyte Y, sbyte L)
    {
        bool Result = base.PlaceHX(Map, X, Y, L);
        if (Result)
        {
            Activate();
        }

        return Result;
    }

    // Army.PAS:3252
    public void ShowEnchantments(int X, int Y, in SurfaceDesc SurfaceDesc)
    {
        P2UnitShowInfo Info = default;
        if (FArmy.Count > FUnitIndex)
        {
            Info.Overlays = Set<UnitOverlayModesElement>.Of();
            Info.Animation = UnitAnimation.None;
            Info.Frame = AoWHSMap.Current.Frame + (int)(((uint)(X + Y + Y)) >> 4);
            Info.Looped = true;
            FArmy.GetUnit(FUnitIndex).ShowEx(X - 16, Y - 20, SurfaceDesc, Info);
        }
    }

    // Army.PAS:3490
    public bool PlaceEx(sbyte X, sbyte Y, sbyte L, Set<ArmyPlaceModeElement> Mode)
    {
        MapLevel Level;
        AoWMapField Field;
        MapFieldMsg Msg = default;
        HNWalkData Walk = default;
        int Player;
        bool Result = false;
        Level = AoWHSMap.Current.MapContainer.GetMapLevel(L);
        HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
        while (!Result && (Walk.Radius == 0 || Mode.Contains(ArmyPlaceModeElement.ScanArea)) && Walk.Radius < 30)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Field = (AoWMapField)Level.GetField(Walk.X, Walk.Y);
                if (Field.FindOwnedChild(131607) == null)
                {
                    Player = Field.GetPlayer();
                    if (Player == -1 || (Player == FArmy.FPlayer && Mode.Contains(ArmyPlaceModeElement.EnableOwnPlayerLocation)))
                    {
                        Msg.MsgID = 4384;
                        Msg.Param[0] = this;
                        Msg.Result = 0;
                        Field.SendMapFieldMsg(ref Msg);
                        if (Msg.Result.Int == 0)
                        {
                            if (FArmy.CanPlace((sbyte)Walk.X, (sbyte)Walk.Y, L))
                            {
                                MoveTo(Field, 0);
                                Result = true;
                            }
                        }
                    }
                }
            }

            HNWalkData.WalkToNextHN(ref Walk);
        }

        return Result;
    }

    private static readonly string P2ArmyRStr = "Army";

    private static bool ConnectingHX(int X1, int Y1, int X2, int Y2)
    {
        bool Result;
        if (X1 == X2)
        {
            Result = Y1 == Y2 + 1 || Y1 == Y2 - 1;
        }
        else
        {
            if (X1 == X2 - 1 || X1 == X2 + 1)
            {
                if (!((X1 & 1) != 0))
                {
                    Result = Y1 == Y2 || Y1 == Y2 + 1;
                }
                else
                {
                    Result = Y1 == Y2 || Y1 == Y2 - 1;
                }
            }
            else
            {
                Result = false;
            }
        }

        return Result;
    }

    internal static MoveArmyTE CreateArmyMoveTE(ArmyHS Army, byte Selection, Movepath Path, MoveAction Action, Set<ArmyMoveModeElement> Mode)
    {
        MoveArmyTE Move;
        MoveArmyTE Result = null;
        switch (Action)
        {
            case MoveAction.Normal:
                Move = new ArmyDefaultMoveTE(null);
                break;
            case MoveAction.Combine:
                Move = new ArmyCombineMoveTE(null);
                break;
            case MoveAction.Attack:
                Move = new ArmyCombatMoveTE(null);
                break;
            case MoveAction.Negotiate:
                Move = new ArmyNegotiateMoveTE(null);
                break;
            default:
                Move = null;
                break;
        }

        if (Move != null)
        {
            if (!Move.Setup(Army, Selection, Path, Mode))
            {
                Result = null;
                Move.Release();
            }
            else
            {
                Result = Move;
            }
        }

        return Result;
    }

    internal static bool MoveArmyEx(ArmyHS Army, byte Selection, Movepath Path, Set<ArmyMoveModeElement> Mode, bool Force)
    {
        int Pos;
        MoveArmyTE Move;
        MoveAction Action;
        bool Moved;
        Moved = false;
        if (Force || !AoWHSMap.Current.FPlayers.GetPlayers(Army.FArmy.FPlayer).GetBusy())
        {
            if (Path.Count > 1)
            {
                Pos = Path.MovePointsToPos(Army.FArmy.MovePoints(Selection));
                if (Pos == 0)
                {
                    var item = Path[Pos];
                    var pathItem = Path[Pos];
                    var item2 = Path[Pos];
                    Action = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(item2.X, pathItem.Y, item.L)).GetMoveAction(Army.FArmy.FPlayer);
                }
                else
                {
                    Action = MoveAction.Normal;
                }

                bool condition = Action == MoveAction.Attack && AoWHSMap.Current.FPlayers.GetPlayers(Army.FArmy.FPlayer).FPlayerType == PlayerType.Human;
                if (condition)
                {
                    var item = Path[0];
                    var pathItem = Path[0];
                    var item2 = Path[0];
                    condition = !AoWHSMap.Current.VisibleForPlayer(item2.X, pathItem.Y, item.L, Army.FArmy.FPlayer);
                }

                if (condition)
                {
                    Action = MoveAction.Normal;
                }

                Move = CreateArmyMoveTE(Army, Selection, Path, Action, Mode);
                if (Move != null)
                {
                    AoWHSMap.Current.FTokenManager.GetTokenControl(Army.FArmy.FPlayer).AddEvent(Move);
                    Move.Release();
                    Moved = true;
                }
            }
        }

        return Moved;
    }

    internal static bool MoveArmy(ArmyHS Army, byte Selection, Movepath Path) => MoveArmyEx(Army, Selection, Path, Set<ArmyMoveModeElement>.Of(ArmyMoveModeElement.SelectEndLocation), false);

    internal static bool MoveArmyXYL(ArmyHS Army, byte Selection, sbyte X, sbyte Y, sbyte L, bool Force)
    {
        bool Result;
        MoveControlSettings Settings;
        Settings = new MoveControlSettings(null);
        var lhx = Army.GetLhx();
        var yhx = Army.GetYhx();
        Settings.SetSourcePos(Army.GetXhx(), yhx, lhx);
        Settings.FPlayer = Army.FArmy.FPlayer;
        Settings.FArmyHS = Army;
        Settings.FSelection = Selection;
        Settings.SetDestinationPos(X, Y, L);
        Army.FArmy.CreateMovePointTable(Selection, Bytes.Of(ref Settings.FMovePointTable));
        AoWHSMap.Current.FMoveControl.ProcessMove(Settings);
        if (Settings.FPath.Count != 0)
        {
            Result = MoveArmyEx(Army, Selection, Settings.FPath, Set<ArmyMoveModeElement>.Of(), Force);
        }
        else
        {
            Result = false;
        }

        Settings.Release();
        return Result;
    }

}
