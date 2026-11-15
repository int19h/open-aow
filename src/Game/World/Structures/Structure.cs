#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;

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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// ID, update count, building player, days left, shadow, fogged
public partial class Structure : FixedImageLibraryTerrainMapObject
{

    public Structure(CustomNode owner) : base(owner)
    {
        BuildingPlayer = -1;
        Fogged = true;
    }

    public new const int ClassId = 131914;

    public int StructureId { get; set; }

    public int UpdateCount { get; private set; }

    public sbyte BuildingPlayer { get; set; }
    public byte BuildingDays { get; private set; }

    private StructureShadow _shadow;

    public bool Fogged { get; private set; }

    // Structur.PAS:1017
    public override void Dispose()
    {
        if (_shadow != null)
        {
            DestroyShadow();
        }

        if (Owner != null && Status.Contains(MapObjectStatusElement.ResourceLinked) && ((StructureResource)Resource).TerrainBound)
        {
            Owner.DestroyChildren(131334);
        }

        base.Dispose();
        if (StrategicMap.Current != null && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            StrategicMap.Current.NotifyStructureDestroyed(this);
        }
    }

    // Structur.PAS:1038
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        BuildingDays = persister.Byte("BuildingDays", value: BuildingDays);
        StructureId = persister.Int32("StructureId", value: StructureId);
        BuildingPlayer = PersistSupport.UnsignedByte(persister, name: "BuildingPlayer", value: BuildingPlayer, fallback: 255);
    }

    // Structur.PAS:1010

    // Structur.PAS:1629
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
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
                NewTurn((sbyte)message.Payload.Int);
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
                        if (!MainValidateMap(message.Payload.As<StringCollection>()))
                        {
                            var data = message.Data;
                            data[0] = 1;
                            message.Data = data;
                        }
                    }
                }
            }
        }
    }

    // Structur.PAS:1281
    public override sbyte GetTerrain(int index)
    {
        sbyte result = Resource.TerrainList.Terrain[index];
        if (result >= 0)
        {
            result = TerrainType;
        }

        return result;
    }

    // Structur.PAS:1275
    public override sbyte GetOverlay(int index)
    {
        sbyte result = Resource.TerrainList.Terrain[index];
        if (result >= 0)
        {
            result = 7;
        }

        return result;
    }

    // Structur.PAS:1287
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info) => ((StructureResource)Resource).TerrainBound || base.CanChangeTerrain(info);

    // Structur.PAS:1297
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        if (Resource.TerrainList.Terrain[info.HexNumber] >= 0)
        {
            if (((StructureResource)Resource).TerrainBound)
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
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out pixelX, out pixelY);
        ((StructureResource)Resource).Show(pixelX - pixelXSurface, pixelY - pixelYSurface, lockedSurface, this);
    }

    // Structur.PAS:1092
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        if (!GetEditMode())
        {
            UpdateRoads(Owner);
        }

        return result;
    }

    // Structur.PAS:1098
    public override void RemoveFromMap()
    {
        MapField mapField;
        mapField = Owner;
        base.RemoveFromMap();
        if (mapField != null && !GetEditMode())
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
                    var data = message.Param[0].As<AITargetQuery>().Data;
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
                                    if (((StructureResource)Resource).ConnectRoads)
                                    {
                                        var param = message.Param;
                                        param[1] = 1;
                                        message.Param = param;
                                    }
                                }
                                else
                                {
                                    if (message.MessageId == 4386)
                                    {
                                        message.Result = ((StructureResource)Resource).ScannerIcon;
                                    }
                                    else
                                    {
                                        if (message.MessageId == 4390)
                                        {
                                            var defensePriority = GetDefensePriority();
                                            message.Result = (int)defensePriority;
                                        }
                                        else
                                        {
                                            if (message.MessageId == 4391)
                                            {
                                                var requirements = message.Param[1].As<DefenseRequirementsQuery>().Requirements;
                                                bool stored = GetDefenseRequirements((sbyte)message.Param[0].Int, ref requirements);
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
                                                        var param2 = message.Param;
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
        if (StrategicMap.Current.ShowUnitShadows && ((StructureResource)Resource).CastsShadow && !GetEditMode())
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
        bool result;
        int x = default;
        int y = default;
        int hexNumber;
        if (!StrategicMap.Current.FogEnabled || !Fogged)
        {
            HexGeometry.PixelToHex(pixelX, pixelY, out x, out y);
            hexNumber = HexGeometry.DeltaHexToHexNumber(Owner.HexX, Owner.HexY, x, y);
            if (hexNumber >= Resource.TerrainList.Count || Resource.TerrainList.Terrain[hexNumber] < 0 && Resource.TerrainList.Overlay[hexNumber] < 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:1206
    public override void Select()
    {
        base.Select();
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            PlaySelectSample();
        }
    }

    // Structur.PAS:1212
    public override void Unselect()
    {
        base.Unselect();
    }

    // Structur.PAS:1308
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result;
        if (((StructureResource)Resource).TerrainBound)
        {
            result = GamePack.Current.PadControl.CanPlacePad(hexX, hexY, hexLevel, ((StructureResource)Resource).FlagType);
        }
        else
        {
            result = base.CanPlace(mapContainer, hexX, hexY, hexLevel);
        }

        return result;
    }

    // Structur.PAS:1315
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result;
        ImageLibraryTerrainMapObject pad;
        if (GetEditMode() || CanPlace(mapContainer, hexX, hexY, hexLevel))
        {
            if (((StructureResource)Resource).TerrainBound)
            {
                if (!GetEditMode())
                {
                    pad = GamePack.Current.PadControl.PlacePad(hexX, hexY, hexLevel, ((StructureResource)Resource).FlagType);
                }
                else
                {
                    pad = null;
                }

                if (pad != null || GetEditMode())
                {
                    if (pad != null)
                    {
                        TerrainType = pad.TerrainType;
                    }

                    result = base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:1292
    public override bool ValidTerrainType(int terrainType) => ((StructureResource)Resource).TerrainBound && GamePack.Current.PadControl.TerrainTypes[terrainType] || base.ValidTerrainType(terrainType);

    // Structur.PAS:1053
    public override int GetTerrainTypeImage(int terrainType)
    {
        int result;
        if (GetRazed())
        {
            if (terrainType == 3)
            {
                result = 23;
            }
            else
            {
                if (terrainType == 5)
                {
                    result = 24;
                }
                else
                {
                    result = 22;
                }
            }
        }
        else
        {
            if (GetBuilding())
            {
                if (terrainType == 3)
                {
                    result = 26;
                }
                else
                {
                    if (terrainType == 5)
                    {
                        result = 27;
                    }
                    else
                    {
                        result = 25;
                    }
                }
            }
            else
            {
                result = base.GetTerrainTypeImage(terrainType);
            }
        }

        return result;
    }

    // Structur.PAS:1159
    public override sbyte ForceTerrainType(MapLevel mapLevel, sbyte hexX, sbyte hexY)
    {
        sbyte result;
        if (mapLevel.HexLevel != 0)
        {
            result = 12;
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Structur.PAS:1239
    public virtual LibraryImage GetTopBorderImage()
    {
        LibraryImage result;
        ImageSequenceList sequences;
        sequences = Resource.ImageSequenceList;
        if (sequences != null)
        {
            if (sequences.GetImageSequence(30) != null)
            {
                result = Resource.ImageSequenceList.GetImageSequence(30).GetPreviewImage();
            }
            else
            {
                result = null;
            }
        }
        else
        {
            result = Resource.ImageLibrary.GetSafe(30);
        }

        return result;
    }

    // Structur.PAS:1251
    public virtual LibraryImage GetMiddleBorderImage()
    {
        LibraryImage result;
        ImageSequenceList sequences;
        sequences = Resource.ImageSequenceList;
        if (sequences != null)
        {
            if (sequences.GetImageSequence(31) != null)
            {
                result = Resource.ImageSequenceList.GetImageSequence(31).GetPreviewImage();
            }
            else
            {
                result = null;
            }
        }
        else
        {
            result = Resource.ImageLibrary.GetSafe(31);
        }

        return result;
    }

    // Structur.PAS:1263
    public virtual LibraryImage GetBottomBorderImage()
    {
        LibraryImage result;
        ImageSequenceList sequences;
        sequences = Resource.ImageSequenceList;
        if (sequences != null)
        {
            if (sequences.GetImageSequence(32) != null)
            {
                result = Resource.ImageSequenceList.GetImageSequence(32).GetPreviewImage();
            }
            else
            {
                result = null;
            }
        }
        else
        {
            result = Resource.ImageLibrary.GetSafe(32);
        }

        return result;
    }

    // Structur.PAS:1174
    public virtual StringCollection GetDescription() => ((StructureResource)Resource).Description;

    // Structur.PAS:1164
    public virtual string GetName() => EditName();

    // Structur.PAS:1346
    public virtual void SetRazed(bool value)
    {
    }

    // Structur.PAS:1341
    public virtual bool GetRazed() => false;

    // Structur.PAS:1336
    public virtual bool GetRazeable() => ((StructureResource)Resource).Razeable;

    // Structur.PAS:1844
    public virtual bool RazeWithoutConfirmation(sbyte player)
    {
        bool result;
        string reason = "";
        StructureTokenEvent eventValue;
        if (CanRaze(player, ref reason) && !StrategicMap.Current.Players.GetPlayers(player).GetBusy())
        {
            eventValue = CreateTokenEvent();
            SetupTokenEvent(eventValue);
            eventValue.Action = -1;
            eventValue.Param = player;
            StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:1755
    public virtual void SetupRazeDefenderAIGroup(UnitUserList attackers, UnitUserList defenders)
    {
        HuntAIGroup group;
        var unitsToIndependentRelation = UnitList.UnitsToIndependentRelation(defenders, 5);
        group = (HuntAIGroup)StrategicMap.Current.AIGroupManager.CreateGroup(0, unitsToIndependentRelation, GroupBehavior.Hunt);
        group.AddUnits(defenders.Units);
        for (int i = attackers.Count - 1; i >= 0; i--)
        {
            if (attackers.GetUnit(i).Player != 0)
            {
                var troop = attackers.GetUnit(i);
                group.TargetUnitIds.Add(troop.UnitId);
            }
        }

        group.Release();
    }

    // Structur.PAS:1130
    public virtual StructureTokenEvent CreateTokenEvent() => new StructureTokenEvent(null) { Holder = CreationHolder() };

    // Structur.PAS:1135
    public virtual void SetupTokenEvent(StructureTokenEvent tokenEvent)
    {
        tokenEvent.Initialize(mapLevel: GetHexLevel(), y: GetHexY(), x: GetHexX(), classId: EngineClass.ClassId);
    }

    // Structur.PAS:1140
    public virtual ExecuteResult ExecuteTokenEvent(StructureTokenEvent tokenEvent, TokenExecutor executor)
    {
        ExecuteResult result;
        Troop troop;
        if (tokenEvent.Action == -1)
        {
            if (ExecuteRaze((sbyte)tokenEvent.Param))
            {
                result = ExecuteResult.Done;
            }
            else
            {
                result = ExecuteResult.DoneHalt;
            }
        }
        else
        {
            if (tokenEvent.Action == -2)
            {
                troop = StrategicMap.Current.UnitControl.FindUnit(tokenEvent.Param);
                if (troop != null && ExecuteRebuild(troop))
                {
                    troop.Killed();
                    result = ExecuteResult.Done;
                }
                else
                {
                    result = ExecuteResult.DoneHalt;
                }
            }
            else
            {
                result = ExecuteResult.DoneHalt;
            }
        }

        return result;
    }

    // Structur.PAS:1505
    public virtual bool GetDefenseRequirements(sbyte player, ref DefenseRequirements requirements) => false;

    // Structur.PAS:1502
    public virtual DefensePriority GetDefensePriority() => DefensePriority.None;

    // Structur.PAS:1476
    public virtual void NewDay()
    {
    }

    // Structur.PAS:1479
    public virtual void NewTurn(sbyte player)
    {
        if (player == BuildingPlayer)
        {
            if (BuildingDays != 0)
            {
                BuildingDays = (byte)(BuildingDays - 1);
            }

            if (BuildingDays == 0)
            {
                BuildingDone();
            }
        }
    }

    // Structur.PAS:1497
    public virtual void SeatedPlayerChanged()
    {
        FogChanged();
    }

    // Structur.PAS:1488
    public virtual void ArmyPlaced(ArmyHexSprite placedArmy)
    {
    }

    // Structur.PAS:1491
    public virtual void ArmyRemoved(ArmyHexSprite army)
    {
    }

    // Structur.PAS:1494
    public virtual void ArmyChanged(ArmyHexSprite army)
    {
    }

    // Structur.PAS:1200
    public virtual void PlaySelectSample()
    {
        if (!GetRazed() && !IsEmpty(((StructureResource)Resource).SelectSfx))
        {
            AudioPlayback.PlaySample(((StructureResource)Resource).SelectSfx);
        }
    }

    // Structur.PAS:1107
    public virtual void CreateShadow()
    {
        if (_shadow != null)
        {
            DestroyShadow();
        }

        _shadow = new StructureShadow(null) { Holder = CreationHolder() };
        _shadow.Structure = this;
        _shadow.Connect();
    }

    // Structur.PAS:1115
    public virtual void DestroyShadow()
    {
        if (_shadow != null)
        {
            if (_shadow.ShadowStatus.Contains(StructureShadowStatusElement.Connected))
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
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out pixelX, out pixelY);
        Resource.ImageSequenceList.Show(90, pixelX - pixelXSurface, pixelY - pixelYSurface, 0, lockedSurface);
    }

    // Structur.PAS:1414
    public virtual void BuildingDone()
    {
        LocationTextEventLog log;
        log = new LocationTextEventLog(null) { Holder = CreationHolder() };
        log.Player = BuildingPlayer;
        if (GetRazed())
        {
            var editName = EditName();
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_structureRebuiltFormat), [editName]);
        }
        else
        {
            var editName = EditName();
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_structureBuiltFormat), [editName]);
        }
        log.SetLocation(level: GetHexLevel(), y: GetHexY(), x: GetHexX());
        EventLog.DistributeLocationEventLog(y: GetHexY(), level: GetHexLevel(), x: GetHexX(), log: log, player: BuildingPlayer, execute: false);
        log.Release();
        SetRazed(false);
        BuildingPlayer = -1;
        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Structur.PAS:1383
    public virtual bool ExecuteRebuild(Troop troop)
    {
        bool result;
        int time = default;
        int cost = default;
        string reason = "";
        if (CanRebuild(troop, ref reason))
        {
            GetRebuildInfo(out cost, out time);
            var players = StrategicMap.Current.Players.GetPlayers(troop.Player);
            StrategicMap.Current.Players.GetPlayers(troop.Player).SetGems(players.Gems - cost);
            BuildingDays = (byte)time;
            BuildingPlayer = troop.Player;
            ValidateObservation(troop.Player);
            ImageIndex = GetTerrainTypeImage(TerrainType);
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:1400
    public virtual bool ExecuteBuild(Troop troop, int days)
    {
        BuildingDays = (byte)days;
        BuildingPlayer = troop.Player;
        ImageIndex = GetTerrainTypeImage(TerrainType);
        ValidateObservation(troop.Player);
        return true;
    }

    // Structur.PAS:1734
    public virtual bool GenerateRazeDefenders(sbyte player, UnitList units)
    {
        int collection;
        UnitIndexList indexes;
        var hexX = GetHexX();
        var hexY = GetHexY();
        RandSeed = hexX + StrategicMap.Current.InitialRandomSeed + hexY;
        collection = ((StructureResource)Resource).Defenders.GetRandomCollection();
        if (collection != -1)
        {
            indexes = new UnitIndexList();
            var unitIndexListCollection = ((StructureResource)Resource).Defenders;
            for (int i = unitIndexListCollection.GetCollectionCount(collection) - 1; i >= 0; i--)
            {
                indexes.Add(unitIndexListCollection.GetU(collection, i));
            }

            Int32List.FillWithRandomUnits(indexes, units.Units, 4, ((StructureResource)Resource).RazeDefenderBudget, Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature, UnitType.Machine));
            while (units.Count > 8)
            {
                units.DeleteUnit(units.GetUnit(units.Count - 1));
            }

            indexes?.Dispose();
        }

        return true;
    }

    // Structur.PAS:1767
    public virtual bool PlaceRazeDefenders(sbyte player)
    {
        bool result;
        UnitList defenders;
        UnitUserList placed;
        UnitUserList attackers;
        Troop troop;
        defenders = new UnitList(null) { Holder = CreationHolder() };
        placed = new UnitUserList(null) { Holder = CreationHolder() };
        attackers = new UnitUserList(null) { Holder = CreationHolder() };
        GenerateRazeDefenders(player, defenders);
        if (defenders.Count != 0)
        {
            for (int i = defenders.Count - 1; i >= 0; i--)
            {
                troop = defenders.GetUnit(i);
                placed.AddUnit(troop);
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                troop.Place(GetHexX(), hexY, hexLevel, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
            }

            ListUnits(attackers);
            SetupRazeDefenderAIGroup(attackers, placed);
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
    public virtual bool ExecuteRaze(sbyte player)
    {
        bool result = default;
        string reason = "";
        LocationTextEventLog log = default;
        ImageSequenceAnimation animation = default;
        if (CanRaze(player, ref reason))
        {
            BeginUpdate();
            log = new LocationTextEventLog(null) { Holder = CreationHolder() };
            log.Player = player;
            var editName = EditName();
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_structureRazedFormat), [editName]);
            log.SetLocation(level: GetHexLevel(), y: GetHexY(), x: GetHexX());
            EventLog.DistributeLocationEventLog(y: GetHexY(), level: GetHexLevel(), log: log, player: player, x: GetHexX(), execute: false);
            log.Release();
            if (Resource.ImageSequenceList.GetImageSequence(20) != null)
            {
                animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                animation.SetImageSequence(Resource.ImageSequenceList.GetImageSequence(20));
                animation.SetPixelXOffset(animation.PixelXOffset - 16);
                animation.SetPixelYOffset(animation.PixelYOffset - 16);
                animation.OnShow += ShowRazeAnimation;
                var hexLevel3 = GetHexLevel();
                var hexY3 = GetHexY();
                var hexX = GetHexX();
                StrategicMap.Current.PlaceHex(animation, hexX, hexY3, hexLevel3);
            }

            if (Resource.ImageSequenceList.GetImageSequence(21) != null)
            {
                animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                animation.SetImageSequence(Resource.ImageSequenceList.GetImageSequence(21));
                animation.FrameCount = 300;
                animation.SetPixelXOffset(animation.PixelXOffset - 16);
                animation.SetPixelYOffset(animation.PixelYOffset - 16);
                animation.Looped = true;
                var hexLevel3 = GetHexLevel();
                var hexY3 = GetHexY();
                var hexX = GetHexX();
                StrategicMap.Current.PlaceHex(animation, hexX, hexY3, hexLevel3);
            }

            ValidateObservation(player);
            SetRazed(true);
            PlaceRazeDefenders(player);
            EndUpdate();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:1169
    public virtual string GetCurrentActivityText() => "-";

    // Structur.PAS:1564
    public virtual bool ValidateMap(StringCollection messages) => true;

    // Structur.PAS:1569
    public virtual bool MainValidateMap(StringCollection messages)
    {
        StringList errors;
        errors = new StringList();
        bool result = ValidateMap(errors);
        if (errors.Count != 0)
        {
            var editName = EditName();
            var hexX = GetHexX();
            var hexY = GetHexY();
            var hexLevel = GetHexLevel();
            messages.Add(AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_atLocationFormat), [editName, "(" + IntToStr(hexX) + ", " + IntToStr(hexY) + ", " + IntToStr(hexLevel) + ")"]));
            for (int i = 0, last = errors.Count - 1; i <= last; i++)
            {
                messages.Add("  " + errors[i]);
            }
        }

        errors?.Dispose();
        return result;
    }

    // Structur.PAS:1046
    public virtual bool ExecuteAI(sbyte player, int phase) => false;

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
    public virtual bool VisibleForPlayer(sbyte player)
    {
        HexNumberWalkData walk = default;
        bool result = false;
        if (player >= 0)
        {
            HexNumberWalkData.InitializeHexNumberWalk(out walk, StoredHexX, StoredHexY, 0);
            while (true)
            {
                var terrainCount = GetTerrainCount();
                if (!(walk.HexNumber < terrainCount && !result))
                    break;
                if (Resource.TerrainList.Used[walk.HexNumber])
                {
                    var hexLevel = GetHexLevel();
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
        HexNumberWalkData walk = default;
        int count;
        MapLevel level;
        ArmyHexSprite army;
        int result = 0;
        count = GetTerrainCount();
        level = Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, StoredHexX, StoredHexY, 0);
        while (walk.HexNumber < count)
        {
            if (Resource.TerrainList.Terrain[walk.HexNumber] >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                army = level.GetField(walk.X, walk.Y).GetArmyHexSprite();
                if (army != null)
                {
                    list.AddUnits(army.Army.Units);
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
        HexNumberWalkData walk = default;
        int count;
        MapLevel level;
        EngineObject army;
        int result;
        using (list.BeginUpdate())
        {
            result = 0;
            count = GetTerrainCount();
            level = Owner.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(out walk, StoredHexX, StoredHexY, 0);
            while (walk.HexNumber < count)
            {
                if (Resource.TerrainList.Terrain[walk.HexNumber] >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    army = level.GetField(walk.X, walk.Y).FindOwnedChild(131607);
                    if (army != null)
                    {
                        list.AddChild(army);
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
        cost = ((StructureResource)Resource).RebuildGold;
        time = ((StructureResource)Resource).RebuildDays;
    }

    // Structur.PAS:1439
    public virtual bool CanRebuild(Troop troop, ref string reason)
    {
        bool result;
        int cost = default;
        int time = default;
        if (GetRazed())
        {
            if (!GetBuilding())
            {
                GetRebuildInfo(out cost, out time);
                if (StrategicMap.Current.Players.GetPlayers(troop.Player).Gems >= cost)
                {
                    result = true;
                }
                else
                {
                    reason = AbilityRules.TranslateResourceString(GetEngine(), GameText.NotEnoughGoldText);
                    result = false;
                }
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:1458
    public virtual bool Rebuild(Troop troop)
    {
        bool result;
        string reason = "";
        StructureTokenEvent eventValue;
        if (CanRebuild(troop, ref reason) && !StrategicMap.Current.Players.GetPlayers(troop.Player).GetBusy())
        {
            eventValue = CreateTokenEvent();
            SetupTokenEvent(eventValue);
            eventValue.Action = -2;
            eventValue.Param = troop.UnitId;
            StrategicMap.Current.TokenManager.GetTokenControl(troop.Player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:1862
    public virtual bool Raze(sbyte player)
    {
        bool result = default;
        string reason = "";
        RazeStructureEventLog log;
        if (CanRaze(player, ref reason) && !StrategicMap.Current.Players.GetPlayers(player).GetBusy())
        {
            if (StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                log = new RazeStructureEventLog(null, holder: CreationHolder());
                log.Structure = this;
                log.Buttons = MessageEventLogButtons.YesNo;
                log.Title = AbilityRules.TranslateResourceString(GetEngine(), GameText.WarningText);
                var razeStructureWarning = AbilityRules.TranslateResourceString(GetEngine(), s_razeStructureWarningText);
                log.Message.Add(razeStructureWarning);
                var translatedText = AbilityRules.TranslateResourceString(GetEngine(), s_doYouWishToContinueText);
                log.Message.Add(translatedText);
                log.Execute(true, ExecuteTrigger.User, null);
                log.Release();
                result = true;
            }
            else
            {
                result = RazeWithoutConfirmation(player);
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:1708
    public virtual bool CanRaze(sbyte player, ref string reason)
    {
        bool result;
        CombatPredictor predictor;
        UnitList defenders;
        UnitUserList units;
        if (!GetRazed() && GetRazeable())
        {
            predictor = new CombatPredictor(this);
            defenders = new UnitList(null) { Holder = CreationHolder() };
            units = new UnitUserList(null) { Holder = CreationHolder() };
            ListUnits(units);
            for (int i = 0, last = units.Count - 1; i <= last; i++)
            {
                if (units.GetUnit(i).Player == player)
                {
                    predictor.AddUnit(units.GetUnit(i));
                }
            }

            units?.Dispose();
            GenerateRazeDefenders(player, defenders);
            for (int i = 0, last = defenders.Count - 1; i <= last; i++)
            {
                predictor.AddOpponent(defenders.GetUnit(i), 0);
            }

            predictor.Execute();
            result = Set<byte>.WithBounds(0, 255).With(3).With(4).With(6).Contains(predictor.Result) && predictor.GetSuperiority() > 50;
            if (!result)
            {
                reason = AbilityRules.TranslateResourceString(GetEngine(), s_notEnoughForcesText);
            }

            defenders?.Dispose();
            predictor?.Dispose();
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:1027
    public void BeginUpdate()
    {
        UpdateCount += 1;
    }

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
    public void UpdateRoads(MapField mapField)
    {
        MapLevel level;
        UInt16List table;
        int x = default;
        int y = default;
        AbstractRoad road;
        level = mapField.Owner;
        table = Resource.TerrainList.CreateSurroundingHexNumberTable();
        for (int i = 0, last = table.Count - 1; i <= last; i++)
        {
            var item = table[i];
            HexGeometry.CenterHexNumberToHex(mapField.HexX, mapField.HexY, item, ref x, ref y);
            if (x >= 0 && y >= 0 && x < level.Width && y < level.Height)
            {
                road = (AbstractRoad)level.GetField(x, y).FindHexSprite(typeof(AbstractRoad));
                if (road != null)
                {
                    road.UpdateTransitions();
                }
            }
        }

        table?.Dispose();
    }

    // Structur.PAS:1087
    public sbyte GetPlayer() => Owner.GetPlayer();

    // Structur.PAS:1217
    public void FogChanged()
    {
        HexNumberWalkData walk = default;
        int count;
        MapLevel level;
        Fogged = StrategicMap.Current.FogEnabled;
        if (Fogged)
        {
            count = GetTerrainCount();
            level = Owner.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(out walk, StoredHexX, StoredHexY, 0);
            while (walk.HexNumber < count && Fogged)
            {
                if (Resource.TerrainList.Terrain[walk.HexNumber] >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    var fieldValue = level.GetField(walk.X, walk.Y);
                    Fogged = fieldValue.FogCount == 0;
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
    }

    // Structur.PAS:1366
    public bool ValidateObservation(sbyte player)
    {
        HexNumberWalkData walk = default;
        MapLevel level;
        bool result = false;
        level = Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, StoredHexX, StoredHexY, 0);
        while (true)
        {
            var terrainCount = GetTerrainCount();
            if (!(walk.HexNumber < terrainCount && !result))
                break;
            if (Resource.TerrainList.Used[walk.HexNumber] && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                var hexLevel = GetHexLevel();
                result = StrategicMap.Current.ValidateObservation(player, (sbyte)walk.X, (sbyte)walk.Y, hexLevel, 0);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        return result;
    }

    // Structur.PAS:1409
    public bool GetBuilding() => BuildingPlayer != -1;

    // Structur.PAS:1655
    public bool ShowBorderImage(LockedSurface lockedSurface, Rect R)
    {
        LibraryImage topImage;
        LibraryImage middleImage;
        LibraryImage bottomImage;
        int height;
        int y;
        bool result = false;
        if (GetTopBorderImage() != null)
        {
            if (lockedSurface.ClipRectangle.Right > R.Right)
            {
                var clipRectangle = lockedSurface.ClipRectangle;
                clipRectangle.Right = R.Right;
                lockedSurface.ClipRectangle = clipRectangle;
            }

            if (lockedSurface.ClipRectangle.Bottom > R.Bottom)
            {
                var clipRectangle2 = lockedSurface.ClipRectangle;
                clipRectangle2.Bottom = R.Bottom;
                lockedSurface.ClipRectangle = clipRectangle2;
            }

            height = R.Bottom - R.Top;
            topImage = GetTopBorderImage();
            middleImage = GetMiddleBorderImage();
            bottomImage = GetBottomBorderImage();
            if (bottomImage != null && topImage != null && middleImage != null)
            {
                // { Original uses the height, rather than R.Bottom, for the bottom image. }
                bottomImage.Show(R.Left, height - bottomImage.Height, lockedSurface);
                if (height > bottomImage.Height)
                {
                    y = R.Bottom - bottomImage.Height - middleImage.Height;
                    int last = (height - bottomImage.Height) / middleImage.Height;
                    for (int i = 0; i <= last; i++)
                    {
                        middleImage.Show(R.Left, y, lockedSurface);
                        y -= middleImage.Height;
                    }

                    topImage.Show(R.Left, R.Top, lockedSurface);
                }

                result = true;
            }
        }

        return result;
    }

    // Structur.PAS:1688
    public bool ShowBorderImageDisplayController(int displayController, Rect R)
    {
        Dib16Surface surface;
        int width;
        int height;
        LockedSurface lockedSurface = default;
        Rect borderRectangle;
        bool result = false;
        if (GetBottomBorderImage() != null)
        {
            width = R.Right - R.Left;
            height = R.Bottom - R.Top;
            surface = new Dib16Surface(width, height, PixelFormats.pfRGB565);
            try
            {
                borderRectangle = new Rect(0, 0, width, height);
                surface.Lock(ref lockedSurface);
                result = ShowBorderImage(lockedSurface, borderRectangle);
                surface.Unlock();
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
        if (((ImageSequenceAnimation)animation).ImageSequence.GetUnloopedFrameIndex(animation.FrameCounter) == 0 && GetEngine().AnimationDetail != (byte)DetailLevel.Low)
        {
            var random = Random(2);
            AudioPlayback.PlaySample(AbilityRules.ExplosionSfx[random]);
        }
    }

    private static readonly string s_structureRazedFormat = "%s razed";
    private static readonly string s_structureBuiltFormat = "%s built";
    private static readonly string s_structureRebuiltFormat = "%s rebuilt";
    private static readonly string s_atLocationFormat = "%0:s at location %1:s";
    private static readonly string s_notEnoughForcesText = "Your forces aren't sufficient";
    private static readonly string s_razeStructureWarningText = "You're about to raze this structure.";
    private static readonly string s_doYouWishToContinueText = "Do you wish to continue?";

    // intf_P1_09.inc:78

    // intf_P1_09.inc:78

}
