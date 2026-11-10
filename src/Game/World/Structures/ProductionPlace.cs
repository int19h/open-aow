// Translated from PrdPlace.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
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
        FProductionControl = CreateProductionControl();
        FProductionControl.FProductionPlace = this;
        if (GetMagicSphere() == MagicSphere.None)
        {
            FIncomeSource = new PlayerStructureIncomeSource(null);
            FIncomeSource.Setup(this);
            FDefenderLevel = 2;
        }
        else
        {
            FPowerSource = new PlayerStructurePowerSource(null);
            FPowerSource.Setup(this);
            FDefenderLevel = 3;
        }
    }

    // PrdPlace.pas:291
    public new const int ClassID = 132026;

    public PlayerStructurePowerSource FPowerSource;
    public PlayerStructureIncomeSource FIncomeSource;
    public ProductionPlaceControl FProductionControl;
    public byte FRazedState;
    public byte FDefenderLevel;

    // PrdPlace.pas:283
    public override void Dispose()
    {
        base.Dispose();
        FPowerSource?.Dispose();
        FIncomeSource?.Dispose();
        FProductionControl?.Dispose();
    }

    // PrdPlace.pas:528
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FProductionControl, 8);
        Stream.rwData(Bytes.Of(ref FDefenderLevel, 1), 1, 9);
        Stream.rwData(Bytes.Of(ref FRazedState, 1), 1, 10);
    }

    // PrdPlace.pas:264

    // PrdPlace.pas:352
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537002242)
        {
            MasterySpellCast((sbyte)Msg.Param[0].Int, (MagicSphere)Msg.Param[1].Int);
        }
    }

    // PrdPlace.pas:692
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4609 && Msg.HN == 0)
        {
            Msg.Result = FProductionControl;
        }
    }

    // PrdPlace.pas:562
    public override void Activate()
    {
        base.Activate();
        FProductionControl.Activate();
        if (FPlayer != -1)
        {
            if (GetMagicSphere() == MagicSphere.None)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FProductionControls.Add(FProductionControl);
            }

            if (FPowerSource != null)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.RegisterPowerSource(FPowerSource);
            }

            if (FIncomeSource != null)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).RegisterIncomeSource(FIncomeSource);
            }
        }
    }

    // PrdPlace.pas:574
    public override void Deactivate()
    {
        if (FPlayer != -1)
        {
            if (GetMagicSphere() == MagicSphere.None)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FProductionControls.Remove(FProductionControl);
            }

            if (FPowerSource != null)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.UnregisterPowerSource(FPowerSource);
            }

            if (FIncomeSource != null)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).UnregisterIncomeSource(FIncomeSource);
            }
        }

        FProductionControl.Deactivate();
        base.Deactivate();
    }

    // PrdPlace.pas:698
    public override void Select()
    {
        base.Select();
    }

    // PrdPlace.pas:391
    public override void SetRazed(bool Value)
    {
        if (Value)
        {
            FProductionControl.ExecuteDeleteProductions();
            SetPlayer(-1);
            FRazedState = 1;
        }
        else
        {
            FRazedState = 0;
        }

        // refresh after either state change
        FImageIndex = GetTerrainTypeImage(FTerrainType);
    }

    // PrdPlace.pas:388
    public override bool GetRazed() => FRazedState == 1;

    // PrdPlace.pas:655
    public override void NewDay()
    {
        base.NewDay();
        if (AoWHSMap.Current.FDay == 1)
        {
            if (FPlayer == 0)
            {
                GenerateDefenders();
            }

            ResetProductions();
        }
    }

    // PrdPlace.pas:686
    public override void NewTurn(sbyte Player)
    {
        base.NewTurn(Player);
        FProductionControl.NewTurn(Player);
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
        string Result;
        if (FProductionControl.FQueue.Count == 0)
        {
            if (FPlayer > 0)
            {
                if (GetMagicSphere() != MagicSphere.None)
                {
                    if (GetPower() != 0)
                    {
                        Result = AbilityRules.TranslateRStr(GameText.GeneratingExtraPowerRStr);
                    }
                    else
                    {
                        Result = "-";
                    }
                }
                else
                {
                    if (GetBaseIncome() != 0)
                    {
                        Result = AbilityRules.TranslateRStr(GameText.ProducingMerchandiseRStr);
                    }
                    else
                    {
                        Result = AbilityRules.TranslateRStr(GameTables.NoneRStr);
                    }
                }
            }
            else
            {
                Result = LowerCase(AbilityRules.TranslateRStr(GameTables.NoneRStr));
            }
        }
        else
        {
            var queue = FProductionControl.FQueue.GetItem(0);
            Result = FProductionControl.GetProductionNameProgressive(queue);
        }

        return Result;
    }

    // PrdPlace.pas:607
    public override void UpdateAITarget(in AITargetInfo Info, ref AITargetData Data)
    {
        if (!GetRazed())
        {
            switch (Info.Mode)
            {
                case AITargetMode.Attack:
                case AITargetMode.Raid:
                    if (Info.Player != FPlayer)
                    {
                        if (FPlayer == 0)
                        {
                            Data.Priority = 50;
                        }
                        else
                        {
                            if (AbilityRules.ValidAttackRelation(Info.Player, Info.IndependentRelation, FPlayer, 0))
                            {
                                var player = AoWHSMap.Current.FPlayers.GetPlayers(Info.Player);
                                Data.Priority = 50 + player.FIncome / 2;
                                if (Data.Priority < 25)
                                {
                                    Data.Priority = 25;
                                }

                                if (Data.Priority > 75)
                                {
                                    Data.Priority = 75;
                                }
                            }
                        }
                    }

                    break;
                case AITargetMode.Produce:
                    if (Info.Player == FPlayer)
                    {
                        Data.Priority = Data.Priority | 2;
                    }

                    break;
                case AITargetMode.Defend:
                    if (Info.Player == FPlayer)
                    {
                        Data.Priority = 30;
                    }

                    break;
            }
        }
    }

    // PrdPlace.pas:536
    public override void Update()
    {
        base.Update();
        if (FIncomeSource != null && FPlayer >= 0)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).UpdateIncome();
        }

        if (FPowerSource != null && FPlayer >= 0)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.Update();
        }
    }

    // PrdPlace.pas:294
    public override bool CanDblClick()
    {
        bool Result;
        if (AoWHSMap.Current.FSeatedPlayer != FPlayer || GetMagicSphere() != MagicSphere.None)
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // PrdPlace.pas:586
    public override void SetPlayer(sbyte Player)
    {
        if (Player != FPlayer)
        {
            FProductionControl.PlayerChanged();
            if (FPlayer != -1)
            {
                if (FPowerSource != null)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.UnregisterPowerSource(FPowerSource);
                }

                if (FIncomeSource != null)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).UnregisterIncomeSource(FIncomeSource);
                }

                if (GetMagicSphere() == MagicSphere.None)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FProductionControls.Remove(FProductionControl);
                }
            }

            base.SetPlayer(Player);
            if (FPlayer != -1)
            {
                if (FPowerSource != null)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.RegisterPowerSource(FPowerSource);
                }

                if (FIncomeSource != null)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).RegisterIncomeSource(FIncomeSource);
                }

                if (GetMagicSphere() == MagicSphere.None)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FProductionControls.Add(FProductionControl);
                }
            }
        }
    }

    // PrdPlace.pas:375
    public override int GetIncome()
    {
        int Income;
        Income = GetBaseIncome();
        if (GetMagicSphere() == MagicSphere.None)
        {
            if (FProductionControl.FStatus != ProductionControl.Status.Producing || FProductionControl.FQueue.GetItem(0).Order is Trade)
            {
                Income += Round((double)Income * 0.25);
            }
        }

        return Income;
    }

    // PrdPlace.pas:358
    public override int GetPower()
    {
        int Result;
        // { The original tests msCosmos, rather than msNone, here. }
        if (FPlayer > 0 && GetMagicSphere() != MagicSphere.Cosmos && !GetRazed() && !GetBuilding())
        {
            var magicSphere = GetMagicSphere();
            Result = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FLeader.GetSpherePicks(magicSphere);
            if (Result != 0)
            {
                Result = Result * 5 + 10;
            }
            else
            {
                Result = 0;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // PrdPlace.pas:543
    public override void StructureCaptured(sbyte OldPlayer, sbyte NewPlayer)
    {
        ProductionPlaceEventLog Log;
        if (!GetRazed() && !GetBuilding() && OldPlayer == 0 && !AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting) && Status.Contains(MapObjectStatusElement.Activated) && !((PlayerStructureResource)Resource).FClaimed)
        {
            Log = new ProductionPlaceEventLog(null);
            Log.FPlayer = NewPlayer;
            var editName = EditName();
            Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.StructureCapturedRStr), [editName]);
            Log.FXhx = Xhx;
            Log.FYhx = Yhx;
            Log.FLhx = Lhx;
            var yhx = Yhx;
            var xhx = Xhx;
            EventLog.DistributeLocationEventLog(Log, FPlayer, xhx, yhx, Lhx, false);
            Log.Release();
        }
    }

    // PrdPlace.pas:300
    public virtual ProductionPlaceControl CreateProductionControl() => new ProductionPlaceControl(null);

    // PrdPlace.pas:625
    public virtual void GenerateDefenders()
    {
        ArmyHS ArmyHS;
        int Budget;
        int Collection;
        UnitIndexList Units;
        if (FDefenderLevel != 0)
        {
            if (FDefenderLevel == 4)
            {
                FDefenderLevel = (byte)(AoWHSMap.Current.Random(3) + 1);
            }

            Budget = (FDefenderLevel - 2) * 25 + 25;
            ArmyHS = (ArmyHS)FOwner.FindOwnedChild(131607);
            if (ArmyHS != null)
            {
                Budget -= ArmyHS.FArmy.ObtainValue(255);
            }

            if (Budget > 0)
            {
                RandSeed = AoWHSMap.Current.Random(1048575);
                Collection = ((StructureResource)Resource).FDefenders.GetRndCollection();
                if (Collection != -1)
                {
                    Units = new UnitIndexList();
                    var unitIndexListCollection = ((StructureResource)Resource).FDefenders;
                    for (int I = unitIndexListCollection.GetCollectionCount(Collection) - 1; I >= 0; I--)
                    {
                        Units.Add(unitIndexListCollection.GetU(Collection, I));
                    }

                    var yhx = Yhx;
                    AbilityRules.PlaceRandomUnits(Xhx, yhx, Lhx, 0, Units, 4, Budget, Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature));
                    ArmyHS = ((AoWMapField)FOwner).GetArmyHS();
                    if (ArmyHS != null && ArmyHS.FArmy.FPlayer == 0)
                    {
                        ArmyHS.FArmy.SetBehavior(255, GroupBehavior.Guard);
                    }

                    Units?.Dispose();
                }
            }
        }
    }

    // PrdPlace.pas:369
    public virtual int GetBaseIncome()
    {
        int Result;
        if (FRazedState != 1 && !GetBuilding())
        {
            Result = 10;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // PrdPlace.pas:303
    public virtual MagicSphere GetMagicSphere() => MagicSphere.None;

    // PrdPlace.pas:403
    public virtual void ListIncomeInfo(StringCollection Names, StringCollection Values)
    {
        int Picks;
        if (GetMagicSphere() == MagicSphere.None)
        {
            if (GetBaseIncome() > 0)
            {
                Names.Add(AbilityRules.TranslateRStr(GameText.BaseIncomeRStr));
                Values.Add(IntToStr(GetBaseIncome()));
                if (FProductionControl.FStatus != ProductionControl.Status.Producing || FProductionControl.FQueue.GetItem(0).Order is Trade && GetBaseIncome() > 0)
                {
                    Names.Add(AbilityRules.TranslateRStr(GameText.ProducingMerchandiseRStr));
                    Values.Add(IntToStr(Round((double)GetBaseIncome() * 0.25)));
                }
            }
        }
        else
        {
            if (FPlayer > 0)
            {
                var magicSphere = GetMagicSphere();
                Picks = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FLeader.GetSpherePicks(magicSphere);
                var magicSphere2 = GetMagicSphere();
                var magicSphereText = AbilityRules.TranslateRStr(GameTables.TMagicSphereRStr[(int)magicSphere2]);
                Names.Add(RtlUnit.Format(AbilityRules.TranslateRStr(XtimeYSphereRStr), [IntToStr(Picks), LowerCase(magicSphereText)]));
                Values.Add(IntToStr(Picks * 5));
                if (FProductionControl.FStatus != ProductionControl.Status.Producing || FProductionControl.FQueue.GetItem(0).Order is Trade)
                {
                    Names.Add(AbilityRules.TranslateRStr(ExtraPowerRStr));
                    Values.Add(IntToStr(Round((double)(Picks * 5) * 0.25)));
                }
            }
        }
    }

    // PrdPlace.pas:306
    public void MasterySpellCast(sbyte Player, MagicSphere Sphere)
    {
        int UnitIndex;
        Unit AUnit;
        ArmyHS ArmyHS;
        UnitEnchantment Enchantment;
        ImageSequenceAnimation Animation;
        if (Sphere == GetMagicSphere() && FPlayer > 0)
        {
            switch (Sphere)
            {
                case MagicSphere.Life:
                    UnitIndex = 216;
                    break;
                case MagicSphere.Death:
                    UnitIndex = 217;
                    break;
                case MagicSphere.Water:
                    UnitIndex = 227;
                    break;
                case MagicSphere.Fire:
                    UnitIndex = 226;
                    break;
                case MagicSphere.Earth:
                    UnitIndex = 225;
                    break;
                case MagicSphere.Air:
                    UnitIndex = 224;
                    break;
                default:
                    UnitIndex = -1;
                    break;
            }

            if (UnitIndex != -1)
            {
                AUnit = new Unit(null);
                AUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(UnitIndex));
                AUnit.SetPlayer(FPlayer);
                var yhx = Yhx;
                ArmyHS = AUnit.Place(Xhx, yhx, Lhx, FPlayer, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                if (ArmyHS != null)
                {
                    if (AUnit.ExpandAbility(155))
                    {
                        Enchantment = ((UnitEnchantmentAbilityData)AUnit.GetAbilityData(155)).FEnchantment;
                        Enchantment.SetUpkeep(SummonedAbility.GetDefaultManaUpkeep(AUnit));
                        Enchantment.SetSource(FPlayer);
                        Enchantment.FDispelResistance = (sbyte)SummonedAbility.GetDefaultDispelResistance(AUnit);
                    }

                    var armyHSYhx = ArmyHS.Yhx;
                    var xhx = ArmyHS.Xhx;
                    if (AoWHSMap.Current.Visible(xhx, armyHSYhx, ArmyHS.Lhx, 0))
                    {
                        Animation = new ImageSequenceAnimation(null);
                        Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(29).FCastImages.GetImageSequence(0));
                        var yhx2 = Yhx;
                        var xhx2 = Xhx;
                        AoWHSMap.Current.PlaceHX(Animation, xhx2, yhx2, Lhx);
                    }
                }

                AUnit.Release();
            }
        }
    }

    // PrdPlace.pas:469
    public bool CanProduce(int Index, bool Queue, ref string Reason) => FProductionControl.CanProduceUnit(Index, Queue, ref Reason);

    // PrdPlace.pas:472
    public bool Produce(int Index, bool Queue) => FProductionControl.ProduceUnit(Index, Queue);

    // PrdPlace.pas:475
    public int ProductionGold(int Index) => FProductionControl.UnitProductionGold(Index);

    // PrdPlace.pas:478
    public int ProductionMana(int Index) => FProductionControl.UnitProductionMana(Index);

    // PrdPlace.pas:481
    public int ProductionTime(int Index) => FProductionControl.UnitProductionTime(Index);

    // PrdPlace.pas:484
    public bool CanResearch(int Index, bool Queue, ref string Reason) => FProductionControl.CanResearchUnit(Index, Queue, ref Reason);

    // PrdPlace.pas:487
    public bool Research(int Index, bool Queue) => FProductionControl.ResearchUnit(Index, Queue);

    // PrdPlace.pas:490
    public int ResearchGold(int Index) => FProductionControl.UnitResearchGold(Index);

    // PrdPlace.pas:493
    public int ResearchMana(int Index) => FProductionControl.UnitResearchMana(Index);

    // PrdPlace.pas:496
    public int ResearchTime(int Index) => FProductionControl.UnitResearchTime(Index);

    // PrdPlace.pas:499
    public bool CanTrade(bool Queue, ref string Reason)
    {
        bool Result;
        if (GetMagicSphere() != MagicSphere.None)
        {
            var magicSphere = GetMagicSphere();
            if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FLeader.GetSpherePicks(magicSphere) > 0)
            {
                Result = true;
            }
            else
            {
                Result = false;
                Reason = AbilityRules.TranslateRStr(YouCannotChannelPowerFromThisNodeRStr);
            }
        }
        else
        {
            Result = GetBaseIncome() != 0;
        }

        return Result;
    }

    // PrdPlace.pas:511
    public bool Trade(bool Queue)
    {
        bool Result;
        ProductionItem Item;
        string Reason = "";
        if (CanTrade(Queue, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
        {
            Item = new ProductionItem(null);
            if (Queue)
            {
                Item.Order = new Trade(1);
            }
            else
            {
                Item.Order = new Trade(-1);
            }

            FProductionControl.Produce(Item, Queue);
            Item?.Dispose();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // PrdPlace.pas:671
    public void ResetProductions()
    {
        ProductionPlaceResource R;
        FProductionControl.FUnitProductions.Clear();
        if (GetMagicSphere() == MagicSphere.None)
        {
            R = (ProductionPlaceResource)Resource;
            for (int I = 0, last = R.FUnits.Count - 1; I <= last; I++)
            {
                var unit = R.FUnits[I];
                FProductionControl.FUnitProductions.Add(unit, false);
            }

            FProductionControl.FUnitProductions.SortOnLevel();
            FProductionControl.FUnitProductions.GetItem(0).FResearched = true;
        }
    }

    private static readonly string ExtraPowerRStr = "Extra Power";
    private static readonly string XtimeYSphereRStr = "%0:sx %1:s sphere";
    private static readonly string YouCannotChannelPowerFromThisNodeRStr = "You cannot channel power from this node";

    // PrdPlace.pas:51

    // PrdPlace.pas:51

}
