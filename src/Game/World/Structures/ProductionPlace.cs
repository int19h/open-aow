#nullable disable warnings
// Translated from PrdPlace.pas.
using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Abilities.Passive;
using Aow.Game.Session;
using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.World.Structures;

// power source, income source, production control, razed state, defender level
public partial class ProductionPlace : PlayerStructure
{
    public ProductionPlace(CustomNode owner) : base(owner)
    {
        ProductionControl = CreateProductionControl();
        ProductionControl.ProductionPlace = this;
        if (GetMagicSphere() == MagicSphere.None)
        {
            _incomeSource = new PlayerStructureIncomeSource(null) { Holder = CreationHolder() };
            _incomeSource.Setup(this);
            _defenderLevel = 2;
        }
        else
        {
            _powerSource = new PlayerStructurePowerSource(null) { Holder = CreationHolder() };
            _powerSource.Setup(this);
            _defenderLevel = 3;
        }
    }

    // PrdPlace.pas:291
    public new const int ClassId = 132_026;

    private readonly PlayerStructurePowerSource _powerSource;
    private readonly PlayerStructureIncomeSource _incomeSource;

    public ProductionPlaceControl ProductionControl { get; }

    public int RazedState { get; set; }

    private int _defenderLevel;

    // PrdPlace.pas:283
    public override void Dispose()
    {
        base.Dispose();
        _powerSource?.Dispose();
        _incomeSource?.Dispose();
        ProductionControl?.Dispose();
    }

