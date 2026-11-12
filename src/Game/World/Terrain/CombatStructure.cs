// Translated from CombatStructure.pas.
using Aow.Engine.Nodes;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

public partial class CombatStructure : CombatFixedImageLibraryTerrain
{
    public CombatStructure(ECustomNode owner) : base(owner)
    {
        SetHitPoints(5);
    }

    // CombatStructure.pas:57
    public new const int ClassId = 2229510;

    public int HitPoints;
    public bool PlaySound;

    // CombatStructure.pas:51

    // CombatStructure.pas:229
    public override sbyte GetOverlay(int index)
    {
        sbyte result;
        if (_resource.TerrainList.GetTerrain(index) >= 0)
        {
            if (HitPoints == 0)
            {
                result = 6;
            }
            else
            {
                result = 8;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // CombatStructure.pas:104
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        AbilityCombatInfo abilityInfo = default;
        SpellCombatInfo spellInfo = default;
        base.MapFieldMessageProc(ref message);
        if (message.HexNumber == 0)
        {
            switch (message.MessageId)
            {
                case 200704:
                    AoWHSSet.Current.AbilityControl.GetAbility(65).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense = GetDefense();
                    var executeDamageRole = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - defense);
                    SetHitPoints(HitPoints - executeDamageRole);
                    break;
                case 200705:
                    AoWHSSet.Current.AbilityControl.GetAbility(24).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense2 = GetDefense();
                    var executeDamageRole2 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - defense2);
                    SetHitPoints(HitPoints - executeDamageRole2);
                    break;
                case 200706:
                    AoWHSSet.Current.AbilityControl.GetAbility(44).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense3 = GetDefense();
                    var executeDamageRole3 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - defense3);
                    SetHitPoints(HitPoints - executeDamageRole3);
                    break;
                case 200707:
                    AoWHSSet.Current.SpellControl.GetSpell(117).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense4 = GetDefense();
                    var executeDamageRole4 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense4);
                    SetHitPoints(HitPoints - executeDamageRole4);
                    break;
                case 200714:
                    AoWHSSet.Current.AbilityControl.GetAbility(31).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense5 = GetDefense();
                    var executeDamageRole5 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - defense5);
                    SetHitPoints(HitPoints - executeDamageRole5);
                    break;
                case 200715:
                    AoWHSSet.Current.SpellControl.GetSpell(113).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense6 = GetDefense();
                    var executeDamageRole6 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense6);
                    SetHitPoints(HitPoints - executeDamageRole6);
                    break;
                case 200722:
                    if (message.Param[1].Int == 30 || message.Param[1].Int == 86)
                    {
                        AoWHSSet.Current.AbilityControl.GetAbility(message.Param[1].Int).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var defense7 = GetDefense();
                        var executeDamageRole7 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - defense7);
                        SetHitPoints(HitPoints - executeDamageRole7);
                    }

                    break;
                case 200723:
                    if (message.Param[1].Int == 114)
                    {
                        AoWHSSet.Current.SpellControl.GetSpell(message.Param[1].Int).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var defense7 = GetDefense();
                        var executeDamageRole7 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense7);
                        SetHitPoints(HitPoints - executeDamageRole7);
                    }

                    break;
                case 200710:
                    AoWHSSet.Current.SpellControl.GetSpell(116).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense8 = GetDefense();
                    var executeDamageRole8 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense8);
                    SetHitPoints(HitPoints - executeDamageRole8);
                    break;
                case 200717:
                    AoWHSSet.Current.SpellControl.GetSpell(127).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense9 = GetDefense();
                    var executeDamageRole9 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense9);
                    SetHitPoints(HitPoints - executeDamageRole9);
                    break;
                case 200718:
                    AoWHSSet.Current.SpellControl.GetSpell(128).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense10 = GetDefense();
                    var executeDamageRole10 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense10);
                    SetHitPoints(HitPoints - executeDamageRole10);
                    break;
                case 200708:
                    PlaySound = message.Param[1].Int == 0;
                    AoWHSSet.Current.SpellControl.GetSpell(112).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense11 = GetDefense();
                    var executeDamageRole11 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense11);
                    SetHitPoints(HitPoints - executeDamageRole11);
                    message.Param[1] = 1;
                    break;
                case 200711:
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    var defense12 = GetDefense();
                    var executeDamageRole12 = AbilityRules.ExecuteDamageRole(TacticalTables.s_tacticalBattleDamage[1], TacticalTables.s_tacticalBattleAttack[1] - defense12);
                    SetHitPoints(HitPoints - executeDamageRole12);
                    break;
            }
        }
    }

    // CombatStructure.pas:216
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        if (_resource.TerrainList.GetUsed(info.HexNumber))
        {
            if (HitPoints == 0)
            {
                moveResult.Code = ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][6 - -1];
            }
            else
            {
                if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(59))
                {
                    moveResult.Code = -1;
                }
                else
                {
                    moveResult.Code = ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][8 - -1];
                }
            }

            moveResult.Done = true;
        }
    }

    // CombatStructure.pas:60
    public override void Activate()
    {
        PlayerStructure structure;
        bool razed;
        base.Activate();
        if (!CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            PlaySound = true;
            razed = false;
            structure = (PlayerStructure)AoWHSMap.Current.MapContainer.GetMapLevel(CombatPlacement.s_combatLLocation).GetField(CombatPlacement.s_combatXLocation, CombatPlacement.s_combatYLocation).FindNoneTransparentHexSprite(typeof(PlayerStructure));
            if (structure != null)
            {
                razed = structure.GetRazed();
            }

            if (razed)
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
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        base.UpdateMapField(info);
        if (info.HexNumber == 0)
        {
            ((AoWMapField)info.MapField).FieldFlags = ((AoWMapField)info.MapField).FieldFlags | 16384 | 8192;
        }
    }

    // CombatStructure.pas:253
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel) => base.CanPlace(mapContainer, hexX, hexY, hexLevel);

    // CombatStructure.pas:250
    public override bool CanPlaceOnHexSprite(HexSprite hexSprite) => base.CanPlaceOnHexSprite(hexSprite) || hexSprite is CombatRoad;

    // CombatStructure.pas:247
    public override bool ValidTerrainType(int terrainType) => true;

    // CombatStructure.pas:237
    public override int GetTerrainTypeImage(int terrainType)
    {
        int result;
        if (terrainType == 3)
        {
            result = 20;
        }
        else
        {
            if (Set<byte>.WithBounds(0, 255).With(5).With(12).Contains((byte)terrainType))
            {
                result = 40;
            }
            else
            {
                result = 0;
            }
        }

        return result;
    }

    // CombatStructure.pas:244
    public override void ShowShadow(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
    }

    // CombatStructure.pas:79
    public void SetHitPoints(int value)
    {
        BattleEffectSprite effect;
        Point3D point;
        if (value < 0)
        {
            value = 0;
        }

        if (value < HitPoints)
        {
            if (value == 0)
            {
                if (PlaySound && CombatMap.Current.PlaySounds)
                {
                    AudioPlayback.PlaySample("SFX\\HIT_DEATH\\D_WOODLG.WAV");
                }

                effect = CombatMap.Current.AddEffect(BattleEffectType.WallSmoke);
                point = new Point3D();
                effect.Path.Add(point);
                HexGeometry.HexToPixel(((MapField)Owner).HexX, ((MapField)Owner).HexY, ref point.X, ref point.Y);
                point.X = Random(20) + point.X + 6;
                point.Y = Random(20) + point.Y - 20;
            }
            else
            {
                if (PlaySound && CombatMap.Current.PlaySounds)
                {
                    AudioPlayback.PlaySample("SFX\\HIT_DEATH\\D_WOODMD.WAV");
                }
            }
        }

        HitPoints = value;
        PlaySound = true;
    }

    // CombatStructure.pas:23

}
