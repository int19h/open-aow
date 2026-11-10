// Translated from CityWall.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

public partial class TCityWall : TCombatILTerrain
{
    public TCityWall(TECustomNode owner) : base(owner)
    { }

    // CityWall.pas:78
    public new const int ClassID = 2229508;

    public int FHitPoints;
    public int FDamageImage;
    public int FFlagID;
    public bool FPlaySound;
    public byte FWallType;

    // CityWall.pas:133
    public override sbyte GetOverlay(int Index)
    {
        sbyte Result;
        if (Index == 0)
        {
            if (FHitPoints == 0)
            {
                Result = 6;
            }
            else
            {
                Result = 8;
            }
        }
        else
        {
            Result = base.GetOverlay(Index);
        }

        return Result;
    }

    // CityWall.pas:230
    public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
    {
        TAbilityCombatInfo AbilityInfo = default;
        TSpellCombatInfo SpellInfo = default;
        base.MapFieldMsgProc(ref Msg);
        if (Msg.HN == 0)
        {
            switch (Msg.MsgID)
            {
                case 200704:
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(65).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                    var defense = GetDefense();
                    var executeDamageRole = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - defense);
                    SetHitPoints(FHitPoints - executeDamageRole);
                    break;
                case 200705:
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(24).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                    var defense2 = GetDefense();
                    var executeDamageRole2 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - defense2);
                    SetHitPoints(FHitPoints - executeDamageRole2);
                    break;
                case 200706:
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(44).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                    var defense3 = GetDefense();
                    var executeDamageRole3 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - defense3);
                    SetHitPoints(FHitPoints - executeDamageRole3);
                    break;
                case 200707:
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(117).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                    var defense4 = GetDefense();
                    var executeDamageRole4 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense4);
                    SetHitPoints(FHitPoints - executeDamageRole4);
                    break;
                case 200714:
                    if (FWallType == 1)
                    {
                        Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(31).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                        RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                        var defense5 = GetDefense();
                        var executeDamageRole5 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - defense5);
                        SetHitPoints(FHitPoints - executeDamageRole5);
                    }

                    break;
                case 200715:
                    if (FWallType == 1)
                    {
                        Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(113).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                        RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                        var defense5 = GetDefense();
                        var executeDamageRole5 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense5);
                        SetHitPoints(FHitPoints - executeDamageRole5);
                    }

                    break;
                case 200710:
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(116).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                    var defense6 = GetDefense();
                    var executeDamageRole6 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense6);
                    SetHitPoints(FHitPoints - executeDamageRole6);
                    break;
                case 200717:
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(127).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                    var defense7 = GetDefense();
                    var executeDamageRole7 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense7);
                    SetHitPoints(FHitPoints - executeDamageRole7);
                    break;
                case 200718:
                    if (FWallType == 1)
                    {
                        Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(128).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                        RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                        var defense5 = GetDefense();
                        var executeDamageRole5 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense5);
                        SetHitPoints(FHitPoints - executeDamageRole5);
                    }

                    break;
                case 200708:
                    FPlaySound = Msg.Param[1].Int == 0;
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(112).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                    var defense8 = GetDefense();
                    var executeDamageRole8 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense8);
                    SetHitPoints(FHitPoints - executeDamageRole8);
                    Msg.Param[1] = 1;
                    break;
                case 200711:
                    if (FWallType == 1)
                    {
                        RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                        var defense5 = GetDefense();
                        var executeDamageRole5 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(Aow.Game.Combat.TacticalTables.TCDamage[1], Aow.Game.Combat.TacticalTables.TCAttack[1] - defense5);
                        SetHitPoints(FHitPoints - executeDamageRole5);
                    }

                    break;
            }
        }
    }

    // CityWall.pas:155
    public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
    {
        if (Info.HN == 0)
        {
            if (FHitPoints == 0)
            {
                MoveResult.Code = ((TMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][6 - -1];
            }
            else
            {
                if (((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmaiWall))
                {
                    MoveResult.Code = 25;
                    MoveResult.Done = true;
                }
                else
                {
                    if (((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmWallCrushing) && GetXhx() == ((TMoveControl)Info.Sender).FDestinationX && GetYhx() == ((TMoveControl)Info.Sender).FDestinationY)
                    {
                        MoveResult.Code = 0;
                    }
                    else
                    {
                        if (((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1) || ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66) || ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36))
                        {
                            MoveResult.Code = ((TMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][-1 - -1];
                        }
                        else
                        {
                            if (((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59))
                            {
                                MoveResult.Code = -1;
                            }
                            else
                            {
                                MoveResult.Code = ((TMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1];
                            }
                        }
                    }
                }
            }

            MoveResult.Done = true;
        }
    }

    // CityWall.pas:180
    public override int CanMoveOver(in THSSenderInfo Info)
    {
        int Result;
        if (Info.HN == 0)
        {
            if (FHitPoints == 0 || ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66) || ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36) || ((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmClimbing) || ((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmaiWall))
            {
                Result = 1;
            }
            else
            {
                if (((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59))
                {
                    Result = 0;
                }
                else
                {
                    if (((TMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1] != 0)
                    {
                        Result = 1;
                    }
                    else
                    {
                        Result = 0;
                    }
                }
            }
        }
        else
        {
            Result = 1;
        }

        return Result;
    }

    // CityWall.pas:81
    public override void Activate()
    {
        TPlayerStructure Structure;
        bool Razed;
        base.Activate();
        if (!Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
        {
            Razed = false;
            FPlaySound = true;
            Structure = (TPlayerStructure)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetMapLevel(Aow.Game.Combat.CombatPlacement.CombatLLoc).GetField(Aow.Game.Combat.CombatPlacement.CombatXLoc, Aow.Game.Combat.CombatPlacement.CombatYLoc).FindNoneTransparentHS(typeof(TCity));
            if (Structure != null)
            {
                FWallType = ((TCity)Structure).FWallType;
                Razed = Structure.GetRazed();
                if (Structure.FPlayer == -1)
                {
                    FFlagID = -1;
                }
                else
                {
                    FFlagID = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer).FPlayerRace;
                }
            }
            else
            {
                Structure = (TPlayerStructure)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetMapLevel(Aow.Game.Combat.CombatPlacement.CombatLLoc).GetField(Aow.Game.Combat.CombatPlacement.CombatXLoc, Aow.Game.Combat.CombatPlacement.CombatYLoc).FindNoneTransparentHS(typeof(TTower));
                if (Structure != null)
                {
                    FWallType = 1;
                    Razed = Structure.GetRazed();
                    if (Structure.FPlayer == -1)
                    {
                        FFlagID = -1;
                    }
                    else
                    {
                        FFlagID = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer).FPlayerRace;
                    }
                }
                else
                {
                    FWallType = 0;
                }
            }

            if (FWallType == 0)
            {
                RemoveFromMap();
                var yhx = GetYhx();
                var xhx = GetXhx();
                UpdateMapFields(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.GetMapLevel(0), xhx, yhx);
                this.Dispose();
            }
            else
            {
                if (Razed)
                {
                    SetHitPoints(0);
                }
                else
                {
                    SetHitPoints(Aow.Game.Combat.TacticalTables.WallMaxHP[FWallType - 1]);
                }

                if (FFlagID == -1)
                {
                    FFlagID = 32;
                }

                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FWalls.Add(this);
            }
        }
        else
        {
            FWallType = 2;
        }
    }

    // CityWall.pas:147
    public override void UpdateMapField(in THSMapFieldInfo Info)
    {
        base.UpdateMapField(Info);
        if (Info.HN == 0)
        {
            ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 16384 | 8192;
        }
    }

    // CityWall.pas:141
    public override bool ValidTerrainType(int TerrainType) => true;

    // CityWall.pas:144
    public override int GetTerrainTypeImage(int TerrainType) => 0;

    // CityWall.pas:363
    public override void ShowShadow(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
    }

    // CityWall.pas:130
    public byte GetDirection() => ((TCityWallResource)FResource).FDirection;

    // CityWall.pas:197
    public new int GetDefense() => -2;

    // CityWall.pas:200
    public void SetHitPoints(int Value)
    {
        TfxHS Effect;
        TXYZ Point;
        if (Value < 0)
        {
            Value = 0;
        }

        if (Value < FHitPoints)
        {
            if (Value == 0)
            {
                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.FWallBreached = true;
                if (FPlaySound && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlaySounds)
                {
                    Aow.Game.Audio.AudioPlayback.PlaySample("SFX\\HIT_DEATH\\D_WOODLG.WAV");
                }

                Effect = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.AddEffect(TfxType.fxWallSmoke);
                Point = new TXYZ();
                Effect.FPath.Add(Point);
                Aow.Game.World.Map.HexGeometry.HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Point.X, ref Point.Y);
                Point.X = Random(20) + Point.X + 6;
                Point.Y = Random(20) + Point.Y - 20;
            }
            else
            {
                if (FPlaySound && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlaySounds)
                {
                    Aow.Game.Audio.AudioPlayback.PlaySample("SFX\\HIT_DEATH\\D_WOODMD.WAV");
                }
            }
        }

        FHitPoints = Value;
        if (Aow.Game.Combat.TacticalTables.WallMaxHP[FWallType - 1] == FHitPoints)
        {
            FDamageImage = 0;
        }
        else
        {
            if (Aow.Game.Combat.TacticalTables.WallMaxHP[FWallType - 1] / 2 < FHitPoints)
            {
                FDamageImage = 1;
            }
            else
            {
                if (FHitPoints > 0)
                {
                    FDamageImage = 2;
                }
                else
                {
                    FDamageImage = 3;
                }
            }
        }

        FPlaySound = true;
    }

    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        TImageSequence Sequence;
        int ImageIndex;
        int Xhp = default;
        int Yhp = default;
        int TerrainOffset;
        Aow.Game.World.Map.HexGeometry.HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
        XhpSurf = Xhp - XhpSurf;
        YhpSurf = Yhp - YhpSurf;
        var yhx = GetYhx();
        var xhx = GetXhx();
        switch (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.GetMapLevel(0).GetField(xhx, yhx).TerrainType)
        {
            case 3:
                TerrainOffset = 10;
                break;
            case 5:
            case 7:
            case 12:
                TerrainOffset = 20;
                break;
            default:
                TerrainOffset = 0;
                break;
        }

        Sequence = FResource.ImageSequenceList.GetImageSequence(Aow.Game.World.Terrain.TCityWall.WallISIndex[FWallType - 1] + (TerrainOffset + FDamageImage));
        if (Sequence != null)
        {
            do
            {
                switch (Sequence.Layer)
                {
                    case 7:
                        ImageIndex = Sequence.GetFrameIndex(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (ImageIndex != -1)
                        {
                            if (!Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
                            {
                                var xhx2 = GetXhx();
                                var yhx2 = GetYhx();
                                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FFlagControl.ShowFlagEx(XhpSurf + 21 + Sequence.dXshift, YhpSurf + Sequence.dYshift + 16, xhx2 + Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame + yhx2 * 100, (byte)FFlagID, SurfaceDesc, false);
                            }
                        }

                        break;
                    default:
                        {
                            ImageIndex = Sequence.GetUnloopedFrameIndex(FImageIndex);
                            if (ImageIndex != -1)
                            {
                                Sequence.ImageLibrary[ImageIndex].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                            }
                        }

                        break;
                }

                Sequence = Sequence.Next;
            }
            while (!(Sequence == null));
        }
    }

    private static global::Aow.Game.Engine.Buffers.FixedArray_int_2 WallISIndex = global::Aow.Game.Engine.Buffers.FixedArray_int_2.From([30, 0]);
}

public partial class TCityWallResource : TCombatILTerrainResource
{
    public TCityWallResource(TECustomNode owner) : base(owner)
    { }

    // CityWall.pas:366
    public new const int ClassID = 2229509;

    public byte FDirection;

    // CityWall.pas:369
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FDirection, 25);
    }

    // CityWall.pas:378
    public override int ResourceUserClassID() => 2229508;

    // CityWall.pas:375
    public override string ResourceEditName() => "Combat City Wall";

}

public partial class TCityWallDoor : TCityWall
{
    public TCityWallDoor(TECustomNode owner) : base(owner)
    { }

    // CityWall.pas:381
    public new const int ClassID = 2229510;

    // CityWall.pas:428
    public override sbyte GetOverlay(int Index) => -1;

    // CityWall.pas:431
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        TTacticalCombatUnitHS CombatUnit;
        if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting) || Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat == null)
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }
        else
        {
            var yhx = GetYhx();
            var xhx = GetXhx();
            CombatUnit = (TTacticalCombatUnitHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.GetMapLevel(0).GetField(xhx, yhx).FindOwnedChild(2228484);
            if (CombatUnit != null)
            {
                if (CombatUnit.FUnit.GetSide() != TCombatSideIndex.csDefender)
                {
                    CombatUnit = null;
                }
            }

            bool condition = FHitPoints == 0;
            if (!condition)
            {
                var indexOfPlayer = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.FPlayers.IndexOfPlayer((byte)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentPlayer);
                condition = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)indexOfPlayer).FSideIndex != 1 && CombatUnit == null;
            }

            if (condition)
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }
    }

    // CityWall.pas:384
    public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
    {
        if (Info.HN == 0)
        {
            if (FHitPoints == 0)
            {
                MoveResult.Code = ((TMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][6 - -1];
            }
            else
            {
                if (((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmaiWall))
                {
                    MoveResult.Code = 25;
                    MoveResult.Done = true;
                }
                else
                {
                    if (((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmWallCrushing) && GetXhx() == ((TMoveControl)Info.Sender).FDestinationX && GetYhx() == ((TMoveControl)Info.Sender).FDestinationY)
                    {
                        MoveResult.Code = 0;
                    }
                    else
                    {
                        if (((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1) || ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66) || ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide() == TCombatSideIndex.csDefender)
                        {
                            MoveResult.Code = ((TMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][-1 - -1];
                        }
                        else
                        {
                            if (((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59))
                            {
                                MoveResult.Code = -1;
                            }
                            else
                            {
                                MoveResult.Code = ((TMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1];
                            }
                        }
                    }
                }
            }

            MoveResult.Done = true;
        }
    }

    // CityWall.pas:409
    public override int CanMoveOver(in THSSenderInfo Info)
    {
        int Result;
        if (Info.HN == 0)
        {
            if (FHitPoints == 0 || ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide() == TCombatSideIndex.csDefender || ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66) || ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36) || ((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmClimbing) || ((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmaiWall))
            {
                Result = 1;
            }
            else
            {
                if (((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59))
                {
                    Result = 0;
                }
                else
                {
                    if (((TMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1] != 0)
                    {
                        Result = 1;
                    }
                    else
                    {
                        Result = 0;
                    }
                }
            }
        }
        else
        {
            Result = 1;
        }

        return Result;
    }

}

public partial class TCityWallDoorResource : TCityWallResource
{
    public TCityWallDoorResource(TECustomNode owner) : base(owner)
    { }

    // CityWall.pas:451
    public new const int ClassID = 2229511;

    // CityWall.pas:457
    public override int ResourceUserClassID() => 2229510;

    // CityWall.pas:454
    public override string ResourceEditName() => "Combat City Door";

}
