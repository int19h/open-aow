#nullable disable
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
    public CombatStructure(CustomNode owner) : base(owner) => SetHitPoints(5);

    // CombatStructure.pas:57
    public new const int ClassId = 2229510;

    public int HitPoints { get; private set; }

    private bool _playSound;

    // CombatStructure.pas:51

    // CombatStructure.pas:229
    public override sbyte GetOverlay(int index)
    {
        sbyte result = ((MapObject)this).Resource.TerrainList.GetTerrain(index) >= 0 ? HitPoints == 0 ? (sbyte)6 : (sbyte)8 : (sbyte)-1;
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
                    GamePack.Current.AbilityControl.GetAbility(65).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    sbyte defense = GetDefense();
                    int executeDamageRole = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - defense);
                    SetHitPoints(HitPoints - executeDamageRole);
                    break;
                case 200705:
                    GamePack.Current.AbilityControl.GetAbility(24).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    sbyte defense2 = GetDefense();
                    int executeDamageRole2 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - defense2);
                    SetHitPoints(HitPoints - executeDamageRole2);
                    break;
                case 200706:
                    GamePack.Current.AbilityControl.GetAbility(44).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    sbyte defense3 = GetDefense();
                    int executeDamageRole3 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - defense3);
                    SetHitPoints(HitPoints - executeDamageRole3);
                    break;
                case 200707:
                    GamePack.Current.SpellControl.GetSpell(117).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    sbyte defense4 = GetDefense();
                    int executeDamageRole4 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense4);
                    SetHitPoints(HitPoints - executeDamageRole4);
                    break;
                case 200714:
                    GamePack.Current.AbilityControl.GetAbility(31).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    sbyte defense5 = GetDefense();
                    int executeDamageRole5 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - defense5);
                    SetHitPoints(HitPoints - executeDamageRole5);
                    break;
                case 200715:
                    GamePack.Current.SpellControl.GetSpell(113).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    sbyte defense6 = GetDefense();
                    int executeDamageRole6 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense6);
                    SetHitPoints(HitPoints - executeDamageRole6);
                    break;
                case 200722:
                    if (message.Param[1].Int == 30 || message.Param[1].Int == 86)
                    {
                        GamePack.Current.AbilityControl.GetAbility(message.Param[1].Int).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        sbyte defense7 = GetDefense();
                        int executeDamageRole7 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - defense7);
                        SetHitPoints(HitPoints - executeDamageRole7);
                    }

                    break;
                case 200723:
                    if (message.Param[1].Int == 114)
                    {
                        GamePack.Current.SpellControl.GetSpell(message.Param[1].Int).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        sbyte defense7 = GetDefense();
                        int executeDamageRole7 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense7);
                        SetHitPoints(HitPoints - executeDamageRole7);
                    }

                    break;
                case 200710:
                    GamePack.Current.SpellControl.GetSpell(116).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    sbyte defense8 = GetDefense();
                    int executeDamageRole8 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense8);
                    SetHitPoints(HitPoints - executeDamageRole8);
                    break;
                case 200717:
                    GamePack.Current.SpellControl.GetSpell(127).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    sbyte defense9 = GetDefense();
                    int executeDamageRole9 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense9);
                    SetHitPoints(HitPoints - executeDamageRole9);
                    break;
                case 200718:
                    GamePack.Current.SpellControl.GetSpell(128).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    sbyte defense10 = GetDefense();
                    int executeDamageRole10 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense10);
                    SetHitPoints(HitPoints - executeDamageRole10);
                    break;
                case 200708:
                    _playSound = message.Param[1].Int == 0;
                    GamePack.Current.SpellControl.GetSpell(112).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    sbyte defense11 = GetDefense();
                    int executeDamageRole11 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense11);
                    SetHitPoints(HitPoints - executeDamageRole11);
                    MapFieldMessageParameters param = message.Param;
                    param[1] = 1;
                    message.Param = param;
                    break;
                case 200711:
                    RandSeed = StrategicMap.Current.Random(16777215);
                    sbyte defense12 = GetDefense();
                    int executeDamageRole12 = AbilityRules.ExecuteDamageRole(TacticalTables.TacticalBattleDamage[1], TacticalTables.TacticalBattleAttack[1] - defense12);
                    SetHitPoints(HitPoints - executeDamageRole12);
                    break;
            }
        }
    }

    // CombatStructure.pas:216
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        if (((MapObject)this).Resource.TerrainList.GetUsed(info.HexNumber))
        {
            moveResult.Code = HitPoints == 0
                ? ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][6 - MovementTables.FirstOverlay]
                : ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(59)
                    ? -1
                    : ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][8 - MovementTables.FirstOverlay];

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
            _playSound = true;
            razed = false;
            structure = StrategicMap.Current.MapContainer.GetMapLevel(CombatPlacement.CombatLLocation).GetField(CombatPlacement.CombatXLocation, CombatPlacement.CombatYLocation).FindNoneTransparentHexSprite<PlayerStructure>();
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
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOnFlag | MapField.MoveOverFlag;
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
        int result = terrainType == 3 ? 20 : Set<byte>.WithBounds(0, 255).With(5).With(12).Contains((byte)terrainType) ? 40 : 0;
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
                if (_playSound && CombatMap.Current.PlaySounds)
                {
                    AudioPlayback.PlaySample("SFX\\HIT_DEATH\\D_WOODLG.WAV");
                }

                effect = CombatMap.Current.AddEffect(BattleEffectType.WallSmoke);
                point = new Point3D();
                effect.Path.Add(point);
                int x = point.X;
                int y = point.Y;
                HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out x, out y);
                point.X = x;
                point.Y = y;
                point.X = Random(20) + point.X + 6;
                point.Y = Random(20) + point.Y - 20;
            }
            else
            {
                if (_playSound && CombatMap.Current.PlaySounds)
                {
                    AudioPlayback.PlaySample("SFX\\HIT_DEATH\\D_WOODMD.WAV");
                }
            }
        }

        HitPoints = value;
        _playSound = true;
    }

    // CombatStructure.pas:23

}
