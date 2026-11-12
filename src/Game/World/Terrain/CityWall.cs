// Translated from CityWall.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

public partial class CityWall : CombatImageLibraryTerrain
{
    public CityWall(ECustomNode owner) : base(owner)
    { }

    // CityWall.pas:78
    public new const int ClassId = 2229508;

    public int HitPoints { get; private set; }

    private int _damageImage;
    private int _flagId;
    private bool _playSound;

    public byte WallType { get; private set; }

    // CityWall.pas:133
    public override sbyte GetOverlay(int index)
    {
        sbyte result;
        if (index == 0)
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
            result = base.GetOverlay(index);
        }

        return result;
    }

    // CityWall.pas:230
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
                    var defense = GetDefense();
                    var executeDamageRole = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - defense);
                    SetHitPoints(HitPoints - executeDamageRole);
                    break;
                case 200705:
                    GamePack.Current.AbilityControl.GetAbility(24).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    var defense2 = GetDefense();
                    var executeDamageRole2 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - defense2);
                    SetHitPoints(HitPoints - executeDamageRole2);
                    break;
                case 200706:
                    GamePack.Current.AbilityControl.GetAbility(44).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    var defense3 = GetDefense();
                    var executeDamageRole3 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - defense3);
                    SetHitPoints(HitPoints - executeDamageRole3);
                    break;
                case 200707:
                    GamePack.Current.SpellControl.GetSpell(117).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    var defense4 = GetDefense();
                    var executeDamageRole4 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense4);
                    SetHitPoints(HitPoints - executeDamageRole4);
                    break;
                case 200714:
                    if (WallType == 1)
                    {
                        GamePack.Current.AbilityControl.GetAbility(31).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref abilityInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        var defense5 = GetDefense();
                        var executeDamageRole5 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, abilityInfo.Attack - defense5);
                        SetHitPoints(HitPoints - executeDamageRole5);
                    }

                    break;
                case 200715:
                    if (WallType == 1)
                    {
                        GamePack.Current.SpellControl.GetSpell(113).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        var defense5 = GetDefense();
                        var executeDamageRole5 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense5);
                        SetHitPoints(HitPoints - executeDamageRole5);
                    }

                    break;
                case 200710:
                    GamePack.Current.SpellControl.GetSpell(116).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    var defense6 = GetDefense();
                    var executeDamageRole6 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense6);
                    SetHitPoints(HitPoints - executeDamageRole6);
                    break;
                case 200717:
                    GamePack.Current.SpellControl.GetSpell(127).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    var defense7 = GetDefense();
                    var executeDamageRole7 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense7);
                    SetHitPoints(HitPoints - executeDamageRole7);
                    break;
                case 200718:
                    if (WallType == 1)
                    {
                        GamePack.Current.SpellControl.GetSpell(128).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        var defense5 = GetDefense();
                        var executeDamageRole5 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense5);
                        SetHitPoints(HitPoints - executeDamageRole5);
                    }

                    break;
                case 200708:
                    _playSound = message.Param[1].Int == 0;
                    GamePack.Current.SpellControl.GetSpell(112).GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, ref spellInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    var defense8 = GetDefense();
                    var executeDamageRole8 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, spellInfo.Attack - defense8);
                    SetHitPoints(HitPoints - executeDamageRole8);
                    message.Param[1] = 1;
                    break;
                case 200711:
                    if (WallType == 1)
                    {
                        RandSeed = StrategicMap.Current.Random(16777215);
                        var defense5 = GetDefense();
                        var executeDamageRole5 = AbilityRules.ExecuteDamageRole(TacticalTables.TacticalBattleDamage[1], TacticalTables.TacticalBattleAttack[1] - defense5);
                        SetHitPoints(HitPoints - executeDamageRole5);
                    }

                    break;
            }
        }
    }

    // CityWall.pas:155
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        if (info.HexNumber == 0)
        {
            if (HitPoints == 0)
            {
                moveResult.Code = ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][6 - MovementTables.FirstOverlay];
            }
            else
            {
                if (((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModeElement.Wall))
                {
                    moveResult.Code = 25;
                    moveResult.Done = true;
                }
                else
                {
                    if (((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModeElement.WallCrushing) && GetHexX() == ((MoveControl)info.Sender).DestinationX && GetHexY() == ((MoveControl)info.Sender).DestinationY)
                    {
                        moveResult.Code = 0;
                    }
                    else
                    {
                        if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(1) || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(66) || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(36))
                        {
                            moveResult.Code = ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][-1 - MovementTables.FirstOverlay];
                        }
                        else
                        {
                            if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(59))
                            {
                                moveResult.Code = -1;
                            }
                            else
                            {
                                moveResult.Code = ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][8 - MovementTables.FirstOverlay];
                            }
                        }
                    }
                }
            }

            moveResult.Done = true;
        }
    }

    // CityWall.pas:180
    public override int CanMoveOver(in HexSpriteSenderInfo info)
    {
        int result;
        if (info.HexNumber == 0)
        {
            if (HitPoints == 0 || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(66) || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(36) || ((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModeElement.Climbing) || ((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModeElement.Wall))
            {
                result = 1;
            }
            else
            {
                if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(59))
                {
                    result = 0;
                }
                else
                {
                    if (((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][8 - MovementTables.FirstOverlay] != 0)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
        }
        else
        {
            result = 1;
        }

        return result;
    }

    // CityWall.pas:81
    public override void Activate()
    {
        PlayerStructure structure;
        bool razed;
        base.Activate();
        if (!CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            razed = false;
            _playSound = true;
            structure = (PlayerStructure)StrategicMap.Current.MapContainer.GetMapLevel(CombatPlacement.CombatLLocation).GetField(CombatPlacement.CombatXLocation, CombatPlacement.CombatYLocation).FindNoneTransparentHexSprite(typeof(City));
            if (structure != null)
            {
                WallType = ((City)structure).WallType;
                razed = structure.GetRazed();
                if (structure.Player == -1)
                {
                    _flagId = -1;
                }
                else
                {
                    _flagId = StrategicMap.Current.Players.GetPlayers(structure.Player).PlayerRace;
                }
            }
            else
            {
                structure = (PlayerStructure)StrategicMap.Current.MapContainer.GetMapLevel(CombatPlacement.CombatLLocation).GetField(CombatPlacement.CombatXLocation, CombatPlacement.CombatYLocation).FindNoneTransparentHexSprite(typeof(Tower));
                if (structure != null)
                {
                    WallType = 1;
                    razed = structure.GetRazed();
                    if (structure.Player == -1)
                    {
                        _flagId = -1;
                    }
                    else
                    {
                        _flagId = StrategicMap.Current.Players.GetPlayers(structure.Player).PlayerRace;
                    }
                }
                else
                {
                    WallType = 0;
                }
            }

            if (WallType == 0)
            {
                RemoveFromMap();
                var hexY = GetHexY();
                var hexX = GetHexX();
                UpdateMapFields(CombatMap.Current.MapContainer.GetMapLevel(0), hexX, hexY);
                this.Dispose();
            }
            else
            {
                if (razed)
                {
                    SetHitPoints(0);
                }
                else
                {
                    SetHitPoints(TacticalTables.WallMaxHitPoints[WallType - 1]);
                }

                if (_flagId == -1)
                {
                    _flagId = 32;
                }

                CombatMap.Current.Walls.Add(this);
            }
        }
        else
        {
            WallType = 2;
        }
    }

    // CityWall.pas:147
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        base.UpdateMapField(info);
        if (info.HexNumber == 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | 16384 | 8192;
        }
    }

    // CityWall.pas:141
    public override bool ValidTerrainType(int terrainType) => true;

    // CityWall.pas:144
    public override int GetTerrainTypeImage(int terrainType) => 0;

    // CityWall.pas:363
    public override void ShowShadow(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
    }

    // CityWall.pas:130
    public byte GetDirection() => ((Resource)_resource).Direction;

    // CityWall.pas:197
    public new int GetDefense() => -2;

    // CityWall.pas:200
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
                CombatMap.Current.Combat.CombatData.WallBreached = true;
                if (_playSound && CombatMap.Current.PlaySounds)
                {
                    AudioPlayback.PlaySample("SFX\\HIT_DEATH\\D_WOODLG.WAV");
                }

                effect = CombatMap.Current.AddEffect(BattleEffectType.WallSmoke);
                point = new Point3D();
                effect.Path.Add(point);
                HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, ref point.X, ref point.Y);
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
        if (TacticalTables.WallMaxHitPoints[WallType - 1] == HitPoints)
        {
            _damageImage = 0;
        }
        else
        {
            if (TacticalTables.WallMaxHitPoints[WallType - 1] / 2 < HitPoints)
            {
                _damageImage = 1;
            }
            else
            {
                if (HitPoints > 0)
                {
                    _damageImage = 2;
                }
                else
                {
                    _damageImage = 3;
                }
            }
        }

        _playSound = true;
    }

    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int imageIndex;
        int pixelX = default;
        int pixelY = default;
        int terrainOffset;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, ref pixelX, ref pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        var hexY = GetHexY();
        var hexX = GetHexX();
        switch (CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).TerrainType)
        {
            case 3:
                terrainOffset = 10;
                break;
            case 5:
            case 7:
            case 12:
                terrainOffset = 20;
                break;
            default:
                terrainOffset = 0;
                break;
        }

        sequence = _resource.ImageSequenceList.GetImageSequence(WallIsIndex[WallType - 1] + (terrainOffset + _damageImage));
        if (sequence != null)
        {
            do
            {
                switch (sequence.Layer)
                {
                    case 7:
                        imageIndex = sequence.GetFrameIndex(CombatMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (imageIndex != -1)
                        {
                            if (!CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                            {
                                var hexX2 = GetHexX();
                                var hexY2 = GetHexY();
                                GamePack.Current.FlagControl.ShowFlag(pixelXSurface + 21 + sequence.OffsetX, pixelYSurface + sequence.OffsetY + 16, hexX2 + StrategicMap.Current.Frame + hexY2 * 100, (byte)_flagId, lockedSurface, false);
                            }
                        }

                        break;
                    default:
                        {
                            imageIndex = sequence.GetUnloopedFrameIndex(ImageIndex);
                            if (imageIndex != -1)
                            {
                                sequence.ImageLibrary[imageIndex].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface);
                            }
                        }

                        break;
                }

                sequence = sequence.Next;
            }
            while (!(sequence == null));
        }
    }

    private static ReadOnlySpan<int> WallIsIndex => [30, 0];

    public new partial class Resource : CombatImageLibraryTerrainResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // CityWall.pas:366
        public new const int ClassId = 2229509;

        public byte Direction;

        // CityWall.pas:369
        public override void ReadWrite(StorageStream stream)
        {
            base.ReadWrite(stream);
            stream.Byte(ref Direction, 25);
        }

        // CityWall.pas:378
        public override int ResourceUserClassId() => 2229508;

        // CityWall.pas:375
        public override string ResourceEditName() => "Combat City Wall";

    }
}
