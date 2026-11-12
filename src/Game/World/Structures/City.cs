// Translated from City.pas.
using System.Runtime.CompilerServices;
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// Mode, Race, Name, WallType, UpgradeLevel, Production, _flagId, EnchantmentList,
// Enchantments, _overlay, EnchantmentsVisible, WallBackImage, WallFrontImage, CityImage,
// DefendersMode
public partial class City : PlayerCropStructure
{
    public City(ECustomNode owner) : base(owner)
    {
        Production = new CityProductionControl(null);
        Production.City = this;
        _defendersMode = DefendersGenerateMode.Average;
        Race = -1;
        Name = AbilityRules.TranslateResourceString(s_noNameText);
        UpgradeLevel = 1;
        _overlay = new CityOverlayHexSprite(null);
        _overlay.City = this;
        _enchantmentList = new GlobalEnchantmentList(null);
        EnchantmentsVisible = false;
    }

    // City.pas:849
    public new const int ClassId = 131336;

    public Set<ModeElement> Mode;
    public sbyte Race;
    public string Name = "";
    public byte WallType;
    public byte UpgradeLevel;

    public CityProductionControl Production { get; }

    private byte _flagId;
    private GlobalEnchantmentList _enchantmentList;

    public Set<CityEnchantmentId> Enchantments { get; private set; }

    public CityOverlayHexSprite _overlay;

    public bool EnchantmentsVisible { get; private set; }
    public LibraryImage WallBackImage { get; private set; }
    public LibraryImage WallFrontImage { get; private set; }
    public LibraryImage CityImage { get; private set; }

    private DefendersGenerateMode _defendersMode;

    // City.pas:839
    public override void Dispose()
    {
        if (GetVisible())
        {
            AoWHSMap.Current.DynamicVisibleHexSprite.RemoveChild(_overlay);
        }

        _overlay?.Dispose();
        base.Dispose();
        Production?.Dispose();
        _enchantmentList?.Dispose();
        _enchantmentList = null;
    }

