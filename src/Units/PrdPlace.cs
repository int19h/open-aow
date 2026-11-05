// Translated from PrdPlace.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.Animation;
using Aow.Units.PassiveAb;

namespace Aow.Units.PrdPlace
{
    // production place
    public partial class TProductionPlaceControl : TProductionControl
    {
        public TProductionPlaceControl(TECustomNode owner) : base(owner)
        { }

        public TProductionPlace FProductionPlace;
        // PrdPlace.pas:238
        public override bool ExecuteProduction(TProductionItem Item)
        {
            bool Result = base.ExecuteProduction(Item);
            if (Result)
            {
                switch (Item.FProductionID)
                {
                    case 10:
                        FSettings.FTurnsTotal = (sbyte)Item.FParameter;
                        FSettings.FTurnsLeft = 1;
                        var player = GetPlayer();
                        FSettings.FPlayer = player;
                        FStatus = TProductionControlStatus.pcsProducing;
                        break;
                }
            }

            return Result;
        }

        // PrdPlace.pas:169
        public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
        {
            X = this.FProductionPlace.Xhx;
            Y = this.FProductionPlace.Yhx;
            L = FProductionPlace.Lhx;
        }

        // PrdPlace.pas:166
        public override sbyte GetPlayer() => FProductionPlace.FPlayer;

        // PrdPlace.pas:179
        public override bool UnitProduced()
        {
            TUnitEnchantment Enchantment;
            TProductionPlaceEventLog Log;
            var productionPlace = FProductionPlace;
            var yhx = productionPlace.Yhx;
            var xhx = productionPlace.Xhx;
            bool Result = FSettings.FUnit.Place(xhx, yhx, productionPlace.Lhx, productionPlace.FPlayer, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea)) != null;
            if (Result && FProductionPlace.GetMagicSphere() != TMagicSphere.msNone && FSettings.FUnit.ExpandAbility(155))
            {
                Enchantment = ((TUnitEnchantmentAbilityData)FSettings.FUnit.GetAbilityData(155)).FEnchantment;
                Enchantment.SetUpkeep((FSettings.FUnit.GetUnitLevel() + 1) * 2);
                Enchantment.SetSource(FProductionPlace.FPlayer);
                Enchantment.FDispelResistance = (sbyte)(FSettings.FUnit.GetUnitLevel() + 3);
            }

            Log = new TProductionPlaceEventLog(null);
            Log.FPlayer = productionPlace.FPlayer;
            var name = FSettings.FUnit.GetName();
            var fProductionPlaceName = FProductionPlace.GetName();
            Log.FText = Rtl.Rtl.Format(TranslateRStr(XProducedAtYRStr), [name, fProductionPlaceName]);
            var productionPlaceYhx = productionPlace.Yhx;
            Log.SetLocation(productionPlace.Xhx, productionPlaceYhx, productionPlace.Lhx);
            AoWHSMap.FPlayers.GetPlayers(Log.FPlayer).FEventLogbook.AddEvent(Log);
            Log.Release();

            return Result;
        }

        // PrdPlace.pas:203
        public override void UnitResearched()
        {
            TProductionPlaceEventLog Log;
            base.UnitResearched();
            var productionPlace = FProductionPlace;
            Log = new TProductionPlaceEventLog(null);
            Log.FPlayer = productionPlace.FPlayer;
            var unitProduction = FUnitProductions.GetItem(FSettings.FUnitProductionIndex);
            var unitName = AoWHSSet.FUnitResources.GetUnitResource(unitProduction.FUnitIndex).GetUnitName();
            var name = FProductionPlace.GetName();
            Log.FText = Rtl.Rtl.Format(TranslateRStr(XInstalledAtYRStr), [unitName, name]);
            var yhx = productionPlace.Yhx;
            Log.SetLocation(productionPlace.Xhx, yhx, productionPlace.Lhx);
            AoWHSMap.FPlayers.GetPlayers(Log.FPlayer).FEventLogbook.AddEvent(Log);
            Log.Release();
        }

        // PrdPlace.pas:219
        public override void Update()
        {
            base.Update();
            if (FUpdateCount == 0)
            {
                FProductionPlace.Update();
            }
        }

        // PrdPlace.pas:231
        public override void NewTurn(sbyte Player)
        {
            base.NewTurn(Player);
            if (FStatus == TProductionControlStatus.pcsProducing && Player == FSettings.FPlayer && FSettings.FTurnsLeft == 0)
            {
                if (FQueue.GetItem(0).FProductionID == 10)
                {
                    ProductionDone();
                }
            }
        }

