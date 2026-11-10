// Translated from CombatStructure.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

public partial class TCombatStructure : TCombatFixedILTerrain
{
    public TCombatStructure(TECustomNode owner) : base(owner)
    {
        SetHitPoints(5);
    }

    // CombatStructure.pas:57
    public new const int ClassID = 2229510;

    public int FHitPoints;
    public bool FPlaySound;

    // CombatStructure.pas:51

    // CombatStructure.pas:229
    public override sbyte GetOverlay(int Index)
    {
        sbyte Result;
        if (FResource.TerrainList.GetTerrain(Index) >= 0)
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
            Result = -1;
        }

        return Result;
    }

    // CombatStructure.pas:104
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
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(31).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                    var defense5 = GetDefense();
                    var executeDamageRole5 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - defense5);
                    SetHitPoints(FHitPoints - executeDamageRole5);
                    break;
                case 200715:
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(113).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                    var defense6 = GetDefense();
                    var executeDamageRole6 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense6);
                    SetHitPoints(FHitPoints - executeDamageRole6);
                    break;
                case 200722:
                    if (Msg.Param[1].Int == 30 || Msg.Param[1].Int == 86)
                    {
                        Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(Msg.Param[1].Int).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                        RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                        var defense7 = GetDefense();
                        var executeDamageRole7 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - defense7);
                        SetHitPoints(FHitPoints - executeDamageRole7);
                    }

                    break;
                case 200723:
                    if (Msg.Param[1].Int == 114)
                    {
                        Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(Msg.Param[1].Int).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                        RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                        var defense7 = GetDefense();
                        var executeDamageRole7 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense7);
                        SetHitPoints(FHitPoints - executeDamageRole7);
                    }

                    break;
                case 200710:
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(116).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                    var defense8 = GetDefense();
                    var executeDamageRole8 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense8);
                    SetHitPoints(FHitPoints - executeDamageRole8);
                    break;
                case 200717:
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(127).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                    var defense9 = GetDefense();
                    var executeDamageRole9 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense9);
                    SetHitPoints(FHitPoints - executeDamageRole9);
                    break;
                case 200718:
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(128).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                    var defense10 = GetDefense();
                    var executeDamageRole10 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense10);
                    SetHitPoints(FHitPoints - executeDamageRole10);
                    break;
                case 200708:
                    FPlaySound = Msg.Param[1].Int == 0;
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(112).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                    var defense11 = GetDefense();
                    var executeDamageRole11 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - defense11);
                    SetHitPoints(FHitPoints - executeDamageRole11);
                    Msg.Param[1] = 1;
                    break;
                case 200711:
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                    var defense12 = GetDefense();
                    var executeDamageRole12 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(Aow.Game.Combat.TacticalTables.TCDamage[1], Aow.Game.Combat.TacticalTables.TCAttack[1] - defense12);
                    SetHitPoints(FHitPoints - executeDamageRole12);
                    break;
            }
        }
    }

    // CombatStructure.pas:216
    public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
    {
        if (FResource.TerrainList.GetUsed(Info.HN))
        {
            if (FHitPoints == 0)
            {
                MoveResult.Code = ((TMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][6 - -1];
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

            MoveResult.Done = true;
        }
    }

    // CombatStructure.pas:60
    public override void Activate()
    {
        TPlayerStructure Structure;
        bool Razed;
        base.Activate();
        if (!Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
        {
            FPlaySound = true;
            Razed = false;
            Structure = (TPlayerStructure)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetMapLevel(Aow.Game.Combat.CombatPlacement.CombatLLoc).GetField(Aow.Game.Combat.CombatPlacement.CombatXLoc, Aow.Game.Combat.CombatPlacement.CombatYLoc).FindNoneTransparentHS(typeof(TPlayerStructure));
            if (Structure != null)
            {
                Razed = Structure.GetRazed();
            }

            if (Razed)
            {
                SetHitPoints(0);
            }
            else
            {
                SetHitPoints(5);
            }
        }
        else
        {
            SetHitPoints(5);
        }
    }

    // CombatStructure.pas:208
    public override void UpdateMapField(in THSMapFieldInfo Info)
    {
        base.UpdateMapField(Info);
        if (Info.HN == 0)
        {
            ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 16384 | 8192;
        }
    }

    // CombatStructure.pas:253
    public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => base.CanPlace(MapContainer, Xhx, Yhx, Lhx);

    // CombatStructure.pas:250
    public override bool CanPlaceOnHS(THexagonSprite HS) => base.CanPlaceOnHS(HS) || HS is TCombatRoad;

    // CombatStructure.pas:247
    public override bool ValidTerrainType(int TerrainType) => true;

    // CombatStructure.pas:237
    public override int GetTerrainTypeImage(int TerrainType)
    {
        int Result;
        if (TerrainType == 3)
        {
            Result = 20;
        }
        else
        {
            if (TSet<byte>.WithBounds(0, 255).With(5).With(12).Contains((byte)TerrainType))
            {
                Result = 40;
            }
            else
            {
                Result = 0;
            }
        }

        return Result;
    }

    // CombatStructure.pas:244
    public override void ShowShadow(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
    }

    // CombatStructure.pas:79
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
        FPlaySound = true;
    }

    // CombatStructure.pas:23

}

public partial class TCombatStructureResource : TCombatFixedILTerrainResource
{
    public TCombatStructureResource(TECustomNode owner) : base(owner)
    { }

    // CombatStructure.pas:256
    public new const int ClassID = 2229511;

    // CombatStructure.pas:262
    public override int ResourceUserClassID() => 2229510;

    // CombatStructure.pas:259
    public override string ResourceEditName() => "Combat Structure";

}