    // PrdPlace.pas:528
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("ProductionControl", objectValue: ProductionControl);
        _defenderLevel = persister.RawByte("DefenderLevel", value: _defenderLevel.TruncateToByte());
        RazedState = persister.RawByte("RazedState", value: RazedState.TruncateToByte());
    }

    // PrdPlace.pas:352
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537002242)
        {
            MasterySpellCast(PlayerId.FromInt32((sbyte)message.Param[0].Int), sphere: (MagicSphere)message.Param[1].Int);
        }
    }

    // PrdPlace.pas:692
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 4609 && message.HexNumber == 0)
        {
            message.Result = ProductionControl;
        }
    }

    // PrdPlace.pas:562
    public override void Activate()
    {
        base.Activate();
        ProductionControl.Activate();
        if (Player is not null)
        {
            if (GetMagicSphere() == MagicSphere.None)
            {
                _ = GetEngine().StrategicMap.Players.GetPlayers(Player).ProductionControls.Add(ProductionControl);
            }

            if (_powerSource is not null)
            {
                _ = GetEngine().StrategicMap.Players.GetPlayers(Player).MagicControl.RegisterPowerSource(_powerSource);
            }

            if (_incomeSource is not null)
            {
                GetEngine().StrategicMap.Players.GetPlayers(Player).RegisterIncomeSource(_incomeSource);
            }
        }
    }

    // PrdPlace.pas:574
    public override void Deactivate()
    {
        if (Player is not null)
        {
            if (GetMagicSphere() == MagicSphere.None)
            {
                _ = GetEngine().StrategicMap.Players.GetPlayers(Player).ProductionControls.Remove(ProductionControl);
            }

            if (_powerSource is not null)
            {
                _ = GetEngine().StrategicMap.Players.GetPlayers(Player).MagicControl.UnregisterPowerSource(_powerSource);
            }

            if (_incomeSource is not null)
            {
                GetEngine().StrategicMap.Players.GetPlayers(Player).UnregisterIncomeSource(_incomeSource);
            }
        }

        ProductionControl.Deactivate();
        base.Deactivate();
    }

    // PrdPlace.pas:698
    public override void Select() => base.Select();

    // PrdPlace.pas:391
    public override void SetRazed(bool value)
    {
        if (value)
        {
            ProductionControl.ExecuteDeleteProductions();
            SetPlayer(null);
            RazedState = 1;
        }
        else
        {
            RazedState = 0;
        }

        // refresh after either state change
        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // PrdPlace.pas:388
    public override bool GetRazed() => RazedState == 1;

    // PrdPlace.pas:655
    public override void NewDay()
    {
        base.NewDay();
        if (GetEngine().StrategicMap.Day == 1)
        {
            if (Player == PlayerId.Independent)
            {
                GenerateDefenders();
            }

            ResetProductions();
        }
    }

    // PrdPlace.pas:686
    public override void NewTurn(PlayerId player)
    {
        base.NewTurn(player);
        ProductionControl.NewTurn(player);
    }

    // PrdPlace.pas:665
    public override void BuildingDone()
    {
        base.BuildingDone();
        ResetProductions();
    }

    // PrdPlace.pas:434
    public override string GetCurrentActivityText()
    {
        if (ProductionControl.Queue.Count == 0)
        {
            return PlayerIdExtensions.ToOriginal(Player) > 0
                ? GetMagicSphere() != MagicSphere.None
                    ? GetPower() != 0 ? AbilityRules.TranslateResourceString(GetEngine(), S: GameText.GeneratingExtraPowerText) : "-"
                    : BaseIncome != 0
                        ? AbilityRules.TranslateResourceString(GetEngine(), S: GameText.ProducingMerchandiseText)
                        : AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.NoneText)
                : LowerCase(AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.NoneText));
        }
        else
        {
            ProductionItem? queue = ProductionControl.Queue.GetItem(0);
            return ProductionControl.GetProductionNameProgressive(queue);
        }
    }

    // PrdPlace.pas:607
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        if (!GetRazed())
        {
            switch (info.Mode)
            {
                case AITargetMode.Attack:
                case AITargetMode.Raid:
                    if (info.Player != Player)
                    {
                        if (Player == PlayerId.Independent)
                        {
                            data.Priority = 50;
                        }
                        else
                        {
                            if (AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, targetPlayer: Player, targetRace: 0, engine: GetEngine()!))
                            {
                                Player? player = GetEngine().StrategicMap.Players.GetPlayers(info.Player);
                                data.Priority = 50 + (player.Income / 2);
                                if (data.Priority < 25)
                                {
                                    data.Priority = 25;
                                }

                                if (data.Priority > 75)
                                {
                                    data.Priority = 75;
                                }
                            }
                        }
                    }

                    break;
                case AITargetMode.Produce:
                    if (info.Player == Player)
                    {
                        data.Priority |= 0b0010;
                    }

                    break;
                case AITargetMode.Defend:
                    if (info.Player == Player)
                    {
                        data.Priority = 30;
                    }

                    break;
                case AITargetMode.Recruit:
                case AITargetMode.Hunt:
                case AITargetMode.Fortify:
                case AITargetMode.GuardArea:
                case AITargetMode.AttackArmies:
                default:
                    break;
            }
        }
    }

    // PrdPlace.pas:536
    public override void Update()
    {
        base.Update();
        if (_incomeSource is not null && PlayerIdExtensions.ToOriginal(Player) >= 0)
        {
            GetEngine().StrategicMap.Players.GetPlayers(Player).UpdateIncome();
        }

        if (_powerSource is not null && PlayerIdExtensions.ToOriginal(Player) >= 0)
        {
            GetEngine().StrategicMap.Players.GetPlayers(Player).MagicControl.Update();
        }
    }

    // PrdPlace.pas:294
    public override bool CanDoubleClick() => GetEngine().StrategicMap.SeatedPlayer == Player && GetMagicSphere() == MagicSphere.None;

    // PrdPlace.pas:586
    public override void SetPlayer(PlayerId? player)
    {
        if (player != Player)
        {
            ProductionControl.PlayerChanged();
            if (Player is not null)
            {
                if (_powerSource is not null)
                {
                    _ = GetEngine().StrategicMap.Players.GetPlayers(Player).MagicControl.UnregisterPowerSource(_powerSource);
                }

                if (_incomeSource is not null)
                {
                    GetEngine().StrategicMap.Players.GetPlayers(Player).UnregisterIncomeSource(_incomeSource);
                }

                if (GetMagicSphere() == MagicSphere.None)
                {
                    _ = GetEngine().StrategicMap.Players.GetPlayers(Player).ProductionControls.Remove(ProductionControl);
                }
            }

            base.SetPlayer(player);
            if (Player is not null)
            {
                if (_powerSource is not null)
                {
                    _ = GetEngine().StrategicMap.Players.GetPlayers(Player).MagicControl.RegisterPowerSource(_powerSource);
                }

                if (_incomeSource is not null)
                {
                    GetEngine().StrategicMap.Players.GetPlayers(Player).RegisterIncomeSource(_incomeSource);
                }

                if (GetMagicSphere() == MagicSphere.None)
                {
                    _ = GetEngine().StrategicMap.Players.GetPlayers(Player).ProductionControls.Add(ProductionControl);
                }
            }
        }
    }

    // PrdPlace.pas:375
    public override int GetIncome()
    {
        int income = BaseIncome;
        if (GetMagicSphere() == MagicSphere.None)
        {
            if (ProductionControl.Status != ProductionControlStatus.Producing || ProductionControl.Queue.GetItem(0).Order is Trade)
            {
                income += Round(income * 0.25);
            }
        }

        return income;
    }

    // PrdPlace.pas:358
    public override int GetPower()
    {
        int result;
        // { The original tests msCosmos, rather than msNone, here. }
        if (PlayerIdExtensions.ToOriginal(Player) > 0 && GetMagicSphere() != MagicSphere.Cosmos && !GetRazed() && !Building)
        {
            MagicSphere magicSphere = GetMagicSphere();
            result = GetEngine().StrategicMap.Players.GetPlayers(Player).Leader.GetSpherePicks(magicSphere);
            return result != 0 ? (result * 5) + 10 : 0;
        }
        else
        {
            return 0;
        }
    }

    // PrdPlace.pas:543
    public override void StructureCaptured(PlayerId? oldPlayer, PlayerId? newPlayer)
    {
        if (!GetRazed() && !Building && oldPlayer == PlayerId.Independent && !GetEngine().StrategicMap.Status.Contains(HexSpriteMapStatuses.Editting) && Status.Contains(MapObjectStatuses.Activated) && !Resource.Claimed)
        {
            ProductionPlaceEventLog log = new(null, holder: CreationHolder())
            {
                Player = newPlayer
            };
            string editName = EditName();
            log.Text = Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.StructureCapturedFormat), args: [editName]);
            log.Location = new MapCoordinate(HexX, HexY, HexLevel);
            EventLog.DistributeLocationEventLog(y: HexY, x: HexX, log: log, player: Player, level: HexLevel, execute: false);
            _ = log.Release();
        }
    }

    // PrdPlace.pas:300
    public virtual ProductionPlaceControl CreateProductionControl() => new(null) { Holder = CreationHolder() };

    // PrdPlace.pas:625
    public virtual void GenerateDefenders()
    {
        if (_defenderLevel != 0)
        {
            if (_defenderLevel == 4)
            {
                _defenderLevel = (GetEngine().StrategicMap.Random(3) + 1).TruncateToByte();
            }

            int budget = ((_defenderLevel - 2) * 25) + 25;
            ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
            if (armyHexSprite is not null)
            {
                budget -= armyHexSprite.Army.ObtainValue(255);
            }

            if (budget > 0)
            {
                RandSeed = GetEngine().StrategicMap.Random(1_048_575);
                int collection = Resource.Defenders.GetRandomCollection();
                if (collection != -1)
                {
                    UnitIndexList units = [];
                    UnitIndexListCollection unitIndexListCollection = Resource.Defenders;
                    for (int i = unitIndexListCollection.GetCollectionCount(collection) - 1; i >= 0; i--)
                    {
                        _ = units.Add(unitIndexListCollection.GetU(collection, index: i));
                    }

                    int hexY = HexY;
                    _ = AbilityRules.PlaceRandomUnits(HexX, hexY, L: HexLevel, player: PlayerId.Independent, units, level: 4, value: budget, types: UnitTypes.Humanoid | UnitTypes.Creature, engine: GetEngine());
                    armyHexSprite = Owner.GetArmyHexSprite();
                    if (armyHexSprite is not null && armyHexSprite.Army.Player == PlayerId.Independent)
                    {
                        armyHexSprite.Army.SetBehavior(255, behavior: GroupBehavior.Guard);
                    }
                }
            }
        }
    }

    // PrdPlace.pas:369
    public virtual int BaseIncome => RazedState != 1 && !Building ? 10 : 0;

    // PrdPlace.pas:303
    public virtual MagicSphere GetMagicSphere() => MagicSphere.None;

    // PrdPlace.pas:403
    public virtual void ListIncomeInfo(DelphiStringCollection names, DelphiStringCollection values)
    {
        if (GetMagicSphere() == MagicSphere.None)
        {
            if (BaseIncome > 0)
            {
                _ = names.Add(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.BaseIncomeText));
                _ = values.Add(BaseIncome.ToString(CultureInfo.InvariantCulture));
                if (ProductionControl.Status != ProductionControlStatus.Producing || (ProductionControl.Queue.GetItem(0).Order is Trade && BaseIncome > 0))
                {
                    _ = names.Add(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.ProducingMerchandiseText));
                    _ = values.Add(Round(BaseIncome * 0.25).ToString(CultureInfo.InvariantCulture));
                }
            }
        }
        else
        {
            if (PlayerIdExtensions.ToOriginal(Player) > 0)
            {
                MagicSphere magicSphere = GetMagicSphere();
                int picks = GetEngine().StrategicMap.Players.GetPlayers(Player).Leader.GetSpherePicks(magicSphere);
                MagicSphere magicSphere2 = GetMagicSphere();
                string magicSphereText = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.MagicSphereTexts[(int)magicSphere2]);
                _ = names.Add(Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_sphereCountFormat), args: [picks.ToString(CultureInfo.InvariantCulture), LowerCase(magicSphereText)]));
                _ = values.Add((picks * 5).ToString(CultureInfo.InvariantCulture));
                if (ProductionControl.Status != ProductionControlStatus.Producing || ProductionControl.Queue.GetItem(0).Order is Trade)
                {
                    _ = names.Add(AbilityRules.TranslateResourceString(GetEngine(), S: s_extraPowerText));
                    _ = values.Add(Round(picks * 5 * 0.25).ToString(CultureInfo.InvariantCulture));
                }
            }
        }
    }

    // PrdPlace.pas:306
    public void MasterySpellCast(PlayerId player, MagicSphere sphere)
    {
        if (sphere == GetMagicSphere() && PlayerIdExtensions.ToOriginal(Player) > 0)
        {
            int unitIndex = sphere switch
            {
                MagicSphere.Life => 216,
                MagicSphere.Death => 217,
                MagicSphere.Water => 227,
                MagicSphere.Fire => 226,
                MagicSphere.Earth => 225,
                MagicSphere.Air => 224,
                MagicSphere.Cosmos or MagicSphere.Secret or MagicSphere.None or _ => -1,
            };
            if (unitIndex != -1)
            {
                Unit unit = new(null) { Holder = CreationHolder() };
                unit.SetUnitResource(GetEngine().GamePack.UnitResources.GetUnitResource(unitIndex));
                unit.SetPlayer(Player);
                int hexY = HexY;
                ArmyHexSprite? armyHexSprite = unit.Place(HexX, hexY, L: HexLevel, Player, mode: UnitPlaceModes.ScanArea);
                if (armyHexSprite is not null)
                {
                    if (unit.ExpandAbility(155))
                    {
                        UnitEnchantment? enchantment = unit.GetAbilityData<UnitEnchantmentAbilityData>(155).Enchantment;
                        enchantment.SetUpkeep(SummonedAbility.GetDefaultManaUpkeep(unit));
                        enchantment.SetSource(Player);
                        enchantment.DispelResistance = SummonedAbility.GetDefaultDispelResistance(unit).TruncateToSByte();
                    }

                    int armyHexSpriteHexY = armyHexSprite.HexY;
                    int hexX = armyHexSprite.HexX;
                    StrategicMap strategicMap = GetEngine().StrategicMap;
                    if (strategicMap.Visible(hexX, armyHexSpriteHexY, L: armyHexSprite.HexLevel, radius: 0))
                    {
                        ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
                        animation.SetImageSequence(GetEngine().GamePack.SpellControl[29].CastImages.GetImageSequence(0));
                        int hexY2 = HexY;
                        int hexX2 = HexX;
                        _ = strategicMap.PlaceHex(animation, hexX2, hexY2, L: HexLevel);
                    }
                }

                _ = unit.Release();
            }
        }
    }

    // PrdPlace.pas:469
    public bool CanProduce(int index, bool queue, string reason) => ProductionControl.CanProduceUnit(index, queue, reason);

    // PrdPlace.pas:472
    public bool Produce(int index, bool queue) => ProductionControl.ProduceUnit(index, queue);

    // PrdPlace.pas:475
    public int ProductionGold(int index) => ProductionControl.UnitProductionGold(index);

    // PrdPlace.pas:478
    public int ProductionMana(int index) => ProductionControl.UnitProductionMana(index);

    // PrdPlace.pas:481
    public int ProductionTime(int index) => ProductionControl.UnitProductionTime(index);

    // PrdPlace.pas:484
    public bool CanResearch(int index, bool queue, ref string reason) => ProductionControl.CanResearchUnit(index, queue, ref reason);

    // PrdPlace.pas:487
    public bool Research(int index, bool queue) => ProductionControl.ResearchUnit(index, queue);

    // PrdPlace.pas:490
    public int ResearchGold(int index) => ProductionControl.UnitResearchGold(index);

    // PrdPlace.pas:493
    public int ResearchMana(int index) => ProductionControl.UnitResearchMana(index);

    // PrdPlace.pas:496
    public int ResearchTime(int index) => ProductionControl.UnitResearchTime(index);

    // PrdPlace.pas:499
    public bool CanTrade(bool queue, ref string reason)
    {
        if (GetMagicSphere() != MagicSphere.None)
        {
            MagicSphere magicSphere = GetMagicSphere();
            if (GetEngine().StrategicMap.Players.GetPlayers(Player).Leader.GetSpherePicks(magicSphere) > 0)
            {
                return true;
            }
            else
            {
                reason = AbilityRules.TranslateResourceString(GetEngine(), S: s_youCannotChannelPowerFromThisNodeText);
            }
        }
        else
        {
            return BaseIncome != 0;
        }

        return false;
    }

    // PrdPlace.pas:511
    public bool Trade(bool queue)
    {
        string reason = "";
        if (CanTrade(queue, ref reason) && !GetEngine().StrategicMap.Players.GetPlayers(Player).GetBusy())
        {
            ProductionItem item = new(null)
            {
                Holder = CreationHolder(),
                Order = queue ? new Trade(1) : new Trade(-1)
            };

            _ = ProductionControl.Produce(item, queue);
            item?.Dispose();
            return true;
        }
        else
        {
            return false;
        }
    }

    // PrdPlace.pas:671
    public void ResetProductions()
    {
        ProductionControl.UnitProductions.Clear();
        if (GetMagicSphere() == MagicSphere.None)
        {
            // Unit lists live on the production resource. Another resource throws, as the old cast did.
            ProductionPlaceResource r = GetResource<ProductionPlaceResource>();
            for (int i = 0, last = r.Units.Count - 1; i <= last; i++)
            {
                int unit = r.Units[i];
                ProductionControl.UnitProductions.Add(unit, researched: false);
            }

            ProductionControl.UnitProductions.SortOnLevel();
            ProductionControl.UnitProductions.GetItem(0).Researched = true;
        }
    }

    private static readonly string s_extraPowerText = "Extra Power";
    private static readonly string s_sphereCountFormat = "%0:sx %1:s sphere";
    private static readonly string s_youCannotChannelPowerFromThisNodeText = "You cannot channel power from this node";
}
