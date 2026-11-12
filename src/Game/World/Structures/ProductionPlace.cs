// Translated from PrdPlace.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// power source, income source, production control, razed state, defender level
public partial class ProductionPlace : PlayerStructure
{
    public ProductionPlace(ECustomNode owner) : base(owner)
    {
        ProductionControl = CreateProductionControl();
        ProductionControl.ProductionPlace = this;
        if (GetMagicSphere() == MagicSphere.None)
        {
            IncomeSource = new PlayerStructureIncomeSource(null);
            IncomeSource.Setup(this);
            DefenderLevel = 2;
        }
        else
        {
            PowerSource = new PlayerStructurePowerSource(null);
            PowerSource.Setup(this);
            DefenderLevel = 3;
        }
    }

    // PrdPlace.pas:291
    public new const int ClassId = 132026;

    public PlayerStructurePowerSource PowerSource;
    public PlayerStructureIncomeSource IncomeSource;
    public ProductionPlaceControl ProductionControl;
    public byte RazedState;
    public byte DefenderLevel;

    // PrdPlace.pas:283
    public override void Dispose()
    {
        base.Dispose();
        PowerSource?.Dispose();
        IncomeSource?.Dispose();
        ProductionControl?.Dispose();
    }

    // PrdPlace.pas:528
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(ProductionControl, 8);
        stream.rwData(Bytes.Of(ref DefenderLevel, 1), 1, 9);
        stream.rwData(Bytes.Of(ref RazedState, 1), 1, 10);
    }

    // PrdPlace.pas:264

    // PrdPlace.pas:352
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537002242)
        {
            MasterySpellCast((sbyte)message.Param[0].Int, (MagicSphere)message.Param[1].Int);
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
        if (Player != -1)
        {
            if (GetMagicSphere() == MagicSphere.None)
            {
                AoWHSMap.Current.Players.GetPlayers(Player).ProductionControls.Add(ProductionControl);
            }

            if (PowerSource != null)
            {
                AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.RegisterPowerSource(PowerSource);
            }

            if (IncomeSource != null)
            {
                AoWHSMap.Current.Players.GetPlayers(Player).RegisterIncomeSource(IncomeSource);
            }
        }
    }

    // PrdPlace.pas:574
    public override void Deactivate()
    {
        if (Player != -1)
        {
            if (GetMagicSphere() == MagicSphere.None)
            {
                AoWHSMap.Current.Players.GetPlayers(Player).ProductionControls.Remove(ProductionControl);
            }

            if (PowerSource != null)
            {
                AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.UnregisterPowerSource(PowerSource);
            }

            if (IncomeSource != null)
            {
                AoWHSMap.Current.Players.GetPlayers(Player).UnregisterIncomeSource(IncomeSource);
            }
        }

        ProductionControl.Deactivate();
        base.Deactivate();
    }

    // PrdPlace.pas:698
    public override void Select()
    {
        base.Select();
    }

    // PrdPlace.pas:391
    public override void SetRazed(bool value)
    {
        if (value)
        {
            ProductionControl.ExecuteDeleteProductions();
            SetPlayer(-1);
            RazedState = 1;
        }
        else
        {
            RazedState = 0;
        }

        // refresh after either state change
        _imageIndex = GetTerrainTypeImage(_terrainType);
    }

    // PrdPlace.pas:388
    public override bool GetRazed() => RazedState == 1;

    // PrdPlace.pas:655
    public override void NewDay()
    {
        base.NewDay();
        if (AoWHSMap.Current.Day == 1)
        {
            if (Player == 0)
            {
                GenerateDefenders();
            }

            ResetProductions();
        }
    }

    // PrdPlace.pas:686
    public override void NewTurn(sbyte player)
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
        string result;
        if (ProductionControl.Queue.Count == 0)
        {
            if (Player > 0)
            {
                if (GetMagicSphere() != MagicSphere.None)
                {
                    if (GetPower() != 0)
                    {
                        result = AbilityRules.TranslateResourceString(GameText.s_generatingExtraPowerText);
                    }
                    else
                    {
                        result = "-";
                    }
                }
                else
                {
                    if (GetBaseIncome() != 0)
                    {
                        result = AbilityRules.TranslateResourceString(GameText.s_producingMerchandiseText);
                    }
                    else
                    {
                        result = AbilityRules.TranslateResourceString(GameTables.s_noneText);
                    }
                }
            }
            else
            {
                result = LowerCase(AbilityRules.TranslateResourceString(GameTables.s_noneText));
            }
        }
        else
        {
            var queue = ProductionControl.Queue.GetItem(0);
            result = ProductionControl.GetProductionNameProgressive(queue);
        }

        return result;
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
                        if (Player == 0)
                        {
                            data.Priority = 50;
                        }
                        else
                        {
                            if (AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, Player, 0))
                            {
                                var player = AoWHSMap.Current.Players.GetPlayers(info.Player);
                                data.Priority = 50 + player.Income / 2;
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
                        data.Priority = data.Priority | 2;
                    }

                    break;
                case AITargetMode.Defend:
                    if (info.Player == Player)
                    {
                        data.Priority = 30;
                    }

                    break;
            }
        }
    }

    // PrdPlace.pas:536
    public override void Update()
    {
        base.Update();
        if (IncomeSource != null && Player >= 0)
        {
            AoWHSMap.Current.Players.GetPlayers(Player).UpdateIncome();
        }

        if (PowerSource != null && Player >= 0)
        {
            AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.Update();
        }
    }

    // PrdPlace.pas:294
    public override bool CanDoubleClick()
    {
        bool result;
        if (AoWHSMap.Current.SeatedPlayer != Player || GetMagicSphere() != MagicSphere.None)
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // PrdPlace.pas:586
    public override void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            ProductionControl.PlayerChanged();
            if (Player != -1)
            {
                if (PowerSource != null)
                {
                    AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.UnregisterPowerSource(PowerSource);
                }

                if (IncomeSource != null)
                {
                    AoWHSMap.Current.Players.GetPlayers(Player).UnregisterIncomeSource(IncomeSource);
                }

                if (GetMagicSphere() == MagicSphere.None)
                {
                    AoWHSMap.Current.Players.GetPlayers(Player).ProductionControls.Remove(ProductionControl);
                }
            }

            base.SetPlayer(player);
            if (Player != -1)
            {
                if (PowerSource != null)
                {
                    AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.RegisterPowerSource(PowerSource);
                }

                if (IncomeSource != null)
                {
                    AoWHSMap.Current.Players.GetPlayers(Player).RegisterIncomeSource(IncomeSource);
                }

                if (GetMagicSphere() == MagicSphere.None)
                {
                    AoWHSMap.Current.Players.GetPlayers(Player).ProductionControls.Add(ProductionControl);
                }
            }
        }
    }

    // PrdPlace.pas:375
    public override int GetIncome()
    {
        int income;
        income = GetBaseIncome();
        if (GetMagicSphere() == MagicSphere.None)
        {
            if (ProductionControl.Status != ProductionControlStatus.Producing || ProductionControl.Queue.GetItem(0).Order is Trade)
            {
                income += Round((double)income * 0.25);
            }
        }

        return income;
    }

    // PrdPlace.pas:358
    public override int GetPower()
    {
        int result;
        // { The original tests msCosmos, rather than msNone, here. }
        if (Player > 0 && GetMagicSphere() != MagicSphere.Cosmos && !GetRazed() && !GetBuilding())
        {
            var magicSphere = GetMagicSphere();
            result = AoWHSMap.Current.Players.GetPlayers(Player).Leader.GetSpherePicks(magicSphere);
            if (result != 0)
            {
                result = result * 5 + 10;
            }
            else
            {
                result = 0;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // PrdPlace.pas:543
    public override void StructureCaptured(sbyte oldPlayer, sbyte newPlayer)
    {
        ProductionPlaceEventLog log;
        if (!GetRazed() && !GetBuilding() && oldPlayer == 0 && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && Status.Contains(MapObjectStatusElement.Activated) && !((PlayerStructureResource)Resource).Claimed)
        {
            log = new ProductionPlaceEventLog(null);
            log._player = newPlayer;
            var editName = EditName();
            log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.s_structureCapturedFormat), [editName]);
            log.HexX = HexX;
            log.HexY = HexY;
            log.HexLevel = HexLevel;
            var hexY = HexY;
            var hexX = HexX;
            EventLog.DistributeLocationEventLog(log, Player, hexX, hexY, HexLevel, false);
            log.Release();
        }
    }

    // PrdPlace.pas:300
    public virtual ProductionPlaceControl CreateProductionControl() => new ProductionPlaceControl(null);

    // PrdPlace.pas:625
    public virtual void GenerateDefenders()
    {
        ArmyHexSprite armyHexSprite;
        int budget;
        int collection;
        UnitIndexList units;
        if (DefenderLevel != 0)
        {
            if (DefenderLevel == 4)
            {
                DefenderLevel = (byte)(AoWHSMap.Current.Random(3) + 1);
            }

            budget = (DefenderLevel - 2) * 25 + 25;
            armyHexSprite = (ArmyHexSprite)_owner.FindOwnedChild(131607);
            if (armyHexSprite != null)
            {
                budget -= armyHexSprite.Army.ObtainValue(255);
            }

            if (budget > 0)
            {
                RandSeed = AoWHSMap.Current.Random(1048575);
                collection = ((StructureResource)Resource).Defenders.GetRandomCollection();
                if (collection != -1)
                {
                    units = new UnitIndexList();
                    var unitIndexListCollection = ((StructureResource)Resource).Defenders;
                    for (int I = unitIndexListCollection.GetCollectionCount(collection) - 1; I >= 0; I--)
                    {
                        units.Add(unitIndexListCollection.GetU(collection, I));
                    }

                    var hexY = HexY;
                    AbilityRules.PlaceRandomUnits(HexX, hexY, HexLevel, 0, units, 4, budget, Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature));
                    armyHexSprite = ((AoWMapField)_owner).GetArmyHexSprite();
                    if (armyHexSprite != null && armyHexSprite.Army.Player == 0)
                    {
                        armyHexSprite.Army.SetBehavior(255, GroupBehavior.Guard);
                    }

                    units?.Dispose();
                }
            }
        }
    }

    // PrdPlace.pas:369
    public virtual int GetBaseIncome()
    {
        int result;
        if (RazedState != 1 && !GetBuilding())
        {
            result = 10;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // PrdPlace.pas:303
    public virtual MagicSphere GetMagicSphere() => MagicSphere.None;

    // PrdPlace.pas:403
    public virtual void ListIncomeInfo(StringCollection names, StringCollection values)
    {
        int picks;
        if (GetMagicSphere() == MagicSphere.None)
        {
            if (GetBaseIncome() > 0)
            {
                names.Add(AbilityRules.TranslateResourceString(GameText.s_baseIncomeText));
                values.Add(IntToStr(GetBaseIncome()));
                if (ProductionControl.Status != ProductionControlStatus.Producing || ProductionControl.Queue.GetItem(0).Order is Trade && GetBaseIncome() > 0)
                {
                    names.Add(AbilityRules.TranslateResourceString(GameText.s_producingMerchandiseText));
                    values.Add(IntToStr(Round((double)GetBaseIncome() * 0.25)));
                }
            }
        }
        else
        {
            if (Player > 0)
            {
                var magicSphere = GetMagicSphere();
                picks = AoWHSMap.Current.Players.GetPlayers(Player).Leader.GetSpherePicks(magicSphere);
                var magicSphere2 = GetMagicSphere();
                var magicSphereText = AbilityRules.TranslateResourceString(GameTables.s_magicSphereTexts[(int)magicSphere2]);
                names.Add(RtlUnit.Format(AbilityRules.TranslateResourceString(s_sphereCountFormat), [IntToStr(picks), LowerCase(magicSphereText)]));
                values.Add(IntToStr(picks * 5));
                if (ProductionControl.Status != ProductionControlStatus.Producing || ProductionControl.Queue.GetItem(0).Order is Trade)
                {
                    names.Add(AbilityRules.TranslateResourceString(s_extraPowerText));
                    values.Add(IntToStr(Round((double)(picks * 5) * 0.25)));
                }
            }
        }
    }

    // PrdPlace.pas:306
    public void MasterySpellCast(sbyte player, MagicSphere sphere)
    {
        int unitIndex;
        Unit unit;
        ArmyHexSprite armyHexSprite;
        UnitEnchantment enchantment;
        ImageSequenceAnimation animation;
        if (sphere == GetMagicSphere() && Player > 0)
        {
            switch (sphere)
            {
                case MagicSphere.Life:
                    unitIndex = 216;
                    break;
                case MagicSphere.Death:
                    unitIndex = 217;
                    break;
                case MagicSphere.Water:
                    unitIndex = 227;
                    break;
                case MagicSphere.Fire:
                    unitIndex = 226;
                    break;
                case MagicSphere.Earth:
                    unitIndex = 225;
                    break;
                case MagicSphere.Air:
                    unitIndex = 224;
                    break;
                default:
                    unitIndex = -1;
                    break;
            }

            if (unitIndex != -1)
            {
                unit = new Unit(null);
                unit.SetUnitResource(AoWHSSet.Current.UnitResources.GetUnitResource(unitIndex));
                unit.SetPlayer(Player);
                var hexY = HexY;
                armyHexSprite = unit.Place(HexX, hexY, HexLevel, Player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                if (armyHexSprite != null)
                {
                    if (unit.ExpandAbility(155))
                    {
                        enchantment = ((UnitEnchantmentAbilityData)unit.GetAbilityData(155)).Enchantment;
                        enchantment.SetUpkeep(SummonedAbility.GetDefaultManaUpkeep(unit));
                        enchantment.SetSource(Player);
                        enchantment.DispelResistance = (sbyte)SummonedAbility.GetDefaultDispelResistance(unit);
                    }

                    var armyHexSpriteHexY = armyHexSprite.HexY;
                    var hexX = armyHexSprite.HexX;
                    if (AoWHSMap.Current.Visible(hexX, armyHexSpriteHexY, armyHexSprite.HexLevel, 0))
                    {
                        animation = new ImageSequenceAnimation(null);
                        animation.SetImageSequence(AoWHSSet.Current.SpellControl.GetSpell(29).CastImages.GetImageSequence(0));
                        var hexY2 = HexY;
                        var hexX2 = HexX;
                        AoWHSMap.Current.PlaceHex(animation, hexX2, hexY2, HexLevel);
                    }
                }

                unit.Release();
            }
        }
    }

    // PrdPlace.pas:469
    public bool CanProduce(int index, bool queue, ref string reason) => ProductionControl.CanProduceUnit(index, queue, ref reason);

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
        bool result;
        if (GetMagicSphere() != MagicSphere.None)
        {
            var magicSphere = GetMagicSphere();
            if (AoWHSMap.Current.Players.GetPlayers(Player).Leader.GetSpherePicks(magicSphere) > 0)
            {
                result = true;
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(s_youCannotChannelPowerFromThisNodeText);
            }
        }
        else
        {
            result = GetBaseIncome() != 0;
        }

        return result;
    }

    // PrdPlace.pas:511
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

            ProductionControl.Produce(item, queue);
            item?.Dispose();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // PrdPlace.pas:671
    public void ResetProductions()
    {
        ProductionPlaceResource R;
        ProductionControl.UnitProductions.Clear();
        if (GetMagicSphere() == MagicSphere.None)
        {
            R = (ProductionPlaceResource)Resource;
            for (int I = 0, last = R.Units.Count - 1; I <= last; I++)
            {
                var unit = R.Units[I];
                ProductionControl.UnitProductions.Add(unit, false);
            }

            ProductionControl.UnitProductions.SortOnLevel();
            ProductionControl.UnitProductions.GetItem(0).Researched = true;
        }
    }

    private static readonly string s_extraPowerText = "Extra Power";
    private static readonly string s_sphereCountFormat = "%0:sx %1:s sphere";
    private static readonly string s_youCannotChannelPowerFromThisNodeText = "You cannot channel power from this node";

    // PrdPlace.pas:51

    // PrdPlace.pas:51

}