        // PrdPlace.pas:225
        public override bool CanExecuteProduction(TProductionItem Item, ref string Reason)
        {
            bool Result;
            if (Item.FProductionID == 10)
            {
                Result = true;
            }
            else
            {
                Result = base.CanExecuteProduction(Item, ref Reason);
            }

            return Result;
        }

        // PrdPlace.pas:252
        public override string GetProductionName(TProductionItem Item)
        {
            string Result;
            if (Item.FProductionID == 1 || Item.FProductionID == 2)
            {
                Result = base.GetProductionName(Item);
            }
            else
            {
                Result = FProductionPlace.GetProductionName(Item);
            }

            return Result;
        }

        // PrdPlace.pas:258
        public override string GetProductionNameProgressive(TProductionItem Item)
        {
            string Result;
            if (Item.FProductionID == 1 || Item.FProductionID == 2)
            {
                Result = base.GetProductionNameProgressive(Item);
            }
            else
            {
                Result = FProductionPlace.GetProductionNameProgressive(Item);
            }

            return Result;
        }

        // PrdPlace.pas:139
        public override int UnitProductionGold(int Index)
        {
            int Result;
            if (FProductionPlace.GetMagicSphere() != TMagicSphere.msNone)
            {
                Result = 0;
            }
            else
            {
                Result = base.UnitProductionGold(Index);
            }

            return Result;
        }

        // PrdPlace.pas:145
        public override int UnitProductionMana(int Index)
        {
            int Result;
            // { The original uses the gold production cost as the node's mana cost. }
            if (FProductionPlace.GetMagicSphere() != TMagicSphere.msNone)
            {
                Result = base.UnitProductionGold(Index);
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // PrdPlace.pas:152
        public override int UnitProductionGoldUpkeep(int Index)
        {
            int Result;
            // { The original uses the inherited mana production cost for gold upkeep. }
            if (FProductionPlace.GetMagicSphere() != TMagicSphere.msNone)
            {
                Result = 0;
            }
            else
            {
                Result = base.UnitProductionMana(Index);
            }

            return Result;
        }

        // PrdPlace.pas:159
        public override int UnitProductionManaUpkeep(int Index)
        {
            int Result;
            if (FProductionPlace.GetMagicSphere() != TMagicSphere.msNone)
            {
                var unitProduction = FUnitProductions.GetItem(Index);
                Result = AoWHSSet.FUnitResources.GetUnitResource(unitProduction.FUnitIndex).FLevel * 5;
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // PrdPlace.pas:121
        public override int UnitProductionTime(int Index)
        {
            sbyte Player;
            int Picks;
            Index = FUnitProductions.GetItem(Index).FUnitIndex;
            int Result = AoWHSSet.FUnitResources.GetUnitResource(Index).FLevel * 2;
            if (FProductionPlace.GetMagicSphere() != TMagicSphere.msNone)
            {
                Player = FProductionPlace.FPlayer;
                if (Player > 0)
                {
                    var magicSphere = FProductionPlace.GetMagicSphere();
                    Picks = AoWHSMap.FPlayers.GetPlayers(Player).FLeader.GetSpherePicks(magicSphere);
                }
                else
                {
                    Picks = 0;
                }

                Result -= Picks / 2;
                if (Result < 1)
                {
                    Result = 1;
                }
            }

            return Result;
        }

        // intf_P1_02.inc:120

    }

    // power source, income source, production control, razed state, defender level
    public partial class TProductionPlace : TPlayerStructure
    {
        public TProductionPlace(TECustomNode owner) : base(owner)
        {
            FProductionControl = CreateProductionControl();
            FProductionControl.FProductionPlace = this;
            if (GetMagicSphere() == TMagicSphere.msNone)
            {
                FIncomeSource = new TPlayerStructureIncomeSource(null);
                FIncomeSource.Setup(this);
                FDefenderLevel = 2;
            }
            else
            {
                FPowerSource = new TPlayerStructurePowerSource(null);
                FPowerSource.Setup(this);
                FDefenderLevel = 3;
            }
        }

        // PrdPlace.pas:291
        public new const int ClassID = 132026;

        public TPlayerStructurePowerSource FPowerSource;
        public TPlayerStructureIncomeSource FIncomeSource;
        public TProductionPlaceControl FProductionControl;
        public byte FRazedState;
        public byte FDefenderLevel;
        // PrdPlace.pas:283
        public override void Destroy()
        {
            base.Destroy();
            FPowerSource.Free();
            FIncomeSource.Free();
            FProductionControl.Free();
        }

        // PrdPlace.pas:528
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FProductionControl, 8);
            Stream.rwData(Bytes.Of(ref FDefenderLevel, 1), 1, 9);
            Stream.rwData(Bytes.Of(ref FRazedState, 1), 1, 10);
        }

        // PrdPlace.pas:264


        // PrdPlace.pas:352
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537002242)
            {
                MasterySpellCast((sbyte)Msg.Param[0].Value, (TMagicSphere)Msg.Param[1].Value);
            }
        }

