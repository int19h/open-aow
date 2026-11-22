#nullable disable warnings
// Translated from CombatTerrain.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Hexes;

public class CombatTerrain(CustomNode owner) : TerrainMapObject(owner)
{

    // CombatTerrain.pas:82
    public new const int ClassId = 2_228_582;

    private CombatTerrainShadow? _shadow;
    private int _hitPoints;

    private int _defense;

    // CombatTerrain.pas:85
    public override void Dispose()
    {
        DestroyShadow();
        base.Dispose();
    }

    // CombatTerrain.pas:269
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        if (result)
        {
            int hexY = HexY;
            int hexX = HexX;
            UpdateMapFields(Owner.Owner, hexX, hexY);
        }

        return result;
    }

    // CombatTerrain.pas:275
    public override void RemoveFromMap()
    {
        MapLevel mapLevel = Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(mapLevel, StoredHexX, StoredHexY);
    }

    // CombatTerrain.pas:149
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.HexNumber == 0)
        {
            if ((byte)base.Resource.DecorationType is >= 1 and <= 3)
            {
                AbilityCombatInfo abilityInfo = default;
                SpellCombatInfo spellInfo = default;
                switch (message.MessageId)
                {
                    case 200704:
                        GamePack.Current.AbilityControl[65].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref abilityInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int executeDamageRole = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, attackDefense: abilityInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole);
                        break;
                    case 200705:
                        GamePack.Current.AbilityControl[24].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref abilityInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int executeDamageRole2 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, attackDefense: abilityInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole2);
                        break;
                    case 200706:
                        GamePack.Current.AbilityControl[44].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref abilityInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int executeDamageRole3 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, attackDefense: abilityInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole3);
                        break;
                    case 200707:
                        GamePack.Current.SpellControl[117].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref spellInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int executeDamageRole4 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, attackDefense: spellInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole4);
                        break;
                    case 200714:
                        GamePack.Current.AbilityControl[31].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref abilityInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int executeDamageRole5 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, attackDefense: abilityInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole5);
                        break;
                    case 200715:
                        GamePack.Current.SpellControl[113].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref spellInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int executeDamageRole6 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, attackDefense: spellInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole6);
                        break;
                    case 200722:
                        if (message.Param[1].Int is 30 or 86)
                        {
                            GamePack.Current.AbilityControl[message.Param[1].Int].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref abilityInfo);
                            RandSeed = StrategicMap.Current.Random(16777215);
                            int executeDamageRole7 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, attackDefense: abilityInfo.Attack - _defense);
                            SetHitPoints(_hitPoints - executeDamageRole7);
                        }

                        break;
                    case 200723:
                        if (message.Param[1].Int == 114)
                        {
                            GamePack.Current.SpellControl[message.Param[1].Int].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref spellInfo);
                            RandSeed = StrategicMap.Current.Random(16777215);
                            int executeDamageRole7 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, attackDefense: spellInfo.Attack - _defense);
                            SetHitPoints(_hitPoints - executeDamageRole7);
                        }

                        break;
                    case 200710:
                        GamePack.Current.SpellControl[116].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref spellInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int executeDamageRole8 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, attackDefense: spellInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole8);
                        break;
                    case 200717:
                        GamePack.Current.SpellControl[127].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref spellInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int executeDamageRole9 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, attackDefense: spellInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole9);
                        break;
                    case 200718:
                        GamePack.Current.SpellControl[128].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref spellInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int executeDamageRole10 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, attackDefense: spellInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole10);
                        break;
                    case 200708:
                        GamePack.Current.SpellControl[112].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref spellInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int executeDamageRole11 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, attackDefense: spellInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole11);
                        MapFieldMessageParameters param = message.Param;
                        param[1] = 1;
                        message.Param = param;
                        break;
                    case 200711:
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int executeDamageRole12 = AbilityRules.ExecuteDamageRole(TacticalTables.TacticalBattleDamage[1], attackDefense: TacticalTables.TacticalBattleAttack[1] - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole12);
                        break;
                    default:
                        break;
                }
            }

            switch (message.MessageId)
            {
                case 135169:
                    if (base.Resource.TerrainList.GetTerrain(message.HexNumber) >= 0 && message.Param[0].Int < GetResource<Resource>().Defense)
                    {
                        MapFieldMessageParameters param2 = message.Param;
                        param2[0] = (sbyte)GetResource<Resource>().Defense;
                        message.Param = param2;
                    }

                    break;
                default:
                    break;
            }
        }
    }

    // CombatTerrain.pas:291
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(59))
        {
            if (base.Resource.TerrainList.GetOverlay(info.HexNumber) == 8)
            {
                moveResult.Code = -1;
                moveResult.Done = true;
            }
        }
    }

    // CombatTerrain.pas:254
    public override void Activate()
    {
        base.Activate();
        _hitPoints = (byte)base.Resource.DecorationType is >= 1 and <= 3 ? 3 : 0;

        _defense = -2;
        CreateShadow();
    }

    // CombatTerrain.pas:263
    public override void Deactivate()
    {
        DestroyShadow();
        base.Deactivate();
    }

    // CombatTerrain.pas:284
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        base.UpdateMapField(info);
        if (info.HexNumber < base.Resource.TerrainList.Count)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOnFlag;
        }
    }

    // CombatTerrain.pas:124
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!Status.Contains(MapObjectStatuses.Visible))
        {
            base.MakeVisible(visibilityList);
            if (_shadow is not null)
            {
                _ = visibilityList.AddSortChild(sortValue: _shadow.GetShowPriority(), child: _shadow);
            }
        }
    }

    // CombatTerrain.pas:91
    public override MapObjectResourceList GetResourceList(GameEngine? engine) => GamePack.Current.CombatResources;

    // CombatTerrain.pas:106
    public override bool CanPlaceOnHexSprite(HexSprite hexSprite) => base.CanPlaceOnHexSprite(hexSprite) || hexSprite is AbstractRoad;

    // CombatTerrain.pas:96
    public void DestroyShadow()
    {
        if (_shadow is not null)
        {
            if (Visible)
            {
                _ = base.Resource.Engine.HexSpriteMap.DynamicVisibleHexSprite.RemoveChild(_shadow);
            }

            _shadow?.Dispose();
            _shadow = null;
        }
    }

    // CombatTerrain.pas:111
    public void CreateShadow()
    {
        if (base.Resource.ImageSequenceList.GetImageSequence(40) is not null)
        {
            if (_shadow is not null)
            {
                DestroyShadow();
            }

            if (StrategicMap.Current is null || StrategicMap.Current.ShowUnitShadows)
            {
                _shadow = new CombatTerrainShadow(null)
                {
                    Holder = CreationHolder(),
                    Terrain = this
                };
            }
        }
    }

    // CombatTerrain.pas:133
    public void SetHitPoints(int value)
    {
    }

    // CombatTerrain.pas:138
    public void ShowShadow(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out int x, out int y);
        pixelXSurface = x - pixelXSurface;
        pixelYSurface = y - pixelYSurface;
        base.Resource.ShowComplexImage(pixelXSurface, pixelYSurface, lockedSurface, index: 40, offset: (uint)(Owner.HexX + (Owner.HexY << 4)));
    }

    public new class Resource(CustomNode owner) : TerrainMapObjectResource(owner)
    {

        // CombatTerrain.pas:301
        public new const int ClassId = 2_228_583;

        private sbyte _burnValue;

        public int Defense { get; private set; }

        private int _hitPoints;
        private bool _meleeTarget;

        // CombatTerrain.pas:310
        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            Defense = persister.UnsignedByte(name: "Defense", value: Defense.TruncateToSByte());
            _burnValue = persister.UnsignedByte(name: "BurnValue", value: _burnValue);
            _hitPoints = persister.Byte("HitPoints", value: _hitPoints.TruncateToByte());
            _meleeTarget = persister.Boolean("MeleeTarget", value: _meleeTarget);
        }

        // CombatTerrain.pas:304
        public override int ResourceUserClassId() => CombatTerrain.ClassId;

        // CombatTerrain.pas:307
        public override string ResourceEditName() => "Combat Terrain";

    }
}