    // City.pas:2157
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwString(ref Name, 12);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Race), 13);
        stream.rwData(Bytes.Of(ref WallType, 1), 1, 15);
        stream.rwByte(ref UpgradeLevel, 16);
        stream.rwEObject(Production, 18);
        RwSet(ref Mode, 1, 22, stream.rwData, 0);
        stream.rwData(Bytes.Of(ref _defendersMode, 1), 1, 27);
        stream.rwEObject(_enchantmentList, 29);
    }

    // City.pas:824

    // City.pas:2191
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001989)
        {
            if (Race >= 0)
            {
                if (AoWHSMap.Current.Races.GetRace(Race) == null || !AoWHSMap.Current.Races.GetRace(Race).Enabled)
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
        int X = default;
        int Y = default;
        HexGeometry.HexToPixel(_hexX, _hexY, ref X, ref Y);
        pixelXSurface = X - pixelXSurface;
        pixelYSurface = Y - pixelYSurface;
        if (!Mode.Contains(ModeElement.Razed) || GetBuilding())
        {
            if ((AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) || !Fogged || !AoWHSMap.Current.FogEnabled) && _flagId != 63 && (((PlayerStructureResource)_resource).Claimed || (_flagId & 32) == 0))
            {
                var cityResource = (PlayerStructureResource)_resource;
                if (cityResource.FlagType <= 1)
                {
                    AoWHSSet.Current.FlagControl.ShowFlag(cityResource.FlagX + pixelXSurface, cityResource.FlagY + pixelYSurface, cityResource.Engine.HexSpriteMap.Frame + X, this._flagId, lockedSurface);
                }
                else
                {
                    AoWHSSet.Current.FlagControl.ShowStructureFlag(cityResource.FlagX + pixelXSurface, cityResource.FlagY + pixelYSurface, cityResource.Engine.HexSpriteMap.Frame + X, this._flagId, lockedSurface);
                }
            }
        }

        if (Mode.Contains(ModeElement.TownQuake))
        {
            pixelXSurface = pixelXSurface + Random(5) - 2;
            pixelYSurface = pixelYSurface + Random(5) - 2;
        }

        if (!Mode.Contains(ModeElement.Razed))
        {
            ((Resource)_resource).ShowCity(pixelXSurface, pixelYSurface, lockedSurface, this);
        }
        else
        {
            ((StructureResource)_resource).Show(pixelXSurface, pixelYSurface, lockedSurface, this);
        }
    }

    // City.pas:2170
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        if (result)
        {
            UpdateMapFields(Owner.Owner, _hexX, _hexY);
        }

        return result;
    }

    // City.pas:2176
    public override void RemoveFromMap()
    {
        MapLevel mapLevel;
        mapLevel = Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(mapLevel, _hexX, _hexY);
    }

    // City.pas:2378
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (!(message.MessageId == 4364 && _resource.TerrainList.Terrain[message.HexNumber] >= 0))
        {
            base.MapFieldMessageProc(ref message);
        }

        if (_resource.TerrainList.Terrain[message.HexNumber] >= 0)
        {
            if (message.MessageId == 4364 && message.HexNumber == 0)
            {
                if ((!AoWHSMap.Current.FogEnabled || !Fogged) && Player >= 0)
                {
                    if (Player == 0)
                    {
                        message.Param[0] = message.Param[0].Int + 851968;
                    }
                    else
                    {
                        var player = AoWHSMap.Current.Players.GetPlayers(Player);
                        message.Param[0] = message.Param[0].Int + (player.PlayerRace + 1) * 65536;
                    }
                }
                else
                {
                    message.Param[0] = message.Param[0].Int + 917504;
                }

                message.Param[0] = message.Param[0].Int + (((Resource)_resource).Size - 1) * 4194304;
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
                        ReturnBattleFieldInfo(ref message.Param[0].As<BattleFieldQuery>().Info);
                    }
                    else
                    {
                        if (message.MessageId == 4359)
                        {
                            InitializeCombat(message.Param[0].As<Battle>(), message.Param[1].As<ArmyHexSprite>());
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
                                    if (WallType != 0 && !Mode.Contains(ModeElement.WarMode) && !GetRazed())
                                    {
                                        if (this == message.Param[0].As<MapField>().FindNoneTransparentHexSprite(typeof(City)))
                                        {
                                            message.Result = WallType;
                                        }
                                    }
                                }
                                else
                                {
                                    if (message.MessageId == 4389)
                                    {
                                        if (Player == 0)
                                        {
                                            var independentRelation = GetIndependentRelation();
                                            message.Result = independentRelation;
                                        }
                                    }
                                    else
                                    {
                                        if (message.MessageId == 4366)
                                        {
                                            if (IncomingNegotiateRequest(message.Param[0].As<NegotiateRequestEventLog>()))
                                            {
                                                message.Param[1] = 1;
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
                                                if (message.MessageId == 4393 && Enchantments.Contains(CityEnchantmentId.AntiMagicShell))
                                                {
                                                    message.Param[0] = 1;
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
            AoWHSMap.Current.Players.GetPlayers(Player).ProductionControls.Remove(Production);
        }

        Production.Deactivate();
        base.Deactivate();
    }

    // City.pas:2184
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        base.UpdateMapField(info);
        if (_resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            ((AoWMapField)info.MapField).FieldFlags = ((AoWMapField)info.MapField).FieldFlags | 67108864;
        }
    }

    // City.pas:1246
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!Status.Contains(MapObjectStatusElement.Visible))
        {
            base.MakeVisible(visibilityList);
            if ((Set<CityEnchantmentId>.Of(CityEnchantmentId.GoldRush, CityEnchantmentId.AntiMagicShell, CityEnchantmentId.Warmonger) & Enchantments) != Set<CityEnchantmentId>.Of())
            {
                _overlay.MakeVisible(visibilityList);
            }
        }
    }

    // City.pas:2617
    public override void Select()
    {
        base.Select();
    }

    // City.pas:1787
    public override sbyte GetValidTerrainType(MapLevel mapLevel, sbyte hexX, sbyte hexY) => GetMostFrequentMapFieldTerrainType(mapLevel, hexX, hexY, -1);

    // City.pas:2422
    public override LibraryImage GetTopBorderImage()
    {
        int index;
        ImageSequenceList sequences;
        LibraryImage result = GetTopCityActionImage(GetActiveOrder());
        if (result == null)
        {
            if (Race != -1)
            {
                if (GetHexLevel() == 0)
                {
                    result = AoWHSSet.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(40);
                }
                else
                {
                    result = AoWHSSet.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(43);
                }
            }
            else
            {
                result = null;
            }
        }

        if (Mode.Contains(ModeElement.Razed))
        {
            index = 42;
            sequences = _resource.ImageSequenceList;
            if (sequences != null)
            {
                if (sequences.GetImageSequence(index) != null)
                {
                    result = _resource.ImageSequenceList.GetImageSequence(index).GetPreviewImage();
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                result = null;
            }
        }

        return result;
    }

    // City.pas:2449
    public override LibraryImage GetMiddleBorderImage()
    {
        int index;
        ImageSequenceList sequences;
        LibraryImage result = GetMiddleCityActionImage(GetActiveOrder());
        if (result == null)
        {
            if (Race != -1)
            {
                if (GetHexLevel() == 0)
                {
                    result = AoWHSSet.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(41);
                }
                else
                {
                    result = AoWHSSet.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(44);
                }
            }
            else
            {
                result = null;
            }
        }

        if (Mode.Contains(ModeElement.Razed))
        {
            index = 43;
            sequences = _resource.ImageSequenceList;
            if (sequences != null)
            {
                if (sequences.GetImageSequence(index) != null)
                {
                    result = _resource.ImageSequenceList.GetImageSequence(index).GetPreviewImage();
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                result = null;
            }
        }

        return result;
    }

    // City.pas:2476
    public override LibraryImage GetBottomBorderImage()
    {
        int index;
        ImageSequenceList sequences;
        LibraryImage result = GetBottomCityActionImage(GetActiveOrder());
        if (result == null)
        {
            if (Race >= 0)
            {
                if (GetHexLevel() == 0)
                {
                    result = AoWHSSet.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(42);
                }
                else
                {
                    result = AoWHSSet.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(45);
                }
            }
            else
            {
                result = null;
            }
        }

        if (Mode.Contains(ModeElement.Razed))
        {
            index = 44;
            sequences = _resource.ImageSequenceList;
            if (sequences != null)
            {
                if (sequences.GetImageSequence(index) != null)
                {
                    result = _resource.ImageSequenceList.GetImageSequence(index).GetPreviewImage();
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                result = null;
            }
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

            if (value)
            {
                Mode = Mode + Set<ModeElement>.Of(ModeElement.Razed);
            }
            else
            {
                Mode = Mode - Set<ModeElement>.Of(ModeElement.Razed);
            }

            ImageIndex = GetTerrainTypeImage(TerrainType);
            ResetEnchantments();
            UpdateImages();
            UpdateCrops();
            Update();
            SetWallType(0);
        }
    }

    // City.pas:1618
    public override bool GetRazed() => Mode.Contains(ModeElement.Razed);

    // City.pas:1111
    public override void SetupRazeDefenderAIGroup(UnitList armies, UnitList units)
    {
        AIGroup group;
        if (AoWHSMap.Current.Random(8) == 0)
        {
            var unitsToIndependentRelation = UnitList.UnitsToIndependentRelation(units, 5);
            group = AoWHSMap.Current.AIGroupManager.CreateGroup(0, unitsToIndependentRelation, GroupBehavior.Refuge);
            group.AddUnits(units);
            group.Release();
        }
        else
        {
            base.SetupRazeDefenderAIGroup(armies, units);
        }
    }

    // City.pas:864
    public override ExecuteResult ExecuteTokenEvent(StructureTokenEvent tokenEvent, TokenExecutor executor) => base.ExecuteTokenEvent(tokenEvent, executor);

    // City.pas:1134
    public override bool GetDefenseRequirements(sbyte player, ref DefenseRequirements requirements)
    {
        bool result;
        BattleFieldInfo info = default;
        int maximum;
        int minimum;
        Status relation;
        if ((Mode & Set<ModeElement>.Of(ModeElement.WarMode, ModeElement.Razed)) == Set<ModeElement>.Of())
        {
            BattleFieldInfo.SetupBattleFieldInfo(ref info, player, 0, Set<BattleFieldInfoRequestsElement>.Of(), AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));
            info.Requests = info.Requests + Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents);
            var hexLevel = GetHexLevel();
            var hexY = GetHexY();
            var hexX = GetHexX();
            AoWHSMap.Current.RetrieveBattleFieldInfo(hexX, hexY, hexLevel, 10, ref info);
            requirements.Strength = info.Strength;
            maximum = Round(GetSize() * 5000);
            minimum = Round((double)(GetSize() * 5000) * AoWHSMap.Current.Players.GetPlayers(player).AIControl.DefenseStrengthMultiplier);
            if (player != 0)
            {
                relation = AoWHSMap.Current.Races.GetRace(Race).GetPlayerRelation(player);
            }
            else
            {
                relation = Session.Status.Friendly;
            }

            // { Original condition makes this neutral-player adjustment unreachable. }
            if (Set<Status>.Of(Session.Status.Hostile, Session.Status.Wary, Session.Status.Neutral).Contains(relation) && player == Player && Player == 0)
            {
                maximum = Round(maximum) + 7500;
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

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // City.pas:1123
    public override DefensePriority GetDefensePriority()
    {
        DefensePriority result;
        if ((Mode & Set<ModeElement>.Of(ModeElement.WarMode, ModeElement.Razed)) == Set<ModeElement>.Of())
        {
            if (GetSize() == 1)
            {
                result = DefensePriority.Low;
            }
            else
            {
                result = DefensePriority.Average;
            }

            if (Production.Status == ProductionControlStatus.Producing && result == DefensePriority.Low)
            {
                result = DefensePriority.Average;
            }

            bool condition = Player != 0;
            if (condition)
            {
                var playerRelation = AoWHSMap.Current.Races.GetRace(Race).GetPlayerRelation(Player);
                condition = Set<Status>.Of(Session.Status.Hostile, Session.Status.Wary).Contains(playerRelation);
            }

            if (condition)
            {
                result = DefensePriority.High;
            }
        }
        else
        {
            result = DefensePriority.None;
        }

        return result;
    }

    // City.pas:1713
    public override void NewDay()
    {
        int I;
        base.NewDay();
        if (Race < 0)
        {
            if (Race < -1 && AoWHSMap.Current.GameType == GameType.Campaign)
            {
                I = Math.Abs(Race) - 3;
                if (I >= 0 && I < GameTables.GameEngine.Campaign.RaceHistory.Count)
                {
                    SetRace((sbyte)GameTables.GameEngine.Campaign.RaceHistory[I]);
                }
                else
                {
                    SetRace(AoWHSMap.Current.Races.GetRandomRace());
                }
            }
            else
            {
                SetRace(AoWHSMap.Current.Races.GetRandomRace());
            }
        }

        if (AoWHSMap.Current.Day == 1)
        {
            Production.UnitProductions.Clear();
            SetUpgradeLevel(UpgradeLevel);
            GenerateDefenders();
            if (WallType == 3)
            {
                WallType = (byte)AoWHSMap.Current.Random(3);
                UpdateImages();
            }
        }

        CheckRebellion();
        if (_overlay.Mode == CityOverlayHexSprite.OverlayMode.ShowRazeAnimation)
        {
            _overlay.SetMode(CityOverlayHexSprite.OverlayMode.Idle);
        }
    }

    // City.pas:1705
    public override void NewTurn(sbyte player)
    {
        base.NewTurn(player);
        if (!Mode.Contains(ModeElement.WarMode))
        {
            Production.NewTurn(player);
        }

        if (player == Player)
        {
            if (AoWHSMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
            {
                UpdateAIMigration();
            }
        }
    }

    // City.pas:1335
    public override void ArmyPlaced(ArmyHexSprite placedArmy)
    {
        base.ArmyPlaced(placedArmy);
        if (Enchantments.Contains(CityEnchantmentId.FountainOfLife))
        {
            ExecuteFountainOfLife();
        }

        if (Enchantments.Contains(CityEnchantmentId.AntiMagicShell))
        {
            _overlay.PlayFadeShellAnimation();
        }
    }

    // City.pas:1342
    public override void ArmyRemoved(ArmyHexSprite army)
    {
        base.ArmyRemoved(army);
        UpdatePlayer();
    }

    // City.pas:886
    public override void BuildingDone()
    {
        UpgradeLevel = 1;
        WallType = 0;
        SetRace(AoWHSMap.Current.Players.GetPlayers(BuildingPlayer).PlayerRace);
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
            var playerRelationValue = AoWHSMap.Current.Races.GetRace(Race).GetPlayerRelationValue(player);
            AoWHSMap.Current.Races.GetRace(Race).SetPlayerRelationValue(player, playerRelationValue + -30);
        }

        return result;
    }

    // City.pas:2611
    public override string GetCurrentActivityText()
    {
        string result;
        if (Production.Queue.Count == 0)
        {
            result = AbilityRules.TranslateResourceString(GameText.ProducingMerchandiseText);
        }
        else
        {
            var queue = Production.Queue.GetItem(0);
            result = Production.GetProductionNameProgressive(queue);
        }

        return result;
    }

    // City.pas:876
    public override bool ValidateMap(StringCollection messages)
    {
        bool result = base.ValidateMap(messages);
        var noName = AbilityRules.TranslateResourceString(s_noNameText);
        if (CompareText(Name, noName) == 0 || IsEmpty(Name))
        {
            messages.Add(AbilityRules.TranslateResourceString(s_cityWarningText) + ": " + AbilityRules.TranslateResourceString(s_noNameSpecifiedText));
            result = false;
        }

        return result;
    }

    // City.pas:1162
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        int amount;
        int priority;
        if (!Mode.Contains(ModeElement.Razed))
        {
            switch (info.Mode)
            {
                case AITargetMode.Attack:
                case AITargetMode.Raid:
                    if (info.Player != Player)
                    {
                        priority = data.Priority;
                        if (Player > 0)
                        {
                            if (AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, Player, Race))
                            {
                                priority = 100 + (GetSize() - 1) * 10;
                            }
                        }
                        else
                        {
                            if (Player == 0)
                            {
                                var playerRelation = AoWHSMap.Current.Races.GetRace(Race).GetPlayerRelation(info.Player);
                                if (Set<Status>.Of(Session.Status.Polite, Session.Status.Friendly).Contains(playerRelation))
                                {
                                    amount = JoinAmount(info.Player);
                                    if (amount != -1)
                                    {
                                        data.Cost = amount;
                                        priority = 100 + (GetSize() - 1) * 10;
                                        data.TargetType = AITargetType.Negotiate;
                                    }
                                    else
                                    {
                                        priority = 100 + (GetSize() - 1) * 10;
                                    }
                                }
                                else
                                {
                                    priority = 100 + (GetSize() - 1) * 10;
                                }
                            }
                            else
                            {
                                if (Mode.Contains(ModeElement.WarMode) && priority != 0)
                                {
                                    priority = 100 + (GetSize() - 1) * 10;
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
                        data.Priority = data.Priority | 2;
                    }

                    break;
                case AITargetMode.Fortify:
                    bool condition = info.Player == Player;
                    if (condition)
                    {
                        var playerRelation = AoWHSMap.Current.Races.GetRace(Race).GetPlayerRelation(info.Player);
                        condition = Set<Status>.Of(Session.Status.Polite, Session.Status.Friendly).Contains(playerRelation);
                    }

                    if (condition && GetActiveAction() != CityAction.Migrate)
                    {
                        var size = GetSize();
                        data.Priority = 500 + size * 100 + WallType * 45;
                        if (Enchantments.Contains(CityEnchantmentId.AntiMagicShell))
                        {
                            data.Priority += 150;
                        }

                        if (GetHexLevel() != 0)
                        {
                            data.Priority += 50;
                        }
                    }

                    break;
                case AITargetMode.Defend:
                    if (info.Player == Player)
                    {
                        var size = GetSize();
                        data.Priority = 50 + size * 20;
                    }

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
    public override bool CanDoubleClick() => AoWHSMap.Current.SeatedPlayer == Player && !GetRazed() && !GetBuilding();

    // City.pas:1639
    public override void GetRebuildInfo(ref int cost, ref int time)
    {
        cost = GetSize() * 25;
        time = GetSize() + 1;
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
                AoWHSMap.Current.Players.GetPlayers(Player).ProductionControls.Remove(Production);
            }

            base.SetPlayer(player);
            if (GetActiveAction() == CityAction.Migrate)
            {
                Production.ExecuteCancelProduction();
                Production.Queue.GetItem(0)?.Dispose();
            }

            ValidateAICityAction();
            UpdateAIMigration();
            if (Player != -1)
            {
                AoWHSMap.Current.Players.GetPlayers(Player).ProductionControls.Add(Production);
            }
        }
    }

    // City.pas:2071
    public override void UpdateFlagId()
    {
        if (!Status.Contains(MapObjectStatusElement.EditMode) && (!Mode.Contains(ModeElement.Razed) || GetBuilding()) && !Mode.Contains(ModeElement.WarMode))
        {
            if (Player > 0)
            {
                _flagId = (byte)(AoWHSMap.Current.Players.GetPlayers(Player).PlayerRace + (UpgradeLevel - 1) * 64);
            }
            else
            {
                _flagId = (byte)(32 + (UpgradeLevel - 1) * 64);
            }
        }
        else
        {
            _flagId = 63;
        }
    }

    // City.pas:1792
    public override int VisibilityRange()
    {
        int result = 6;
        if (GetHexLevel() != 0 || Player > 0 && (((byte)AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments.Image) & 8) != 0)
        {
            result = (int)(((uint)(result + 1)) >> 1);
        }

        return result;
    }

    // City.pas:2225
    public override void FinalizeCombat(Battle combat)
    {
        if (combat.CombatResult == CombatResult.Victory || combat.CombatResult == CombatResult.NoDefenders)
        {
            var combatData = combat.CombatData;
            SetPlayer(combatData.Players.GetPlayers(combatData.AttackerPlayer).Player);

            UpdatePlayer();
        }
    }

    // City.pas:2214
    public override string GetCombatEventText(Battle combat)
    {
        string result = "";
        if (!GetRazed())
        {
            if (combat.CombatResult == CombatResult.Victory || combat.CombatResult == CombatResult.NoDefenders)
            {
                var cityBattleWon = AbilityRules.TranslateResourceString(s_cityBattleWonFormat);
                var name = GetName();
                result = RtlUnit.Format(cityBattleWon, [name]);
            }
            else
            {
                if (combat.CombatResult == CombatResult.Defeat)
                {
                    var cityBattleLost = AbilityRules.TranslateResourceString(s_cityBattleLostFormat);
                    var name = GetName();
                    result = RtlUnit.Format(cityBattleLost, [name]);
                }
                else
                {
                    if (combat.CombatResult != CombatResult.CannotBreachWall)
                    {
                        var cityBattleDrawn = AbilityRules.TranslateResourceString(s_cityBattleDrawnFormat);
                        var name = GetName();
                        result = RtlUnit.Format(cityBattleDrawn, [name]);
                    }
                }
            }
        }

        return result;
    }

    // City.pas:1062
    public override void StructureCaptured(sbyte oldPlayer, sbyte newPlayer)
    {
        CityProductionEventLog log;
        if (!GetRazed() && !GetBuilding() && oldPlayer == 0 && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && Status.Contains(MapObjectStatusElement.Activated) && !((PlayerStructureResource)_resource).Claimed)
        {
            log = new CityProductionEventLog(null);
            log._player = newPlayer;
            var editName = EditName();
            log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.StructureCapturedFormat), [editName]);
            log.HexX = GetHexX();
            log.HexY = GetHexY();
            log.HexLevel = GetHexLevel();
            var hexY = GetHexY();
            var hexLevel = GetHexLevel();
            var hexX = GetHexX();
            EventLog.DistributeLocationEventLog(log, Player, hexX, hexY, hexLevel, false);
            log.Release();
        }
    }

    // City.pas:2271
    public override MoveAction GetMoveAction(sbyte player, MoveAction action)
    {
        MoveAction result = action;
        if (Player == 0)
        {
            if (Set<MoveAction>.Of(MoveAction.Normal, MoveAction.Attack).Contains(action) && !GetRazed() && !Mode.Contains(ModeElement.WarMode))
            {
                if (JoinAmount(player) != -1)
                {
                    result = MoveAction.Negotiate;
                }
                else
                {
                    result = MoveAction.Attack;
                }
            }
        }
        else
        {
            if (Player != player && Player != -1)
            {
                result = MoveAction.Attack;
            }
        }

        return result;
    }

    // City.pas:1285
    public override bool UpdatePlayer()
    {
        bool multiplePlayers;
        MapLevel mapLevel;
        int count;
        HexNumberWalkData walk = default;
        ArmyHexSprite army;
        sbyte player;
        if (!(Mode.Contains(ModeElement.Razed) && !GetBuilding()))
        {
            multiplePlayers = false;
            player = -1;
            count = GetTerrainCount();
            mapLevel = Owner.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(ref walk, _hexX, _hexY, 0);
            while (walk.HexNumber < count && !multiplePlayers)
            {
                if (_resource.TerrainList.GetTerrain(walk.HexNumber) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    army = (ArmyHexSprite)mapLevel.GetField(walk.X, walk.Y).FindOwnedChild(131607);
                    if (army != null)
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
                if (!Mode.Contains(ModeElement.WarMode))
                {
                    Mode = Mode + Set<ModeElement>.Of(ModeElement.WarMode);
                    SetPlayer(-1);
                    UpdateFlagId();
                    Update();
                }
            }
            else
            {
                if (Mode.Contains(ModeElement.WarMode))
                {
                    Mode = Mode - Set<ModeElement>.Of(ModeElement.WarMode);
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
        if (!GetRazed() && GetActiveAction() != CityAction.Migrate && !GetBuilding())
        {
            SetIncome((byte)(GetCropCount() * 2 + GetSize() * 5));
            if (Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(GetActiveAction()))
            {
                SetIncome((byte)(Income + Round((double)Income * 0.25)));
            }

            if (Enchantments.Contains(CityEnchantmentId.GoldRush))
            {
                SetIncome((byte)(Income * 2));
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
        int count;
        if (!GetEditMode())
        {
            if (GetRazed())
            {
                count = 0;
            }
            else
            {
                if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                {
                    count = ((PlayerCropStructureResource)_resource).CropLocations.Count;
                }
                else
                {
                    count = -1;
                }
            }

            if (count != -1)
            {
                SetCropCount(count);
            }

            UpdateCropSettings();
            UpdateIncome();
        }
    }

    // City.pas:2108
    public override sbyte GetRace() => Race;

    // City.pas:1211
    public override void CropTrampled(sbyte player)
    {
        int race;
        if (player != Player && player != 0)
        {
            if (Player > 0)
            {
                race = AoWHSMap.Current.Players.GetPlayers(Player).PlayerRace;
            }
            else
            {
                race = Race;
            }

            var playerRelationValue = AoWHSMap.Current.Races.GetRace(race).GetPlayerRelationValue(player);
            AoWHSMap.Current.Races.GetRace(race).SetPlayerRelationValue(player, playerRelationValue + -5);
        }
    }

    // City.pas:2097
    public virtual void SetRace(sbyte race)
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
    public CityAction GetActiveAction() => GetActiveOrder()?.CityAction ?? CityAction.None;

    // The order the city carries out, null when it produces nothing.
    public ProductionOrder GetActiveOrder() => Production.Status == ProductionControlStatus.Producing ? Production.Queue.GetItem(0).Order : null;

    // City.pas:894
    public void UpdateActiveEnchantments()
    {
        Enchantments = Set<CityEnchantmentId>.Of();
        for (int I = _enchantmentList.Count - 1; I >= 0; I--)
        {
            var enchantment = _enchantmentList.GetEnchantments(I);
            Enchantments = Enchantments + Set<CityEnchantmentId>.Of(((CityEnchantment)enchantment)._enchantmentId);
        }
    }

    // City.pas:902
    public void RegisterEnchantments()
    {
        for (int I = _enchantmentList.Count - 1; I >= 0; I--)
        {
            var enchantment = _enchantmentList.GetEnchantments(I);
            _enchantmentList.GetEnchantments(I).Activate(enchantment.PlayerIndex);
        }

        UpdateActiveEnchantments();
    }

    // City.pas:910
    public void UnregisterEnchantments()
    {
        for (int I = _enchantmentList.Count - 1; I >= 0; I--)
        {
            var enchantment = _enchantmentList.GetEnchantments(I);
            _enchantmentList.GetEnchantments(I).Deactivate((byte)enchantment.PlayerIndex);
        }
    }

    // City.pas:917
    public void ResetEnchantments()
    {
        UnregisterEnchantments();
        _enchantmentList.Clear();
        Enchantments = Set<CityEnchantmentId>.Of();
    }

    // City.pas:924
    public int AICityEnchantmentUpkeepPriority(CityEnchantmentId id)
    {
        int result;
        switch (id)
        {
            case CityEnchantmentId.FountainOfLife:
                result = GetSize() * 25 + 600;
                break;
            case CityEnchantmentId.GoldRush:
                result = GetSize() * 25 + 600;
                break;
            case CityEnchantmentId.AntiMagicShell:
                result = GetSize() * 25 + 700;
                break;
            case CityEnchantmentId.Warmonger:
                result = GetSize() * 25 + 600;
                break;
            default:
                result = 0;
                break;
        }

        return result;
    }

    // City.pas:935
    public void ActivateEnchantment(CityEnchantment enchantment)
    {
        enchantment.Owner = _enchantmentList;
        enchantment.Activate(enchantment.PlayerIndex);
        Enchantments = Enchantments + Set<CityEnchantmentId>.Of(enchantment._enchantmentId);
        if (enchantment._enchantmentId == CityEnchantmentId.FountainOfLife)
        {
            ExecuteFountainOfLife();
        }

        if (enchantment._enchantmentId == CityEnchantmentId.GoldRush)
        {
            UpdateIncome();
        }

        if (enchantment._enchantmentId == CityEnchantmentId.AntiMagicShell)
        {
            _overlay.PlaySetupShellAnimation();
        }
    }

    // City.pas:945
    public void DeactivateEnchantment(CityEnchantment enchantment)
    {
        enchantment.Owner = null;
        enchantment.Deactivate((byte)enchantment.PlayerIndex);
        UpdateActiveEnchantments();
        if (enchantment._enchantmentId == CityEnchantmentId.GoldRush)
        {
            UpdateIncome();
        }

        enchantment.Release();
        Update();
    }

    // City.pas:955
    public void ListEnhancementsInfo(StringCollection list)
    {
        list.Add(RtlUnit.Format(AbilityRules.TranslateResourceString(s_upgradeLevelFormat), [IntToStr(UpgradeLevel)]));
        if (WallType == 1)
        {
            list.Add(AbilityRules.TranslateResourceString(s_cityWoodenWallText));
        }

        if (WallType == 2)
        {
            list.Add(AbilityRules.TranslateResourceString(s_cityStoneWallText));
        }

        if (Enchantments.Contains(CityEnchantmentId.FountainOfLife))
        {
            list.Add(AbilityRules.TranslateResourceString(GameText.FountainOfLifeText) + ": " + AbilityRules.TranslateResourceString(GameTables.EnchantmentText));
        }

        if (Enchantments.Contains(CityEnchantmentId.GoldRush))
        {
            list.Add(AbilityRules.TranslateResourceString(GameText.GoldRushText) + ": " + AbilityRules.TranslateResourceString(GameTables.EnchantmentText));
        }

        if (Enchantments.Contains(CityEnchantmentId.AntiMagicShell))
        {
            list.Add(AbilityRules.TranslateResourceString(GameText.AntiMagicShellText) + ": " + AbilityRules.TranslateResourceString(GameTables.EnchantmentText));
        }

        if (Enchantments.Contains(CityEnchantmentId.Warmonger))
        {
            list.Add(AbilityRules.TranslateResourceString(GameText.WarmongerText) + ": " + AbilityRules.TranslateResourceString(GameTables.EnchantmentText));
        }
    }

    // City.pas:970
    public void StartTownQuake()
    {
        Mode = Mode + Set<ModeElement>.Of(ModeElement.TownQuake);
    }

    // City.pas:973
    public void StopTownQuake()
    {
        Mode = Mode - Set<ModeElement>.Of(ModeElement.TownQuake);
    }

    // City.pas:976
    public void ExecuteAnarchy()
    {
        Mode = Mode + Set<ModeElement>.Of(ModeElement.Anarchy);
        CheckRebellion();
        Mode = Mode - Set<ModeElement>.Of(ModeElement.Anarchy);
    }

    // City.pas:983
    public bool ExecuteFountainOfLife()
    {
        ArmyHSList armies;
        ImageSequenceAnimation animation;
        bool playSound;
        armies = new ArmyHSList(null);
        ListArmies(armies);
        bool result = false;
        playSound = false;
        for (int I = 0, last = armies.Count - 1; I <= last; I++)
        {
            if (armies.GetArmyHexSprite(I).Army.HealUnits(255))
            {
                var hexLevel = armies.GetArmyHexSprite(I).GetHexLevel();
                var hexY = armies.GetArmyHexSprite(I).GetHexY();
                var hexX = armies.GetArmyHexSprite(I).GetHexX();
                if (AoWHSMap.Current.VisibleForPlayer(hexX, hexY, hexLevel, AoWHSMap.Current.SeatedPlayer))
                {
                    animation = new ImageSequenceAnimation(null);
                    animation.SetImageSequence(AoWHSSet.Current.AbilityControl.GetAbility(47).ImageSequences.ImageSequences[0]);
                    animation.AnimationLevel = 27;
                    var hexLevel2 = armies.GetArmyHexSprite(I).GetHexLevel();
                    var hexY2 = armies.GetArmyHexSprite(I).GetHexY();
                    var hexX2 = armies.GetArmyHexSprite(I).GetHexX();
                    AoWHSMap.Current.PlaceHex(animation, hexX2, hexY2, hexLevel2);
                    bool condition = playSound || armies.GetArmyHexSprite(I).Army.Player == AoWHSMap.Current.SeatedPlayer;
                    if (!condition)
                    {
                        var hexLevel3 = armies.GetArmyHexSprite(I).GetHexLevel();
                        var hexY3 = armies.GetArmyHexSprite(I).GetHexY();
                        var hexX3 = armies.GetArmyHexSprite(I).GetHexX();
                        condition = AoWHSMap.Current.Watching(hexX3, hexY3, hexLevel3, 1);
                    }

                    playSound = condition;
                }

                result = true;
            }
        }

        if (playSound)
        {
            AoWHSSet.Current.AbilityControl.GetAbility(47).SfxLibrary.Play(0);
        }

        armies?.Dispose();
        return result;
    }

    // City.pas:1015
    public void UpdateAIMigration()
    {
        MigrationList selection;
        int value;
        int bestIndex;
        int bestValue;
        ProductionItem item;
        if (AoWHSMap.Current.Day != 0 && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            if (Player > 0)
            {
                bool condition = AoWHSMap.Current.Players.GetPlayers(Player).PlayerType != PlayerType.Human && (Set<ModeElement>.Of(ModeElement.WarMode, ModeElement.Razed) & Mode) == Set<ModeElement>.Of();
                if (condition)
                {
                    var activeAction = GetActiveAction();
                    condition = Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(activeAction);
                }

                bool test = condition;
                if (test)
                {
                    var playerRelation = AoWHSMap.Current.Races.GetRace(Race).GetPlayerRelation(Player);
                    test = Set<Status>.Of(Session.Status.Hostile, Session.Status.Wary, Session.Status.Neutral).Contains(playerRelation);
                }

                if (test)
                {
                    selection = CreateMigrationSelection();
                    bestValue = 0;
                    bestIndex = -1;
                    for (int I = 0, last = selection.Count - 1; I <= last; I++)
                    {
                        var itemRelation = selection.GetItems(I);
                        if (Set<Status>.Of(Session.Status.Polite, Session.Status.Friendly).Contains(itemRelation.Relation))
                        {
                            var itemRace = selection.GetItems(I);
                            bool passed = AoWHSMap.Current.Players.RaceToPlayer(itemRace.Race) == -1;
                            if (!passed)
                            {
                                var migrationItem = selection.GetItems(I);
                                var player = AoWHSMap.Current.Players.GetPlayers(Player);
                                passed = migrationItem.Race == player.PlayerRace;
                            }

                            if (passed)
                            {
                                value = 10;
                            }
                            else
                            {
                                value = 5;
                            }
                        }
                        else
                        {
                            value = 0;
                        }

                        value += selection.GetItems(I).Time;
                        if (value > bestValue || value == bestValue && AoWHSMap.Current.Random(2) == 1)
                        {
                            bestValue = value;
                            bestIndex = I;
                        }
                    }

                    if (bestIndex != -1)
                    {
                        Production.ExecuteDeleteProductions();
                        item = new ProductionItem(null);
                        item.Order = new Migrate(selection.GetItems(bestIndex).Race);
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
                    var activeAction = GetActiveAction();
                    condition = Set<CityAction>.Of(CityAction.Migrate, CityAction.Loot).Contains(activeAction);
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
        bool cancel;
        bool condition = Player > 0 && AoWHSMap.Current.Players.GetPlayers(Player).PlayerType != PlayerType.Human && (Set<ModeElement>.Of(ModeElement.WarMode, ModeElement.Razed) & Mode) == Set<ModeElement>.Of();
        if (condition)
        {
            var activeAction = GetActiveAction();
            condition = !Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(activeAction);
        }

        if (condition)
        {
            var playerRelation = AoWHSMap.Current.Races.GetRace(Race).GetPlayerRelation(Player);
            bool contains = Set<Status>.Of(Session.Status.Hostile, Session.Status.Wary, Session.Status.Neutral).Contains(playerRelation);
            if (contains)
            {
                bool test = false;
                if (GetActiveOrder() is Migrate migrate)
                {
                    var playerRelation2 = AoWHSMap.Current.Races.GetRace(migrate.Race).GetPlayerRelation(Player);
                    test = Set<Status>.Of(Session.Status.Polite, Session.Status.Friendly).Contains(playerRelation2);
                }

                contains = !test;
            }

            cancel = contains;
            if (cancel)
            {
                Production.ExecuteDeleteProductions();
            }
        }
    }

    // City.pas:1222
    public bool MoveArmiesOutOfCity(Set<MoveOutOfCitySelectionElement> selection)
    {
        MapLevel mapLevel;
        ArmyHexSprite army;
        int count;
        HexNumberWalkData walk = default;
        bool result = true;
        count = GetTerrainCount();
        mapLevel = Owner.Owner;
        // { This eliminated copy preserves the original saved registers. }
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, _hexX, _hexY, 0);
        while (walk.HexNumber < count && result)
        {
            if (_resource.TerrainList.GetTerrain(walk.HexNumber) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                army = (ArmyHexSprite)mapLevel.GetField(walk.X, walk.Y).FindOwnedChild(131607);
                if (army != null && (!selection.Contains(MoveOutOfCitySelectionElement.SkipOwnPlayer) || army.Army.Player != Player))
                {
                    var hexLevel = GetHexLevel();
                    result = army.Place((sbyte)walk.X, (sbyte)walk.Y, hexLevel, Set<ArmyPlaceModeElement>.Of(ArmyPlaceModeElement.Safe, ArmyPlaceModeElement.ScanArea));
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        return result;
    }

    // City.pas:1256
    public int GetSize() => ((Resource)_resource).Size;

    // City.pas:1259
    public int GetObtainUnitsValue()
    {
        int count;
        MapLevel mapLevel;
        HexNumberWalkData Walk = default;
        ArmyHexSprite army;
        ref var walk = ref Walk;
        int result = 0;
        count = GetTerrainCount();
        mapLevel = Owner.Owner;
        var hexY = GetHexY();
        var hexX = GetHexX();
        HexNumberWalkData.InitializeHexNumberWalk(ref Walk, hexX, hexY, 0);
        while (walk.HexNumber < count)
        {
            if (_resource.TerrainList.GetTerrain(walk.HexNumber) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                army = (ArmyHexSprite)mapLevel.GetField(walk.X, walk.Y).FindOwnedChild(131607);
                if (army != null)
                {
                    result += army.Army.ObtainValue(255);
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref Walk);
        }

        return result;
    }

    // City.pas:1348
    public int UpgradeCost()
    {
        var size = GetSize();
        return s_upgradePrices[size - 1];
    }

    // City.pas:1351
    public int UpgradeTime() => 3;

    // City.pas:1356
    public bool CanUpgrade(bool queue, ref string reason)
    {
        bool result;
        int level;
        level = UpgradeLevel;
        if (queue)
        {
            for (int I = 0, last = Production.Queue.Count - 1; I <= last; I++)
            {
                var queueLocal = Production.Queue.GetItem(I);
                if (queueLocal.Order is Upgrade)
                {
                    level += 1;
                }
            }
        }

        if (level < GetSize())
        {
            result = true;
        }
        else
        {
            result = false;
            reason = AbilityRules.TranslateResourceString(s_maxUpgradeLevelReachedText);
        }

        return result;
    }

    // City.pas:1371
    public bool Upgrade(bool queue)
    {
        bool result;
        ProductionItem item;
        string reason = "";
        if (CanUpgrade(queue, ref reason) && !AoWHSMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            item = new ProductionItem(null);
            item.Order = new Upgrade();
            Production.Produce(item, queue);
            item?.Dispose();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // City.pas:1386
    public int FortifyCost()
    {
        int result;
        switch (WallType)
        {
            case 0:
                var size = GetSize();
                result = s_woodWallPrices[size - 1];
                break;
            case 1:
                var size2 = GetSize();
                result = s_stoneWallPrices[size2 - 1];
                break;
            default:
                result = -1;
                break;
        }

        return result;
    }

    // City.pas:1395
    public int FortifyTime() => 2;

    // City.pas:1400
    public bool CanFortify(bool queue, ref string reason)
    {
        bool result;
        int level;
        level = WallType;
        if (queue)
        {
            for (int I = 0, last = Production.Queue.Count - 1; I <= last; I++)
            {
                var queueLocal = Production.Queue.GetItem(I);
                if (queueLocal.Order is Fortify)
                {
                    level += 1;
                }
            }
        }

        if (level != 2)
        {
            result = true;
        }
        else
        {
            result = false;
            reason = AbilityRules.TranslateResourceString(s_maxFortifyLevelReachedText);
        }

        return result;
    }

    // City.pas:1415
    public bool Fortify(bool queue)
    {
        bool result;
        ProductionItem item;
        string reason = "";
        if (CanFortify(queue, ref reason) && !AoWHSMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            item = new ProductionItem(null);
            item.Order = new Fortify();
            Production.Produce(item, queue);
            item?.Dispose();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // City.pas:1430
    public int ProductionCost(int index) => Production.UnitProductionGold(index);

    // City.pas:1435
    public int ProductionTime(int index) => Production.UnitProductionTime(index);

    // City.pas:1440
    public bool CanProduce(int index, bool queue, ref string reason) => Production.CanProduceUnit(index, queue, ref reason);

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
    public bool CanTrade(bool queue, ref string reason) => true;

    // City.pas:1475
    public bool Trade(bool queue)
    {
        bool result;
        ProductionItem item;
        string reason = "";
        if (CanTrade(queue, ref reason) && !AoWHSMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            item = new ProductionItem(null);
            if (queue)
            {
                item.Order = new Trade(1);
            }
            else
            {
                item.Order = new Trade(-1);
            }

            Production.Produce(item, queue);
            item?.Dispose();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // City.pas:1491
    public int LootTime() => GetSize();

    // City.pas:1496
    public bool CanLoot(ref string reason) => !Mode.Contains(ModeElement.Razed);

    // City.pas:1499
    public bool Loot()
    {
        bool result;
        ProductionItem item;
        string reason = "";
        if (CanLoot(ref reason) && !AoWHSMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            item = new ProductionItem(null);
            item.Order = new Loot();
            Production.Produce(item, false);
            item?.Dispose();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // City.pas:1514
    public int MigrationTime(sbyte race)
    {
        int result;
        string reason = "";
        HexNumberWalkData walk = default;
        MapLevel mapLevel;
        int radius;
        City city;
        if (CanMigrate(race, ref reason))
        {
            if (race != Race)
            {
                radius = -1;
                mapLevel = Owner.Owner;
                HexNumberWalkData.InitializeHexNumberWalk(ref walk, _hexX, _hexY, 1);
                while (walk.Radius <= 20 && radius == -1)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                    {
                        city = (City)mapLevel.GetField(walk.X, walk.Y).FindNoneTransparentHexSprite(typeof(City));
                        if (city != null && city.Race == race)
                        {
                            radius = walk.Radius;
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }

                if (radius != -1)
                {
                    result = radius / 10 + 3;
                }
                else
                {
                    result = 5;
                }
            }
            else
            {
                result = 0;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // City.pas:1544
    public bool CanMigrate(sbyte race, ref string reason)
    {
        bool result;
        int I;
        sbyte player;
        if (Race != race)
        {
            result = RaceInCity(race, Player);
            I = AoWHSMap.Current.Players.Count;
            while (I != 1 && !result)
            {
                I -= 1;
                player = (sbyte)I;
                result = AoWHSMap.Current.Players.GetDiplomaticRelation((sbyte)(player << 0), Player) == DiplomaticRelation.Alliance && RaceInCity(race, player);
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // City.pas:1561
    public bool Migrate(sbyte race)
    {
        bool result;
        ProductionItem item;
        string reason = "";
        if (CanMigrate(race, ref reason) && !AoWHSMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            item = new ProductionItem(null);
            item.Order = new Migrate(race);
            Production.Produce(item, false);
            item?.Dispose();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // City.pas:1577
    public MigrationItem CreateMigrationItem(sbyte race)
    {
        MigrationItem result;
        string reason = "";
        if (CanMigrate(race, ref reason))
        {
            result = new MigrationItem(null);
            result.Race = race;
            result.Time = MigrationTime(race);
            result.Relation = AoWHSMap.Current.Races.GetRace(race).GetPlayerRelation(Player);
        }
        else
        {
            result = null;
        }

        return result;
    }

    // City.pas:1589
    public MigrationList CreateMigrationSelection()
    {
        MigrationItem item;
        MigrationList result = new MigrationList(null);
        for (int I = 0, last = AoWHSMap.Current.Races.Count - 1; I <= last; I++)
        {
            if (AoWHSMap.Current.Races.GetRace(I).Enabled)
            {
                item = CreateMigrationItem((sbyte)I);
                if (item != null && Set<Status>.Of(Session.Status.Neutral, Session.Status.Polite, Session.Status.Friendly).Contains(item.Relation))
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
        CivilStatus status;
        int chance;
        CityRebellionEventLog log;
        UnitList units;
        if (Player != 0)
        {
            status = GetCivilStatus();
            if (Set<CivilStatus>.Of(CivilStatus.Unruly, CivilStatus.Unrest).Contains(status) && !GetRazed())
            {
                if (status == CivilStatus.Unruly)
                {
                    chance = 50;
                }
                else
                {
                    chance = 10;
                }

                if (chance >= AoWHSMap.Current.Random(100))
                {
                    MoveArmiesOutOfCity(Set<MoveOutOfCitySelectionElement>.Of());
                    log = new CityRebellionEventLog(null);
                    log._player = Player;
                    log.CityName = GetName();
                    log.HexX = GetHexX();
                    log.HexY = GetHexY();
                    log.HexLevel = GetHexLevel();
                    var hexY = GetHexY();
                    var hexLevel = GetHexLevel();
                    var hexX = GetHexX();
                    EventLog.DistributeLocationEventLog(log, Player, hexX, hexY, hexLevel, false);
                    log.Release();
                    SetPlayer(0);
                    units = new UnitList(null);
                    GenerateRebelUnits(units);
                    for (int I = units.Count - 1; I >= 0; I--)
                    {
                        var hexLevel2 = GetHexLevel();
                        var hexY2 = GetHexY();
                        var hexX2 = GetHexX();
                        units.GetUnit(I).Place(hexX2, hexY2, hexLevel2, 0, Set<UnitPlaceModeElement>.Of());
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
        int value;
        ArmyHSList armies;
        if (!GetRazed() && _defendersMode != DefendersGenerateMode.None && !Mode.Contains(ModeElement.WarMode) && Player == 0)
        {
            armies = new ArmyHSList(null);
            ListArmies(armies);
            if (_defendersMode == DefendersGenerateMode.Random)
            {
                _defendersMode = (DefendersGenerateMode)(Random(3) + 2);
            }

            value = ((int)_defendersMode - 2) * 25 + 25;
            value += (GetSize() - 1) * value / 2;
            for (int I = 0, last = armies.Count - 1; I <= last; I++)
            {
                value -= armies.GetArmyHexSprite(I).Army.ObtainValue(255);
            }

            if (value > 0)
            {
                RandSeed = AoWHSMap.Current.Random(1048575);
                var raceResource = AoWHSSet.Current.RaceResources.GetRaceResource(Race);
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                AbilityRules.PlaceRandomUnits(GetHexX(), hexY, hexLevel, 0, raceResource.UnitIndexes, UpgradeLevel, value, Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature));
            }

            armies?.Dispose();
        }
    }

    // City.pas:1755
    public void ListIncomeInfo(StringCollection names, StringCollection values)
    {
        int income;
        income = GetSize() * 5;
        names.Add(AbilityRules.TranslateResourceString(GameText.BaseIncomeText));
        values.Add(IntToStr(income));
        income += GetCropCount() * 2;
        var cropCount = GetCropCount();
        names.Add(RtlUnit.Format(AbilityRules.TranslateResourceString(s_cropFieldsFormat), [IntToStr(cropCount)]));
        values.Add(IntToStr(GetCropCount() * 2));
        if (Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(GetActiveAction()))
        {
            names.Add(AbilityRules.TranslateResourceString(GameText.ProducingMerchandiseText));
            values.Add(IntToStr(Round((double)income * 0.25)));
            income += Round((double)income * 0.25);
        }

        if (Enchantments.Contains(CityEnchantmentId.GoldRush))
        {
            names.Add(AbilityRules.TranslateResourceString(GameText.GoldRushText));
            values.Add(IntToStr(income));
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
    public void ListCivilStatusInfo(StringCollection list)
    {
        CombatPredictor predictor;
        ArmyHSList armies;
        UnitList units;
        Troop U;
        if (Player != 0 && Race >= 0)
        {
            var playerRelation = AoWHSMap.Current.Races.GetRace(Race).GetPlayerRelation(Player);
            var status = AbilityRules.TranslateResourceString(GameTables.StatusTexts[(int)playerRelation]);
            list.Add(RtlUnit.Format(AbilityRules.TranslateResourceString(s_raceRelationFormat), [status]));
            if (RaceTerrain[Race].Penalty.Contains((byte)TerrainType))
            {
                var terrainPenaltyX = AbilityRules.TranslateResourceString(s_terrainPenaltyFormat);
                var terrainType = AoWHSSet.Current.TerrainTypes[TerrainType];
                list.Add(RtlUnit.Format(terrainPenaltyX, [terrainType]));
            }

            if (RaceTerrain[Race].Bonus.Contains((byte)TerrainType))
            {
                var terrainBonusX = AbilityRules.TranslateResourceString(s_terrainBonusFormat);
                var terrainType = AoWHSSet.Current.TerrainTypes[TerrainType];
                list.Add(RtlUnit.Format(terrainBonusX, [terrainType]));
            }

            predictor = new CombatPredictor();
            armies = new ArmyHSList(null);
            ListArmies(armies);
            for (int I = 0, last = armies.Count - 1; I <= last; I++)
            {
                for (int J = 0, lastJ = armies.GetArmyHexSprite(I).Army.Count - 1; J <= lastJ; J++)
                {
                    U = armies.GetArmyHexSprite(I).Army.GetUnit(J);
                    if (U.MoraleValue <= 40)
                    {
                        predictor.AddOpponent(U, 0);
                    }
                    else
                    {
                        predictor.AddUnit(U);
                    }
                }
            }

            units = new UnitList(null);
            GenerateRebelUnits(units);
            for (int J = 0, last = units.Count - 1; J <= last; J++)
            {
                predictor.AddOpponent(units.GetUnit(J), 0);
            }

            if (predictor.Execute() == CombatResult.Victory)
            {
                if (predictor.GetSuperiority() / 4 >= 75)
                {
                    var strong = AbilityRules.TranslateResourceString(s_strongText);
                    list.Add(RtlUnit.Format(AbilityRules.TranslateResourceString(s_occupationForcesFormat), [strong]));
                }
                else
                {
                    var average = AbilityRules.TranslateResourceString(s_averageText);
                    list.Add(RtlUnit.Format(AbilityRules.TranslateResourceString(s_occupationForcesFormat), [average]));
                }
            }
            else
            {
                var weak = AbilityRules.TranslateResourceString(s_weakText);
                list.Add(RtlUnit.Format(AbilityRules.TranslateResourceString(s_occupationForcesFormat), [weak]));
            }

            predictor?.Dispose();
            units?.Dispose();
            armies?.Dispose();
        }
    }

    // City.pas:1861
    public CivilStatus GetCivilStatus()
    {
        CivilStatus result;
        Status relation;
        int value;
        int armyValue;
        int bonus;
        CombatPredictor predictor;
        ArmyHSList armies;
        UnitList units;
        Troop U;
        if (Player != 0 && !Mode.Contains(ModeElement.WarMode))
        {
            if (Mode.Contains(ModeElement.Anarchy))
            {
                relation = Session.Status.Hostile;
            }
            else
            {
                relation = AoWHSMap.Current.Races.GetRace(Race).GetPlayerRelation(Player);
            }

            switch (relation)
            {
                case Session.Status.Hostile:
                    value = -35;
                    break;
                case Session.Status.Wary:
                    value = -25;
                    break;
                case Session.Status.Neutral:
                    value = 0;
                    break;
                case Session.Status.Polite:
                    value = 15;
                    break;
                case Session.Status.Friendly:
                    value = 25;
                    break;
                default:
                    value = 0;
                    break;
            }

            value = value + WallType * 5 + UpgradeLevel * 5;
            if (Race >= 0)
            {
                if (RaceTerrain[Race].Penalty.Contains((byte)TerrainType))
                {
                    value += -10;
                }

                if (RaceTerrain[Race].Bonus.Contains((byte)TerrainType))
                {
                    value += 10;
                }

                predictor = new CombatPredictor();
                armyValue = 0;
                armies = new ArmyHSList(null);
                ListArmies(armies);
                for (int I = 0, last = armies.Count - 1; I <= last; I++)
                {
                    for (int J = 0, lastJ = armies.GetArmyHexSprite(I).Army.Count - 1; J <= lastJ; J++)
                    {
                        U = armies.GetArmyHexSprite(I).Army.GetUnit(J);
                        if (U.MoraleValue <= 40)
                        {
                            predictor.AddOpponent(U, 0);
                            // { Original anarchy counts disloyal defenders twice. }
                            if (Mode.Contains(ModeElement.Anarchy))
                            {
                                predictor.AddOpponent(U, 0);
                            }
                        }
                        else
                        {
                            armyValue += U.GetObtainValue();
                            predictor.AddUnit(U);
                        }
                    }
                }

                armies?.Dispose();
                if (Set<Status>.Of(Session.Status.Hostile, Session.Status.Wary).Contains(relation))
                {
                    units = new UnitList(null);
                    GenerateRebelUnits(units);
                    for (int I = 0, last = units.Count - 1; I <= last; I++)
                    {
                        predictor.AddOpponent(units.GetUnit(I), 0);
                        if (Mode.Contains(ModeElement.Anarchy))
                        {
                            predictor.AddOpponent(units.GetUnit(I), 0);
                        }
                    }

                    if (predictor.Execute() == CombatResult.Victory)
                    {
                        bonus = predictor.GetSuperiority() / 4;
                        if (bonus >= 75)
                        {
                            bonus = 75;
                        }
                        else
                        {
                            if (bonus >= 26)
                            {
                                bonus = 50;
                            }
                            else
                            {
                                bonus = 25;
                            }
                        }
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
                    if (Set<Status>.Of(Session.Status.Polite, Session.Status.Friendly).Contains(relation))
                    {
                        armyValue = armyValue / (GetSize() * 10);
                        if (armyValue > 10)
                        {
                            armyValue = 10;
                        }

                        value += armyValue;
                    }
                }

                predictor?.Dispose();
                if (value < -30)
                {
                    result = CivilStatus.Unruly;
                }
                else
                {
                    if (value < -10)
                    {
                        result = CivilStatus.Unrest;
                    }
                    else
                    {
                        if (value < 10)
                        {
                            result = CivilStatus.Stable;
                        }
                        else
                        {
                            if (value < 30)
                            {
                                if (Set<Status>.Of(Session.Status.Hostile, Session.Status.Wary).Contains(relation))
                                {
                                    result = CivilStatus.Oppressed;
                                }
                                else
                                {
                                    result = CivilStatus.Content;
                                }
                            }
                            else
                            {
                                if (Set<Status>.Of(Session.Status.Hostile, Session.Status.Wary).Contains(relation))
                                {
                                    result = CivilStatus.Slavery;
                                }
                                else
                                {
                                    result = CivilStatus.Cheerful;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                result = CivilStatus.Stable;
            }
        }
        else
        {
            result = CivilStatus.Stable;
        }

        return result;
    }

    // City.pas:2034
    public void SetUpgradeLevel(byte level)
    {
        RaceResource raceResource = default;
        int unitLevel = default;
        bool researched = default;
        if (level > GetSize())
        {
            level = (byte)GetSize();
        }

        if (level < 1)
        {
            level = 1;
        }

        UpgradeLevel = level;
        if (Race >= 0)
        {
            raceResource = AoWHSSet.Current.RaceResources.GetRaceResource(Race);
            for (int I = raceResource.UnitIndexes.Count - 1; I >= 0; I--)
            {
                if (raceResource.UnitIndexes[I] != -1)
                {
                    var unitIndex = raceResource.UnitIndexes[I];
                    if (Production.UnitProductions.IndexOf(unitIndex) == -1)
                    {
                        var unitIndex2 = raceResource.UnitIndexes[I];
                        var unitResource = AoWHSSet.Current.UnitResources.GetUnitResource(unitIndex2);
                        var size = GetSize();
                        unitLevel = UnitLevelToCityLevel(unitResource.Level, size);
                        if (unitLevel != -1 && unitLevel <= UpgradeLevel)
                        {
                            researched = unitLevel == 1 && !(raceResource.SpecialUnitIndexes.Count >= 4 && raceResource.UnitIndexes[I] == raceResource.SpecialUnitIndexes[3]);
                            var unitIndex3 = raceResource.UnitIndexes[I];
                            Production.UnitProductions.Add(unitIndex3, researched);
                        }
                    }
                }
            }

            s_sortingCity = this;
            Production.UnitProductions._children.Sort(CompareCityProductions);
        }
    }

    // City.pas:2085
    public void SetName(string name)
    {
        Name = name;
        Update();
    }

    // City.pas:2091
    public void SetWallType(byte wallType)
    {
        WallType = wallType;
        UpdateImages();
    }

    // City.pas:2206
    public void InitializeCombat(Battle combat, ArmyHexSprite army)
    {
        if (Player == 0)
        {
            var enemiesIndependentRelation = GetEnemiesIndependentRelation(army.Army.Player);
            combat.CombatData.Players.GetPlayers(combat.CombatData.DefenderPlayer).IndependentRelation = (byte)enemiesIndependentRelation;
        }

        var hexLevel = army.GetHexLevel();
        var hexY = army.GetHexY();
        var hexX = army.GetHexX();
        combat.InitializeDefaultArmies(army.Army, hexX, hexY, hexLevel);
    }

    // City.pas:2234
    public int GetObtainValue()
    {
        var size = GetSize();
        int result = s_upgradePrices[size - 1] * UpgradeLevel;
        if (WallType == 1)
        {
            var size2 = GetSize();
            result += s_woodWallPrices[size2 - 1];
        }

        if (WallType == 2)
        {
            var size2 = GetSize();
            var size3 = GetSize();
            result = result + s_woodWallPrices[size2 - 1] + s_stoneWallPrices[size3 - 1];
        }

        return result;
    }

    // City.pas:2241
    public int JoinAmount(sbyte player)
    {
        ArmyHSList armies;
        int I;
        Army army;
        bool canJoin;
        int value;
        byte relation;
        byte selection;
        int result = 0;
        armies = new ArmyHSList(null);
        ListArmies(armies);
        I = armies.Count;
        canJoin = true;
        if (I != 0)
        {
            while (I != 0 && canJoin)
            {
                I -= 1;
                army = armies.GetArmyHexSprite(I).Army;
                selection = army.GetCanJoinSelection(player);
                canJoin = selection == army.ValidateSelection(255);
                if (canJoin)
                {
                    result += army.JoinAmount(player, 255);
                }
            }

            if (canJoin)
            {
                value = GetObtainValue() / 4;
                relation = (byte)AoWHSMap.Current.Races.GetRace(Race).GetPlayerRelation(player);
                result += s_joinPercentByStatus[(int)(Status)relation] * value / 100;
            }
            else
            {
                result = -1;
            }
        }
        else
        {
            var playerRelation = AoWHSMap.Current.Races.GetRace(Race).GetPlayerRelation(player);
            if (Set<Status>.Of(Session.Status.Polite, Session.Status.Friendly).Contains(playerRelation))
            {
                result = 0;
            }
            else
            {
                result = -1;
            }
        }

        armies?.Dispose();
        return result;
    }

    // City.pas:2282
    public bool IncomingNegotiateRequest(NegotiateRequestEventLog request)
    {
        bool result;
        int amount;
        if (Player == 0 && !Mode.Contains(ModeElement.WarMode))
        {
            amount = JoinAmount(request._player);
            if (amount != -1)
            {
                request.Object = NegotiateObject.City;
                var name = GetName();
                request.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(s_cityJoinHeaderFormat), [name]);
                request.Gold = amount;
                if (request.Gold == 0)
                {
                    var cityFreeJoin = AbilityRules.TranslateResourceString(s_cityFreeJoinText);
                    request.JoinMessage.Text = cityFreeJoin;
                }
                else
                {
                    var format = RtlUnit.Format(AbilityRules.TranslateResourceString(s_cityTributeJoinFormat), [IntToStr(request.Gold)]);
                    request.JoinMessage.Text = format;
                }

                result = true;
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

    // City.pas:2302
    public bool IncomingNegotiateOffer(NegotiateTokenEvent offer)
    {
        bool result;
        int amount;
        ArmyHSList armies;
        LocationTextEventLog log;
        if (offer.Object == NegotiateObject.City)
        {
            amount = JoinAmount(offer.Player);
            if (offer.Offer == NegotiateTokenEvent.NegotiateOffer.OfferBribe && amount <= offer.Gold && amount != -1)
            {
                if (AoWHSMap.Current.Players.GetPlayers(offer.Player).Gems >= offer.Gold)
                {
                    SetPlayer(offer.Player);
                    armies = new ArmyHSList(null);
                    ListArmies(armies);
                    for (int I = 0, last = armies.Count - 1; I <= last; I++)
                    {
                        if (armies.GetArmyHexSprite(I).Army.Player == 0)
                        {
                            armies.GetArmyHexSprite(I).Army.SetPlayer(offer.Player);
                        }
                    }

                    armies?.Dispose();
                    log = new LocationTextEventLog(null);
                    log._player = Player;
                    var name = GetName();
                    log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(s_overthrownFormat), [name]);
                    var hexLevel = GetHexLevel();
                    var hexY = GetHexY();
                    log.SetLocation(GetHexX(), hexY, hexLevel);
                    var hexY2 = GetHexY();
                    var hexLevel2 = GetHexLevel();
                    var hexX = GetHexX();
                    EventLog.DistributeLocationEventLog(log, Player, hexX, hexY2, hexLevel2, false);
                    log.Release();
                    offer.Accept();
                }
                else
                {
                    var notEnoughGold = AbilityRules.TranslateResourceString(GameText.NotEnoughGoldText);
                    AoWHSMap.Current.ShowMessage(notEnoughGold);
                }
            }
            else
            {
                offer.Reject();
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // City.pas:2334
    public sbyte GetEnemiesIndependentRelation(sbyte player)
    {
        UnitUserList units;
        units = new UnitUserList(null);
        ListUnits(units);
        for (int I = units.Count - 1; I >= 0; I--)
        {
            if (units.GetUnit(I).CanJoin(player))
            {
                units.DeleteUnit(units.GetUnit(I));
            }
        }

        // { Original uses the race index as the default relation. }
        sbyte result = UnitList.UnitsToIndependentRelation(units, Race);
        units?.Dispose();
        return result;
    }

    // City.pas:2345
    public sbyte GetIndependentRelation()
    {
        UnitUserList units;
        units = new UnitUserList(null);
        ListUnits(units);
        // { Original uses the race index as the default relation. }
        sbyte result = UnitList.UnitsToIndependentRelation(units, Race);
        units?.Dispose();
        return result;
    }

    // City.pas:2354
    public void ReturnBattleFieldInfo(ref BattleFieldInfo info)
    {
        int value;
        DiplomaticRelation relation;
        if (info.Requests.Contains(BattleFieldInfoRequestsElement.EffectiveStrength) && Player > 0)
        {
            value = 5000;
            var playerRelation = AoWHSMap.Current.Races.GetRace(Race).GetPlayerRelation(Player);
            if (Set<Status>.Of(Session.Status.Hostile, Session.Status.Wary).Contains(playerRelation))
            {
                value = value * 2;
            }

            relation = AbilityRules.DiplomaticRelation(info.Player, info.IndependentRelation, Player, 0);
            if (info.FriendlyRelations.Contains((byte)(int)relation))
            {
                info.FriendlyStrength -= value;
            }

            if (info.EnemyRelations.Contains((byte)(int)relation))
            {
                info.Strength -= value;
            }
        }
    }

    // City.pas:2367
    public void UpdateVisibleForSeatedPlayer()
    {
        bool visible;
        visible = VisibleForPlayer(AoWHSMap.Current.SeatedPlayer);
        if (EnchantmentsVisible ^ visible)
        {
            EnchantmentsVisible = visible;
            if (Enchantments != Set<CityEnchantmentId>.Of())
            {
                AoWHSMap.Current.GlobalMagicControl.TriggerChangedEvent();
            }
        }
    }

    // City.pas:2503
    public LibraryImage GetTopCityActionImage(ProductionOrder order) => GetCityActionImage(order, 0, Race != -1, 40, 43);

    // City.pas:2539
    public LibraryImage GetMiddleCityActionImage(ProductionOrder order) => GetCityActionImage(order, 1, Race != -1, 41, 44);

    // City.pas:2575
    public LibraryImage GetBottomCityActionImage(ProductionOrder order) => GetCityActionImage(order, 2, Race >= 0, 42, 45);

    // Row 0 to 2 of the city's map image while Order runs: an image sequence of the city when the
    // order names one, else the race image (Image on the surface, UndergroundImage below it).
    private LibraryImage GetCityActionImage(ProductionOrder order, int row, bool hasRace, int image, int undergroundImage)
    {
        LibraryImage result;
        int? sequence = order?.CitySequence(this, row);
        if (sequence is int index)
        {
            if (_resource.ImageSequenceList != null && index != -1)
            {
                if (_resource.ImageSequenceList.GetImageSequence(index) != null)
                {
                    result = _resource.ImageSequenceList.GetImageSequence(index).GetPreviewImage();
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                result = null;
            }
        }
        else
        {
            if (hasRace)
            {
                if (GetHexLevel() == 0)
                {
                    result = AoWHSSet.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(image);
                }
                else
                {
                    result = AoWHSSet.Current.RaceResources.GetRaceResource(Race).ImageLibrary.GetSafe(undergroundImage);
                }
            }
            else
            {
                result = null;
            }
        }

        return result;
    }

    // City.pas:138
    public void GenerateRebelUnits(UnitList units)
    {
        int saved = RandSeed;
        RandSeed = GetHexX() * 10 + AoWHSMap.Current.InitialRandomSeed + GetHexY();
        Int32List.FillWithRandomUnits(AoWHSSet.Current.RaceResources.GetRaceResource(Race).UnitIndexes, units, 2, GetSize() * 25, Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature));
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

    internal static RaceTerrainInfo[] RaceTerrain { get; } = new RaceTerrainInfo[12]
    {
        new RaceTerrainInfo
        {
            Bonus = Set<byte>.WithBounds(0, 15),
            Penalty = Set<byte>.WithBounds(0, 15)
        },
        new RaceTerrainInfo
        {
            Bonus = Set<byte>.WithBounds(0, 15).With((byte)2),
            Penalty = Set<byte>.WithBounds(0, 15).With((byte)3).With((byte)6)
                .With((byte)13)
        },
        new RaceTerrainInfo
        {
            Bonus = Set<byte>.WithBounds(0, 15).With((byte)0),
            Penalty = Set<byte>.WithBounds(0, 15).With((byte)2).With((byte)5)
        },
        new RaceTerrainInfo
        {
            Bonus = Set<byte>.WithBounds(0, 15).With((byte)3).With((byte)6)
                .With((byte)13),
            Penalty = Set<byte>.WithBounds(0, 15).With((byte)2)
        },
        new RaceTerrainInfo
        {
            Bonus = Set<byte>.WithBounds(0, 15).With((byte)1),
            Penalty = Set<byte>.WithBounds(0, 15).With((byte)5)
        },
        new RaceTerrainInfo
        {
            Bonus = Set<byte>.WithBounds(0, 15).With((byte)1),
            Penalty = Set<byte>.WithBounds(0, 15).With((byte)5)
        },
        new RaceTerrainInfo
        {
            Bonus = Set<byte>.WithBounds(0, 15).With((byte)12),
            Penalty = Set<byte>.WithBounds(0, 15)
        },
        new RaceTerrainInfo
        {
            Bonus = Set<byte>.WithBounds(0, 15),
            Penalty = Set<byte>.WithBounds(0, 15)
        },
        new RaceTerrainInfo
        {
            Bonus = Set<byte>.WithBounds(0, 15).With((byte)12),
            Penalty = Set<byte>.WithBounds(0, 15)
        },
        new RaceTerrainInfo
        {
            Bonus = Set<byte>.WithBounds(0, 15),
            Penalty = Set<byte>.WithBounds(0, 15)
        },
        new RaceTerrainInfo
        {
            Bonus = Set<byte>.WithBounds(0, 15).With((byte)12),
            Penalty = Set<byte>.WithBounds(0, 15)
        },
        new RaceTerrainInfo
        {
            Bonus = Set<byte>.WithBounds(0, 15).With((byte)5),
            Penalty = Set<byte>.WithBounds(0, 15).With((byte)1)
        }
    };

    private static Int32Array5 s_joinPercentByStatus = Int32Array5.From([200, 175, 150, 125, 100]);
    private static ByteArray4 s_upgradePrices = ByteArray4.From((ReadOnlySpan<byte>)new byte[4] { 250, 250, 250, 250 });
    private static ByteArray4 s_woodWallPrices = ByteArray4.From((ReadOnlySpan<byte>)new byte[4] { 20, 40, 60, 80 });
    private static ByteArray4 s_stoneWallPrices = ByteArray4.From((ReadOnlySpan<byte>)new byte[4] { 20, 40, 60, 80 });
    private static readonly string s_cityWarningText = "Warning";
    private static readonly string s_cityWoodenWallText = "Wooden Wall";
    private static readonly string s_cityStoneWallText = "Stone Wall";
    private static City s_sortingCity;

    private static bool RaceInCity(sbyte race, sbyte player)
    {
        bool flag = false;
        Player players = AoWHSMap.Current.Players.GetPlayers(player);
        for (int i = 0; i != players.Structures.Count; i++)
        {
            if (flag)
            {
                break;
            }
            if (players.Structures.GetStructure(i) is City)
            {
                flag = !players.Structures.GetStructure(i).GetBuilding() && ((City)players.Structures.GetStructure(i)).Race == race;
            }
        }
        return flag;
    }

    private static int UnitLevelToCityLevel(int level, int size)
    {
        return level;
    }

    private static int CompareCityProductions(object item1, object item2)
    {
        UnitProduction unitProduction = (UnitProduction)item1;
        int number;
        if (AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex) != null)
        {
            number = AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex).Level * 2;
            if (s_sortingCity.Race != -1 && AoWHSSet.Current.RaceResources.GetRaceResource(s_sortingCity.Race).SpecialUnitIndexes.Count > 3 && AoWHSSet.Current.RaceResources.GetRaceResource(s_sortingCity.Race).SpecialUnitIndexes[3] == unitProduction.UnitIndex)
            {
                number++;
            }
        }
        else
        {
            number = 0;
        }
        UnitProduction unitProduction2 = (UnitProduction)item2;
        int number2;
        if (AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction2.UnitIndex) != null)
        {
            number2 = AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction2.UnitIndex).Level * 2;
            if (s_sortingCity.Race != -1 && AoWHSSet.Current.RaceResources.GetRaceResource(s_sortingCity.Race).SpecialUnitIndexes.Count > 3 && AoWHSSet.Current.RaceResources.GetRaceResource(s_sortingCity.Race).SpecialUnitIndexes[3] == unitProduction2.UnitIndex)
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
            number3 = ((UnitProduction)item1).UnitIndex - ((UnitProduction)item2).UnitIndex;
        }
        return number3;
    }

    public enum ModeElement : byte
    {
        WarMode = 0, Razed = 1, TownQuake = 2, Anarchy = 3
    }

    public enum MoveOutOfCitySelectionElement : byte
    {
        SkipOwnPlayer = 0
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
