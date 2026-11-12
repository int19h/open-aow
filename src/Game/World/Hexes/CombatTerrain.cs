// Translated from CombatTerrain.pas.
using System.Runtime.CompilerServices;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

public partial class CombatTerrain : TerrainMapObject
{
    public CombatTerrain(ECustomNode owner) : base(owner)
    { }

    // CombatTerrain.pas:82
    public new const int ClassId = 2228582;

    private CombatTerrainShadow _shadow;
    private int _hitPoints;

    public bool Damaged { get; private set; }

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
            var hexY = HexY;
            var hexX = HexX;
            UpdateMapFields(Owner.Owner, hexX, hexY);
        }

        return result;
    }

    // CombatTerrain.pas:275
    public override void RemoveFromMap()
    {
        MapLevel mapLevel;
        mapLevel = Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(mapLevel, _hexX, _hexY);
    }

    // CombatTerrain.pas:149
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        AbilityCombatInfo abilityInfo = default;
        SpellCombatInfo spellInfo = default;
        base.MapFieldMessageProc(ref message);
        if (message.HexNumber == 0)
        {
            if (Set<byte>.WithBounds(0, 255).WithRange(1, 3).Contains((byte)((TerrainMapObjectResource)_resource).DecorationType))
            {
                switch (message.MessageId)
                {
                    case 200704:
                        AoWHSSet.Current.AbilityControl.GetAbility(65).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var executeDamageRole = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole);
                        break;
                    case 200705:
                        AoWHSSet.Current.AbilityControl.GetAbility(24).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var executeDamageRole2 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole2);
                        break;
                    case 200706:
                        AoWHSSet.Current.AbilityControl.GetAbility(44).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var executeDamageRole3 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole3);
                        break;
                    case 200707:
                        AoWHSSet.Current.SpellControl.GetSpell(117).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var executeDamageRole4 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole4);
                        break;
                    case 200714:
                        AoWHSSet.Current.AbilityControl.GetAbility(31).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var executeDamageRole5 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole5);
                        break;
                    case 200715:
                        AoWHSSet.Current.SpellControl.GetSpell(113).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var executeDamageRole6 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole6);
                        break;
                    case 200722:
                        if (message.Param[1].Int == 30 || message.Param[1].Int == 86)
                        {
                            AoWHSSet.Current.AbilityControl.GetAbility(message.Param[1].Int).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                            RandSeed = AoWHSMap.Current.Random(16777215);
                            var executeDamageRole7 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - _defense);
                            SetHitPoints(_hitPoints - executeDamageRole7);
                        }

                        break;
                    case 200723:
                        if (message.Param[1].Int == 114)
                        {
                            AoWHSSet.Current.SpellControl.GetSpell(message.Param[1].Int).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                            RandSeed = AoWHSMap.Current.Random(16777215);
                            var executeDamageRole7 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - _defense);
                            SetHitPoints(_hitPoints - executeDamageRole7);
                        }

                        break;
                    case 200710:
                        AoWHSSet.Current.SpellControl.GetSpell(116).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var executeDamageRole8 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole8);
                        break;
                    case 200717:
                        AoWHSSet.Current.SpellControl.GetSpell(127).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var executeDamageRole9 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole9);
                        break;
                    case 200718:
                        AoWHSSet.Current.SpellControl.GetSpell(128).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var executeDamageRole10 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole10);
                        break;
                    case 200708:
                        Damaged = message.Param[1].Int == 0;
                        AoWHSSet.Current.SpellControl.GetSpell(112).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var executeDamageRole11 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole11);
                        message.Param[1] = 1;
                        break;
                    case 200711:
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        var executeDamageRole12 = AbilityRules.ExecuteDamageRole(TacticalTables.s_tacticalBattleDamage[1], TacticalTables.s_tacticalBattleAttack[1] - _defense);
                        SetHitPoints(_hitPoints - executeDamageRole12);
                        break;
                }
            }

            switch (message.MessageId)
            {
                case 135169:
                    if (_resource.TerrainList.GetTerrain(message.HexNumber) >= 0 && message.Param[0].Int < ((Resource)_resource).Defense)
                    {
                        message.Param[0] = ((Resource)_resource).Defense;
                    }

                    break;
            }
        }
    }

    // CombatTerrain.pas:291
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(59))
        {
            if (_resource.TerrainList.GetOverlay(info.HexNumber) == 8)
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
        if (Set<byte>.WithBounds(0, 255).WithRange(1, 3).Contains((byte)((TerrainMapObjectResource)_resource).DecorationType))
        {
            _hitPoints = 3;
        }
        else
        {
            _hitPoints = 0;
        }

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
        if (info.HexNumber < _resource.TerrainList.Count)
        {
            ((AoWMapField)info.MapField).FieldFlags = ((AoWMapField)info.MapField).FieldFlags | 16384;
        }
    }

    // CombatTerrain.pas:124
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!Status.Contains(MapObjectStatusElement.Visible))
        {
            base.MakeVisible(visibilityList);
            if (_shadow != null)
            {
                var showPriority = _shadow.GetShowPriority();
                visibilityList.AddSortChild(_shadow, showPriority);
            }
        }
    }

    // CombatTerrain.pas:91
    public override MapObjectResourceList GetResourceList(GameEngine engine) => AoWHSSet.Current.CombatResources;

    // CombatTerrain.pas:106
    public override bool CanPlaceOnHexSprite(HexSprite hexSprite) => base.CanPlaceOnHexSprite(hexSprite) || hexSprite is AbstractRoad;

    // CombatTerrain.pas:96
    public void DestroyShadow()
    {
        if (_shadow != null)
        {
            if (GetVisible())
            {
                _resource.Engine.HexSpriteMap.DynamicVisibleHexSprite.RemoveChild(_shadow);
            }

            _shadow?.Dispose();
            _shadow = null;
        }
    }

    // CombatTerrain.pas:111
    public void CreateShadow()
    {
        if (_resource.ImageSequenceList.GetImageSequence(40) != null)
        {
            if (_shadow != null)
            {
                DestroyShadow();
            }

            if (AoWHSMap.Current == null || AoWHSMap.Current.ShowUnitShadows)
            {
                _shadow = new CombatTerrainShadow(null);
                _shadow._terrain = this;
            }
        }
    }

    // CombatTerrain.pas:133
    public void SetHitPoints(int value)
    {
        // { Original ignores Value and only marks the terrain damaged. }
        Damaged = true;
    }

    // CombatTerrain.pas:138
    public void ShowShadow(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        HexGeometry.HexToPixel(_hexX, _hexY, ref X, ref Y);
        pixelXSurface = X - pixelXSurface;
        pixelYSurface = Y - pixelYSurface;
        _resource.ShowComplexImage(pixelXSurface, pixelYSurface, lockedSurface, 40, (uint)(Owner.HexX + (Owner.HexY << 4)));
    }

    public new partial class Resource : TerrainMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // CombatTerrain.pas:301
        public new const int ClassId = 2228583;

        private sbyte _burnValue;
        public sbyte Defense;
        private byte _hitPoints;
        private bool _meleeTarget;

        // CombatTerrain.pas:310
        public override void ReadWrite(EStorageStream stream)
        {
            base.ReadWrite(stream);
            stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Defense), 20);
            stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _burnValue), 21);
            stream.rwByte(ref _hitPoints, 22);
            stream.rwBool(ref _meleeTarget, 23);
        }

        // CombatTerrain.pas:304
        public override int ResourceUserClassId() => 2228582;

        // CombatTerrain.pas:307
        public override string ResourceEditName() => "Combat Terrain";

    }

}
