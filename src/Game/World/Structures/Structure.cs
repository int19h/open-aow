// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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

    public Structure(ECustomNode owner) : base(owner)
    {
        BuildingPlayer = -1;
        Fogged = true;
    }

    public new const int ClassId = 131914;

    public int StructureId;

    public int UpdateCount { get; private set; }

    public sbyte BuildingPlayer;
    public byte BuildingDays;
    private StructureShadow _shadow;

    public bool Fogged { get; private set; }

    // Structur.PAS:1017
    public override void Dispose()
    {
        if (_shadow != null)
        {
            DestroyShadow();
        }

        if (Owner != null && Status.Contains(MapObjectStatusElement.ResourceLinked) && ((StructureResource)_resource).TerrainBound)
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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref BuildingDays, 0);
        stream.Int32(ref StructureId, 98);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref BuildingPlayer), 255, 99);
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
                            message.Data[0] = 1;
                        }
                    }
                }
            }
        }
    }

    // Structur.PAS:1281
    public override sbyte GetTerrain(int index)
    {
        sbyte result = _resource.TerrainList.Terrain[index];
        if (result >= 0)
        {
            result = TerrainType;
        }

        return result;
    }

    // Structur.PAS:1275
    public override sbyte GetOverlay(int index)
    {
        sbyte result = _resource.TerrainList.Terrain[index];
        if (result >= 0)
        {
            result = 7;
        }

        return result;
    }

    // Structur.PAS:1287
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info) => ((StructureResource)_resource).TerrainBound || base.CanChangeTerrain(info);

    // Structur.PAS:1297
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        if (_resource.TerrainList.Terrain[info.HexNumber] >= 0)
        {
            if (((StructureResource)_resource).TerrainBound)
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
        HexGeometry.HexToPixel(_hexX, _hexY, out pixelX, out pixelY);
        ((StructureResource)_resource).Show(pixelX - pixelXSurface, pixelY - pixelYSurface, lockedSurface, this);
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
        if (_resource.TerrainList.Terrain[message.HexNumber] >= 0)
        {
            if (message.MessageId == 4358)
            {
                FogChanged();
            }
            else
            {
                if (message.MessageId == 4608)
                {
                    UpdateAITarget(message.Param[0].As<AITargetQuery>().Info, ref message.Param[0].As<AITargetQuery>().Data);
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
                                    if (((StructureResource)_resource).ConnectRoads)
                                    {
                                        message.Param[1] = 1;
                                    }
                                }
                                else
                                {
                                    if (message.MessageId == 4386)
                                    {
                                        message.Result = ((StructureResource)_resource).ScannerIcon;
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
                                                if (GetDefenseRequirements((sbyte)message.Param[0].Int, ref message.Param[1].As<DefenseRequirementsQuery>().Requirements))
                                                {
                                                    message.Result = 1;
                                                }
                                            }
                                            else
                                            {
                                                if (message.MessageId == 4364 && message.HexNumber == 0)
                                                {
                                                    if (_resource.TerrainList.Terrain[message.HexNumber] >= 0 && (!StrategicMap.Current.FogEnabled || !Fogged))
                                                    {
                                                        message.Param[0] = message.Param[0].Int + 3584;
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
        if (StrategicMap.Current.ShowUnitShadows && ((StructureResource)_resource).CastsShadow && !GetEditMode())
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
        if (_resource.TerrainList.Terrain[info.HexNumber] >= 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | 134217728;
        }
    }

    // Structur.PAS:1184
    public override bool CanSelect(int pixelX, int pixelY)
    {
        bool result;
        int X = default;
        int Y = default;
        int hexNumber;
        if (!StrategicMap.Current.FogEnabled || !Fogged)
        {
            HexGeometry.PixelToHex(pixelX, pixelY, out X, out Y);
            hexNumber = HexGeometry.DeltaHexToHexNumber(Owner.HexX, Owner.HexY, X, Y);
            if (hexNumber >= _resource.TerrainList.Count || _resource.TerrainList.Terrain[hexNumber] < 0 && _resource.TerrainList.Overlay[hexNumber] < 0)
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
        if (((StructureResource)_resource).TerrainBound)
        {
            result = GamePack.Current.PadControl.CanPlacePad(hexX, hexY, hexLevel, ((StructureResource)_resource).FlagType);
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
            if (((StructureResource)_resource).TerrainBound)
            {
                if (!GetEditMode())
                {
                    pad = GamePack.Current.PadControl.PlacePad(hexX, hexY, hexLevel, ((StructureResource)_resource).FlagType);
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
    public override bool ValidTerrainType(int terrainType) => ((StructureResource)_resource).TerrainBound && GamePack.Current.PadControl.TerrainTypes[terrainType] || base.ValidTerrainType(terrainType);

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
        sequences = _resource.ImageSequenceList;
        if (sequences != null)
        {
            if (sequences.GetImageSequence(30) != null)
            {
                result = _resource.ImageSequenceList.GetImageSequence(30).GetPreviewImage();
            }
            else
            {
                result = null;
            }
        }
        else
        {
            result = _resource.ImageLibrary.GetSafe(30);
        }

        return result;
    }

    // Structur.PAS:1251
    public virtual LibraryImage GetMiddleBorderImage()
    {
        LibraryImage result;
        ImageSequenceList sequences;
        sequences = _resource.ImageSequenceList;
        if (sequences != null)
        {
            if (sequences.GetImageSequence(31) != null)
            {
                result = _resource.ImageSequenceList.GetImageSequence(31).GetPreviewImage();
            }
            else
            {
                result = null;
            }
        }
        else
        {
            result = _resource.ImageLibrary.GetSafe(31);
        }

        return result;
    }

    // Structur.PAS:1263
    public virtual LibraryImage GetBottomBorderImage()
    {
        LibraryImage result;
        ImageSequenceList sequences;
        sequences = _resource.ImageSequenceList;
        if (sequences != null)
        {
            if (sequences.GetImageSequence(32) != null)
            {
                result = _resource.ImageSequenceList.GetImageSequence(32).GetPreviewImage();
            }
            else
            {
                result = null;
            }
        }
        else
        {
            result = _resource.ImageLibrary.GetSafe(32);
        }

        return result;
    }

    // Structur.PAS:1174
    public virtual StringCollection GetDescription() => ((StructureResource)_resource).Description;

    // Structur.PAS:1164
    public virtual string GetName() => EditName();

    // Structur.PAS:1346
    public virtual void SetRazed(bool value)
    {
    }

    // Structur.PAS:1341
    public virtual bool GetRazed() => false;

    // Structur.PAS:1336
    public virtual bool GetRazeable() => ((StructureResource)_resource).Razeable;

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
    public virtual void SetupRazeDefenderAIGroup(UnitList attackers, UnitList defenders)
    {
        HuntAIGroup group;
        var unitsToIndependentRelation = UnitList.UnitsToIndependentRelation(defenders, 5);
        group = (HuntAIGroup)StrategicMap.Current.AIGroupManager.CreateGroup(0, unitsToIndependentRelation, GroupBehavior.Hunt);
        group.AddUnits(defenders);
        for (int I = attackers.Count - 1; I >= 0; I--)
        {
            if (attackers.GetUnit(I).Player != 0)
            {
                var troop = attackers.GetUnit(I);
                group.TargetUnitIds.Add(troop.UnitId);
            }
        }

        group.Release();
    }

    // Structur.PAS:1130
    public virtual StructureTokenEvent CreateTokenEvent() => new StructureTokenEvent(null);

    // Structur.PAS:1135
    public virtual void SetupTokenEvent(StructureTokenEvent tokenEvent)
    {
        var hexLevel = GetHexLevel();
        var hexY = GetHexY();
        var hexX = GetHexX();
        tokenEvent.Initialize(EngineClass.ClassId, hexX, hexY, hexLevel);
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
        if (!GetRazed() && !IsEmpty(((StructureResource)_resource).SelectSfx))
        {
            AudioPlayback.PlaySample(((StructureResource)_resource).SelectSfx);
        }
    }

    // Structur.PAS:1107
    public virtual void CreateShadow()
    {
        if (_shadow != null)
        {
            DestroyShadow();
        }

        _shadow = new StructureShadow(null);
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
        HexGeometry.HexToPixel(_hexX, _hexY, out pixelX, out pixelY);
        _resource.ImageSequenceList.Show(90, pixelX - pixelXSurface, pixelY - pixelYSurface, 0, lockedSurface);
    }

    // Structur.PAS:1414
    public virtual void BuildingDone()
    {
        LocationTextEventLog log;
        log = new LocationTextEventLog(null);
        log._player = BuildingPlayer;
        if (GetRazed())
        {
            var editName = EditName();
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(s_structureRebuiltFormat), [editName]);
        }
        else
        {
            var editName = EditName();
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(s_structureBuiltFormat), [editName]);
        }

        var hexLevel = GetHexLevel();
        var hexY = GetHexY();
        log.SetLocation(GetHexX(), hexY, hexLevel);
        var hexY2 = GetHexY();
        var hexLevel2 = GetHexLevel();
        var hexX = GetHexX();
        EventLog.DistributeLocationEventLog(log, BuildingPlayer, hexX, hexY2, hexLevel2, false);
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
        collection = ((StructureResource)_resource).Defenders.GetRandomCollection();
        if (collection != -1)
        {
            indexes = new UnitIndexList();
            var unitIndexListCollection = ((StructureResource)_resource).Defenders;
            for (int I = unitIndexListCollection.GetCollectionCount(collection) - 1; I >= 0; I--)
            {
                indexes.Add(unitIndexListCollection.GetU(collection, I));
            }

            Int32List.FillWithRandomUnits(indexes, units, 4, ((StructureResource)_resource).RazeDefenderBudget, Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature, UnitType.Machine));
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
        UnitList placed;
        UnitList attackers;
        Troop troop;
        defenders = new UnitList(null);
        placed = new UnitUserList(null);
        attackers = new UnitUserList(null);
        GenerateRazeDefenders(player, defenders);
        if (defenders.Count != 0)
        {
            for (int I = defenders.Count - 1; I >= 0; I--)
            {
                troop = defenders.GetUnit(I);
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
            log = new LocationTextEventLog(null);
            log._player = player;
            var editName = EditName();
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(s_structureRazedFormat), [editName]);
            var hexLevel = GetHexLevel();
            var hexY = GetHexY();
            log.SetLocation(GetHexX(), hexY, hexLevel);
            var hexY2 = GetHexY();
            var hexLevel2 = GetHexLevel();
            EventLog.DistributeLocationEventLog(log, player, GetHexX(), hexY2, hexLevel2, false);
            log.Release();
            if (_resource.ImageSequenceList.GetImageSequence(20) != null)
            {
                animation = new ImageSequenceAnimation(null);
                animation.SetImageSequence(_resource.ImageSequenceList.GetImageSequence(20));
                animation.SetPixelXOffset(animation.PixelXOffset - 16);
                animation.SetPixelYOffset(animation.PixelYOffset - 16);
                animation.OnShow += ShowRazeAnimation;
                var hexLevel3 = GetHexLevel();
                var hexY3 = GetHexY();
                var hexX = GetHexX();
                StrategicMap.Current.PlaceHex(animation, hexX, hexY3, hexLevel3);
            }

            if (_resource.ImageSequenceList.GetImageSequence(21) != null)
            {
                animation = new ImageSequenceAnimation(null);
                animation.SetImageSequence(_resource.ImageSequenceList.GetImageSequence(21));
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
            messages.Add(AbilityRules.Format(AbilityRules.TranslateResourceString(s_atLocationFormat), [editName, "(" + IntToStr(hexX) + ", " + IntToStr(hexY) + ", " + IntToStr(hexLevel) + ")"]));
            for (int I = 0, last = errors.Count - 1; I <= last; I++)
            {
                messages.Add("  " + errors[I]);
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
            HexNumberWalkData.InitializeHexNumberWalk(out walk, _hexX, _hexY, 0);
            while (true)
            {
                var terrainCount = GetTerrainCount();
                if (!(walk.HexNumber < terrainCount && !result))
                    break;
                if (_resource.TerrainList.Used[walk.HexNumber])
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
    public virtual int ListUnits(UnitList list)
    {
        HexNumberWalkData walk = default;
        int count;
        MapLevel level;
        ArmyHexSprite army;
        int result = 0;
        count = GetTerrainCount();
        level = Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, _hexX, _hexY, 0);
        while (walk.HexNumber < count)
        {
            if (_resource.TerrainList.Terrain[walk.HexNumber] >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                army = level.GetField(walk.X, walk.Y).GetArmyHexSprite();
                if (army != null)
                {
                    list.AddUnits(army.Army);
                    result += army.Army.Count;
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
        list.BeginUpdate();
        int result = 0;
        count = GetTerrainCount();
        level = Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, _hexX, _hexY, 0);
        while (walk.HexNumber < count)
        {
            if (_resource.TerrainList.Terrain[walk.HexNumber] >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
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

        list.EndUpdate();
        return result;
    }

    // Structur.PAS:1430
    public virtual void GetRebuildInfo(out int cost, out int time)
    {
        cost = ((StructureResource)_resource).RebuildGold;
        time = ((StructureResource)_resource).RebuildDays;
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
                    reason = AbilityRules.TranslateResourceString(GameText.NotEnoughGoldText);
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
                log = new RazeStructureEventLog(null);
                log.Structure = this;
                log.Buttons = MessageEventLogButtons.YesNo;
                log.Title = AbilityRules.TranslateResourceString(GameText.WarningText);
                var razeStructureWarning = AbilityRules.TranslateResourceString(s_razeStructureWarningText);
                log.Message.Add(razeStructureWarning);
                var translatedText = AbilityRules.TranslateResourceString(s_doYouWishToContinueText);
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
        UnitList units;
        if (!GetRazed() && GetRazeable())
        {
            predictor = new CombatPredictor();
            defenders = new UnitList(null);
            units = new UnitUserList(null);
            ListUnits(units);
            for (int I = 0, last = units.Count - 1; I <= last; I++)
            {
                if (units.GetUnit(I).Player == player)
                {
                    predictor.AddUnit(units.GetUnit(I));
                }
            }

            units?.Dispose();
            GenerateRazeDefenders(player, defenders);
            for (int I = 0, last = defenders.Count - 1; I <= last; I++)
            {
                predictor.AddOpponent(defenders.GetUnit(I), 0);
            }

            predictor.Execute();
            result = Set<byte>.WithBounds(0, 255).With(3).With(4).With(6).Contains(predictor.Result) && predictor.GetSuperiority() > 50;
            if (!result)
            {
                reason = AbilityRules.TranslateResourceString(s_notEnoughForcesText);
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
        int X = default;
        int Y = default;
        AbstractRoad road;
        level = mapField.Owner;
        table = _resource.TerrainList.CreateSurroundingHexNumberTable();
        for (int I = 0, last = table.Count - 1; I <= last; I++)
        {
            var item = table[I];
            HexGeometry.CenterHexNumberToHex(mapField.HexX, mapField.HexY, item, ref X, ref Y);
            if (X >= 0 && Y >= 0 && X < level.Width && Y < level.Height)
            {
                road = (AbstractRoad)level.GetField(X, Y).FindHexSprite(typeof(AbstractRoad));
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
            HexNumberWalkData.InitializeHexNumberWalk(out walk, _hexX, _hexY, 0);
            while (walk.HexNumber < count && Fogged)
            {
                if (_resource.TerrainList.Terrain[walk.HexNumber] >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    var fieldValue = level.GetField(walk.X, walk.Y);
                    Fogged = Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref fieldValue.FieldFlags).Hi).Lo == 0;
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
        HexNumberWalkData.InitializeHexNumberWalk(out walk, _hexX, _hexY, 0);
        while (true)
        {
            var terrainCount = GetTerrainCount();
            if (!(walk.HexNumber < terrainCount && !result))
                break;
            if (_resource.TerrainList.Used[walk.HexNumber] && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
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
        int Y;
        bool result = false;
        if (GetTopBorderImage() != null)
        {
            if (lockedSurface.ClipRectangle.Right > R.Right)
            {
                lockedSurface.ClipRectangle.Right = R.Right;
            }

            if (lockedSurface.ClipRectangle.Bottom > R.Bottom)
            {
                lockedSurface.ClipRectangle.Bottom = R.Bottom;
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
                    Y = R.Bottom - bottomImage.Height - middleImage.Height;
                    int last = (height - bottomImage.Height) / middleImage.Height;
                    for (int I = 0; I <= last; I++)
                    {
                        middleImage.Show(R.Left, Y, lockedSurface);
                        Y -= middleImage.Height;
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
        if (((ImageSequenceAnimation)animation).ImageSequence.GetUnloopedFrameIndex(animation.FrameCounter) == 0 && GameTables.GameEngine.AnimationDetail != (byte)DetailLevel.Low)
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
