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

public partial class CityWall : CombatILTerrain
{
    public CityWall(ECustomNode owner) : base(owner)
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
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        AbilityCombatInfo AbilityInfo = default;
        SpellCombatInfo SpellInfo = default;
        base.MapFieldMsgProc(ref Msg);
        if (Msg.HN == 0)
        {
            switch (Msg.MsgID)
            {
                case 200704:
                    AoWHSSet.Current.FAbilityControl.GetAbility(65).GetCombatInfo(Msg.Param[0].As<CombatUnit>().FUnit, ref AbilityInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense = GetDefense();
                    var executeDamageRole = AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - defense);
                    SetHitPoints(FHitPoints - executeDamageRole);
                    break;
                case 200705:
                    AoWHSSet.Current.FAbilityControl.GetAbility(24).GetCombatInfo(Msg.Param[0].As<CombatUnit>().FUnit, ref AbilityInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense2 = GetDefense();
                    var executeDamageRole2 = AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - defense2);
                    SetHitPoints(FHitPoints - executeDamageRole2);
                    break;
                case 200706:
                    AoWHSSet.Current.FAbilityControl.GetAbility(44).GetCombatInfo(Msg.Param[0].As<CombatUnit>().FUnit, ref AbilityInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense3 = GetDefense();
                    var executeDamageRole3 = AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - defense3);
                    SetHitPoints(FHitPoints - executeDamageRole3);
                    break;
                case 200707:
                    AoWHSSet.Current.FSpellControl.GetSpell(117).GetCombatInfo(Msg.Param[0].As<CombatUnit>().FUnit, ref SpellInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense4 = GetDefense();
                    var executeDamageRole4 = AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense4);
                    SetHitPoints(FHitPoints - executeDamageRole4);
                    break;
                case 200714:
                    if (FWallType == 1)
                    {
                        AoWHSSet.Current.FAbilityControl.GetAbility(31).GetCombatInfo(Msg.Param[0].As<CombatUnit>().FUnit, ref AbilityInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var defense5 = GetDefense();
                        var executeDamageRole5 = AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - defense5);
                        SetHitPoints(FHitPoints - executeDamageRole5);
                    }

                    break;
                case 200715:
                    if (FWallType == 1)
                    {
                        AoWHSSet.Current.FSpellControl.GetSpell(113).GetCombatInfo(Msg.Param[0].As<CombatUnit>().FUnit, ref SpellInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var defense5 = GetDefense();
                        var executeDamageRole5 = AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense5);
                        SetHitPoints(FHitPoints - executeDamageRole5);
                    }

                    break;
                case 200710:
                    AoWHSSet.Current.FSpellControl.GetSpell(116).GetCombatInfo(Msg.Param[0].As<CombatUnit>().FUnit, ref SpellInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense6 = GetDefense();
                    var executeDamageRole6 = AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense6);
                    SetHitPoints(FHitPoints - executeDamageRole6);
                    break;
                case 200717:
                    AoWHSSet.Current.FSpellControl.GetSpell(127).GetCombatInfo(Msg.Param[0].As<CombatUnit>().FUnit, ref SpellInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense7 = GetDefense();
                    var executeDamageRole7 = AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense7);
                    SetHitPoints(FHitPoints - executeDamageRole7);
                    break;
                case 200718:
                    if (FWallType == 1)
                    {
                        AoWHSSet.Current.FSpellControl.GetSpell(128).GetCombatInfo(Msg.Param[0].As<CombatUnit>().FUnit, ref SpellInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var defense5 = GetDefense();
                        var executeDamageRole5 = AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense5);
                        SetHitPoints(FHitPoints - executeDamageRole5);
                    }

                    break;
                case 200708:
                    FPlaySound = Msg.Param[1].Int == 0;
                    AoWHSSet.Current.FSpellControl.GetSpell(112).GetCombatInfo(Msg.Param[0].As<CombatUnit>().FUnit, ref SpellInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense8 = GetDefense();
                    var executeDamageRole8 = AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense8);
                    SetHitPoints(FHitPoints - executeDamageRole8);
                    Msg.Param[1] = 1;
                    break;
                case 200711:
                    if (FWallType == 1)
                    {
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var defense5 = GetDefense();
                        var executeDamageRole5 = AbilityRules.ExecuteDamageRole(TacticalTables.TCDamage[1], TacticalTables.TCAttack[1] - defense5);
                        SetHitPoints(FHitPoints - executeDamageRole5);
                    }

                    break;
            }
        }
    }

    // CityWall.pas:155
    public override void CanMoveOn(in HSMoveOnInfo Info, ref HSMoveOnResult MoveResult)
    {
        if (Info.HN == 0)
        {
            if (FHitPoints == 0)
            {
                MoveResult.Code = ((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][6 - -1];
            }
            else
            {
                if (((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Wall))
                {
                    MoveResult.Code = 25;
                    MoveResult.Done = true;
                }
                else
                {
                    if (((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.WallCrushing) && GetXhx() == ((MoveControl)Info.Sender).FDestinationX && GetYhx() == ((MoveControl)Info.Sender).FDestinationY)
                    {
                        MoveResult.Code = 0;
                    }
                    else
                    {
                        if (((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1) || ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66) || ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36))
                        {
                            MoveResult.Code = ((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][-1 - -1];
                        }
                        else
                        {
                            if (((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59))
                            {
                                MoveResult.Code = -1;
                            }
                            else
                            {
                                MoveResult.Code = ((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1];
                            }
                        }
                    }
                }
            }

            MoveResult.Done = true;
        }
    }

    // CityWall.pas:180
    public override int CanMoveOver(in HSSenderInfo Info)
    {
        int Result;
        if (Info.HN == 0)
        {
            if (FHitPoints == 0 || ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66) || ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36) || ((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Climbing) || ((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Wall))
            {
                Result = 1;
            }
            else
            {
                if (((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59))
                {
                    Result = 0;
                }
                else
                {
                    if (((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1] != 0)
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
        PlayerStructure Structure;
        bool Razed;
        base.Activate();
        if (!AoWCombatMap.Current.Status.Contains(HSMapStatusElement.Editting))
        {
            Razed = false;
            FPlaySound = true;
            Structure = (PlayerStructure)AoWHSMap.Current.MapContainer.GetMapLevel(CombatPlacement.CombatLLoc).GetField(CombatPlacement.CombatXLoc, CombatPlacement.CombatYLoc).FindNoneTransparentHS(typeof(City));
            if (Structure != null)
            {
                FWallType = ((City)Structure).FWallType;
                Razed = Structure.GetRazed();
                if (Structure.FPlayer == -1)
                {
                    FFlagID = -1;
                }
                else
                {
                    FFlagID = AoWHSMap.Current.FPlayers.GetPlayers(Structure.FPlayer).FPlayerRace;
                }
            }
            else
            {
                Structure = (PlayerStructure)AoWHSMap.Current.MapContainer.GetMapLevel(CombatPlacement.CombatLLoc).GetField(CombatPlacement.CombatXLoc, CombatPlacement.CombatYLoc).FindNoneTransparentHS(typeof(Tower));
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
                        FFlagID = AoWHSMap.Current.FPlayers.GetPlayers(Structure.FPlayer).FPlayerRace;
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
                UpdateMapFields(AoWCombatMap.Current.MapContainer.GetMapLevel(0), xhx, yhx);
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
                    SetHitPoints(TacticalTables.WallMaxHP[FWallType - 1]);
                }

                if (FFlagID == -1)
                {
                    FFlagID = 32;
                }

                AoWCombatMap.Current.FWalls.Add(this);
            }
        }
        else
        {
            FWallType = 2;
        }
    }

    // CityWall.pas:147
    public override void UpdateMapField(in HSMapFieldInfo Info)
    {
        base.UpdateMapField(Info);
        if (Info.HN == 0)
        {
            ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 16384 | 8192;
        }
    }

    // CityWall.pas:141
    public override bool ValidTerrainType(int TerrainType) => true;

    // CityWall.pas:144
    public override int GetTerrainTypeImage(int TerrainType) => 0;

    // CityWall.pas:363
    public override void ShowShadow(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
    }

    // CityWall.pas:130
    public byte GetDirection() => ((Resource)FResource).FDirection;

    // CityWall.pas:197
    public new int GetDefense() => -2;

    // CityWall.pas:200
    public void SetHitPoints(int Value)
    {
        TfxHS Effect;
        XYZ Point;
        if (Value < 0)
        {
            Value = 0;
        }

        if (Value < FHitPoints)
        {
            if (Value == 0)
            {
                AoWCombatMap.Current.FCombat.FCombatData.FWallBreached = true;
                if (FPlaySound && AoWCombatMap.Current.FPlaySounds)
                {
                    Audio.AudioPlayback.PlaySample("SFX\\HIT_DEATH\\D_WOODLG.WAV");
                }

                Effect = AoWCombatMap.Current.AddEffect(TfxType.WallSmoke);
                Point = new XYZ();
                Effect.FPath.Add(Point);
                HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Point.X, ref Point.Y);
                Point.X = Random(20) + Point.X + 6;
                Point.Y = Random(20) + Point.Y - 20;
            }
            else
            {
                if (FPlaySound && AoWCombatMap.Current.FPlaySounds)
                {
                    Audio.AudioPlayback.PlaySample("SFX\\HIT_DEATH\\D_WOODMD.WAV");
                }
            }
        }

        FHitPoints = Value;
        if (TacticalTables.WallMaxHP[FWallType - 1] == FHitPoints)
        {
            FDamageImage = 0;
        }
        else
        {
            if (TacticalTables.WallMaxHP[FWallType - 1] / 2 < FHitPoints)
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

    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        ImageSequence Sequence;
        int ImageIndex;
        int Xhp = default;
        int Yhp = default;
        int TerrainOffset;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        XhpSurf = Xhp - XhpSurf;
        YhpSurf = Yhp - YhpSurf;
        var yhx = GetYhx();
        var xhx = GetXhx();
        switch (AoWCombatMap.Current.MapContainer.GetMapLevel(0).GetField(xhx, yhx).TerrainType)
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

        Sequence = FResource.ImageSequenceList.GetImageSequence(WallISIndex[FWallType - 1] + (TerrainOffset + FDamageImage));
        if (Sequence != null)
        {
            do
            {
                switch (Sequence.Layer)
                {
                    case 7:
                        ImageIndex = Sequence.GetFrameIndex(AoWCombatMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (ImageIndex != -1)
                        {
                            if (!AoWCombatMap.Current.Status.Contains(HSMapStatusElement.Editting))
                            {
                                var xhx2 = GetXhx();
                                var yhx2 = GetYhx();
                                AoWHSSet.Current.FFlagControl.ShowFlagEx(XhpSurf + 21 + Sequence.dXshift, YhpSurf + Sequence.dYshift + 16, xhx2 + AoWHSMap.Current.Frame + yhx2 * 100, (byte)FFlagID, SurfaceDesc, false);
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

    private static Engine.Buffers.FixedArray_int_2 WallISIndex = Engine.Buffers.FixedArray_int_2.From([30, 0]);

    public new partial class Resource : CombatILTerrainResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // CityWall.pas:366
        public new const int ClassID = 2229509;

        public byte FDirection;

        // CityWall.pas:369
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FDirection, 25);
        }

        // CityWall.pas:378
        public override int ResourceUserClassID() => 2229508;

        // CityWall.pas:375
        public override string ResourceEditName() => "Combat City Wall";

    }
}

public partial class CityWallDoor : CityWall
{
    public CityWallDoor(ECustomNode owner) : base(owner)
    { }

    // CityWall.pas:381
    public new const int ClassID = 2229510;

    // CityWall.pas:428
    public override sbyte GetOverlay(int Index) => -1;

    // CityWall.pas:431
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        TacticalCombatUnitHS CombatUnit;
        if (AoWCombatMap.Current.Status.Contains(HSMapStatusElement.Editting) || AoWCombatMap.Current.FCombat == null)
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }
        else
        {
            var yhx = GetYhx();
            var xhx = GetXhx();
            CombatUnit = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.GetMapLevel(0).GetField(xhx, yhx).FindOwnedChild(2228484);
            if (CombatUnit != null)
            {
                if (CombatUnit.FUnit.GetSide() != CombatSideIndex.Defender)
                {
                    CombatUnit = null;
                }
            }

            bool condition = FHitPoints == 0;
            if (!condition)
            {
                var indexOfPlayer = AoWCombatMap.Current.FCombat.FCombatData.FPlayers.IndexOfPlayer((byte)AoWCombatMap.Current.FCurrentPlayer);
                condition = AoWCombatMap.Current.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)indexOfPlayer).FSideIndex != 1 && CombatUnit == null;
            }

            if (condition)
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }
    }

    // CityWall.pas:384
    public override void CanMoveOn(in HSMoveOnInfo Info, ref HSMoveOnResult MoveResult)
    {
        if (Info.HN == 0)
        {
            if (FHitPoints == 0)
            {
                MoveResult.Code = ((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][6 - -1];
            }
            else
            {
                if (((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Wall))
                {
                    MoveResult.Code = 25;
                    MoveResult.Done = true;
                }
                else
                {
                    if (((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.WallCrushing) && GetXhx() == ((MoveControl)Info.Sender).FDestinationX && GetYhx() == ((MoveControl)Info.Sender).FDestinationY)
                    {
                        MoveResult.Code = 0;
                    }
                    else
                    {
                        if (((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1) || ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66) || ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide() == CombatSideIndex.Defender)
                        {
                            MoveResult.Code = ((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][-1 - -1];
                        }
                        else
                        {
                            if (((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59))
                            {
                                MoveResult.Code = -1;
                            }
                            else
                            {
                                MoveResult.Code = ((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1];
                            }
                        }
                    }
                }
            }

            MoveResult.Done = true;
        }
    }

    // CityWall.pas:409
    public override int CanMoveOver(in HSSenderInfo Info)
    {
        int Result;
        if (Info.HN == 0)
        {
            if (FHitPoints == 0 || ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide() == CombatSideIndex.Defender || ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66) || ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36) || ((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Climbing) || ((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Wall))
            {
                Result = 1;
            }
            else
            {
                if (((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59))
                {
                    Result = 0;
                }
                else
                {
                    if (((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1] != 0)
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

    public new partial class Resource : CityWall.Resource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // CityWall.pas:451
        public new const int ClassID = 2229511;

        // CityWall.pas:457
        public override int ResourceUserClassID() => 2229510;

        // CityWall.pas:454
        public override string ResourceEditName() => "Combat City Door";

    }

}
