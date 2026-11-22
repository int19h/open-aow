#nullable disable warnings
// Translated from CityWall.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
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
using Aow.Rtl;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Terrain;

public class CityWall(CustomNode? owner) : CombatImageLibraryTerrain(owner)
{

    // CityWall.pas:78
    public new const int ClassId = 2_229_508;

    public int HitPoints { get; private set; }

    private int _damageImage;
    private int _flagId;
    private bool _playSound;

    public byte WallType { get; private set; }

    // CityWall.pas:133
    public override sbyte GetOverlay(int index) => index == 0 ? HitPoints == 0 ? (sbyte)6 : (sbyte)8 : base.GetOverlay(index);

    // CityWall.pas:230
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.HexNumber == 0)
        {
            AbilityCombatInfo abilityInfo = default;
            SpellCombatInfo spellInfo = default;
            switch (message.MessageId)
            {
                case 200704:
                    GamePack.Current.AbilityControl[65].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref abilityInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    int defense = GetDefense();
                    int executeDamageRole = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, attackDefense: abilityInfo.Attack - defense);
                    SetHitPoints(HitPoints - executeDamageRole);
                    break;
                case 200705:
                    GamePack.Current.AbilityControl[24].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref abilityInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    int defense2 = GetDefense();
                    int executeDamageRole2 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, attackDefense: abilityInfo.Attack - defense2);
                    SetHitPoints(HitPoints - executeDamageRole2);
                    break;
                case 200706:
                    GamePack.Current.AbilityControl[44].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref abilityInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    int defense3 = GetDefense();
                    int executeDamageRole3 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, attackDefense: abilityInfo.Attack - defense3);
                    SetHitPoints(HitPoints - executeDamageRole3);
                    break;
                case 200707:
                    GamePack.Current.SpellControl[117].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref spellInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    int defense4 = GetDefense();
                    int executeDamageRole4 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, attackDefense: spellInfo.Attack - defense4);
                    SetHitPoints(HitPoints - executeDamageRole4);
                    break;
                case 200714:
                    if (WallType == 1)
                    {
                        GamePack.Current.AbilityControl[31].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref abilityInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int defense5 = GetDefense();
                        int executeDamageRole5 = AbilityRules.ExecuteDamageRole(abilityInfo.Damage, attackDefense: abilityInfo.Attack - defense5);
                        SetHitPoints(HitPoints - executeDamageRole5);
                    }

                    break;
                case 200715:
                    if (WallType == 1)
                    {
                        GamePack.Current.SpellControl[113].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref spellInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int defense5 = GetDefense();
                        int executeDamageRole5 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, attackDefense: spellInfo.Attack - defense5);
                        SetHitPoints(HitPoints - executeDamageRole5);
                    }

                    break;
                case 200710:
                    GamePack.Current.SpellControl[116].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref spellInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    int defense6 = GetDefense();
                    int executeDamageRole6 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, attackDefense: spellInfo.Attack - defense6);
                    SetHitPoints(HitPoints - executeDamageRole6);
                    break;
                case 200717:
                    GamePack.Current.SpellControl[127].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref spellInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    int defense7 = GetDefense();
                    int executeDamageRole7 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, attackDefense: spellInfo.Attack - defense7);
                    SetHitPoints(HitPoints - executeDamageRole7);
                    break;
                case 200718:
                    if (WallType == 1)
                    {
                        GamePack.Current.SpellControl[128].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref spellInfo);
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int defense5 = GetDefense();
                        int executeDamageRole5 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, attackDefense: spellInfo.Attack - defense5);
                        SetHitPoints(HitPoints - executeDamageRole5);
                    }

                    break;
                case 200708:
                    _playSound = message.Param[1].Int == 0;
                    GamePack.Current.SpellControl[112].GetCombatInfo(message.Param[0].As<CombatUnit>().Troop, info: ref spellInfo);
                    RandSeed = StrategicMap.Current.Random(16777215);
                    int defense8 = GetDefense();
                    int executeDamageRole8 = AbilityRules.ExecuteDamageRole(spellInfo.Damage, attackDefense: spellInfo.Attack - defense8);
                    SetHitPoints(HitPoints - executeDamageRole8);
                    MapFieldMessageParameters param = message.Param;
                    param[1] = 1;
                    message.Param = param;
                    break;
                case 200711:
                    if (WallType == 1)
                    {
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int defense5 = GetDefense();
                        int executeDamageRole5 = AbilityRules.ExecuteDamageRole(TacticalTables.TacticalBattleDamage[1], attackDefense: TacticalTables.TacticalBattleAttack[1] - defense5);
                        SetHitPoints(HitPoints - executeDamageRole5);
                    }

                    break;
                default:
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
                if (((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModes.Wall))
                {
                    moveResult.Code = 25;
                    moveResult.Done = true;
                }
                else
                {
                    moveResult.Code = ((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModes.WallCrushing) && HexX == ((MoveControl)info.Sender).Destination.X && HexY == ((MoveControl)info.Sender).Destination.Y
                        ? 0
                        : ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(1) || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(66) || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(36)
                            ? ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][-1 - MovementTables.FirstOverlay]
                            : ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(59)
                                ? -1
                                : ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][8 - MovementTables.FirstOverlay];
                }
            }

            moveResult.Done = true;
        }
    }

    // CityWall.pas:180
    public override int CanMoveOver(in HexSpriteSenderInfo info)
    {
        return info.HexNumber == 0
            ? HitPoints == 0 || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(66) || ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(36) || ((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModes.Climbing) || ((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModes.Wall)
                ? 1
                : ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(59)
                    ? 0
                    : ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][8 - MovementTables.FirstOverlay] != 0 ? 1 : 0
            : 1;
    }

    // CityWall.pas:81
    public override void Activate()
    {
        base.Activate();
        if (!CombatMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            bool razed = false;
            _playSound = true;
            MapField field = StrategicMap.Current.MapContainer.GetMapLevel(CombatPlacement.CombatLLocation).GetField(CombatPlacement.CombatXLocation, CombatPlacement.CombatYLocation);
            City city = field.FindNoneTransparentHexSprite<City>();
            if (city is not null)
            {
                WallType = city.WallType;
                razed = city.GetRazed();
                _flagId = city.Player == -1 ? -1 : StrategicMap.Current.Players.GetPlayers(city.Player).PlayerRace;
            }
            else
            {
                Tower tower = field.FindNoneTransparentHexSprite<Tower>();
                if (tower is not null)
                {
                    WallType = 1;
                    razed = tower.GetRazed();
                    _flagId = tower.Player == -1 ? -1 : StrategicMap.Current.Players.GetPlayers(tower.Player).PlayerRace;
                }
                else
                {
                    WallType = 0;
                }
            }

            if (WallType == 0)
            {
                RemoveFromMap();
                int hexY = HexY;
                int hexX = HexX;
                UpdateMapFields(CombatMap.Current.MapContainer.GetMapLevel(0), hexX, hexY);
                Dispose();
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
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOnFlag | MapField.MoveOverFlag;
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
    public int Direction => GetResource<Resource>().Direction;

    // CityWall.pas:197
    public new int GetDefense() => -2;

    // CityWall.pas:200
    public void SetHitPoints(int value)
    {
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
                    _ = AudioPlayback.PlaySample("SFX\\HIT_DEATH\\D_WOODLG.WAV");
                }

                BattleEffectSprite effect = CombatMap.Current.AddEffect(BattleEffectType.WallSmoke);
                Point3D point = new();
                effect.Path.Add(point);
                _ = point.X;
                _ = point.Y;
                HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int x, out int y);
                point.X = x;
                point.Y = y;
                point.X = Random(20) + point.X + 6;
                point.Y = Random(20) + point.Y - 20;
            }
            else
            {
                if (_playSound && CombatMap.Current.PlaySounds)
                {
                    _ = AudioPlayback.PlaySample("SFX\\HIT_DEATH\\D_WOODMD.WAV");
                }
            }
        }

        HitPoints = value;
        _damageImage = TacticalTables.WallMaxHitPoints[WallType - 1] == HitPoints
            ? 0
            : TacticalTables.WallMaxHitPoints[WallType - 1] / 2 < HitPoints ? 1 : HitPoints > 0 ? 2 : 3;

        _playSound = true;
    }

    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        int hexY = HexY;
        int hexX = HexX;
        int terrainOffset = CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).TerrainType switch
        {
            3 => 10,
            5 or 7 or 12 => 20,
            _ => 0,
        };
        ImageSequence? sequence = base.Resource.ImageSequenceList.GetImageSequence(WallIsIndex[WallType - 1] + terrainOffset + _damageImage);
        if (sequence is not null)
        {
            do
            {
                int imageIndex;
                switch (sequence.Layer)
                {
                    case 7:
                        imageIndex = sequence.GetFrameIndex(CombatMap.Current.Frame, offset: FrameFixtureInputs.AnimationOffset(this));
                        if (imageIndex != -1)
                        {
                            if (!CombatMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
                            {
                                int hexX2 = HexX;
                                int hexY2 = HexY;
                                GamePack.Current.FlagControl.ShowFlag(pixelXSurface + 21 + sequence.OffsetX, pixelYSurface + sequence.OffsetY + 16, frameOffset: hexX2 + StrategicMap.Current.Frame + (hexY2 * 100), flagId: (byte)_flagId, lockedSurface, showBase: false);
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
            while (sequence is not null);
        }
    }

    private static ReadOnlySpan<int> WallIsIndex => [30, 0];

    public new class Resource(CustomNode owner) : CombatImageLibraryTerrainResource(owner)
    {

        // CityWall.pas:366
        public new const int ClassId = 2_229_509;

        public int Direction { get; private set; }

        // CityWall.pas:369
        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            Direction = persister.Byte("Direction", value: Direction.TruncateToByte());
        }

        // CityWall.pas:378
        public override int ResourceUserClassId() => CityWall.ClassId;

        // CityWall.pas:375
        public override string ResourceEditName() => "Combat City Wall";

    }
}
