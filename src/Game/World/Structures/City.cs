#nullable disable warnings
// Translated from City.pas.
using System.Globalization;
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Draw.Images;

using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.World.Structures;

// Mode, Race, Name, WallType, UpgradeLevel, Production, _flagId, EnchantmentList,
// Enchantments, _overlay, EnchantmentsVisible, WallBackImage, WallFrontImage, CityImage,
// DefendersMode
public partial class City : PlayerCropStructure
{
    public City(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Production = new CityProductionControl(null)
        {
            Holder = CreationHolder(),
            City = this
        };
        _defendersMode = DefendersGenerateMode.Average;
        Race = -1;
        Name = AbilityRules.TranslateResourceString(GetEngine(), S: s_noNameText);
        UpgradeLevel = 1;
        Overlay = new CityOverlayHexSprite(null)
        {
            Holder = CreationHolder(),
            City = this
        };
        _enchantmentList = new GlobalEnchantmentList(null) { Holder = CreationHolder() };
        EnchantmentsVisible = false;
    }

    // City.pas:849
    public new const int ClassId = 131_336;

    public Modes Mode { get; set; }
    public int Race { get; private set; }
    public string Name { get; private set; } = "";
    public int WallType { get; set; }
    public int UpgradeLevel { get; private set; }

    public CityProductionControl Production { get; }

    private byte _flagId;
    private GlobalEnchantmentList? _enchantmentList;

    public CityEnchantmentIds Enchantments { get; private set; }

    public CityOverlayHexSprite Overlay { get; }

    public bool EnchantmentsVisible { get; private set; }
    public LibraryImage? WallBackImage { get; private set; }
    public LibraryImage? WallFrontImage { get; private set; }
    public LibraryImage? CityImage { get; private set; }

    private DefendersGenerateMode _defendersMode;

    // City.pas:839
    public override void Dispose()
    {
        if (Visible)
        {
            _ = StrategicMap.Current.DynamicVisibleHexSprite.RemoveChild(Overlay);
        }

        Overlay?.Dispose();
        base.Dispose();
        Production?.Dispose();
        _enchantmentList?.Dispose();
        _enchantmentList = null;
    }