        // PrdPlace.pas:692
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
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
                if (GetMagicSphere() == TMagicSphere.msNone)
                {
                    AoWHSMap.FPlayers.GetPlayers(FPlayer).FProductionControls.Add(FProductionControl);
                }

                if (FPowerSource != null)
                {
                    AoWHSMap.FPlayers.GetPlayers(FPlayer).FMagicControl.RegisterPowerSource(FPowerSource);
                }

                if (FIncomeSource != null)
                {
                    AoWHSMap.FPlayers.GetPlayers(FPlayer).RegisterIncomeSource(FIncomeSource);
                }
            }
        }

        // PrdPlace.pas:574
        public override void Deactivate()
        {
            if (FPlayer != -1)
            {
                if (GetMagicSphere() == TMagicSphere.msNone)
                {
                    AoWHSMap.FPlayers.GetPlayers(FPlayer).FProductionControls.Remove(FProductionControl);
                }

                if (FPowerSource != null)
                {
                    AoWHSMap.FPlayers.GetPlayers(FPlayer).FMagicControl.UnregisterPowerSource(FPowerSource);
                }

                if (FIncomeSource != null)
                {
                    AoWHSMap.FPlayers.GetPlayers(FPlayer).UnregisterIncomeSource(FIncomeSource);
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
            if (AoWHSMap.FDay == 1)
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
                    if (GetMagicSphere() != TMagicSphere.msNone)
                    {
                        if (GetPower() != 0)
                        {
                            Result = TranslateRStr(GeneratingExtraPowerRStr);
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
                            Result = TranslateRStr(ProducingMerchandiseRStr);
                        }
                        else
                        {
                            Result = TranslateRStr(NoneRStr);
                        }
                    }
                }
                else
                {
                    Result = LowerCase(TranslateRStr(NoneRStr));
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
        public override void UpdateAITarget(in TAITargetInfo Info, ref TAITargetData Data)
        {
            if (!GetRazed())
            {
                switch (Info.Mode)
                {
                    case TAITargetMode.tmAttack:
                    case TAITargetMode.tmRaid:
                        if (Info.Player != FPlayer)
                        {
                            if (FPlayer == 0)
                            {
                                Data.Priority = 50;
                            }
                            else
                            {
                                if (ValidAttackRelation(Info.Player, Info.IndependentRelation, FPlayer, 0))
                                {
                                    var player = AoWHSMap.FPlayers.GetPlayers(Info.Player);
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
                    case TAITargetMode.tmProduce:
                        if (Info.Player == FPlayer)
                        {
                            Data.Priority = Data.Priority | 2;
                        }

                        break;
                    case TAITargetMode.tmDefend:
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
                AoWHSMap.FPlayers.GetPlayers(FPlayer).UpdateIncome();
            }

            if (FPowerSource != null && FPlayer >= 0)
            {
                AoWHSMap.FPlayers.GetPlayers(FPlayer).FMagicControl.Update();
            }
        }

        // PrdPlace.pas:294
        public override bool CanDblClick()
        {
            bool Result;
            if (AoWHSMap.FSeatedPlayer != FPlayer || GetMagicSphere() != TMagicSphere.msNone)
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
                        AoWHSMap.FPlayers.GetPlayers(FPlayer).FMagicControl.UnregisterPowerSource(FPowerSource);
                    }

                    if (FIncomeSource != null)
                    {
                        AoWHSMap.FPlayers.GetPlayers(FPlayer).UnregisterIncomeSource(FIncomeSource);
                    }

                    if (GetMagicSphere() == TMagicSphere.msNone)
                    {
                        AoWHSMap.FPlayers.GetPlayers(FPlayer).FProductionControls.Remove(FProductionControl);
                    }
                }

                base.SetPlayer(Player);
                if (FPlayer != -1)
                {
                    if (FPowerSource != null)
                    {
                        AoWHSMap.FPlayers.GetPlayers(FPlayer).FMagicControl.RegisterPowerSource(FPowerSource);
                    }

                    if (FIncomeSource != null)
                    {
                        AoWHSMap.FPlayers.GetPlayers(FPlayer).RegisterIncomeSource(FIncomeSource);
                    }

                    if (GetMagicSphere() == TMagicSphere.msNone)
                    {
                        AoWHSMap.FPlayers.GetPlayers(FPlayer).FProductionControls.Add(FProductionControl);
                    }
                }
            }
        }

        // PrdPlace.pas:375
        public override int GetIncome()
        {
            int Income;
            Income = GetBaseIncome();
            if (GetMagicSphere() == TMagicSphere.msNone)
            {
                if (FProductionControl.FStatus != TProductionControlStatus.pcsProducing || FProductionControl.FQueue.GetItem(0).FProductionID == 10)
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
            if (FPlayer > 0 && GetMagicSphere() != TMagicSphere.msCosmos && !GetRazed() && !GetBuilding())
            {
                var magicSphere = GetMagicSphere();
                Result = AoWHSMap.FPlayers.GetPlayers(FPlayer).FLeader.GetSpherePicks(magicSphere);
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
            TProductionPlaceEventLog Log;
            if (!GetRazed() && !GetBuilding() && OldPlayer == 0 && !AoWHSMap.Status.Contains(THSMapStatusElement.msEditting) && Status.Contains(TMapObjectStatusElement.moActivated) && !((TPlayerStructureResource)Resource).FClaimed)
            {
                Log = new TProductionPlaceEventLog(null);
                Log.FPlayer = NewPlayer;
                var editName = EditName();
                Log.FText = Rtl.Rtl.Format(TranslateRStr(StructureCapturedRStr), [editName]);
                Log.FXhx = Xhx;
                Log.FYhx = Yhx;
                Log.FLhx = Lhx;
                var yhx = Yhx;
                var xhx = Xhx;
                DistributeLocationEventLog(Log, FPlayer, xhx, yhx, Lhx, false);
                Log.Release();
            }
        }

        // PrdPlace.pas:300
        public virtual TProductionPlaceControl CreateProductionControl() => new TProductionPlaceControl(null);

        // PrdPlace.pas:625
        public virtual void GenerateDefenders()
        {
            TArmyHS ArmyHS;
            int Budget;
            int Collection;
            TUnitIndexList Units;
            if (FDefenderLevel != 0)
            {
                if (FDefenderLevel == 4)
                {
                    FDefenderLevel = (byte)(AoWHSMap.Random(3) + 1);
                }

                Budget = (FDefenderLevel - 2) * 25 + 25;
                ArmyHS = (TArmyHS)FOwner.FindOwnedChild(131607);
                if (ArmyHS != null)
                {
                    Budget -= ArmyHS.FArmy.ObtainValue(255);
                }

                if (Budget > 0)
                {
                    RandSeed = AoWHSMap.Random(1048575);
                    Collection = ((TStructureResource)Resource).FDefenders.GetRndCollection();
                    if (Collection != -1)
                    {
                        Units = new TUnitIndexList();
                        var unitIndexListCollection = ((TStructureResource)Resource).FDefenders;
                        for (int I = unitIndexListCollection.GetCollectionCount(Collection) - 1; I >= 0; I--)
                        {
                            Units.Add(unitIndexListCollection.GetU(Collection, I));
                        }

                        var yhx = Yhx;
                        PlaceRandomUnits(Xhx, yhx, Lhx, 0, Units, 4, Budget, TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature));
                        ArmyHS = ((TAoWMapField)FOwner).GetArmyHS();
                        if (ArmyHS != null && ArmyHS.FArmy.FPlayer == 0)
                        {
                            ArmyHS.FArmy.SetBehavior(255, TGroupBehavior.gbGuard);
                        }

                        Units.Free();
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
        public virtual TMagicSphere GetMagicSphere() => TMagicSphere.msNone;

        // PrdPlace.pas:453
        public virtual string GetProductionName(TProductionItem Item)
        {
            string Result;
            if (Item.FProductionID == 10)
            {
                if (GetMagicSphere() != TMagicSphere.msNone)
                {
                    Result = TranslateRStr(GenerateExtraPowerRStr);
                }
                else
                {
                    Result = TranslateRStr(ProduceMerchandiseRStr);
                }
            }
            else
            {
                Result = TranslateRStr(UnknownRStr);
            }

            return Result;
        }

        // PrdPlace.pas:461
        public virtual string GetProductionNameProgressive(TProductionItem Item)
        {
            string Result;
            if (Item.FProductionID == 10)
            {
                if (GetMagicSphere() != TMagicSphere.msNone)
                {
                    Result = TranslateRStr(GeneratingExtraPowerRStr);
                }
                else
                {
                    Result = TranslateRStr(ProducingMerchandiseRStr);
                }
            }
            else
            {
                Result = TranslateRStr(UnknownRStr);
            }

            return Result;
        }

        // PrdPlace.pas:403
        public virtual void ListIncomeInfo(TStrings Names, TStrings Values)
        {
            int Picks;
            if (GetMagicSphere() == TMagicSphere.msNone)
            {
                if (GetBaseIncome() > 0)
                {
                    Names.Add(TranslateRStr(BaseIncomeRStr));
                    Values.Add(IntToStr(GetBaseIncome()));
                    if (FProductionControl.FStatus != TProductionControlStatus.pcsProducing || FProductionControl.FQueue.GetItem(0).FProductionID == 10 && GetBaseIncome() > 0)
                    {
                        Names.Add(TranslateRStr(ProducingMerchandiseRStr));
                        Values.Add(IntToStr(Round((double)GetBaseIncome() * 0.25)));
                    }
                }
            }
            else
            {
                if (FPlayer > 0)
                {
                    var magicSphere = GetMagicSphere();
                    Picks = AoWHSMap.FPlayers.GetPlayers(FPlayer).FLeader.GetSpherePicks(magicSphere);
                    var magicSphere2 = GetMagicSphere();
                    var magicSphere3 = TranslateRStr(TMagicSphereRStr[(int)magicSphere2]);
                    Names.Add(Rtl.Rtl.Format(TranslateRStr(XtimeYSphereRStr), [IntToStr(Picks), LowerCase(magicSphere3)]));
                    Values.Add(IntToStr(Picks * 5));
                    if (FProductionControl.FStatus != TProductionControlStatus.pcsProducing || FProductionControl.FQueue.GetItem(0).FProductionID == 10)
                    {
                        Names.Add(TranslateRStr(ExtraPowerRStr));
                        Values.Add(IntToStr(Round((double)(Picks * 5) * 0.25)));
                    }
                }
            }
        }

        // PrdPlace.pas:306
        public void MasterySpellCast(sbyte Player, TMagicSphere Sphere)
        {
            int UnitIndex;
            TUnit AUnit;
            TArmyHS ArmyHS;
            TUnitEnchantment Enchantment;
            TImageSequenceAnimation Animation;
            if (Sphere == GetMagicSphere() && FPlayer > 0)
            {
                switch (Sphere)
                {
                    case TMagicSphere.msLife:
                        UnitIndex = 216;
                        break;
                    case TMagicSphere.msDeath:
                        UnitIndex = 217;
                        break;
                    case TMagicSphere.msWater:
                        UnitIndex = 227;
                        break;
                    case TMagicSphere.msFire:
                        UnitIndex = 226;
                        break;
                    case TMagicSphere.msEarth:
                        UnitIndex = 225;
                        break;
                    case TMagicSphere.msAir:
                        UnitIndex = 224;
                        break;
                    default:
                        UnitIndex = -1;
                        break;
                }

                if (UnitIndex != -1)
                {
                    AUnit = new TUnit(null);
                    AUnit.SetUnitResource(AoWHSSet.FUnitResources.GetUnitResource(UnitIndex));
                    AUnit.SetPlayer(FPlayer);
                    var yhx = Yhx;
                    ArmyHS = AUnit.Place(Xhx, yhx, Lhx, FPlayer, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea));
                    if (ArmyHS != null)
                    {
                        if (AUnit.ExpandAbility(155))
                        {
                            Enchantment = ((TUnitEnchantmentAbilityData)AUnit.GetAbilityData(155)).FEnchantment;
                            Enchantment.SetUpkeep(((TSummonedAbility)(object)AoWHSSet.FSpellControl.GetSpell(155)).GetDefaultManaUpkeep(AUnit));
                            Enchantment.SetSource(FPlayer);
                            Enchantment.FDispelResistance = (sbyte)((TSummonedAbility)(object)AoWHSSet.FSpellControl.GetSpell(155)).GetDefaultDispelResistance(AUnit);
                        }

                        var armyHSYhx = ArmyHS.Yhx;
                        var xhx = ArmyHS.Xhx;
                        if (AoWHSMap.Visible(xhx, armyHSYhx, ArmyHS.Lhx, 0))
                        {
                            Animation = new TImageSequenceAnimation(null);
                            Animation.SetImageSequence(AoWHSSet.FSpellControl.GetSpell(29).FCastImages.GetImageSequence(0));
                            var yhx2 = Yhx;
                            var xhx2 = Xhx;
                            AoWHSMap.PlaceHX(Animation, xhx2, yhx2, Lhx);
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
            if (GetMagicSphere() != TMagicSphere.msNone)
            {
                var magicSphere = GetMagicSphere();
                if (AoWHSMap.FPlayers.GetPlayers(FPlayer).FLeader.GetSpherePicks(magicSphere) > 0)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                    Reason = TranslateRStr(YouCannotChannelPowerFromThisNodeRStr);
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
            TProductionItem Item;
            string Reason = "";
            if (CanTrade(Queue, ref Reason) && !AoWHSMap.FPlayers.GetPlayers(FPlayer).GetBusy())
            {
                Item = new TProductionItem(null);
                Item.FProductionID = 10;
                if (Queue)
                {
                    Item.FParameter = 1;
                }
                else
                {
                    Item.FParameter = -1;
                }

                FProductionControl.Produce(Item, Queue);
                Item.Free();
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
            TProductionPlaceResource R;
            FProductionControl.FUnitProductions.Clear();
            if (GetMagicSphere() == TMagicSphere.msNone)
            {
                R = (TProductionPlaceResource)Resource;
                for (int I = 0, last = R.FUnits.Count - 1; I <= last; I++)
                {
                    var unit = R.FUnits[I];
                    FProductionControl.FUnitProductions.Add(unit, false);
                }

                FProductionControl.FUnitProductions.SortOnLevel();
                FProductionControl.FUnitProductions.GetItem(0).FResearched = true;
            }
        }

        // PrdPlace.pas:51


    }

    // available unit indexes
    public partial class TProductionPlaceResource : TPlayerStructureResource
    {
        public TProductionPlaceResource(TECustomNode owner) : base(owner)
        {
            FClaimed = false;
            FUnits = new TUnitIndexList();
            FRazeable = true;
        }

        // PrdPlace.pas:718
        public new const int ClassID = 132027;

        public TUnitIndexList FUnits;
        // PrdPlace.pas:709
        public override void Destroy()
        {
            FUnits.Free();
            base.Destroy();
        }

        // PrdPlace.pas:730
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStreamProc(18, FUnits.LoadFromStream, FUnits.SaveToStream);
        }

        // PrdPlace.pas:701


        // PrdPlace.pas:724
        public override EditorClass Editor()
        {
            EditorClass Result = Engine.Editors.FindEditor(132027);
            if (Result == null)
            {
                Result = base.Editor();
            }

            return Result;
        }

        // PrdPlace.pas:721
        public override int ResourceUserClassID() => 132026;

        // PrdPlace.pas:100


        // PrdPlace.pas:715
        public override string ResourceEditName() => "";

    }

    public partial class TProductionPlaceEventLog : TProductionEventLog
    {
        public TProductionPlaceEventLog(TECustomNode owner) : base(owner)
        { }

        // PrdPlace.pas:736
        public new const int ClassID = 131835;

        // PrdPlace.pas:744
        public override bool Execute(bool Mode, TExecuteTrigger Trigger, TExecuteDoneEvent Done)
        {
            TProductionPlace Place;
            if (Trigger == TExecuteTrigger.etUser)
            {
                Place = GetProductionPlace();
                if (Place != null && AoWHSMap.FSeatedPlayer == Place.FPlayer)
                {
                    AoWHSMap.CenterViewTo(FXhx, FYhx, FLhx);
                    AoWHSMap.TriggerExecuteEventLog(this, true, Done);
                }
            }

            return false;
        }

        // PrdPlace.pas:739
        public TProductionPlace GetProductionPlace() => (TProductionPlace)AoWHSMap.MapContainer.GetField(FXhx, FYhx, FLhx).FindNoneTransparentHS(typeof(TProductionPlace));

    }

    public static partial class PrdPlace
    {
        // PrdPlace.pas:760
        public static void RegisterProductionPlace(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TProductionPlace>(), EClass.Of<TProductionPlaceResource>(), EClass.Of<TProductionPlaceEventLog>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
