#nullable disable warnings
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine;
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Draw.Images;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;

using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.World.Structures;

// ID, update count, building player, days left, shadow, fogged
public partial class Structure(CustomNode? owner) : FixedImageLibraryTerrainMapObject(owner)
{

    public new const int ClassId = 131_914;

    public override StructureResource Resource => GetResource<StructureResource>();

    public int StructureId { get; set; }

    public int UpdateCount { get; private set; }

    public PlayerId? BuildingPlayer { get; set; } = null;
    public int BuildingDays { get; private set; }

    private StructureShadow? _shadow;

    public bool Fogged { get; private set; } = true;

    // Structur.PAS:1017
    public override void Dispose()
    {
        if (_shadow is not null)
        {
            DestroyShadow();
        }

        if (Owner is not null && Status.Contains(MapObjectStatuses.ResourceLinked) && Resource.TerrainBound)
        {
            _ = Owner.DestroyChildren(Pad.ClassId);
        }

        base.Dispose();
        if (StrategicMap.Current is not null && !StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Destroying))
        {
            StrategicMap.Current.NotifyStructureDestroyed(this);
        }
    }

    // Structur.PAS:1038
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        BuildingDays = persister.Byte("BuildingDays", value: BuildingDays.TruncateToByte());
        StructureId = persister.Int32("StructureId", value: StructureId);
        BuildingPlayer = persister.OptionalPlayer(name: "BuildingPlayer", value: BuildingPlayer, fallback: 255, width: IdWidth.UnsignedByte);
    }

    // Structur.PAS:1629
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001985)
        {
            NewDay();
        }
        else
        {
            if (message.MessageId == 537001986)
            {
                NewTurn(PlayerId.FromInt32((sbyte)message.Payload.Int));
            }
            else
            {
                if (message.MessageId == 537001990)
                {
                    SeatedPlayerChanged();
                }
                else
                {
                    if (message.MessageId == 537002243)
                    {
                        if (!MainValidateMap(message.Payload.As<DelphiStringCollection>()))
                        {
                            Array4<int> data = message.Data;
                            data[0] = 1;
                            message.Data = data;
                        }
                    }
                }
            }
        }
    }

    // Structur.PAS:1281
    public override int GetTerrain(int index)
    {
        int result = Resource.TerrainList.Terrain[index];
        return result >= 0 ? TerrainType : result;
    }

    // Structur.PAS:1275
    public override int GetOverlay(int index)
    {
        int result = Resource.TerrainList.Terrain[index];
        return result >= 0 ? 7 : result;
    }

    // Structur.PAS:1287
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info) => Resource.TerrainBound || base.CanChangeTerrain(info);

    // Structur.PAS:1297
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        if (Resource.TerrainList.Terrain[info.HexNumber] >= 0)
        {
            if (Resource.TerrainBound)
            {
                TerrainType = info.TerrainType;
                ImageIndex = GetTerrainTypeImage(TerrainType);
            }
            else
            {
                base.TerrainChanged(info);
            }
        }
    }

    // Structur.PAS:1647
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out int pixelX, out int pixelY);
        Resource.Show(pixelX - pixelXSurface, pixelY - pixelYSurface, lockedSurface, structure: this);
    }

    // Structur.PAS:1092
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        if (!EditMode)
        {
            UpdateRoads(Owner);
        }

        return result;
    }

    // Structur.PAS:1098
    public override void RemoveFromMap()
    {
        MapField? mapField = Owner;
        base.RemoveFromMap();
        if (mapField is not null && !EditMode)
        {
            UpdateRoads(mapField);
        }
    }

    // Structur.PAS:1599
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (Resource.TerrainList.Terrain[message.HexNumber] >= 0)
        {
            if (message.MessageId == 4358)
            {
                FogChanged();
            }
            else
            {
                if (message.MessageId == 4608)
                {
                    AITargetData data = message.Param[0].As<AITargetQuery>().Data;
                    UpdateAITarget(message.Param[0].As<AITargetQuery>().Info, ref data);
                    message.Param[0].As<AITargetQuery>().Data = data;
                    if (message.Param[0].As<AITargetQuery>().Data.Priority < 0)
                    {
                        message.Result = 1;
                    }
                }
                else
                {
                    if (message.MessageId == 4388)
                    {
                        ArmyChanged(message.Param[0].As<ArmyHexSprite>());
                    }
                    else
                    {
                        if (message.MessageId == 4354)
                        {
                            ArmyPlaced(message.Param[0].As<ArmyHexSprite>());
                        }
                        else
                        {
                            if (message.MessageId == 4355)
                            {
                                ArmyRemoved(message.Param[0].As<ArmyHexSprite>());
                            }
                            else
                            {
                                if (message.MessageId == 4401)
                                {
                                    if (Resource.ConnectRoads)
                                    {
                                        MapFieldMessageParameters param = message.Param;
                                        param[1] = 1;
                                        message.Param = param;
                                    }
                                }
                                else
                                {
                                    if (message.MessageId == 4386)
                                    {
                                        message.Result = Resource.ScannerIcon;
                                    }
                                    else
                                    {
                                        if (message.MessageId == 4390)
                                        {
                                            DefensePriority defensePriority = GetDefensePriority();
                                            message.Result = (int)defensePriority;
                                        }
                                        else
                                        {
                                            if (message.MessageId == 4391)
                                            {
                                                DefenseRequirements requirements = message.Param[1].As<DefenseRequirementsQuery>().Requirements;
                                                bool stored = GetDefenseRequirements(PlayerId.FromOriginal((sbyte)message.Param[0].Int), ref requirements);
                                                message.Param[1].As<DefenseRequirementsQuery>().Requirements = requirements;
                                                if (stored)
                                                {
                                                    message.Result = 1;
                                                }
                                            }
                                            else
                                            {
                                                if (message.MessageId == 4364 && message.HexNumber == 0)
                                                {
                                                    if (Resource.TerrainList.Terrain[message.HexNumber] >= 0 && (!StrategicMap.Current.FogEnabled || !Fogged))
                                                    {
                                                        MapFieldMessageParameters param2 = message.Param;
                                                        param2[0] = param2[0].Int + 3584;
                                                        message.Param = param2;
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

    // Structur.PAS:1585
    public override void Activate()
    {
        base.Activate();
        StrategicMap.Current.StructureControl.Register(this);
        if (StrategicMap.Current.ShowUnitShadows && Resource.CastsShadow && !EditMode)
        {
            CreateShadow();
        }
    }

    // Structur.PAS:1593
    public override void Deactivate()
    {
        StrategicMap.Current.StructureControl.Unregister(this);
        base.Deactivate();
    }

    // Structur.PAS:1886
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (Resource.TerrainList.Terrain[info.HexNumber] >= 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.AITargetFlag;
        }
    }

    // Structur.PAS:1184
    public override bool CanSelect(int pixelX, int pixelY)
    {
        if (!StrategicMap.Current.FogEnabled || !Fogged)
        {
            HexGeometry.PixelToHex(pixelX, vertical: pixelY, out int x, out int y);
            int hexNumber = HexGeometry.DeltaHexToHexNumber(Owner.HexX, Owner.HexY, x, y);
            return hexNumber < Resource.TerrainList.Count && (Resource.TerrainList.Terrain[hexNumber] >= 0 || Resource.TerrainList.Overlay[hexNumber] >= 0);
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:1206
    public override void Select()
    {
        base.Select();
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            PlaySelectSample();
        }
    }

    // Structur.PAS:1212
    public override void Unselect() => base.Unselect();

    // Structur.PAS:1308
    public override bool CanPlace(MapContainer mapContainer, int hexX, int hexY, int hexLevel)
    {
        return Resource.TerrainBound
            ? GamePack.Current.PadControl.CanPlacePad(hexX, hexY, L: hexLevel, size: Resource.FlagType)
            : base.CanPlace(mapContainer, hexX, hexY, hexLevel);
    }

    // Structur.PAS:1315
    public override bool PlaceHex(MapContainer mapContainer, int hexX, int hexY, int hexLevel)
    {
        if (EditMode || CanPlace(mapContainer, hexX, hexY, hexLevel))
        {
            if (Resource.TerrainBound)
            {
                ImageLibraryTerrainMapObject? pad = !EditMode ? GamePack.Current.PadControl.PlacePad(hexX, hexY, L: hexLevel, size: Resource.FlagType) : null;

                if (pad is not null || EditMode)
                {
                    if (pad is not null)
                    {
                        TerrainType = pad.TerrainType;
                    }

                    return base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
            }
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:1292
    public override bool ValidTerrainType(int terrainType) => (Resource.TerrainBound && GamePack.Current.PadControl.TerrainTypes[terrainType]) || base.ValidTerrainType(terrainType);

    // Structur.PAS:1053
    public override int GetTerrainTypeImage(int terrainType)
    {
        return GetRazed()
            ? terrainType == 3 ? 23 : terrainType == 5 ? 24 : 22
            : Building ? terrainType == 3 ? 26 : terrainType == 5 ? 27 : 25 : base.GetTerrainTypeImage(terrainType);
    }

    // Structur.PAS:1159
    public override int ForceTerrainType(MapLevel mapLevel, int hexX, int hexY) => mapLevel.HexLevel != 0 ? 12 : -1;

    // Structur.PAS:1239
    public virtual LibraryImage? GetTopBorderImage()
    {
        ImageSequenceList? sequences = Resource.ImageSequenceList;
        return sequences is not null
            ? sequences.GetImageSequence(30) is not null ? Resource.ImageSequenceList.GetImageSequence(30).GetPreviewImage() : null
            : Resource.ImageLibrary.GetSafe(30);
    }

    // Structur.PAS:1251
    public virtual LibraryImage? GetMiddleBorderImage()
    {
        ImageSequenceList? sequences = Resource.ImageSequenceList;
        return sequences is not null
            ? sequences.GetImageSequence(31) is not null ? Resource.ImageSequenceList.GetImageSequence(31).GetPreviewImage() : null
            : Resource.ImageLibrary.GetSafe(31);
    }

    // Structur.PAS:1263
    public virtual LibraryImage? GetBottomBorderImage()
    {
        ImageSequenceList? sequences = Resource.ImageSequenceList;
        return sequences is not null
            ? sequences.GetImageSequence(32) is not null ? Resource.ImageSequenceList.GetImageSequence(32).GetPreviewImage() : null
            : Resource.ImageLibrary.GetSafe(32);
    }

    // Structur.PAS:1174
    public virtual DelphiStringCollection Description => Resource.Description;

    // Structur.PAS:1164
    public virtual string GetName() => EditName();

    // Structur.PAS:1346
    public virtual void SetRazed(bool value)
    {
    }

    // Structur.PAS:1341
    public virtual bool GetRazed() => false;

    // Structur.PAS:1336
    public virtual bool Razeable => Resource.Razeable;

    // Structur.PAS:1844
    public virtual bool RazeWithoutConfirmation(PlayerId? player)
    {
        string reason = "";
        if (CanRaze(player, ref reason) && !StrategicMap.Current.Players.GetPlayers(player).GetBusy())
        {
            StructureTokenEvent eventValue = CreateTokenEvent();
            SetupTokenEvent(eventValue);
            eventValue.Action = -1;
            eventValue.Param = PlayerIdExtensions.ToOriginal(player);
            StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:1755
    public virtual void SetupRazeDefenderAIGroup(UnitUserList attackers, UnitUserList defenders)
    {
        int unitsToIndependentRelation = UnitList.UnitsToIndependentRelation(defenders, defaultRelation: 5);
        HuntAIGroup group = StrategicMap.Current.AIGroupManager.CreateGroup<HuntAIGroup>(PlayerId.Independent, relation: unitsToIndependentRelation);
        _ = group.AddUnits(defenders.Units);
        for (int i = attackers.Count - 1; i >= 0; i--)
        {
            if (attackers.GetUnit(i).Player != PlayerId.Independent)
            {
                Troop? troop = attackers.GetUnit(i);
                _ = group.TargetUnitIds.Add(troop.UnitId);
            }
        }

        _ = group.Release();
    }

    // Structur.PAS:1130
    public virtual StructureTokenEvent CreateTokenEvent() => new(null) { Holder = CreationHolder() };

    // Structur.PAS:1135
    public virtual void SetupTokenEvent(StructureTokenEvent tokenEvent) => tokenEvent.Initialize(mapLevel: HexLevel, y: HexY, x: HexX, classId: EngineClass.ClassId);

    // Structur.PAS:1140
    public virtual ExecuteResult ExecuteTokenEvent(StructureTokenEvent tokenEvent, TokenExecutor executor)
    {
        if (tokenEvent.Action == -1)
        {
            return ExecuteRaze(PlayerId.FromOriginal((sbyte)tokenEvent.Param)) ? ExecuteResult.Done : ExecuteResult.DoneHalt;
        }
        else
        {
            if (tokenEvent.Action == -2)
            {
                Troop? troop = StrategicMap.Current.UnitControl.FindUnit(tokenEvent.Param);
                if (troop is not null && ExecuteRebuild(troop))
                {
                    troop.Killed();
                    return ExecuteResult.Done;
                }
                else
                {
                    return ExecuteResult.DoneHalt;
                }
            }
            else
            {
                return ExecuteResult.DoneHalt;
            }
        }
    }

    // Structur.PAS:1505
    public virtual bool GetDefenseRequirements(PlayerId? player, ref DefenseRequirements requirements) => false;

    // Structur.PAS:1502
    public virtual DefensePriority GetDefensePriority() => DefensePriority.None;

    // Structur.PAS:1476
    public virtual void NewDay()
    {
    }

    // Structur.PAS:1479
    public virtual void NewTurn(PlayerId player)
    {
        if (player == BuildingPlayer)
        {
            if (BuildingDays != 0)
            {
                BuildingDays = (BuildingDays - 1).TruncateToByte();
            }

            if (BuildingDays == 0)
            {
                BuildingDone();
            }
        }
    }

    // Structur.PAS:1497
    public virtual void SeatedPlayerChanged() => FogChanged();

    // Structur.PAS:1488
    public virtual void ArmyPlaced(ArmyHexSprite? placedArmy)
    {
    }

    // Structur.PAS:1491
    public virtual void ArmyRemoved(ArmyHexSprite? army)
    {
    }

    // Structur.PAS:1494
    public virtual void ArmyChanged(ArmyHexSprite? army)
    {
    }

    // Structur.PAS:1200
    public virtual void PlaySelectSample()
    {
        if (!GetRazed() && !IsEmpty(Resource.SelectSfx))
        {
            _ = AudioPlayback.PlaySample(Resource.SelectSfx);
        }
    }

    // Structur.PAS:1107
    public virtual void CreateShadow()
    {
        if (_shadow is not null)
        {
            DestroyShadow();
        }

        _shadow = new StructureShadow(null)
        {
            Holder = CreationHolder(),
            Structure = this
        };
        _shadow.Connect();
    }

    // Structur.PAS:1115
    public virtual void DestroyShadow()
    {
        if (_shadow is not null)
        {
            if (_shadow.ShadowStatus.Contains(StructureShadowStatuses.Connected))
            {
                _shadow.Disconnect();
            }

            _shadow?.Dispose();
            _shadow = null;
        }
    }

    // Structur.PAS:1639
    public virtual void ShowShadow(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out int pixelX, out int pixelY);
        _ = Resource.ImageSequenceList.Show(90, pixelX - pixelXSurface, pixelY - pixelYSurface, frame: 0, lockedSurface);
    }

    // Structur.PAS:1414
    public virtual void BuildingDone()
    {
        LocationTextEventLog log = new(null)
        {
            Holder = CreationHolder(),
            Player = BuildingPlayer
        };
        if (GetRazed())
        {
            string editName = EditName();
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_structureRebuiltFormat), arguments: [editName]);
        }
        else
        {
            string editName = EditName();
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_structureBuiltFormat), arguments: [editName]);
        }
        log.SetLocation(level: HexLevel, y: HexY, x: HexX);
        EventLog.DistributeLocationEventLog(y: HexY, level: HexLevel, x: HexX, log: log, player: BuildingPlayer, execute: false);
        _ = log.Release();
        SetRazed(false);
        BuildingPlayer = null;
        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Structur.PAS:1383
    public virtual bool ExecuteRebuild(Troop troop)
    {
        string reason = "";
        if (CanRebuild(troop, ref reason))
        {
            GetRebuildInfo(out int cost, out int time);
            Player? players = StrategicMap.Current.Players.GetPlayers(troop.Player);
            StrategicMap.Current.Players.GetPlayers(troop.Player).SetGems(players.Gems - cost);
            BuildingDays = time.TruncateToByte();
            BuildingPlayer = troop.Player;
            _ = ValidateObservation(troop.Player);
            ImageIndex = GetTerrainTypeImage(TerrainType);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:1400
    public virtual bool ExecuteBuild(Troop troop, int days)
    {
        BuildingDays = days.TruncateToByte();
        BuildingPlayer = troop.Player;
        ImageIndex = GetTerrainTypeImage(TerrainType);
        _ = ValidateObservation(troop.Player);
        return true;
    }

    // Structur.PAS:1734
    public virtual bool GenerateRazeDefenders(PlayerId? player, UnitList units)
    {
        int hexX = HexX;
        int hexY = HexY;
        RandSeed = hexX + StrategicMap.Current.InitialRandomSeed + hexY;
        int collection = Resource.Defenders.GetRandomCollection();
        if (collection != -1)
        {
            UnitIndexList indexes = [];
            UnitIndexListCollection unitIndexListCollection = Resource.Defenders;
            for (int i = unitIndexListCollection.GetCollectionCount(collection) - 1; i >= 0; i--)
            {
                _ = indexes.Add(unitIndexListCollection.GetU(collection, index: i));
            }

            _ = Int32List.FillWithRandomUnits(indexes, units.Units, level: 4, value: Resource.RazeDefenderBudget, types: UnitTypes.Humanoid | UnitTypes.Creature | UnitTypes.Machine);
            while (units.Count > 8)
            {
                _ = units.DeleteUnit(units.GetUnit(units.Count - 1));
            }
        }

        return true;
    }

    // Structur.PAS:1767
    public virtual bool PlaceRazeDefenders(PlayerId? player)
    {
        UnitList defenders = new(null) { Holder = CreationHolder() };
        UnitUserList placed = new(null) { Holder = CreationHolder() };
        UnitUserList attackers = new(null) { Holder = CreationHolder() };
        _ = GenerateRazeDefenders(player, units: defenders);
        bool result;
        if (defenders.Count != 0)
        {
            for (int i = defenders.Count - 1; i >= 0; i--)
            {
                Troop? troop = defenders.GetUnit(i);
                _ = placed.AddUnit(troop);
                int hexLevel = HexLevel;
                int hexY = HexY;
                _ = troop.Place(HexX, hexY, L: hexLevel, player: PlayerId.Independent, mode: UnitPlaceModes.ScanArea);
            }

            _ = ListUnits(attackers);
            SetupRazeDefenderAIGroup(attackers, defenders: placed);
            result = true;
        }
        else
        {
            result = false;
        }

        attackers?.Dispose();
        placed?.Dispose();
        defenders?.Dispose();
        return result;
    }

    // Structur.PAS:1801
    public virtual bool ExecuteRaze(PlayerId? player)
    {
        string reason = "";
        if (CanRaze(player, ref reason))
        {
            BeginUpdate();
            LocationTextEventLog log = new(null)
            {
                Holder = CreationHolder(),
                Player = player
            };
            string editName = EditName();
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_structureRazedFormat), arguments: [editName]);
            log.SetLocation(level: HexLevel, y: HexY, x: HexX);
            EventLog.DistributeLocationEventLog(y: HexY, level: HexLevel, log: log, player: player, x: HexX, execute: false);
            _ = log.Release();
            ImageSequenceAnimation animation;
            if (Resource.ImageSequenceList.GetImageSequence(20) is not null)
            {
                animation = new ImageSequenceAnimation(null, show: ShowRazeAnimation) { Holder = CreationHolder() };
                animation.SetImageSequence(Resource.ImageSequenceList.GetImageSequence(20));
                animation.SetPixelXOffset(animation.PixelXOffset - 16);
                animation.SetPixelYOffset(animation.PixelYOffset - 16);
                int hexLevel3 = HexLevel;
                int hexY3 = HexY;
                int hexX = HexX;
                _ = StrategicMap.Current.PlaceHex(animation, hexX, hexY3, L: hexLevel3);
            }

            if (Resource.ImageSequenceList.GetImageSequence(21) is not null)
            {
                animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                animation.SetImageSequence(Resource.ImageSequenceList.GetImageSequence(21));
                animation.FrameCount = 300;
                animation.SetPixelXOffset(animation.PixelXOffset - 16);
                animation.SetPixelYOffset(animation.PixelYOffset - 16);
                animation.Looped = true;
                int hexLevel3 = HexLevel;
                int hexY3 = HexY;
                int hexX = HexX;
                _ = StrategicMap.Current.PlaceHex(animation, hexX, hexY3, L: hexLevel3);
            }

            _ = ValidateObservation(player);
            SetRazed(true);
            _ = PlaceRazeDefenders(player);
            EndUpdate();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:1169
    public virtual string GetCurrentActivityText() => "-";

    // Structur.PAS:1564
    public virtual bool ValidateMap(DelphiStringCollection messages) => true;

    // Structur.PAS:1569
    public virtual bool MainValidateMap(DelphiStringCollection? messages)
    {
        DelphiStringList errors = [];
        bool result = ValidateMap(errors);
        if (errors.Count != 0)
        {
            string editName = EditName();
            int hexX = HexX;
            int hexY = HexY;
            int hexLevel = HexLevel;
            _ = messages.Add(AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_atLocationFormat), arguments: [editName, "(" + hexX.ToString(CultureInfo.InvariantCulture) + ", " + hexY.ToString(CultureInfo.InvariantCulture) + ", " + hexLevel.ToString(CultureInfo.InvariantCulture) + ")"]));
            for (int i = 0, last = errors.Count - 1; i <= last; i++)
            {
                _ = messages.Add($"  {errors[i]}");
            }
        }
        return result;
    }

    // Structur.PAS:1046
    public virtual bool ExecuteAI(PlayerId? player, int phase) => false;

    // Structur.PAS:1049
    public virtual void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
    }

    // Structur.PAS:1125
    public virtual void Update()
    {
        if (UpdateCount == 0)
        {
            StrategicMap.Current.NotifyStructureChanged(this);
        }
    }

    // Structur.PAS:1349
    public virtual bool VisibleForPlayer(PlayerId? player)
    {
        bool result = false;
        if (PlayerIdExtensions.ToOriginal(player) >= 0)
        {
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
            while (true)
            {
                int terrainCount = TerrainCount;
                if (!(walk.HexNumber < terrainCount && !result))
                {
                    break;
                }

                if (Resource.TerrainList.Used[walk.HexNumber])
                {
                    int hexLevel = HexLevel;
                    result = StrategicMap.Current.VisibleForPlayer(walk.X, walk.Y, hexLevel, player);
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        return result;
    }

    // Structur.PAS:1179
    public virtual bool CanDoubleClick() => false;

    // Structur.PAS:1508
    public virtual int ListUnits(UnitUserList list)
    {
        int result = 0;
        int count = TerrainCount;
        MapLevel level = Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
        while (walk.HexNumber < count)
        {
            if (Resource.TerrainList.Terrain[walk.HexNumber] >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                ArmyHexSprite army = level.GetField(walk.X, walk.Y).GetArmyHexSprite();
                if (army is not null)
                {
                    _ = list.AddUnits(army.Army.Units);
                    result += army.Army.Units.Count;
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        return result;
    }

    // Structur.PAS:1535
    public virtual int ListArmies(ArmyHSList list)
    {
        int result;
        using (list.BeginUpdate())
        {
            result = 0;
            int count = TerrainCount;
            MapLevel level = Owner.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
            while (walk.HexNumber < count)
            {
                if (Resource.TerrainList.Terrain[walk.HexNumber] >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    EngineObject? army = level.GetField(walk.X, walk.Y).FindOwnedChild(ArmyHexSprite.ClassId);
                    if (army is not null)
                    {
                        _ = list.AddChild(army);
                        result += 1;
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
        return result;
    }

    // Structur.PAS:1430
    public virtual void GetRebuildInfo(out int cost, out int time)
    {
        cost = Resource.RebuildGold;
        time = Resource.RebuildDays;
    }

    // Structur.PAS:1439
    public virtual bool CanRebuild(Troop troop, ref string reason)
    {
        if (GetRazed())
        {
            if (!Building)
            {
                GetRebuildInfo(out int cost, time: out _);
                if (StrategicMap.Current.Players.GetPlayers(troop.Player).Gems >= cost)
                {
                    return true;
                }
                else
                {
                    reason = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NotEnoughGoldText);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:1458
    public virtual bool Rebuild(Troop troop)
    {
        string reason = "";
        if (CanRebuild(troop, ref reason) && !StrategicMap.Current.Players.GetPlayers(troop.Player).GetBusy())
        {
            StructureTokenEvent eventValue = CreateTokenEvent();
            SetupTokenEvent(eventValue);
            eventValue.Action = -2;
            eventValue.Param = troop.UnitId;
            StrategicMap.Current.TokenManager.GetTokenControl(troop.Player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:1862
    public virtual bool Raze(PlayerId? player)
    {
        string reason = "";
        if (CanRaze(player, ref reason) && !StrategicMap.Current.Players.GetPlayers(player).GetBusy())
        {
            if (StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                RazeStructureEventLog log = new(null, holder: CreationHolder())
                {
                    Structure = this,
                    Buttons = MessageEventLogButtons.YesNo,
                    Title = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.WarningText)
                };
                string razeStructureWarning = AbilityRules.TranslateResourceString(GetEngine(), S: s_razeStructureWarningText);
                _ = log.Message.Add(razeStructureWarning);
                string translatedText = AbilityRules.TranslateResourceString(GetEngine(), S: s_doYouWishToContinueText);
                _ = log.Message.Add(translatedText);
                _ = log.Execute(true, trigger: ExecuteTrigger.User, done: null);
                _ = log.Release();
                return true;
            }
            else
            {
                return RazeWithoutConfirmation(player);
            }
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:1708
    public virtual bool CanRaze(PlayerId? player, ref string reason)
    {
        bool result;
        if (!GetRazed() && Razeable)
        {
            CombatPredictor predictor = new(this);
            UnitList defenders = new(null) { Holder = CreationHolder() };
            UnitUserList units = new(null) { Holder = CreationHolder() };
            _ = ListUnits(units);
            for (int i = 0, last = units.Count - 1; i <= last; i++)
            {
                if (units.GetUnit(i).Player == player)
                {
                    predictor.AddUnit(units.GetUnit(i));
                }
            }

            units?.Dispose();
            _ = GenerateRazeDefenders(player, units: defenders);
            for (int i = 0, last = defenders.Count - 1; i <= last; i++)
            {
                predictor.AddOpponent(defenders.GetUnit(i), behindWall: 0);
            }

            _ = predictor.Execute();
            result = predictor.Result is 3 or 4 or 6 && predictor.GetSuperiority() > 50;
            if (!result)
            {
                reason = AbilityRules.TranslateResourceString(GetEngine(), S: s_notEnoughForcesText);
            }

            defenders?.Dispose();
            predictor?.Dispose();
        }
        else
        {
            return false;
        }

        return result;
    }

    // Structur.PAS:1027
    public void BeginUpdate() => UpdateCount += 1;

    // Structur.PAS:1032
    public void EndUpdate()
    {
        UpdateCount -= 1;
        if (UpdateCount == 0)
        {
            StrategicMap.Current.NotifyStructureChanged(this);
        }
    }

    // Structur.PAS:1066
    public void UpdateRoads(MapField? mapField)
    {
        int x = default;
        int y = default;
        MapLevel level = mapField.Owner;
        UInt16List table = Resource.TerrainList.CreateSurroundingHexNumberTable();
        for (int i = 0, last = table.Count - 1; i <= last; i++)
        {
            int item = table[i];
            HexGeometry.CenterHexNumberToHex(mapField.HexX, mapField.HexY, hexNumber: item, ref x, ref y);
            if (x >= 0 && y >= 0 && x < level.Width && y < level.Height)
            {
                AbstractRoad road = level.GetField(x, y).FindHexSprite<AbstractRoad>();
                road?.UpdateTransitions();
            }
        }
    }

    // Structur.PAS:1087
    public PlayerId? GetPlayer() => Owner.GetPlayer();

    // Structur.PAS:1217
    public void FogChanged()
    {
        Fogged = StrategicMap.Current.FogEnabled;
        if (Fogged)
        {
            int count = TerrainCount;
            MapLevel level = Owner.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
            while (walk.HexNumber < count && Fogged)
            {
                if (Resource.TerrainList.Terrain[walk.HexNumber] >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    MapField fieldValue = level.GetField(walk.X, walk.Y);
                    Fogged = fieldValue.FogCount == 0;
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
    }

    // Structur.PAS:1366
    public bool ValidateObservation(PlayerId? player)
    {
        bool result = false;
        MapLevel level = Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
        while (true)
        {
            int terrainCount = TerrainCount;
            if (!(walk.HexNumber < terrainCount && !result))
            {
                break;
            }

            if (Resource.TerrainList.Used[walk.HexNumber] && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                int hexLevel = HexLevel;
                result = StrategicMap.Current.ValidateObservation(player, walk.X.TruncateToSByte(), walk.Y.TruncateToSByte(), hexLevel, radius: 0);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        return result;
    }

    // Structur.PAS:1409
    public bool Building => BuildingPlayer is not null;

    // Structur.PAS:1655
    public bool ShowBorderImage(LockedSurface lockedSurface, Rect R)
    {
        if (GetTopBorderImage() is not null)
        {
            if (lockedSurface.ClipRectangle.Right > R.Right)
            {
                Rect clipRectangle = lockedSurface.ClipRectangle;
                clipRectangle.Right = R.Right;
                lockedSurface.ClipRectangle = clipRectangle;
            }

            if (lockedSurface.ClipRectangle.Bottom > R.Bottom)
            {
                Rect clipRectangle2 = lockedSurface.ClipRectangle;
                clipRectangle2.Bottom = R.Bottom;
                lockedSurface.ClipRectangle = clipRectangle2;
            }

            int height = R.Bottom - R.Top;
            LibraryImage? topImage = GetTopBorderImage();
            LibraryImage? middleImage = GetMiddleBorderImage();
            LibraryImage? bottomImage = GetBottomBorderImage();
            if (bottomImage is not null && topImage is not null && middleImage is not null)
            {
                // { Original uses the height, rather than R.Bottom, for the bottom image. }
                bottomImage.Show(R.Left, height - bottomImage.Height, lockedSurface);
                if (height > bottomImage.Height)
                {
                    int y = R.Bottom - bottomImage.Height - middleImage.Height;
                    int last = (height - bottomImage.Height) / middleImage.Height;
                    for (int i = 0; i <= last; i++)
                    {
                        middleImage.Show(R.Left, y, lockedSurface);
                        y -= middleImage.Height;
                    }

                    topImage.Show(R.Left, R.Top, lockedSurface);
                }

                return true;
            }
        }

        return false;
    }

    // Structur.PAS:1688
    public bool ShowBorderImageDisplayController(int displayController, Rect R)
    {
        bool result = false;
        if (GetBottomBorderImage() is not null)
        {
            int width = R.Right - R.Left;
            int height = R.Bottom - R.Top;
            Dib16Surface surface = new(width, height, pixelFormatId: PixelFormat.Rgb565);
            try
            {
                Rect borderRectangle = new(0, 0, width, height);
                LockedSurface lockedSurface = default;
                _ = surface.Lock(ref lockedSurface);
                result = ShowBorderImage(lockedSurface, R: borderRectangle);
                _ = surface.Unlock();
                /* TODO: Implement cell or array backing for this address. */
                throw new NotImplementedException();
            }
            finally
            {
                surface?.Dispose();
            }
        }

        return result;
    }

    // Structur.PAS:1795
    public void ShowRazeAnimation(Animation animation, int X, int Y, in LockedSurface lockedSurface)
    {
        // The show event passes the animation. The raze handler reads an image sequence, and another kind fails here.
        if (((ImageSequenceAnimation)animation).ImageSequence.GetUnloopedFrameIndex(animation.FrameCounter) == 0 && GetEngine().AnimationDetail != (byte)DetailLevel.Low)
        {
            int random = Random(2);
            _ = AudioPlayback.PlaySample(AbilityRules.ExplosionSfx[random]);
        }
    }

    private static readonly string s_structureRazedFormat = "%s razed";
    private static readonly string s_structureBuiltFormat = "%s built";
    private static readonly string s_structureRebuiltFormat = "%s rebuilt";
    private static readonly string s_atLocationFormat = "%0:s at location %1:s";
    private static readonly string s_notEnoughForcesText = "Your forces aren't sufficient";
    private static readonly string s_razeStructureWarningText = "You're about to raze this structure.";
    private static readonly string s_doYouWishToContinueText = "Do you wish to continue?";
}