    // City.pas:2157
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Name = persister.String("Name", value: Name);
        Race = persister.UnsignedByte(name: "Race", value: Race.TruncateToSByte());
        WallType = persister.RawByte("WallType", value: WallType.TruncateToByte());
        UpgradeLevel = persister.Byte("UpgradeLevel", value: UpgradeLevel.TruncateToByte());
        persister.Object("Production", objectValue: Production);
        Mode = persister.Set("Mode", value: Mode);
        _defendersMode = persister.Enum("DefendersMode", value: _defendersMode);
        persister.Object("EnchantmentList", objectValue: _enchantmentList);
    }

    // City.pas:2191
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001989)
        {
            if (Race >= 0)
            {
                if (StrategicMap.Current.Races.GetRace(Race) is null || !StrategicMap.Current.Races.GetRace(Race).Enabled)
                {
                    Race = -1;
                    UpdateImages();
                }
            }
        }
        else
        {
            if (message.MessageId == 537001993)
            {
                UpdateVisibleForSeatedPlayer();
            }
        }
    }

    // City.pas:2065
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        base.TerrainChanged(info);
        UpdateImages();
    }

    // City.pas:2132
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out int x, out int y);
        pixelXSurface = x - pixelXSurface;
        pixelYSurface = y - pixelYSurface;
        if (!Mode.Contains(Modes.Razed) || Building)
        {
            if ((StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting) || !Fogged || !StrategicMap.Current.FogEnabled) && _flagId != 63 && (base.Resource.Claimed || (_flagId & 0b0010_0000) == 0))
            {
                PlayerStructureResource cityResource = base.Resource;
                if (cityResource.FlagType <= 1)
                {
                    GamePack.Current.FlagControl.ShowFlag(cityResource.FlagX + pixelXSurface, cityResource.FlagY + pixelYSurface, frameOffset: cityResource.Engine.HexSpriteMap.Frame + x, _flagId, lockedSurface);
                }
                else
                {
                    GamePack.Current.FlagControl.ShowStructureFlag(cityResource.FlagX + pixelXSurface, cityResource.FlagY + pixelYSurface, frameOffset: cityResource.Engine.HexSpriteMap.Frame + x, _flagId, lockedSurface);
                }
            }
        }

        if (Mode.Contains(Modes.TownQuake))
        {
            pixelXSurface = pixelXSurface + Random(5) - 2;
            pixelYSurface = pixelYSurface + Random(5) - 2;
        }

        if (!Mode.Contains(Modes.Razed))
        {
            GetResource<Resource>().ShowCity(pixelXSurface, pixelYSurface, lockedSurface, city: this);
        }
        else
        {
            base.Resource.Show(pixelXSurface, pixelYSurface, lockedSurface, structure: this);
        }
    }

    // City.pas:2170
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        if (result)
        {
            UpdateMapFields(Owner.Owner, StoredHexX, StoredHexY);
        }

        return result;
    }

    // City.pas:2176
    public override void RemoveFromMap()
    {
        MapLevel mapLevel = Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(mapLevel, StoredHexX, StoredHexY);
    }

    // City.pas:2378
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (!(message.MessageId == 4364 && Resource.TerrainList.Terrain[message.HexNumber] >= 0))
        {
            base.MapFieldMessageProc(ref message);
        }

        if (Resource.TerrainList.Terrain[message.HexNumber] >= 0)
        {
            if (message.MessageId == 4364 && message.HexNumber == 0)
            {
                if ((!StrategicMap.Current.FogEnabled || !Fogged) && Player >= 0)
                {
                    if (Player == 0)
                    {
                        MapFieldMessageParameters param = message.Param;
                        param[0] = param[0].Int + 851968;
                        message.Param = param;
                    }
                    else
                    {
                        Player? player = StrategicMap.Current.Players.GetPlayers(Player);
                        MapFieldMessageParameters param2 = message.Param;
                        param2[0] = param2[0].Int + ((player.PlayerRace + 1) * 65_536);
                        message.Param = param2;
                    }
                }
                else
                {
                    MapFieldMessageParameters param3 = message.Param;
                    param3[0] = param3[0].Int + 917504;
                    message.Param = param3;
                }

                MapFieldMessageParameters param4 = message.Param;
                param4[0] = param4[0].Int + ((GetResource<Resource>().Size - 1) * 4_194_304);
                message.Param = param4;
            }
            else
            {
                if (message.MessageId == 4609)
                {
                    message.Result = Production;
                }
                else
                {
                    if (message.MessageId == 4360)
                    {
                        BattleFieldInfo info = message.Param[0].As<BattleFieldQuery>().Info;
                        ReturnBattleFieldInfo(ref info);
                        message.Param[0].As<BattleFieldQuery>().Info = info;
                    }
                    else
                    {
                        if (message.MessageId == 4359)
                        {
                            InitializeCombat(message.Param[0].As<Battle>(), army: message.Param[1].As<ArmyHexSprite>());
                            message.Result = 1;
                        }
                        else
                        {
                            if (message.MessageId == 4365)
                            {
                                UpdateVisibleForSeatedPlayer();
                            }
                            else
                            {
                                if (message.MessageId == 4387)
                                {
                                    if (WallType != 0 && !Mode.Contains(Modes.WarMode) && !GetRazed())
                                    {
                                        if (this == message.Param[0].As<MapField>().FindNoneTransparentHexSprite(typeof(City)))
                                        {
                                            message.Result = (byte)WallType;
                                        }
                                    }
                                }
                                else
                                {
                                    if (message.MessageId == 4389)
                                    {
                                        if (Player == 0)
                                        {
                                            int independentRelation = GetIndependentRelation();
                                            message.Result = (sbyte)independentRelation;
                                        }
                                    }
                                    else
                                    {
                                        if (message.MessageId == 4366)
                                        {
                                            if (IncomingNegotiateRequest(message.Param[0].As<NegotiateRequestEventLog>()))
                                            {
                                                MapFieldMessageParameters param5 = message.Param;
                                                param5[1] = 1;
                                                message.Param = param5;
                                            }
                                        }
                                        else
                                        {
                                            if (message.MessageId == 4367)
                                            {
                                                if (IncomingNegotiateOffer(message.Param[0].As<NegotiateTokenEvent>()))
                                                {
                                                    message.Result = 1;
                                                }
                                            }
                                            else
                                            {
                                                if (message.MessageId == 4393 && Enchantments.Contains(CityEnchantmentIds.AntiMagicShell))
                                                {
                                                    MapFieldMessageParameters param6 = message.Param;
                                                    param6[0] = 1;
                                                    message.Param = param6;
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

    // City.pas:2124
    public override void Deactivate()
    {
        UnregisterEnchantments();
        if (Player != -1)
        {
            _ = StrategicMap.Current.Players.GetPlayers(Player).ProductionControls.Remove(Production);
        }

        Production.Deactivate();
        base.Deactivate();
    }

    // City.pas:2184
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        base.UpdateMapField(info);
        if (Resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.BattleFieldInfoFlag;
        }
    }

    // City.pas:1246
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!Status.Contains(MapObjectStatuses.Visible))
        {
            base.MakeVisible(visibilityList);
            if (((CityEnchantmentIds.GoldRush | CityEnchantmentIds.AntiMagicShell | CityEnchantmentIds.Warmonger) & Enchantments) != CityEnchantmentIds.None)
            {
                Overlay.MakeVisible(visibilityList);
            }
        }
    }

    // City.pas:2617
    public override void Select() => base.Select();

    // City.pas:1787
    public override int GetValidTerrainType(MapLevel mapLevel, int hexX, int hexY) => GetMostFrequentMapFieldTerrainType(mapLevel, hexX, hexY, defaultValue: -1);

    // City.pas:2422
    public override LibraryImage? GetTopBorderImage()
    {
        LibraryImage? result = GetTopCityActionImage(ActiveOrder);
        result ??= Race != -1
            ? HexLevel == 0
                ? GamePack.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(40)
                : GamePack.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(43)
            : null;

        if (Mode.Contains(Modes.Razed))
        {
            int index = 42;
            ImageSequenceList? sequences = Resource.ImageSequenceList;
            return sequences is not null
                ? sequences.GetImageSequence(index) is not null ? Resource.ImageSequenceList.GetImageSequence(index).GetPreviewImage() : null
                : null;
        }

        return result;
    }

    // City.pas:2449
    public override LibraryImage? GetMiddleBorderImage()
    {
        LibraryImage? result = GetMiddleCityActionImage(ActiveOrder);
        result ??= Race != -1
            ? HexLevel == 0
                ? GamePack.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(41)
                : GamePack.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(44)
            : null;

        if (Mode.Contains(Modes.Razed))
        {
            int index = 43;
            ImageSequenceList? sequences = Resource.ImageSequenceList;
            return sequences is not null
                ? sequences.GetImageSequence(index) is not null ? Resource.ImageSequenceList.GetImageSequence(index).GetPreviewImage() : null
                : null;
        }

        return result;
    }

    // City.pas:2476
    public override LibraryImage? GetBottomBorderImage()
    {
        LibraryImage? result = GetBottomCityActionImage(ActiveOrder);
        result ??= Race >= 0
            ? HexLevel == 0
                ? GamePack.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(42)
                : GamePack.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(45)
            : null;

        if (Mode.Contains(Modes.Razed))
        {
            int index = 44;
            ImageSequenceList? sequences = Resource.ImageSequenceList;
            return sequences is not null
                ? sequences.GetImageSequence(index) is not null ? Resource.ImageSequenceList.GetImageSequence(index).GetPreviewImage() : null
                : null;
        }

        return result;
    }

    // City.pas:2080
    public override string GetName() => Name;

    // City.pas:1623
    public override void SetRazed(bool value)
    {
        if (value != GetRazed())
        {
            Production.ExecuteDeleteProductions();
            if (value)
            {
                SetPlayer(-1);
            }

            Mode = value ? Mode | Modes.Razed : Mode & ~Modes.Razed;

            ImageIndex = GetTerrainTypeImage(TerrainType);
            ResetEnchantments();
            UpdateImages();
            UpdateCrops();
            Update();
            SetWallType(0);
        }
    }

    // City.pas:1618
    public override bool GetRazed() => Mode.Contains(Modes.Razed);

    // City.pas:1111
    public override void SetupRazeDefenderAIGroup(UnitUserList armies, UnitUserList units)
    {
        if (StrategicMap.Current.Random(8) == 0)
        {
            int unitsToIndependentRelation = UnitList.UnitsToIndependentRelation(units, defaultRelation: 5);
            AIGroup? group = StrategicMap.Current.AIGroupManager.CreateGroup(0, relation: unitsToIndependentRelation, behavior: GroupBehavior.Refuge);
            _ = group.AddUnits(units.Units);
            _ = group.Release();
        }
        else
        {
            base.SetupRazeDefenderAIGroup(armies, defenders: units);
        }
    }

    // City.pas:864
    public override ExecuteResult ExecuteTokenEvent(StructureTokenEvent tokenEvent, TokenExecutor executor) => base.ExecuteTokenEvent(tokenEvent, executor);

    // City.pas:1134
    public override bool GetDefenseRequirements(sbyte player, ref DefenseRequirements requirements)
    {
        if ((Mode & (Modes.WarMode | Modes.Razed)) == Modes.None)
        {
            BattleFieldInfo.SetupBattleFieldInfo(out BattleFieldInfo info, player, relation: 0, requests: BattleFieldInfoRequests.None, damageTypes: AbilityRules.AllDamageTypes & ~DamageTypes.Wall);
            info.Requests |= BattleFieldInfoRequests.IgnorePassiveIndependents;
            int hexLevel = HexLevel;
            int hexY = HexY;
            int hexX = HexX;
            StrategicMap.Current.RetrieveBattleFieldInfo(hexX, hexY, hexLevel, radius: 10, ref info);
            requirements.Strength = info.Strength;
            int maximum = Round(Size * 5_000);
            int minimum = Round((double)(Size * 5_000) * StrategicMap.Current.Players.GetPlayers(player).AIControl.DefenseStrengthMultiplier);
            Status relation = player != 0 ? StrategicMap.Current.Races.GetRace(Race).GetPlayerRelation(player) : Session.Status.Friendly;

            // { Original condition makes this neutral-player adjustment unreachable. }
            if (relation is >= Session.Status.Hostile and <= Session.Status.Neutral && player == Player && Player == 0)
            {
                maximum = Round(maximum) + 7_500;
                minimum = maximum;
            }

            if (requirements.Strength < minimum)
            {
                requirements.Strength = minimum;
            }

            if (maximum < requirements.Strength)
            {
                requirements.Strength = maximum;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    // City.pas:1123
    public override DefensePriority GetDefensePriority()
    {
        DefensePriority result;
        if ((Mode & (Modes.WarMode | Modes.Razed)) == Modes.None)
        {
            result = Size == 1 ? DefensePriority.Low : DefensePriority.Average;

            if (Production.Status == ProductionControlStatus.Producing && result == DefensePriority.Low)
            {
                result = DefensePriority.Average;
            }

            bool condition = Player != 0;
            if (condition)
            {
                Status playerRelation = StrategicMap.Current.Races.GetRace(Race).GetPlayerRelation(Player);
                condition = playerRelation is Session.Status.Hostile or Session.Status.Wary;
            }

            if (condition)
            {
                return DefensePriority.High;
            }
        }
        else
        {
            return DefensePriority.None;
        }

        return result;
    }

    // City.pas:1713
    public override void NewDay()
    {
        base.NewDay();
        if (Race < 0)
        {
            if (Race < -1 && StrategicMap.Current.GameType == GameType.Campaign)
            {
                int i = Math.Abs((sbyte)Race) - 3;
                if (i >= 0 && i < GetEngine().Campaign.RaceHistory.Count)
                {
                    SetRace(GetEngine().Campaign.RaceHistory[i].TruncateToSByte());
                }
                else
                {
                    SetRace(StrategicMap.Current.Races.GetRandomRace());
                }
            }
            else
            {
                SetRace(StrategicMap.Current.Races.GetRandomRace());
            }
        }

        if (StrategicMap.Current.Day == 1)
        {
            Production.UnitProductions.Clear();
            SetUpgradeLevel(UpgradeLevel);
            GenerateDefenders();
            if (WallType == 3)
            {
                WallType = StrategicMap.Current.Random(3).TruncateToByte();
                UpdateImages();
            }
        }

        CheckRebellion();
        if (Overlay.Mode == CityOverlayHexSprite.OverlayMode.ShowRazeAnimation)
        {
            Overlay.SetMode(CityOverlayHexSprite.OverlayMode.Idle);
        }
    }

    // City.pas:1705
    public override void NewTurn(sbyte player)
    {
        base.NewTurn(player);
        if (!Mode.Contains(Modes.WarMode))
        {
            Production.NewTurn(player);
        }

        if (player == Player)
        {
            if (StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
            {
                UpdateAIMigration();
            }
        }
    }

    // City.pas:1335
    public override void ArmyPlaced(ArmyHexSprite? placedArmy)
    {
        base.ArmyPlaced(placedArmy);
        if (Enchantments.Contains(CityEnchantmentIds.FountainOfLife))
        {
            _ = ExecuteFountainOfLife();
        }

        if (Enchantments.Contains(CityEnchantmentIds.AntiMagicShell))
        {
            Overlay.PlayFadeShellAnimation();
        }
    }

    // City.pas:1342
    public override void ArmyRemoved(ArmyHexSprite? army)
    {
        base.ArmyRemoved(army);
        _ = UpdatePlayer();
    }

    // City.pas:886
    public override void BuildingDone()
    {
        UpgradeLevel = 1;
        WallType = 0;
        SetRace(StrategicMap.Current.Players.GetPlayers(BuildingPlayer).PlayerRace);
        base.BuildingDone();
    }

    // City.pas:1604
    public override bool GenerateRazeDefenders(sbyte player, UnitList units)
    {
        GenerateRebelUnits(units);
        return true;
    }

    // City.pas:1610
    public override bool ExecuteRaze(sbyte player)
    {
        bool result = base.ExecuteRaze(player);
        if (result)
        {
            int playerRelationValue = StrategicMap.Current.Races.GetRace(Race).GetPlayerRelationValue(player);
            StrategicMap.Current.Races.GetRace(Race).SetPlayerRelationValue(player, value: playerRelationValue + -30);
        }

        return result;
    }

    // City.pas:2611
    public override string GetCurrentActivityText()
    {
        if (Production.Queue.Count == 0)
        {
            return AbilityRules.TranslateResourceString(GetEngine(), S: GameText.ProducingMerchandiseText);
        }
        else
        {
            ProductionItem? queue = Production.Queue.GetItem(0);
            return Production.GetProductionNameProgressive(queue);
        }
    }

    // City.pas:876
    public override bool ValidateMap(DelphiStringCollection messages)
    {
        bool result = base.ValidateMap(messages);
        if (CompareText(b: AbilityRules.TranslateResourceString(GetEngine(), S: s_noNameText), a: Name) == 0 || IsEmpty(Name))
        {
            _ = messages.Add($"{AbilityRules.TranslateResourceString(GetEngine(), S: s_cityWarningText)}: {AbilityRules.TranslateResourceString(GetEngine(), S: s_noNameSpecifiedText)}");
            return false;
        }

        return result;
    }

    // City.pas:1162
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        if (!Mode.Contains(Modes.Razed))
        {
            switch (info.Mode)
            {
                case AITargetMode.Attack:
                case AITargetMode.Raid:
                    if (info.Player != Player)
                    {
                        int priority = data.Priority;
                        if (Player > 0)
                        {
                            if (AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, targetPlayer: Player, targetRace: Race))
                            {
                                priority = 100 + ((Size - 1) * 10);
                            }
                        }
                        else
                        {
                            if (Player == 0)
                            {
                                Status playerRelation = StrategicMap.Current.Races.GetRace(Race).GetPlayerRelation(info.Player);
                                if (playerRelation is Session.Status.Polite or Session.Status.Friendly)
                                {
                                    int amount = JoinAmount(info.Player);
                                    if (amount != -1)
                                    {
                                        data.Cost = amount;
                                        priority = 100 + ((Size - 1) * 10);
                                        data.TargetType = AITargetType.Negotiate;
                                    }
                                    else
                                    {
                                        priority = 100 + ((Size - 1) * 10);
                                    }
                                }
                                else
                                {
                                    priority = 100 + ((Size - 1) * 10);
                                }
                            }
                            else
                            {
                                if (Mode.Contains(Modes.WarMode) && priority != 0)
                                {
                                    priority = 100 + ((Size - 1) * 10);
                                }
                            }
                        }

                        if (priority > data.Priority)
                        {
                            data.Priority = priority;
                        }
                    }

                    break;
                case AITargetMode.Produce:
                    if (info.Player == Player)
                    {
                        data.Priority |= 0b0010;
                    }

                    break;
                case AITargetMode.Fortify:
                    bool condition = info.Player == Player;
                    if (condition)
                    {
                        Status playerRelation = StrategicMap.Current.Races.GetRace(Race).GetPlayerRelation(info.Player);
                        condition = playerRelation is Session.Status.Polite or Session.Status.Friendly;
                    }

                    if (condition && ActiveAction != CityAction.Migrate)
                    {
                        int size = Size;
                        data.Priority = 500 + (size * 100) + (WallType * 45);
                        if (Enchantments.Contains(CityEnchantmentIds.AntiMagicShell))
                        {
                            data.Priority += 150;
                        }

                        if (HexLevel != 0)
                        {
                            data.Priority += 50;
                        }
                    }

                    break;
                case AITargetMode.Defend:
                    if (info.Player == Player)
                    {
                        int size = Size;
                        data.Priority = 50 + (size * 20);
                    }

                    break;
                case AITargetMode.Recruit:
                case AITargetMode.Hunt:
                case AITargetMode.GuardArea:
                case AITargetMode.AttackArmies:
                default:
                    break;
            }
        }
    }

    // City.pas:867
    public override void Update()
    {
        if (UpdateCount == 0)
        {
            UpdateIncome();
            base.Update();
        }
    }

    // City.pas:852
    public override bool CanDoubleClick() => StrategicMap.Current.SeatedPlayer == Player && !GetRazed() && !Building;

    // City.pas:1639
    public override void GetRebuildInfo(out int cost, out int time)
    {
        cost = Size * 25;
        time = Size + 1;
    }

    // City.pas:1092
    public override void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            Production.PlayerChanged();
            ResetEnchantments();
            if (Player != -1)
            {
                _ = StrategicMap.Current.Players.GetPlayers(Player).ProductionControls.Remove(Production);
            }

            base.SetPlayer(player);
            if (ActiveAction == CityAction.Migrate)
            {
                Production.ExecuteCancelProduction();
                Production.Queue.GetItem(0)?.Dispose();
            }

            ValidateAICityAction();
            UpdateAIMigration();
            if (Player != -1)
            {
                _ = StrategicMap.Current.Players.GetPlayers(Player).ProductionControls.Add(Production);
            }
        }
    }

    // City.pas:2071
    public override void UpdateFlagId()
    {
        _flagId = !Status.Contains(MapObjectStatuses.EditMode) && (!Mode.Contains(Modes.Razed) || Building) && !Mode.Contains(Modes.WarMode)
            ? Player > 0
                ? (byte)(StrategicMap.Current.Players.GetPlayers(Player).PlayerRace + ((UpgradeLevel - 1) * 64))
                : (byte)(32 + ((UpgradeLevel - 1) * 64))
            : (byte)63;
    }

    // City.pas:1792
    public override int VisibilityRange()
    {
        int result = 6;
        return HexLevel != 0 || (Player > 0 && (((byte)StrategicMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments) & 8) != 0)
            ? (result + 1) >>> 1
            : result;
    }

    // City.pas:2225
    public override void FinalizeCombat(Battle? combat)
    {
        if (combat.CombatResult is CombatResult.Victory or CombatResult.NoDefenders)
        {
            CombatData combatData = combat.CombatData;
            SetPlayer(combatData.Players.GetPlayers(combatData.AttackerPlayer).Player);

            _ = UpdatePlayer();
        }
    }

    // City.pas:2214
    public override string GetCombatEventText(Battle? combat)
    {
        if (!GetRazed())
        {
            if (combat.CombatResult is CombatResult.Victory or CombatResult.NoDefenders)
            {
                string cityBattleWon = AbilityRules.TranslateResourceString(GetEngine(), S: s_cityBattleWonFormat);
                string name = GetName();
                return Format(cityBattleWon, args: [name]);
            }
            else
            {
                if (combat.CombatResult == CombatResult.Defeat)
                {
                    string cityBattleLost = AbilityRules.TranslateResourceString(GetEngine(), S: s_cityBattleLostFormat);
                    string name = GetName();
                    return Format(cityBattleLost, args: [name]);
                }
                else
                {
                    if (combat.CombatResult != CombatResult.CannotBreachWall)
                    {
                        string cityBattleDrawn = AbilityRules.TranslateResourceString(GetEngine(), S: s_cityBattleDrawnFormat);
                        string name = GetName();
                        return Format(cityBattleDrawn, args: [name]);
                    }
                }
            }
        }

        return "";
    }

    // City.pas:1062
    public override void StructureCaptured(sbyte oldPlayer, sbyte newPlayer)
    {
        if (!GetRazed() && !Building && oldPlayer == 0 && !StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting) && Status.Contains(MapObjectStatuses.Activated) && !base.Resource.Claimed)
        {
            CityProductionEventLog log = new(null)
            {
                Holder = CreationHolder(),
                Player = newPlayer
            };
            string editName = EditName();
            log.Text = Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.StructureCapturedFormat), args: [editName]);
            log.Location = new MapCoordinate(HexX, HexY, HexLevel);
            EventLog.DistributeLocationEventLog(y: HexY, level: HexLevel, x: HexX, log: log, player: Player, execute: false);
            _ = log.Release();
        }
    }

    // City.pas:2271
    public override MoveAction GetMoveAction(sbyte player, MoveAction action)
    {
        MoveAction result = action;
        if (Player == 0)
        {
            if (action is MoveAction.Normal or MoveAction.Attack && !GetRazed() && !Mode.Contains(Modes.WarMode))
            {
                return JoinAmount(player) != -1 ? MoveAction.Negotiate : MoveAction.Attack;
            }
        }
        else
        {
            if (Player != player && Player != -1)
            {
                return MoveAction.Attack;
            }
        }

        return result;
    }

    // City.pas:1285
    public override bool UpdatePlayer()
    {
        if (!(Mode.Contains(Modes.Razed) && !Building))
        {
            bool multiplePlayers = false;
            sbyte player = -1;
            int count = TerrainCount;
            MapLevel mapLevel = Owner.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
            while (walk.HexNumber < count && !multiplePlayers)
            {
                if (Resource.TerrainList.GetTerrain(walk.HexNumber) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    ArmyHexSprite army = mapLevel.GetField(walk.X, walk.Y).GetArmyHexSprite();
                    if (army is not null)
                    {
                        if (player == -1)
                        {
                            player = army.Army.Player;
                        }
                        else
                        {
                            multiplePlayers = army.Army.Player != player;
                        }
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            if (multiplePlayers)
            {
                if (!Mode.Contains(Modes.WarMode))
                {
                    Mode |= Modes.WarMode;
                    SetPlayer(-1);
                    UpdateFlagId();
                    Update();
                }
            }
            else
            {
                if (Mode.Contains(Modes.WarMode))
                {
                    Mode &= ~Modes.WarMode;
                    if (player != -1)
                    {
                        SetPlayer(player);
                    }
                    else
                    {
                        SetPlayer(0);
                    }

                    UpdateFlagId();
                    Update();
                }
                else
                {
                    if (player != -1)
                    {
                        SetPlayer(player);
                    }
                }
            }
        }

        return true;
    }

    // City.pas:1777
    public override void UpdateIncome()
    {
        if (!GetRazed() && ActiveAction != CityAction.Migrate && !Building)
        {
            SetIncome(((GetCropCount() * 2) + (Size * 5)).TruncateToByte());
            if (ActiveAction is CityAction.None or CityAction.Trade)
            {
                SetIncome((Income + Round(Income * 0.25)).TruncateToByte());
            }

            if (Enchantments.Contains(CityEnchantmentIds.GoldRush))
            {
                SetIncome((Income * 2).TruncateToByte());
            }
        }
        else
        {
            SetIncome(0);
        }
    }

    // City.pas:1741
    public override void UpdateCrops()
    {
        if (!EditMode)
        {
            int count = GetRazed()
                ? 0
                : StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting)
                    ? base.Resource.CropLocations.Count
                    : -1;

            if (count != -1)
            {
                SetCropCount(count);
            }

            UpdateCropSettings();
            UpdateIncome();
        }
    }

    // City.pas:2108
    public override int GetRace() => Race;

    // City.pas:1211
    public override void CropTrampled(sbyte player)
    {
        if (player != Player && player != 0)
        {
            int race = Player > 0 ? StrategicMap.Current.Players.GetPlayers(Player).PlayerRace : (sbyte)Race;

            int playerRelationValue = StrategicMap.Current.Races.GetRace(race).GetPlayerRelationValue(player);
            StrategicMap.Current.Races.GetRace(race).SetPlayerRelationValue(player, value: playerRelationValue + -5);
        }
    }

    // City.pas:2097
    public virtual void SetRace(int race)
    {
        if (race != Race)
        {
            Race = race;
            if (Race >= 0)
            {
                SetDefaultProductions();
            }

            UpdateCrops();
            UpdateImages();
        }
    }

    // City.pas:857
    public CityAction ActiveAction => ActiveOrder?.CityAction ?? CityAction.None;

    // The order the city carries out, null when it produces nothing.
    public ProductionOrder? ActiveOrder => Production.Status == ProductionControlStatus.Producing ? Production.Queue.GetItem(0).Order : null;

    // City.pas:894
    public void UpdateActiveEnchantments()
    {
        Enchantments = CityEnchantmentIds.None;
        for (int i = _enchantmentList.Count - 1; i >= 0; i--)
        {
            CityEnchantment enchantment = _enchantmentList.GetChild<CityEnchantment>(i);
            Enchantments |= enchantment.CityEnchantmentId.ToFlag();
        }
    }

    // City.pas:902
    public void RegisterEnchantments()
    {
        for (int i = _enchantmentList.Count - 1; i >= 0; i--)
        {
            GlobalEnchantment enchantment = _enchantmentList.GetEnchantments(i);
            _ = _enchantmentList.GetEnchantments(i).Activate(enchantment.PlayerIndex);
        }

        UpdateActiveEnchantments();
    }

    // City.pas:910
    public void UnregisterEnchantments()
    {
        for (int i = _enchantmentList.Count - 1; i >= 0; i--)
        {
            GlobalEnchantment enchantment = _enchantmentList.GetEnchantments(i);
            _ = _enchantmentList.GetEnchantments(i).Deactivate((byte)enchantment.PlayerIndex);
        }
    }

    // City.pas:917
    public void ResetEnchantments()
    {
        UnregisterEnchantments();
        _enchantmentList.Clear();
        Enchantments = CityEnchantmentIds.None;
    }

    // City.pas:924
    public int AICityEnchantmentUpkeepPriority(CityEnchantmentId id)
    {
        return id switch
        {
            CityEnchantmentId.FountainOfLife => (Size * 25) + 600,
            CityEnchantmentId.GoldRush => (Size * 25) + 600,
            CityEnchantmentId.AntiMagicShell => (Size * 25) + 700,
            CityEnchantmentId.Warmonger => (Size * 25) + 600,
            _ => 0,
        };
    }

    // City.pas:935
    public void ActivateEnchantment(CityEnchantment enchantment)
    {
        enchantment.Owner = _enchantmentList;
        _ = enchantment.Activate(enchantment.PlayerIndex);
        Enchantments |= enchantment.CityEnchantmentId.ToFlag();
        if (enchantment.CityEnchantmentId == CityEnchantmentId.FountainOfLife)
        {
            _ = ExecuteFountainOfLife();
        }

        if (enchantment.CityEnchantmentId == CityEnchantmentId.GoldRush)
        {
            UpdateIncome();
        }

        if (enchantment.CityEnchantmentId == CityEnchantmentId.AntiMagicShell)
        {
            Overlay.PlaySetupShellAnimation();
        }
    }

    // City.pas:945
    public void DeactivateEnchantment(CityEnchantment enchantment)
    {
        enchantment.Owner = null;
        _ = enchantment.Deactivate((byte)enchantment.PlayerIndex);
        UpdateActiveEnchantments();
        if (enchantment.CityEnchantmentId == CityEnchantmentId.GoldRush)
        {
            UpdateIncome();
        }

        _ = enchantment.Release();
        Update();
    }

    // City.pas:955
    public void ListEnhancementsInfo(DelphiStringCollection list)
    {
        _ = list.Add(Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_upgradeLevelFormat), args: [UpgradeLevel.ToString(CultureInfo.InvariantCulture)]));
        if (WallType == 1)
        {
            _ = list.Add(AbilityRules.TranslateResourceString(GetEngine(), S: s_cityWoodenWallText));
        }

        if (WallType == 2)
        {
            _ = list.Add(AbilityRules.TranslateResourceString(GetEngine(), S: s_cityStoneWallText));
        }

        if (Enchantments.Contains(CityEnchantmentIds.FountainOfLife))
        {
            _ = list.Add($"{AbilityRules.TranslateResourceString(GetEngine(), S: GameText.FountainOfLifeText)}: {AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.EnchantmentText)}");
        }

        if (Enchantments.Contains(CityEnchantmentIds.GoldRush))
        {
            _ = list.Add($"{AbilityRules.TranslateResourceString(GetEngine(), S: GameText.GoldRushText)}: {AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.EnchantmentText)}");
        }

        if (Enchantments.Contains(CityEnchantmentIds.AntiMagicShell))
        {
            _ = list.Add($"{AbilityRules.TranslateResourceString(GetEngine(), S: GameText.AntiMagicShellText)}: {AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.EnchantmentText)}");
        }

        if (Enchantments.Contains(CityEnchantmentIds.Warmonger))
        {
            _ = list.Add($"{AbilityRules.TranslateResourceString(GetEngine(), S: GameText.WarmongerText)}: {AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.EnchantmentText)}");
        }
    }

    // City.pas:970
    public void StartTownQuake() => Mode |= Modes.TownQuake;

    // City.pas:973
    public void StopTownQuake() => Mode &= ~Modes.TownQuake;

    // City.pas:976
    public void ExecuteAnarchy()
    {
        Mode |= Modes.Anarchy;
        CheckRebellion();
        Mode &= ~Modes.Anarchy;
    }

    // City.pas:983
    public bool ExecuteFountainOfLife()
    {
        ArmyHSList armies = new(null) { Holder = CreationHolder() };
        _ = ListArmies(armies);
        bool result = false;
        bool playSound = false;
        for (int i = 0, last = armies.Count - 1; i <= last; i++)
        {
            if (armies[i].Army.HealUnits(255))
            {
                int hexLevel = armies[i].HexLevel;
                int hexY = armies[i].HexY;
                int hexX = armies[i].HexX;
                if (StrategicMap.Current.VisibleForPlayer(hexX, hexY, hexLevel, player: StrategicMap.Current.SeatedPlayer))
                {
                    ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
                    animation.SetImageSequence(GamePack.Current.AbilityControl[47].ImageSequences.ImageSequences[0]);
                    animation.AnimationLevel = 27;
                    int hexLevel2 = armies[i].HexLevel;
                    int hexY2 = armies[i].HexY;
                    int hexX2 = armies[i].HexX;
                    _ = StrategicMap.Current.PlaceHex(animation, hexX2, hexY2, L: hexLevel2);
                    bool condition = playSound || armies[i].Army.Player == StrategicMap.Current.SeatedPlayer;
                    if (!condition)
                    {
                        int hexLevel3 = armies[i].HexLevel;
                        int hexY3 = armies[i].HexY;
                        int hexX3 = armies[i].HexX;
                        condition = StrategicMap.Current.Watching(hexX3, hexY3, L: hexLevel3, radius: 1);
                    }

                    playSound = condition;
                }

                result = true;
            }
        }

        if (playSound)
        {
            _ = GamePack.Current.AbilityControl[47].SfxLibrary.Play(0);
        }

        armies?.Dispose();
        return result;
    }

    // City.pas:1015
    public void UpdateAIMigration()
    {
        if (StrategicMap.Current.Day != 0 && !StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            if (Player > 0)
            {
                bool condition = StrategicMap.Current.Players.GetPlayers(Player).PlayerType != PlayerType.Human && ((Modes.WarMode | Modes.Razed) & Mode) == Modes.None;
                if (condition)
                {
                    CityAction activeAction = ActiveAction;
                    condition = activeAction is CityAction.None or CityAction.Trade;
                }

                bool test = condition;
                if (test)
                {
                    Status playerRelation = StrategicMap.Current.Races.GetRace(Race).GetPlayerRelation(Player);
                    test = playerRelation is >= Session.Status.Hostile and <= Session.Status.Neutral;
                }

                if (test)
                {
                    MigrationList selection = CreateMigrationSelection();
                    int bestValue = 0;
                    int bestIndex = -1;
                    for (int i = 0, last = selection.Count - 1; i <= last; i++)
                    {
                        MigrationItem itemRelation = selection.GetItems(i);
                        int value;
                        if (itemRelation.Relation is Session.Status.Polite or Session.Status.Friendly)
                        {
                            MigrationItem itemRace = selection.GetItems(i);
                            bool passed = StrategicMap.Current.Players.RaceToPlayer(itemRace.Race) == -1;
                            if (!passed)
                            {
                                MigrationItem migrationItem = selection.GetItems(i);
                                Player? player = StrategicMap.Current.Players.GetPlayers(Player);
                                passed = migrationItem.Race == player.PlayerRace;
                            }

                            value = passed ? 10 : 5;
                        }
                        else
                        {
                            value = 0;
                        }

                        value += selection.GetItems(i).Time;
                        if (value > bestValue || (value == bestValue && StrategicMap.Current.Random(2) == 1))
                        {
                            bestValue = value;
                            bestIndex = i;
                        }
                    }

                    if (bestIndex != -1)
                    {
                        Production.ExecuteDeleteProductions();
                        ProductionItem item = new(null)
                        {
                            Holder = CreationHolder(),
                            Order = new Migrate(selection.GetItems(bestIndex).Race)
                        };
                        Production.ExecuteAddProduction(item);
                        item?.Dispose();
                    }

                    selection?.Dispose();
                }
            }
            else
            {
                bool condition = Player == 0;
                if (condition)
                {
                    CityAction activeAction = ActiveAction;
                    condition = activeAction is CityAction.Migrate or CityAction.Loot;
                }

                if (condition)
                {
                    Production.ExecuteDeleteProductions();
                }
            }
        }
    }

    // City.pas:1081
    public void ValidateAICityAction()
    {
        bool condition = Player > 0 && StrategicMap.Current.Players.GetPlayers(Player).PlayerType != PlayerType.Human && ((Modes.WarMode | Modes.Razed) & Mode) == Modes.None;
        if (condition)
        {
            CityAction activeAction = ActiveAction;
            condition = activeAction is not (CityAction.None or CityAction.Trade);
        }

        if (condition)
        {
            Status playerRelation = StrategicMap.Current.Races.GetRace(Race).GetPlayerRelation(Player);
            bool contains = playerRelation is >= Session.Status.Hostile and <= Session.Status.Neutral;
            if (contains)
            {
                bool test = false;
                if (ActiveOrder is Migrate migrate)
                {
                    Status playerRelation2 = StrategicMap.Current.Races.GetRace(migrate.Race).GetPlayerRelation(Player);
                    test = playerRelation2 is Session.Status.Polite or Session.Status.Friendly;
                }

                contains = !test;
            }

            bool cancel = contains;
            if (cancel)
            {
                Production.ExecuteDeleteProductions();
            }
        }
    }

    // City.pas:1222
    public bool MoveArmiesOutOfCity(MoveOutOfCitySelections selection)
    {
        bool result = true;
        int count = TerrainCount;
        MapLevel mapLevel = Owner.Owner;
        // { This eliminated copy preserves the original saved registers. }
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
        while (walk.HexNumber < count && result)
        {
            if (Resource.TerrainList.GetTerrain(walk.HexNumber) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                ArmyHexSprite army = mapLevel.GetField(walk.X, walk.Y).GetArmyHexSprite();
                if (army is not null && (!selection.Contains(MoveOutOfCitySelections.SkipOwnPlayer) || army.Army.Player != Player))
                {
                    result = army.Place(mapLevel: HexLevel, x: walk.X.TruncateToSByte(), y: walk.Y.TruncateToSByte(), mode: ArmyPlaceModes.Safe | ArmyPlaceModes.ScanArea);
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        return result;
    }

    // City.pas:1256
    public int Size => GetResource<Resource>().Size;

    // City.pas:1259
    public int GetObtainUnitsValue()
    {
        HexNumberWalkData hexNumberWalkData = default;
        ref HexNumberWalkData walk = ref hexNumberWalkData;
        int result = 0;
        int count = TerrainCount;
        MapLevel mapLevel = Owner.Owner;
        int hexY = HexY;
        int hexX = HexX;
        HexNumberWalkData.InitializeHexNumberWalk(out hexNumberWalkData, hexX, hexY, radius: 0);
        while (walk.HexNumber < count)
        {
            if (Resource.TerrainList.GetTerrain(walk.HexNumber) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                ArmyHexSprite army = mapLevel.GetField(walk.X, walk.Y).GetArmyHexSprite();
                if (army is not null)
                {
                    result += army.Army.ObtainValue(255);
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref hexNumberWalkData);
        }

        return result;
    }

    // City.pas:1348
    public int UpgradeCost()
    {
        int size = Size;
        return UpgradePrices[size - 1];
    }

    // City.pas:1351
    public int UpgradeTime() => 3;

    // City.pas:1356
    public bool CanUpgrade(bool queue, ref string reason)
    {
        int level = UpgradeLevel;
        if (queue)
        {
            level += Production.Queue.Where(item => item.Order is Upgrade).Sum(item => 1);
        }
        if (level < Size)
        {
            return true;
        }
        else
        {
            reason = AbilityRules.TranslateResourceString(GetEngine(), S: s_maxUpgradeLevelReachedText);
        }

        return false;
    }

    // City.pas:1371
    public bool Upgrade(bool queue)
    {
        string reason = "";
        if (CanUpgrade(queue, ref reason) && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            ProductionItem item = new(null)
            {
                Holder = CreationHolder(),
                Order = new Upgrade()
            };
            _ = Production.Produce(item, queue);
            item?.Dispose();
            return true;
        }
        else
        {
            return false;
        }
    }

    // City.pas:1386
    public int FortifyCost()
    {
        switch (WallType)
        {
            case (byte)Combat.WallType.None:
                int size = Size;
                return WoodWallPrices[size - 1];
            case (byte)Combat.WallType.Wood:
                int size2 = Size;
                return StoneWallPrices[size2 - 1];
            default:
                return -1;
        }
    }

    // City.pas:1395
    public int FortifyTime() => 2;

    // City.pas:1400
    public bool CanFortify(bool queue, ref string reason)
    {
        int level = WallType;
        if (queue)
        {
            level += Production.Queue.Where(item => item.Order is Fortify).Sum(item => 1);
        }
        if (level != 2)
        {
            return true;
        }
        else
        {
            reason = AbilityRules.TranslateResourceString(GetEngine(), S: s_maxFortifyLevelReachedText);
        }

        return false;
    }

    // City.pas:1415
    public bool Fortify(bool queue)
    {
        string reason = "";
        if (CanFortify(queue, ref reason) && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            ProductionItem item = new(null)
            {
                Holder = CreationHolder(),
                Order = new Fortify()
            };
            _ = Production.Produce(item, queue);
            item?.Dispose();
            return true;
        }
        else
        {
            return false;
        }
    }

    // City.pas:1430
    public int ProductionCost(int index) => Production.UnitProductionGold(index);

    // City.pas:1435
    public int ProductionTime(int index) => Production.UnitProductionTime(index);

    // City.pas:1440
    public bool CanProduce(int index, bool queue, string reason) => Production.CanProduceUnit(index, queue, reason);

    // City.pas:1445
    public bool Produce(int index, bool queue) => Production.ProduceUnit(index, queue);

    // City.pas:1450
    public int ResearchCost(int index) => Production.UnitResearchGold(index);

    // City.pas:1455
    public int ResearchTime(int index) => Production.UnitResearchTime(index);

    // City.pas:1460
    public bool CanResearch(int index, bool queue, ref string reason) => Production.CanResearchUnit(index, queue, ref reason);

    // City.pas:1465
    public bool Research(int index, bool queue) => Production.ResearchUnit(index, queue);

    // City.pas:1470
    public bool CanTrade(bool queue, string reason) => true;

    // City.pas:1475
    public bool Trade(bool queue)
    {
        string reason = "";
        if (CanTrade(queue, reason) && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            ProductionItem item = new(null)
            {
                Holder = CreationHolder(),
                Order = queue ? new Trade(1) : new Trade(-1)
            };

            _ = Production.Produce(item, queue);
            item?.Dispose();
            return true;
        }
        else
        {
            return false;
        }
    }

    // City.pas:1491
    public int LootTime() => Size;

    // City.pas:1496
    public bool CanLoot(string reason) => !Mode.Contains(Modes.Razed);

    // City.pas:1499
    public bool Loot()
    {
        string reason = "";
        if (CanLoot(reason) && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            ProductionItem item = new(null)
            {
                Holder = CreationHolder(),
                Order = new Loot()
            };
            _ = Production.Produce(item, queue: false);
            item?.Dispose();
            return true;
        }
        else
        {
            return false;
        }
    }

    // City.pas:1514
    public int MigrationTime(int race)
    {
        string reason = "";
        if (CanMigrate(race, reason))
        {
            if (race != Race)
            {
                int radius = -1;
                MapLevel mapLevel = Owner.Owner;
                HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 1);
                while (walk.Radius <= 20 && radius == -1)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                    {
                        City city = mapLevel.GetField(walk.X, walk.Y).FindNoneTransparentHexSprite<City>();
                        if (city is not null && city.Race == race)
                        {
                            radius = walk.Radius;
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }

                return radius != -1 ? (radius / 10) + 3 : 5;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return -1;
        }
    }

    // City.pas:1544
    public bool CanMigrate(int race, string reason)
    {
        bool result;
        if (Race != race)
        {
            result = RaceInCity(race, Player);
            int i = StrategicMap.Current.Players.Count;
            while (i != 1 && !result)
            {
                i -= 1;
                sbyte player = (sbyte)i;
                result = StrategicMap.Current.Players.GetDiplomaticRelation((sbyte)(player << 0), otherPlayer: Player) == DiplomaticRelation.Alliance && RaceInCity(race, player);
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // City.pas:1561
    public bool Migrate(int race)
    {
        string reason = "";
        if (CanMigrate(race, reason) && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            ProductionItem item = new(null)
            {
                Holder = CreationHolder(),
                Order = new Migrate(race)
            };
            _ = Production.Produce(item, queue: false);
            item?.Dispose();
            return true;
        }
        else
        {
            return false;
        }
    }

    // City.pas:1577
    public MigrationItem? CreateMigrationItem(int race)
    {
        string reason = "";
        return CanMigrate(race, reason)
            ? new MigrationItem(null)
            {
                Holder = CreationHolder(),
                Race = race,
                Time = MigrationTime(race),
                Relation = StrategicMap.Current.Races.GetRace(race).GetPlayerRelation(Player)
            }
            : null;
    }

    // City.pas:1589
    public MigrationList CreateMigrationSelection()
    {
        MigrationList result = new(null) { Holder = CreationHolder() };
        for (int i = 0, last = StrategicMap.Current.Races.Count - 1; i <= last; i++)
        {
            if (StrategicMap.Current.Races.GetRace(i).Enabled)
            {
                MigrationItem? item = CreateMigrationItem(i.TruncateToSByte());
                if (item is not null && item.Relation is >= Session.Status.Neutral and <= Session.Status.Friendly)
                {
                    item.Owner = result;
                }
                else
                {
                    item?.Dispose();
                }
            }
        }

        return result;
    }

    // City.pas:1645
    public void CheckRebellion()
    {
        if (Player != 0)
        {
            CivilStatus status = GetCivilStatus();
            if (status is CivilStatus.Unruly or CivilStatus.Unrest && !GetRazed())
            {
                int chance = status == CivilStatus.Unruly ? 50 : 10;

                if (chance >= StrategicMap.Current.Random(100))
                {
                    _ = MoveArmiesOutOfCity(MoveOutOfCitySelections.None);
                    CityRebellionEventLog log = new(null)
                    {
                        Holder = CreationHolder(),
                        Player = Player,
                        CityName = GetName(),
                        Location = new MapCoordinate(HexX, HexY, HexLevel)
                    };
                    EventLog.DistributeLocationEventLog(y: HexY, level: HexLevel, x: HexX, log: log, player: Player, execute: false);
                    _ = log.Release();
                    SetPlayer(0);
                    UnitList units = new(null) { Holder = CreationHolder() };
                    GenerateRebelUnits(units);
                    for (int i = units.Count - 1; i >= 0; i--)
                    {
                        int hexLevel2 = HexLevel;
                        int hexY2 = HexY;
                        int hexX2 = HexX;
                        _ = units.GetUnit(i).Place(hexX2, hexY2, L: hexLevel2, player: 0, mode: UnitPlaceModes.None);
                    }

                    units?.Dispose();
                    Production.ExecuteDeleteProductions();
                }
            }
        }
    }

    // City.pas:1681
    public void GenerateDefenders()
    {
        if (!GetRazed() && _defendersMode != DefendersGenerateMode.None && !Mode.Contains(Modes.WarMode) && Player == 0)
        {
            ArmyHSList armies = new(null) { Holder = CreationHolder() };
            _ = ListArmies(armies);
            if (_defendersMode == DefendersGenerateMode.Random)
            {
                _defendersMode = (DefendersGenerateMode)(Random(3) + 2);
            }

            int value = (((int)_defendersMode - 2) * 25) + 25;
            value += (Size - 1) * value / 2;
            for (int i = 0, last = armies.Count - 1; i <= last; i++)
            {
                value -= armies[i].Army.ObtainValue(255);
            }

            if (value > 0)
            {
                RandSeed = StrategicMap.Current.Random(1_048_575);
                RaceResource? raceResource = GamePack.Current.RaceResources.GetRaceResource(Race);
                int hexLevel = HexLevel;
                int hexY = HexY;
                _ = AbilityRules.PlaceRandomUnits(HexX, hexY, L: hexLevel, player: 0, units: raceResource.UnitIndexes, level: UpgradeLevel, value, types: UnitTypes.Humanoid | UnitTypes.Creature, engine: GetEngine());
            }

            armies?.Dispose();
        }
    }

    // City.pas:1755
    public void ListIncomeInfo(DelphiStringCollection names, DelphiStringCollection values)
    {
        int income = Size * 5;
        _ = names.Add(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.BaseIncomeText));
        _ = values.Add(income.ToString(CultureInfo.InvariantCulture));
        income += GetCropCount() * 2;
        int cropCount = GetCropCount();
        _ = names.Add(Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_cropFieldsFormat), args: [cropCount.ToString(CultureInfo.InvariantCulture)]));
        _ = values.Add((GetCropCount() * 2).ToString(CultureInfo.InvariantCulture));
        if (ActiveAction is CityAction.None or CityAction.Trade)
        {
            _ = names.Add(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.ProducingMerchandiseText));
            _ = values.Add(Round(income * 0.25).ToString(CultureInfo.InvariantCulture));
            income += Round(income * 0.25);
        }

        if (Enchantments.Contains(CityEnchantmentIds.GoldRush))
        {
            _ = names.Add(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.GoldRushText));
            _ = values.Add(income.ToString(CultureInfo.InvariantCulture));
        }
    }

    // City.pas:1800
    public void SetDefaultProductions()
    {
        Production.UnitProductions.Clear();
        if (Race >= 0)
        {
            SetUpgradeLevel(UpgradeLevel);
        }
    }

    // City.pas:1816
    public void ListCivilStatusInfo(DelphiStringCollection list)
    {
        if (Player != 0 && Race >= 0)
        {
            Status playerRelation = StrategicMap.Current.Races.GetRace(Race).GetPlayerRelation(Player);
            string status = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.StatusTexts[(int)playerRelation]);
            _ = list.Add(Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_raceRelationFormat), args: [status]));
            if (RaceTerrain[Race].Penalty.Contains(((TerrainType)TerrainType).ToFlag()))
            {
                string terrainPenaltyX = AbilityRules.TranslateResourceString(GetEngine(), S: s_terrainPenaltyFormat);
                string terrainType = GamePack.Current.TerrainTypes[TerrainType];
                _ = list.Add(Format(terrainPenaltyX, args: [terrainType]));
            }

            if (RaceTerrain[Race].Bonus.Contains(((TerrainType)TerrainType).ToFlag()))
            {
                string terrainBonusX = AbilityRules.TranslateResourceString(GetEngine(), S: s_terrainBonusFormat);
                string terrainType = GamePack.Current.TerrainTypes[TerrainType];
                _ = list.Add(Format(terrainBonusX, args: [terrainType]));
            }

            CombatPredictor predictor = new(this);
            ArmyHSList armies = new(null) { Holder = CreationHolder() };
            _ = ListArmies(armies);
            for (int i = 0, last = armies.Count - 1; i <= last; i++)
            {
                for (int j = 0, lastJ = armies[i].Army.Units.Count - 1; j <= lastJ; j++)
                {
                    Troop? u = armies[i].Army.Units.GetUnit(j);
                    if (u.MoraleValue <= 40)
                    {
                        predictor.AddOpponent(u, behindWall: 0);
                    }
                    else
                    {
                        predictor.AddUnit(u);
                    }
                }
            }

            UnitList units = new(null) { Holder = CreationHolder() };
            GenerateRebelUnits(units);
            for (int j = 0, last = units.Count - 1; j <= last; j++)
            {
                predictor.AddOpponent(units.GetUnit(j), behindWall: 0);
            }

            if (predictor.Execute() == CombatResult.Victory)
            {
                if (predictor.GetSuperiority() / 4 >= 75)
                {
                    string strong = AbilityRules.TranslateResourceString(GetEngine(), S: s_strongText);
                    _ = list.Add(Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_occupationForcesFormat), args: [strong]));
                }
                else
                {
                    string average = AbilityRules.TranslateResourceString(GetEngine(), S: s_averageText);
                    _ = list.Add(Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_occupationForcesFormat), args: [average]));
                }
            }
            else
            {
                string weak = AbilityRules.TranslateResourceString(GetEngine(), S: s_weakText);
                _ = list.Add(Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_occupationForcesFormat), args: [weak]));
            }

            predictor?.Dispose();
            units?.Dispose();
            armies?.Dispose();
        }
    }

    // City.pas:1861
    public CivilStatus GetCivilStatus()
    {
        if (Player != 0 && !Mode.Contains(Modes.WarMode))
        {
            Status relation = Mode.Contains(Modes.Anarchy)
                ? Session.Status.Hostile
                : StrategicMap.Current.Races.GetRace(Race).GetPlayerRelation(Player);

            int value = relation switch
            {
                Session.Status.Hostile => -35,
                Session.Status.Wary => -25,
                Session.Status.Neutral => 0,
                Session.Status.Polite => 15,
                Session.Status.Friendly => 25,
                _ => 0,
            };
            value = value + (WallType * 5) + (UpgradeLevel * 5);
            if (Race >= 0)
            {
                if (RaceTerrain[Race].Penalty.Contains(((TerrainType)TerrainType).ToFlag()))
                {
                    value += -10;
                }

                if (RaceTerrain[Race].Bonus.Contains(((TerrainType)TerrainType).ToFlag()))
                {
                    value += 10;
                }

                CombatPredictor predictor = new(this);
                int armyValue = 0;
                ArmyHSList armies = new(null) { Holder = CreationHolder() };
                _ = ListArmies(armies);
                for (int i = 0, last = armies.Count - 1; i <= last; i++)
                {
                    for (int j = 0, lastJ = armies[i].Army.Units.Count - 1; j <= lastJ; j++)
                    {
                        Troop? u = armies[i].Army.Units.GetUnit(j);
                        if (u.MoraleValue <= 40)
                        {
                            predictor.AddOpponent(u, behindWall: 0);
                            // { Original anarchy counts disloyal defenders twice. }
                            if (Mode.Contains(Modes.Anarchy))
                            {
                                predictor.AddOpponent(u, behindWall: 0);
                            }
                        }
                        else
                        {
                            armyValue += u.GetObtainValue();
                            predictor.AddUnit(u);
                        }
                    }
                }

                armies?.Dispose();
                if (relation is Session.Status.Hostile or Session.Status.Wary)
                {
                    UnitList units = new(null) { Holder = CreationHolder() };
                    GenerateRebelUnits(units);
                    for (int i = 0, last = units.Count - 1; i <= last; i++)
                    {
                        predictor.AddOpponent(units.GetUnit(i), behindWall: 0);
                        if (Mode.Contains(Modes.Anarchy))
                        {
                            predictor.AddOpponent(units.GetUnit(i), behindWall: 0);
                        }
                    }

                    int bonus;
                    if (predictor.Execute() == CombatResult.Victory)
                    {
                        bonus = predictor.GetSuperiority() / 4;
                        bonus = bonus >= 75 ? 75 : bonus >= 26 ? 50 : 25;
                    }
                    else
                    {
                        bonus = 0;
                    }

                    value += bonus;
                    units?.Dispose();
                }
                else
                {
                    if (relation is Session.Status.Polite or Session.Status.Friendly)
                    {
                        armyValue /= Size * 10;
                        if (armyValue > 10)
                        {
                            armyValue = 10;
                        }

                        value += armyValue;
                    }
                }

                predictor?.Dispose();
                return value < -30
                    ? CivilStatus.Unruly
                    : value < -10
                        ? CivilStatus.Unrest
                        : value < 10
                            ? CivilStatus.Stable
                            : value < 30
? relation is Session.Status.Hostile or Session.Status.Wary
                                    ? CivilStatus.Oppressed
                                    : CivilStatus.Content
: relation is Session.Status.Hostile or Session.Status.Wary
                                    ? CivilStatus.Slavery
                                    : CivilStatus.Cheerful;
            }
            else
            {
                return CivilStatus.Stable;
            }
        }
        else
        {
            return CivilStatus.Stable;
        }
    }

    // City.pas:2034
    public void SetUpgradeLevel(int level)
    {
        if (level > Size)
        {
            level = Size.TruncateToByte();
        }

        if (level < 1)
        {
            level = 1;
        }

        UpgradeLevel = level.TruncateToByte();
        if (Race >= 0)
        {
            RaceResource? raceResource = GamePack.Current.RaceResources.GetRaceResource(Race);
            for (int i = raceResource.UnitIndexes.Count - 1; i >= 0; i--)
            {
                if (raceResource.UnitIndexes[i] != -1)
                {
                    int unitIndex = raceResource.UnitIndexes[i];
                    if (Production.UnitProductions.IndexOf(unitIndex) == -1)
                    {
                        int unitIndex2 = raceResource.UnitIndexes[i];
                        UnitResource? unitResource = GamePack.Current.UnitResources.GetUnitResource(unitIndex2);
                        int unitLevel = UnitLevelToCityLevel(unitResource.Level);
                        if (unitLevel != -1 && unitLevel <= UpgradeLevel)
                        {
                            bool researched = unitLevel == 1 && !(raceResource.SpecialUnitIndexes.Count >= 4 && raceResource.UnitIndexes[i] == raceResource.SpecialUnitIndexes[3]);
                            int unitIndex3 = raceResource.UnitIndexes[i];
                            Production.UnitProductions.Add(unitIndex3, researched);
                        }
                    }
                }
            }

            s_sortingCity = this;
            Production.UnitProductions.SortChildren<UnitProduction>(CompareCityProductions);
        }
    }

    // City.pas:2085
    public void SetName(string name)
    {
        Name = name;
        Update();
    }

    // City.pas:2091
    public void SetWallType(int wallType)
    {
        WallType = wallType;
        UpdateImages();
    }

    // City.pas:2206
    public void InitializeCombat(Battle? combat, ArmyHexSprite? army)
    {
        if (Player == 0)
        {
            int enemiesIndependentRelation = GetEnemiesIndependentRelation(army.Army.Player);
            combat.CombatData.Players.GetPlayers(combat.CombatData.DefenderPlayer).IndependentRelation = enemiesIndependentRelation.TruncateToByte();
        }

        int hexLevel = army.HexLevel;
        int hexY = army.HexY;
        int hexX = army.HexX;
        combat.InitializeDefaultArmies(army.Army, hexX, hexY, L: hexLevel);
    }

    // City.pas:2234
    public int GetObtainValue()
    {
        int size = Size;
        int result = UpgradePrices[size - 1] * UpgradeLevel;
        if (WallType == 1)
        {
            int size2 = Size;
            result += WoodWallPrices[size2 - 1];
        }

        if (WallType == 2)
        {
            int size2 = Size;
            int size3 = Size;
            return result + WoodWallPrices[size2 - 1] + StoneWallPrices[size3 - 1];
        }

        return result;
    }

    // City.pas:2241
    public int JoinAmount(sbyte player)
    {
        int result = 0;
        ArmyHSList armies = new(null) { Holder = CreationHolder() };
        _ = ListArmies(armies);
        int i = armies.Count;
        if (i != 0)
        {
            bool canJoin = true;
            while (i != 0 && canJoin)
            {
                i -= 1;
                Army? army = armies[i].Army;
                int selection = army.GetCanJoinSelection(player);
                canJoin = selection == army.ValidateSelection(255);
                if (canJoin)
                {
                    result += army.JoinAmount(player, selection: 255);
                }
            }

            if (canJoin)
            {
                int value = GetObtainValue() / 4;
                int relation = (byte)StrategicMap.Current.Races.GetRace(Race).GetPlayerRelation(player);
                result += JoinPercentByStatus[(int)(Status)relation] * value / 100;
            }
            else
            {
                result = -1;
            }
        }
        else
        {
            Status playerRelation = StrategicMap.Current.Races.GetRace(Race).GetPlayerRelation(player);
            result = playerRelation is Session.Status.Polite or Session.Status.Friendly ? 0 : -1;
        }

        armies?.Dispose();
        return result;
    }

    // City.pas:2282
    public bool IncomingNegotiateRequest(NegotiateRequestEventLog? request)
    {
        if (Player == 0 && !Mode.Contains(Modes.WarMode))
        {
            int amount = JoinAmount(request.Player);
            if (amount != -1)
            {
                request.Object = NegotiateObject.City;
                string name = GetName();
                request.Text = Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_cityJoinHeaderFormat), args: [name]);
                request.Gold = amount;
                if (request.Gold == 0)
                {
                    string cityFreeJoin = AbilityRules.TranslateResourceString(GetEngine(), S: s_cityFreeJoinText);
                    request.JoinMessage.Text = cityFreeJoin;
                }
                else
                {
                    string format = Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_cityTributeJoinFormat), args: [request.Gold.ToString(CultureInfo.InvariantCulture)]);
                    request.JoinMessage.Text = format;
                }

                return true;
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

    // City.pas:2302
    public bool IncomingNegotiateOffer(NegotiateTokenEvent? offer)
    {
        if (offer.Object == NegotiateObject.City)
        {
            int amount = JoinAmount(offer.Player);
            if (offer.Offer == NegotiateTokenEvent.NegotiateOffer.OfferBribe && amount <= offer.Gold && amount != -1)
            {
                if (StrategicMap.Current.Players.GetPlayers(offer.Player).Gems >= offer.Gold)
                {
                    SetPlayer(offer.Player);
                    ArmyHSList armies = new(null) { Holder = CreationHolder() };
                    _ = ListArmies(armies);
                    for (int i = 0, last = armies.Count - 1; i <= last; i++)
                    {
                        if (armies[i].Army.Player == 0)
                        {
                            armies[i].Army.SetPlayer(offer.Player);
                        }
                    }

                    armies?.Dispose();
                    LocationTextEventLog log = new(null)
                    {
                        Holder = CreationHolder(),
                        Player = Player
                    };
                    string name = GetName();
                    log.Text = Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_overthrownFormat), args: [name]);
                    log.SetLocation(level: HexLevel, y: HexY, x: HexX);
                    EventLog.DistributeLocationEventLog(y: HexY, level: HexLevel, x: HexX, log: log, player: Player, execute: false);
                    _ = log.Release();
                    offer.Accept();
                }
                else
                {
                    string notEnoughGold = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NotEnoughGoldText);
                    StrategicMap.Current.ShowMessage(notEnoughGold);
                }
            }
            else
            {
                offer.Reject();
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    // City.pas:2334
    public int GetEnemiesIndependentRelation(sbyte player)
    {
        UnitUserList units = new(null) { Holder = CreationHolder() };
        _ = ListUnits(units);
        for (int i = units.Count - 1; i >= 0; i--)
        {
            if (units.GetUnit(i).CanJoin(player))
            {
                _ = units.DeleteUnit(units.GetUnit(i));
            }
        }

        // { Original uses the race index as the default relation. }
        int result = UnitList.UnitsToIndependentRelation(units, defaultRelation: Race);
        units?.Dispose();
        return result;
    }

    // City.pas:2345
    public int GetIndependentRelation()
    {
        UnitUserList units = new(null) { Holder = CreationHolder() };
        _ = ListUnits(units);
        // { Original uses the race index as the default relation. }
        int result = UnitList.UnitsToIndependentRelation(units, defaultRelation: Race);
        units?.Dispose();
        return result;
    }

    // City.pas:2354
    public void ReturnBattleFieldInfo(ref BattleFieldInfo info)
    {
        if (info.Requests.Contains(BattleFieldInfoRequests.EffectiveStrength) && Player > 0)
        {
            int value = 5_000;
            Status playerRelation = StrategicMap.Current.Races.GetRace(Race).GetPlayerRelation(Player);
            if (playerRelation is Session.Status.Hostile or Session.Status.Wary)
            {
                value *= 2;
            }

            DiplomaticRelation relation = AbilityRules.DiplomaticRelation(info.Player, info.IndependentRelation, otherPlayer: Player, otherIndependentRelation: 0);
            if (info.FriendlyRelations.Contains(relation.ToFlag()))
            {
                info.FriendlyStrength -= value;
            }

            if (info.EnemyRelations.Contains(relation.ToFlag()))
            {
                info.Strength -= value;
            }
        }
    }

    // City.pas:2367
    public void UpdateVisibleForSeatedPlayer()
    {
        bool visible = VisibleForPlayer(StrategicMap.Current.SeatedPlayer);
        if (EnchantmentsVisible ^ visible)
        {
            EnchantmentsVisible = visible;
            if (Enchantments != CityEnchantmentIds.None)
            {
                StrategicMap.Current.GlobalMagicControl.TriggerChangedEvent();
            }
        }
    }

    // City.pas:2503
    public LibraryImage? GetTopCityActionImage(ProductionOrder? order) => GetCityActionImage(order, row: 0, hasRace: Race != -1, image: 40, undergroundImage: 43);

    // City.pas:2539
    public LibraryImage? GetMiddleCityActionImage(ProductionOrder? order) => GetCityActionImage(order, row: 1, hasRace: Race != -1, image: 41, undergroundImage: 44);

    // City.pas:2575
    public LibraryImage? GetBottomCityActionImage(ProductionOrder? order) => GetCityActionImage(order, row: 2, hasRace: Race >= 0, image: 42, undergroundImage: 45);

    // Row 0 to 2 of the city's map image while Order runs: an image sequence of the city when the
    // order names one, else the race image (Image on the surface, UndergroundImage below it).
    private LibraryImage? GetCityActionImage(ProductionOrder? order, int row, bool hasRace, int image, int undergroundImage)
    {
        int? sequence = order?.CitySequence(this, row);
        return sequence is int index
            ? Resource.ImageSequenceList is not null && index != -1
                ? Resource.ImageSequenceList.GetImageSequence(index)?.GetPreviewImage()
                : null
            : hasRace
                ? HexLevel == 0
                    ? GamePack.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(image)
                    : GamePack.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(undergroundImage)
                : null;
    }

    // City.pas:138
    public void GenerateRebelUnits(UnitList units)
    {
        int saved = RandSeed;
        RandSeed = (HexX * 10) + StrategicMap.Current.InitialRandomSeed + HexY;
        _ = Int32List.FillWithRandomUnits(GamePack.Current.RaceResources.GetRaceResource(Race).UnitIndexes, units.Units, level: 2, value: Size * 25, types: UnitTypes.Humanoid | UnitTypes.Creature);
        // The source restores only after a successful fill.
        RandSeed = saved;
    }

    private static readonly string s_averageText = "Average";
    private static readonly string s_cityBattleDrawnFormat = "Battle of %s drawn";
    private static readonly string s_cityBattleLostFormat = "Battle of %s lost";
    private static readonly string s_cityBattleWonFormat = "%s captured";
    private static readonly string s_cityFreeJoinText = "The city council eagerly vows its loyalty and awaits your orders.";
    private static readonly string s_cityJoinHeaderFormat = "A herald from %s:";
    private static readonly string s_cityTributeJoinFormat = "The city council offers its allegiance for %s pieces of gold.";
    private static readonly string s_maxFortifyLevelReachedText = "Max. fortify level reached.";
    private static readonly string s_maxUpgradeLevelReachedText = "Max. upgrade level reached.";
    private static readonly string s_noNameSpecifiedText = "no name specified";
    private static readonly string s_noNameText = "Noname";
    private static readonly string s_occupationForcesFormat = "Occupation Forces: %s";
    private static readonly string s_raceRelationFormat = "Race Relation: %s";
    private static readonly string s_strongText = "Strong";
    private static readonly string s_terrainBonusFormat = "Terrain Bonus: %s";
    private static readonly string s_terrainPenaltyFormat = "Terrain Penalty: %s";
    private static readonly string s_upgradeLevelFormat = "Upgrade Level: %s";
    private static readonly string s_weakText = "Weak";
    private static readonly string s_cropFieldsFormat = "%s Crop Fields";
    private static readonly string s_overthrownFormat = "%s overthrown";

    internal static RaceTerrainInfo[] RaceTerrain { get; } =
    [
        new() {
            Bonus = TerrainTypes.None,
            Penalty = TerrainTypes.None
        },
        new() {
            Bonus = TerrainTypes.Desert,
            Penalty = TerrainTypes.Snow | TerrainTypes.Ice | TerrainTypes.UndergroundIce
        },
        new() {
            Bonus = TerrainTypes.Water,
            Penalty = TerrainTypes.Desert | TerrainTypes.Wasteland
        },
        new() {
            Bonus = TerrainTypes.Snow | TerrainTypes.Ice | TerrainTypes.UndergroundIce,
            Penalty = TerrainTypes.Desert
        },
        new() {
            Bonus = TerrainTypes.Grass,
            Penalty = TerrainTypes.Wasteland
        },
        new() {
            Bonus = TerrainTypes.Grass,
            Penalty = TerrainTypes.Wasteland
        },
        new() {
            Bonus = TerrainTypes.Dirt,
            Penalty = TerrainTypes.None
        },
        new() {
            Bonus = TerrainTypes.None,
            Penalty = TerrainTypes.None
        },
        new() {
            Bonus = TerrainTypes.Dirt,
            Penalty = TerrainTypes.None
        },
        new() {
            Bonus = TerrainTypes.None,
            Penalty = TerrainTypes.None
        },
        new() {
            Bonus = TerrainTypes.Dirt,
            Penalty = TerrainTypes.None
        },
        new() {
            Bonus = TerrainTypes.Wasteland,
            Penalty = TerrainTypes.Grass
        }
    ];

    private static ReadOnlySpan<int> JoinPercentByStatus => [200, 175, 150, 125, 100];
    private static ReadOnlySpan<byte> UpgradePrices => [250, 250, 250, 250];
    private static ReadOnlySpan<byte> WoodWallPrices => [20, 40, 60, 80];
    private static ReadOnlySpan<byte> StoneWallPrices => [20, 40, 60, 80];

    private static readonly string s_cityWarningText = "Warning";
    private static readonly string s_cityWoodenWallText = "Wooden Wall";
    private static readonly string s_cityStoneWallText = "Stone Wall";
    private static City s_sortingCity;

    private static bool RaceInCity(int race, sbyte player)
    {
        bool flag = false;
        Player? players = StrategicMap.Current.Players.GetPlayers(player);
        for (int i = 0; i != players.Structures.Count; i++)
        {
            if (flag)
            {
                break;
            }
            if (players.Structures.GetStructure(i) is City city)
            {
                flag = !city.Building && city.Race == race;
            }
        }
        return flag;
    }

    private static int UnitLevelToCityLevel(int level) => level;

    private static int CompareCityProductions(UnitProduction item1, UnitProduction item2)
    {
        UnitProduction unitProduction = item1;
        int number;
        if (GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex) is not null)
        {
            number = GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex).Level * 2;
            if (s_sortingCity.Race != -1 && GamePack.Current.RaceResources.GetRaceResource(s_sortingCity.Race).SpecialUnitIndexes.Count > 3 && GamePack.Current.RaceResources.GetRaceResource(s_sortingCity.Race).SpecialUnitIndexes[3] == unitProduction.UnitIndex)
            {
                number++;
            }
        }
        else
        {
            number = 0;
        }
        UnitProduction unitProduction2 = item2;
        int number2;
        if (GamePack.Current.UnitResources.GetUnitResource(unitProduction2.UnitIndex) is not null)
        {
            number2 = GamePack.Current.UnitResources.GetUnitResource(unitProduction2.UnitIndex).Level * 2;
            if (s_sortingCity.Race != -1 && GamePack.Current.RaceResources.GetRaceResource(s_sortingCity.Race).SpecialUnitIndexes.Count > 3 && GamePack.Current.RaceResources.GetRaceResource(s_sortingCity.Race).SpecialUnitIndexes[3] == unitProduction2.UnitIndex)
            {
                number2++;
            }
        }
        else
        {
            number2 = 0;
        }
        int number3 = number - number2;
        if (number3 == 0)
        {
            number3 = item1.UnitIndex - item2.UnitIndex;
        }
        return number3;
    }

    [Flags]
    public enum Modes : byte
    {
        None = 0,
        WarMode = 1 << 0,
        Razed = 1 << 1,
        TownQuake = 1 << 2,
        Anarchy = 1 << 3,
    }

    [Flags]
    public enum MoveOutOfCitySelections : byte
    {
        None = 0,
        SkipOwnPlayer = 1 << 0,
    }

    public enum CivilStatus : byte
    {
        Unruly = 0, Unrest = 1, Stable = 2, Content = 3, Cheerful = 4, Oppressed = 5, Slavery = 6
    }

    public enum DefendersGenerateMode : byte
    {
        None = 0, Random = 1, Weak = 2, Average = 3, Strong = 4
    }
}
