// Translated from PrdPlace.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.Animation;
using static Aow.Units.Animation.Animation;
using Aow.Units.PassiveAb;
using static Aow.Units.PassiveAb.PassiveAb;

namespace Aow.Units.PrdPlace
{
    // { TProductionPlaceControl: size $2C; $28 production place. VMT 557BD4D0. }
    public partial class TProductionPlaceControl : Aow.Units.AoWE.TProductionControl
    {
        public TProductionPlaceControl(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        public Aow.Units.PrdPlace.TProductionPlace FProductionPlace;
        // PrdPlace.pas:238
        // { $28 }
        public override bool ExecuteProduction(Aow.Units.AoWE.TProductionItem Item)
        {
            bool Result = default;
            Result = base.ExecuteProduction(Item);
            if (Result)
            {
                switch (Item.FProductionID)
                {
                    case 10:
                        FSettings.FTurnsTotal = unchecked((sbyte)Item.FParameter);
                        FSettings.FTurnsLeft = 1;
                        var call__1 = GetPlayer();
                        FSettings.FPlayer = unchecked((sbyte)call__1);
                        FStatus = Aow.Units.AoWE.TProductionControlStatus.pcsProducing;
                        break;
                }
            }

            return Result;
        }

        // PrdPlace.pas:169
        public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
        {
            var call__3 = this.FProductionPlace.Xhx;
            X = unchecked((sbyte)call__3);
            var call__4 = this.FProductionPlace.Yhx;
            Y = unchecked((sbyte)call__4);
            L = unchecked((sbyte)this.FProductionPlace.Lhx);
        }

        // PrdPlace.pas:166
        public override sbyte GetPlayer()
        {
            sbyte Result = default;
            Result = unchecked((sbyte)FProductionPlace.FPlayer);
            return Result;
        }

        // PrdPlace.pas:179
        public override bool UnitProduced()
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            Aow.Units.PrdPlace.TProductionPlaceEventLog Log = default;
            var productionPlace = FProductionPlace;
            var call__6 = productionPlace.Yhx;
            var call__7 = productionPlace.Xhx;
            var call__8 = FSettings.FUnit.Place(call__7, call__6, productionPlace.Lhx, unchecked((sbyte)productionPlace.FPlayer), TSet<Aow.Units.AoWE.TUnitPlaceModeElement>.Of(Aow.Units.AoWE.TUnitPlaceModeElement.pmScanArea));
            Result = call__8 != null;
            bool bool__9 = Result;
            if (bool__9)
            {
                var call__10 = FProductionPlace.GetMagicSphere();
                bool__9 = (call__10 != Aow.Units.AoWE.TMagicSphere.msNone);
            }

            bool bool__11 = bool__9;
            if (bool__11)
            {
                var call__12 = FSettings.FUnit.ExpandAbility(155);
                bool__11 = call__12;
            }

            if (bool__11)
            {
                var call__13 = FSettings.FUnit.GetAbilityData(155);
                Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__13).FEnchantment;
                var call__14 = FSettings.FUnit.GetUnitLevel();
                Enchantment.SetUpkeep((call__14 + 1) * 2);
                Enchantment.SetSource(unchecked((sbyte)FProductionPlace.FPlayer));
                var call__15 = FSettings.FUnit.GetUnitLevel();
                Enchantment.FDispelResistance = unchecked((sbyte)(call__15 + 3));
            }

            Log = Aow.Units.PrdPlace.TProductionPlaceEventLog.Create(null);
            Log.FPlayer = unchecked((sbyte)productionPlace.FPlayer);
            var call__16 = FSettings.FUnit.GetName();
            var call__17 = FProductionPlace.GetName();
            var call__18 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XProducedAtYRStr);
            var call__19 = Aow.Rtl.Rtl.Format(call__18, [call__16, call__17]);
            Log.FText = call__19;
            var call__20 = productionPlace.Yhx;
            var call__21 = productionPlace.Xhx;
            Log.SetLocation(unchecked((sbyte)call__21), unchecked((sbyte)call__20), unchecked((sbyte)productionPlace.Lhx));
            var call__22 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Log.FPlayer);
            call__22.FEventLogbook.AddEvent(Log);
            Log.Release();

            return Result;
        }

        // PrdPlace.pas:203
        public override void UnitResearched()
        {
            Aow.Units.PrdPlace.TProductionPlaceEventLog Log = default;
            base.UnitResearched();
            var productionPlace = FProductionPlace;
            Log = Aow.Units.PrdPlace.TProductionPlaceEventLog.Create(null);
            Log.FPlayer = unchecked((sbyte)productionPlace.FPlayer);
            var call__24 = FUnitProductions.GetItem(FSettings.FUnitProductionIndex);
            var call__25 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(call__24.FUnitIndex);
            var call__26 = call__25.GetUnitName();
            var call__27 = FProductionPlace.GetName();
            var call__28 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XInstalledAtYRStr);
            var call__29 = Aow.Rtl.Rtl.Format(call__28, [call__26, call__27]);
            Log.FText = call__29;
            var call__30 = productionPlace.Yhx;
            var call__31 = productionPlace.Xhx;
            Log.SetLocation(unchecked((sbyte)call__31), unchecked((sbyte)call__30), unchecked((sbyte)productionPlace.Lhx));
            var call__32 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Log.FPlayer);
            call__32.FEventLogbook.AddEvent(Log);
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
            base.NewTurn(unchecked((sbyte)Player));
            if (((FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsProducing) && (Player == FSettings.FPlayer)) && (FSettings.FTurnsLeft == 0))
            {
                var call__33 = FQueue.GetItem(0);
                if (call__33.FProductionID == 10)
                {
                    ProductionDone();
                }
            }
        }

        // PrdPlace.pas:225
        public override bool CanExecuteProduction(Aow.Units.AoWE.TProductionItem Item, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
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
        public override Aow.Rtl.AnsiString GetProductionName(Aow.Units.AoWE.TProductionItem Item)
        {
            Aow.Rtl.AnsiString Result = default;
            if ((Item.FProductionID == 1) || (Item.FProductionID == 2))
            {
                Result = base.GetProductionName(Item);
            }
            else
            {
                var call__34 = FProductionPlace.GetProductionName(Item);
                Result = call__34;
            }

            return Result;
        }

        // PrdPlace.pas:258
        public override Aow.Rtl.AnsiString GetProductionNameProgressive(Aow.Units.AoWE.TProductionItem Item)
        {
            Aow.Rtl.AnsiString Result = default;
            if ((Item.FProductionID == 1) || (Item.FProductionID == 2))
            {
                Result = base.GetProductionNameProgressive(Item);
            }
            else
            {
                var call__35 = FProductionPlace.GetProductionNameProgressive(Item);
                Result = call__35;
            }

            return Result;
        }

        // PrdPlace.pas:139
        public override int UnitProductionGold(int Index)
        {
            int Result = default;
            var call__36 = FProductionPlace.GetMagicSphere();
            if (call__36 != Aow.Units.AoWE.TMagicSphere.msNone)
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
            int Result = default;
            // { The original uses the gold production cost as the node's mana cost. }
            var call__37 = FProductionPlace.GetMagicSphere();
            if (call__37 != Aow.Units.AoWE.TMagicSphere.msNone)
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
            int Result = default;
            // { The original uses the inherited mana production cost for gold upkeep. }
            var call__38 = FProductionPlace.GetMagicSphere();
            if (call__38 != Aow.Units.AoWE.TMagicSphere.msNone)
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
            int Result = default;
            var call__39 = FProductionPlace.GetMagicSphere();
            if (call__39 != Aow.Units.AoWE.TMagicSphere.msNone)
            {
                var call__40 = FUnitProductions.GetItem(Index);
                var call__41 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(call__40.FUnitIndex);
                Result = call__41.FLevel * 5;
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
            int Result = default;
            sbyte Player = default;
            int Picks = default;
            var call__42 = FUnitProductions.GetItem(Index);
            Index = call__42.FUnitIndex;
            var call__43 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(Index);
            Result = call__43.FLevel * 2;
            var call__44 = FProductionPlace.GetMagicSphere();
            if (call__44 != Aow.Units.AoWE.TMagicSphere.msNone)
            {
                Player = unchecked((sbyte)FProductionPlace.FPlayer);
                if (Player > 0)
                {
                    var call__45 = FProductionPlace.GetMagicSphere();
                    var call__46 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__47 = call__46.FLeader.GetSpherePicks(call__45);
                    Picks = call__47;
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

    // { TProductionPlace: size $4C; $3C power source, $40 income source, $44 production
//     control, $48 razed state, $49 defender level, $4A padding. VMT 557BD68C. }
    public partial class TProductionPlace : Aow.Units.AoWE.TPlayerStructure
    {
        public TProductionPlace(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__48 = CreateProductionControl();
            FProductionControl = call__48;
            FProductionControl.FProductionPlace = this;
            if (GetMagicSphere() == Aow.Units.AoWE.TMagicSphere.msNone)
            {
                var call__49 = new Aow.Units.AoWE.TPlayerStructureIncomeSource(null);
                FIncomeSource = call__49;
                FIncomeSource.Setup(this);
                FDefenderLevel = 2;
            }
            else
            {
                var call__50 = new Aow.Units.AoWE.TPlayerStructurePowerSource(null);
                FPowerSource = call__50;
                FPowerSource.Setup(this);
                FDefenderLevel = 3;
            }
        }

        // PrdPlace.pas:291
        public new const int ClassID = 132026;

        public Aow.Units.AoWE.TPlayerStructurePowerSource FPowerSource;
        // { $3C }
        public Aow.Units.AoWE.TPlayerStructureIncomeSource FIncomeSource;
        // { $40 }
        public Aow.Units.PrdPlace.TProductionPlaceControl FProductionControl;
        // { $44 }
        public byte FRazedState;
        // { $48 }
        public byte FDefenderLevel;
        // { $49 }
        public Aow.Units.FixedArray_byte_2 F_ProductionPlace_4A;
        // PrdPlace.pas:283
        public override void Destroy()
        {
            base.Destroy();
            FPowerSource.Free();
            FIncomeSource.Free();
            FProductionControl.Free();
        }

        // PrdPlace.pas:528
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FProductionControl, 8);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FDefenderLevel, 1), 1, 9);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FRazedState, 1), 1, 10);
        }

        // PrdPlace.pas:264


        // PrdPlace.pas:352
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537002242)
            {
                MasterySpellCast(unchecked((sbyte)Msg.Param[0].Value), (Aow.Units.AoWE.TMagicSphere)Msg.Param[1].Value);
            }
        }

        // PrdPlace.pas:692
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if ((Msg.MsgID == 4609) && (Msg.HN == 0))
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
                if (GetMagicSphere() == Aow.Units.AoWE.TMagicSphere.msNone)
                {
                    var call__51 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__51.FProductionControls.Add(FProductionControl);
                }

                if (FPowerSource != null)
                {
                    var call__52 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__52.FMagicControl.RegisterPowerSource(FPowerSource);
                }

                if (FIncomeSource != null)
                {
                    var call__53 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__53.RegisterIncomeSource(FIncomeSource);
                }
            }
        }

        // PrdPlace.pas:574
        public override void Deactivate()
        {
            if (FPlayer != -1)
            {
                if (GetMagicSphere() == Aow.Units.AoWE.TMagicSphere.msNone)
                {
                    var call__54 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__54.FProductionControls.Remove(FProductionControl);
                }

                if (FPowerSource != null)
                {
                    var call__55 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__55.FMagicControl.UnregisterPowerSource(FPowerSource);
                }

                if (FIncomeSource != null)
                {
                    var call__56 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__56.UnregisterIncomeSource(FIncomeSource);
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
                SetPlayer(unchecked((sbyte)-1));
                FRazedState = 1;
            }
            else
            {
                FRazedState = 0;
            }

            // { 557BE592: refresh after either state change. }
            var call__57 = GetTerrainTypeImage(FTerrainType);
            FImageIndex = call__57;
        }

        // PrdPlace.pas:388
        public override bool GetRazed()
        {
            bool Result = default;
            Result = FRazedState == 1;
            return Result;
        }

        // PrdPlace.pas:655
        public override void NewDay()
        {
            base.NewDay();
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FDay == 1)
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
            base.NewTurn(unchecked((sbyte)Player));
            FProductionControl.NewTurn(unchecked((sbyte)Player));
        }

        // PrdPlace.pas:665
        public override void BuildingDone()
        {
            base.BuildingDone();
            ResetProductions();
        }

        // PrdPlace.pas:434
        public override Aow.Rtl.AnsiString GetCurrentActivityText()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__58 = FProductionControl.FQueue.Count;
            if (call__58 == 0)
            {
                if (FPlayer > 0)
                {
                    if (GetMagicSphere() != Aow.Units.AoWE.TMagicSphere.msNone)
                    {
                        if (GetPower() != 0)
                        {
                            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GeneratingExtraPowerRStr);
                        }
                        else
                        {
                            Result = AnsiString.FromBytes(45);
                        }
                    }
                    else
                    {
                        if (GetBaseIncome() != 0)
                        {
                            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProducingMerchandiseRStr);
                        }
                        else
                        {
                            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoneRStr);
                        }
                    }
                }
                else
                {
                    Result = Aow.Rtl.Rtl.LowerCase(Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoneRStr));
                }
            }
            else
            {
                var call__59 = FProductionControl.FQueue.GetItem(0);
                var call__60 = FProductionControl.GetProductionNameProgressive(call__59);
                Result = call__60;
            }

            return Result;
        }

        // PrdPlace.pas:607
        public override void UpdateAITarget(in Aow.Units.AoWE.TAITargetInfo Info, ref Aow.Units.AoWE.TAITargetData Data)
        {
            if (!GetRazed())
            {
                switch (Info.Mode)
                {
                    case Aow.Units.AoWE.TAITargetMode.tmAttack:
                    case Aow.Units.AoWE.TAITargetMode.tmRaid:
                        if (Info.Player != FPlayer)
                        {
                            if (FPlayer == 0)
                            {
                                Data.Priority = 50;
                            }
                            else
                            {
                                var call__61 = Aow.Units.AoWE.AoWE.ValidAttackRelation(unchecked((sbyte)Info.Player), unchecked((sbyte)Info.IndependentRelation), unchecked((sbyte)FPlayer), 0);
                                if (call__61)
                                {
                                    var call__62 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Info.Player);
                                    Data.Priority = 50 + (call__62.FIncome / 2);
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
                    case Aow.Units.AoWE.TAITargetMode.tmProduce:
                        if (Info.Player == FPlayer)
                        {
                            Data.Priority = Data.Priority | 2;
                        }

                        break;
                    case Aow.Units.AoWE.TAITargetMode.tmDefend:
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
            if ((FIncomeSource != null) && (FPlayer >= 0))
            {
                var call__63 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                call__63.UpdateIncome();
            }

            if ((FPowerSource != null) && (FPlayer >= 0))
            {
                var call__64 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                call__64.FMagicControl.Update();
            }
        }

        // PrdPlace.pas:294
        public override bool CanDblClick()
        {
            bool Result = default;
            bool bool__65 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer != FPlayer);
            if (!bool__65)
            {
                var call__66 = GetMagicSphere();
                bool__65 = (call__66 != Aow.Units.AoWE.TMagicSphere.msNone);
            }

            if (bool__65)
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
                        var call__67 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                        call__67.FMagicControl.UnregisterPowerSource(FPowerSource);
                    }

                    if (FIncomeSource != null)
                    {
                        var call__68 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                        call__68.UnregisterIncomeSource(FIncomeSource);
                    }

                    if (GetMagicSphere() == Aow.Units.AoWE.TMagicSphere.msNone)
                    {
                        var call__69 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                        call__69.FProductionControls.Remove(FProductionControl);
                    }
                }

                base.SetPlayer(unchecked((sbyte)Player));
                if (FPlayer != -1)
                {
                    if (FPowerSource != null)
                    {
                        var call__70 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                        call__70.FMagicControl.RegisterPowerSource(FPowerSource);
                    }

                    if (FIncomeSource != null)
                    {
                        var call__71 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                        call__71.RegisterIncomeSource(FIncomeSource);
                    }

                    if (GetMagicSphere() == Aow.Units.AoWE.TMagicSphere.msNone)
                    {
                        var call__72 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                        call__72.FProductionControls.Add(FProductionControl);
                    }
                }
            }
        }

        // PrdPlace.pas:375
        public override int GetIncome()
        {
            int Result = default;
            int Income = default;
            Income = GetBaseIncome();
            if (GetMagicSphere() == Aow.Units.AoWE.TMagicSphere.msNone)
            {
                bool bool__73 = (FProductionControl.FStatus != Aow.Units.AoWE.TProductionControlStatus.pcsProducing);
                if (!bool__73)
                {
                    var call__74 = FProductionControl.FQueue.GetItem(0);
                    bool__73 = (call__74.FProductionID == 10);
                }

                if (bool__73)
                {
                    Income += Aow.Rtl.Rtl.Round(((double)Income) * 0.25);
                }
            }

            Result = Income;
            return Result;
        }

        // PrdPlace.pas:358
        public override int GetPower()
        {
            int Result = default;
            // { The original tests msCosmos, rather than msNone, here. }
            bool bool__75 = (FPlayer > 0);
            if (bool__75)
            {
                var call__76 = GetMagicSphere();
                bool__75 = (call__76 != Aow.Units.AoWE.TMagicSphere.msCosmos);
            }

            bool bool__77 = bool__75;
            if (bool__77)
            {
                var call__78 = GetRazed();
                bool__77 = !call__78;
            }

            bool bool__79 = bool__77;
            if (bool__79)
            {
                var call__80 = GetBuilding();
                bool__79 = !call__80;
            }

            if (bool__79)
            {
                var call__81 = GetMagicSphere();
                var call__82 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__83 = call__82.FLeader.GetSpherePicks(call__81);
                Result = call__83;
                if (Result != 0)
                {
                    Result = (Result * 5) + 10;
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
            Aow.Units.PrdPlace.TProductionPlaceEventLog Log = default;
            var call__84 = GetRazed();
            bool bool__85 = !call__84;
            if (bool__85)
            {
                var call__86 = GetBuilding();
                bool__85 = !call__86;
            }

            if ((((bool__85 && (OldPlayer == 0)) && !Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting)) && Status.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moActivated)) && !((Aow.Units.AoWE.TPlayerStructureResource)Resource).FClaimed)
            {
                Log = Aow.Units.PrdPlace.TProductionPlaceEventLog.Create(null);
                Log.FPlayer = unchecked((sbyte)NewPlayer);
                var call__87 = EditName();
                var call__88 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.StructureCapturedRStr);
                var call__89 = Aow.Rtl.Rtl.Format(call__88, [call__87]);
                Log.FText = call__89;
                var call__90 = Xhx;
                Log.FXhx = unchecked((sbyte)call__90);
                var call__91 = Yhx;
                Log.FYhx = unchecked((sbyte)call__91);
                Log.FLhx = unchecked((sbyte)Lhx);
                var call__92 = Yhx;
                var call__93 = Xhx;
                Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)FPlayer), unchecked((sbyte)call__93), unchecked((sbyte)call__92), unchecked((sbyte)Lhx), false);
                Log.Release();
            }
        }

        // PrdPlace.pas:300
        public virtual Aow.Units.PrdPlace.TProductionPlaceControl CreateProductionControl()
        {
            Aow.Units.PrdPlace.TProductionPlaceControl Result = default;
            Result = new Aow.Units.PrdPlace.TProductionPlaceControl(null);
            return Result;
        }

        // PrdPlace.pas:625
        public virtual void GenerateDefenders()
        {
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            int Budget = default;
            int Collection = default;
            int I = default;
            Aow.Units.AoWE.TUnitIndexList Units = default;
            if (FDefenderLevel != 0)
            {
                if (FDefenderLevel == 4)
                {
                    var call__94 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(3);
                    FDefenderLevel = unchecked((byte)(call__94 + 1));
                }

                Budget = ((FDefenderLevel - 2) * 25) + 25;
                var call__95 = FOwner.FindOwnedChild(131607);
                ArmyHS = (Aow.Units.AoWE.TArmyHS)call__95;
                if (ArmyHS != null)
                {
                    var call__96 = ArmyHS.FArmy.ObtainValue(255);
                    Budget -= call__96;
                }

                if (Budget > 0)
                {
                    var call__97 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(1048575);
                    Aow.Rtl.Rtl.RandSeed = call__97;
                    var call__98 = ((Aow.Units.AoWE.TStructureResource)Resource).FDefenders.GetRndCollection();
                    Collection = call__98;
                    if (Collection != -1)
                    {
                        Units = new Aow.Units.AoWE.TUnitIndexList();
                        var unitIndexListCollection = ((Aow.Units.AoWE.TStructureResource)Resource).FDefenders;
                        {
                            I = unitIndexListCollection.GetCollectionCount(Collection) - 1;
                            byte end__100 = 0;
                            for (; I >= end__100; I -= 1)
                            {
                                var call__101 = unitIndexListCollection.GetU(Collection, I);
                                Units.Add(call__101);
                            }
                        }

                        var call__102 = Yhx;
                        var call__103 = Xhx;
                        Aow.Units.AoWE.AoWE.PlaceRandomUnits(call__103, call__102, Lhx, 0, Units, 4, Budget, TSet<Aow.Units.AoWE.TUnitType>.Of(Aow.Units.AoWE.TUnitType.utHumanoid, Aow.Units.AoWE.TUnitType.utCreature));
                        var call__104 = ((Aow.Units.AoWE.TAoWMapField)FOwner).GetArmyHS();
                        ArmyHS = call__104;
                        if ((ArmyHS != null) && (ArmyHS.FArmy.FPlayer == 0))
                        {
                            ArmyHS.FArmy.SetBehavior(255, Aow.Units.AoWE.TGroupBehavior.gbGuard);
                        }

                        Units.Free();
                    }
                }
            }
        }

        // PrdPlace.pas:369
        public virtual int GetBaseIncome()
        {
            int Result = default;
            bool bool__105 = (FRazedState != 1);
            if (bool__105)
            {
                var call__106 = GetBuilding();
                bool__105 = !call__106;
            }

            if (bool__105)
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
        public virtual Aow.Units.AoWE.TMagicSphere GetMagicSphere()
        {
            Aow.Units.AoWE.TMagicSphere Result = default;
            Result = Aow.Units.AoWE.TMagicSphere.msNone;
            return Result;
        }

        // PrdPlace.pas:453
        public virtual Aow.Rtl.AnsiString GetProductionName(Aow.Units.AoWE.TProductionItem Item)
        {
            Aow.Rtl.AnsiString Result = default;
            if (Item.FProductionID == 10)
            {
                if (GetMagicSphere() != Aow.Units.AoWE.TMagicSphere.msNone)
                {
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GenerateExtraPowerRStr);
                }
                else
                {
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProduceMerchandiseRStr);
                }
            }
            else
            {
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UnknownRStr);
            }

            return Result;
        }

        // PrdPlace.pas:461
        public virtual Aow.Rtl.AnsiString GetProductionNameProgressive(Aow.Units.AoWE.TProductionItem Item)
        {
            Aow.Rtl.AnsiString Result = default;
            if (Item.FProductionID == 10)
            {
                if (GetMagicSphere() != Aow.Units.AoWE.TMagicSphere.msNone)
                {
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GeneratingExtraPowerRStr);
                }
                else
                {
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProducingMerchandiseRStr);
                }
            }
            else
            {
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UnknownRStr);
            }

            return Result;
        }

        // PrdPlace.pas:403
        public virtual void ListIncomeInfo(Aow.Rtl.TStrings Names, Aow.Rtl.TStrings Values)
        {
            int Picks = default;
            if (GetMagicSphere() == Aow.Units.AoWE.TMagicSphere.msNone)
            {
                if (GetBaseIncome() > 0)
                {
                    var call__107 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.BaseIncomeRStr);
                    Names.Add(call__107);
                    var call__108 = GetBaseIncome();
                    Values.Add(Aow.Rtl.Rtl.IntToStr(call__108));
                    bool bool__109 = (FProductionControl.FStatus != Aow.Units.AoWE.TProductionControlStatus.pcsProducing);
                    if (!bool__109)
                    {
                        var call__110 = FProductionControl.FQueue.GetItem(0);
                        bool bool__111 = (call__110.FProductionID == 10);
                        if (bool__111)
                        {
                            var call__112 = GetBaseIncome();
                            bool__111 = (call__112 > 0);
                        }

                        bool__109 = bool__111;
                    }

                    if (bool__109)
                    {
                        var call__113 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProducingMerchandiseRStr);
                        Names.Add(call__113);
                        var call__114 = GetBaseIncome();
                        Values.Add(Aow.Rtl.Rtl.IntToStr(Aow.Rtl.Rtl.Round(((double)call__114) * 0.25)));
                    }
                }
            }
            else
            {
                if (FPlayer > 0)
                {
                    var call__115 = GetMagicSphere();
                    var call__116 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    var call__117 = call__116.FLeader.GetSpherePicks(call__115);
                    Picks = call__117;
                    var call__118 = GetMagicSphere();
                    var call__119 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TMagicSphereRStr[(int)call__118]);
                    var call__120 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XtimeYSphereRStr);
                    var call__121 = Aow.Rtl.Rtl.Format(call__120, [Aow.Rtl.Rtl.IntToStr(Picks), Aow.Rtl.Rtl.LowerCase(call__119)]);
                    Names.Add(call__121);
                    Values.Add(Aow.Rtl.Rtl.IntToStr(Picks * 5));
                    bool bool__122 = (FProductionControl.FStatus != Aow.Units.AoWE.TProductionControlStatus.pcsProducing);
                    if (!bool__122)
                    {
                        var call__123 = FProductionControl.FQueue.GetItem(0);
                        bool__122 = (call__123.FProductionID == 10);
                    }

                    if (bool__122)
                    {
                        var call__124 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ExtraPowerRStr);
                        Names.Add(call__124);
                        Values.Add(Aow.Rtl.Rtl.IntToStr(Aow.Rtl.Rtl.Round(((double)(Picks * 5)) * 0.25)));
                    }
                }
            }
        }

        // PrdPlace.pas:306
        public void MasterySpellCast(sbyte Player, Aow.Units.AoWE.TMagicSphere Sphere)
        {
            int UnitIndex = default;
            Aow.Units.AoWE.TUnit AUnit = default;
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            Aow.Units.Animation.TImageSequenceAnimation Animation = default;
            var call__125 = GetMagicSphere();
            if ((Sphere == call__125) && (FPlayer > 0))
            {
                switch (Sphere)
                {
                    case Aow.Units.AoWE.TMagicSphere.msLife:
                        UnitIndex = 216;
                        break;
                    case Aow.Units.AoWE.TMagicSphere.msDeath:
                        UnitIndex = 217;
                        break;
                    case Aow.Units.AoWE.TMagicSphere.msWater:
                        UnitIndex = 227;
                        break;
                    case Aow.Units.AoWE.TMagicSphere.msFire:
                        UnitIndex = 226;
                        break;
                    case Aow.Units.AoWE.TMagicSphere.msEarth:
                        UnitIndex = 225;
                        break;
                    case Aow.Units.AoWE.TMagicSphere.msAir:
                        UnitIndex = 224;
                        break;
                    default:
                        UnitIndex = -1;
                        break;
                }

                if (UnitIndex != -1)
                {
                    AUnit = new Aow.Units.AoWE.TUnit(null);
                    var call__126 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(UnitIndex);
                    AUnit.SetUnitResource(call__126);
                    AUnit.SetPlayer(unchecked((sbyte)FPlayer));
                    var call__127 = Yhx;
                    var call__128 = Xhx;
                    var call__129 = AUnit.Place(call__128, call__127, Lhx, unchecked((sbyte)FPlayer), TSet<Aow.Units.AoWE.TUnitPlaceModeElement>.Of(Aow.Units.AoWE.TUnitPlaceModeElement.pmScanArea));
                    ArmyHS = call__129;
                    if (ArmyHS != null)
                    {
                        if (AUnit.ExpandAbility(155))
                        {
                            var call__130 = AUnit.GetAbilityData(155);
                            Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__130).FEnchantment;
                            var call__131 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(155);
                            var call__132 = ((Aow.Units.PassiveAb.TSummonedAbility)((System.Object)call__131)).GetDefaultManaUpkeep(AUnit);
                            Enchantment.SetUpkeep(call__132);
                            Enchantment.SetSource(unchecked((sbyte)FPlayer));
                            var call__133 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(155);
                            var call__134 = ((Aow.Units.PassiveAb.TSummonedAbility)((System.Object)call__133)).GetDefaultDispelResistance(AUnit);
                            Enchantment.FDispelResistance = unchecked((sbyte)call__134);
                        }

                        var call__135 = ArmyHS.Yhx;
                        var call__136 = ArmyHS.Xhx;
                        var call__137 = Aow.Units.AoWE.AoWE.AoWHSMap.Visible(unchecked((sbyte)call__136), unchecked((sbyte)call__135), unchecked((sbyte)ArmyHS.Lhx), 0);
                        if (call__137)
                        {
                            Animation = new Aow.Units.Animation.TImageSequenceAnimation(null);
                            var call__138 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(29);
                            var call__139 = call__138.FCastImages.GetImageSequence(0);
                            Animation.SetImageSequence(call__139);
                            var call__140 = Yhx;
                            var call__141 = Xhx;
                            Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Animation, call__141, call__140, Lhx);
                        }
                    }

                    AUnit.Release();
                }
            }
        }

        // PrdPlace.pas:469
        public bool CanProduce(int Index, bool Queue, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            var call__142 = FProductionControl.CanProduceUnit(Index, Queue, ref Reason);
            Result = call__142;
            return Result;
        }

        // PrdPlace.pas:472
        public bool Produce(int Index, bool Queue)
        {
            bool Result = default;
            var call__143 = FProductionControl.ProduceUnit(Index, Queue);
            Result = call__143;
            return Result;
        }

        // PrdPlace.pas:475
        public int ProductionGold(int Index)
        {
            int Result = default;
            var call__144 = FProductionControl.UnitProductionGold(Index);
            Result = call__144;
            return Result;
        }

        // PrdPlace.pas:478
        public int ProductionMana(int Index)
        {
            int Result = default;
            var call__145 = FProductionControl.UnitProductionMana(Index);
            Result = call__145;
            return Result;
        }

        // PrdPlace.pas:481
        public int ProductionTime(int Index)
        {
            int Result = default;
            var call__146 = FProductionControl.UnitProductionTime(Index);
            Result = call__146;
            return Result;
        }

        // PrdPlace.pas:484
        public bool CanResearch(int Index, bool Queue, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            var call__147 = FProductionControl.CanResearchUnit(Index, Queue, ref Reason);
            Result = call__147;
            return Result;
        }

        // PrdPlace.pas:487
        public bool Research(int Index, bool Queue)
        {
            bool Result = default;
            var call__148 = FProductionControl.ResearchUnit(Index, Queue);
            Result = call__148;
            return Result;
        }

        // PrdPlace.pas:490
        public int ResearchGold(int Index)
        {
            int Result = default;
            var call__149 = FProductionControl.UnitResearchGold(Index);
            Result = call__149;
            return Result;
        }

        // PrdPlace.pas:493
        public int ResearchMana(int Index)
        {
            int Result = default;
            var call__150 = FProductionControl.UnitResearchMana(Index);
            Result = call__150;
            return Result;
        }

        // PrdPlace.pas:496
        public int ResearchTime(int Index)
        {
            int Result = default;
            var call__151 = FProductionControl.UnitResearchTime(Index);
            Result = call__151;
            return Result;
        }

        // PrdPlace.pas:499
        public bool CanTrade(bool Queue, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            if (GetMagicSphere() != Aow.Units.AoWE.TMagicSphere.msNone)
            {
                var call__152 = GetMagicSphere();
                var call__153 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__154 = call__153.FLeader.GetSpherePicks(call__152);
                if (call__154 > 0)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                    Reason = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.YouCannotChannelPowerFromThisNodeRStr);
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
            bool Result = default;
            Aow.Units.AoWE.TProductionItem Item = default;
            Aow.Rtl.AnsiString Reason = default;
            var call__155 = CanTrade(Queue, ref Reason);
            bool bool__156 = call__155;
            if (bool__156)
            {
                var call__157 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__158 = call__157.GetBusy();
                bool__156 = !call__158;
            }

            if (bool__156)
            {
                Item = new Aow.Units.AoWE.TProductionItem(null);
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
            int I = default;
            Aow.Units.PrdPlace.TProductionPlaceResource R = default;
            FProductionControl.FUnitProductions.Clear();
            if (GetMagicSphere() == Aow.Units.AoWE.TMagicSphere.msNone)
            {
                R = (Aow.Units.PrdPlace.TProductionPlaceResource)Resource;
                {
                    I = 0;
                    int end__159 = R.FUnits.Count - 1;
                    for (; I <= end__159; I += 1)
                    {
                        var call__160 = R.FUnits[I];
                        FProductionControl.FUnitProductions.Add(call__160, false);
                    }
                }

                FProductionControl.FUnitProductions.SortOnLevel();
                var call__161 = FProductionControl.FUnitProductions.GetItem(0);
                call__161.FResearched = true;
            }
        }

        // PrdPlace.pas:51


        public new partial class Meta : Aow.Units.AoWE.TPlayerStructure.Meta
        {
            // PrdPlace.pas:291
            public override int ClassID() => Aow.Units.PrdPlace.TProductionPlace.ClassID;

            // PrdPlace.pas:51
            public new Aow.Units.PrdPlace.TProductionPlace Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.PrdPlace.TProductionPlace)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.PrdPlace.TProductionPlace => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TProductionPlaceResource: size $78; $74 available unit indexes. VMT 557BD94C. }
    public partial class TProductionPlaceResource : Aow.Units.AoWE.TPlayerStructureResource
    {
        public TProductionPlaceResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FClaimed = false;
            var call__162 = new Aow.Units.AoWE.TUnitIndexList();
            FUnits = call__162;
            FRazeable = true;
        }

        // PrdPlace.pas:718
        public new const int ClassID = 132027;

        public Aow.Units.AoWE.TUnitIndexList FUnits;
        // PrdPlace.pas:709
        // { $74 }
        public override void Destroy()
        {
            FUnits.Free();
            base.Destroy();
        }

        // PrdPlace.pas:730
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStreamProc(18, FUnits.LoadFromStream, FUnits.SaveToStream);
        }

        // PrdPlace.pas:701


        // PrdPlace.pas:724
        public override Aow.Units.Engine.EditorClass Editor()
        {
            Aow.Units.Engine.EditorClass Result = default;
            var call__163 = Engine.Editors.FindEditor(132027);
            Result = call__163;
            if (Result == null)
            {
                Result = base.Editor();
            }

            return Result;
        }

        // PrdPlace.pas:721
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132026;
            return Result;
        }

        // PrdPlace.pas:100


        // PrdPlace.pas:715
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"";
            return Result;
        }

        public new partial class Meta : Aow.Units.AoWE.TPlayerStructureResource.Meta
        {
            // PrdPlace.pas:718
            public override int ClassID() => Aow.Units.PrdPlace.TProductionPlaceResource.ClassID;

            // PrdPlace.pas:100
            public new Aow.Units.PrdPlace.TProductionPlaceResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.PrdPlace.TProductionPlaceResource)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.PrdPlace.TProductionPlaceResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TProductionPlaceEventLog: size $20; no added fields. VMT 557BDA5C. }
    public partial class TProductionPlaceEventLog : Aow.Units.AoWE.TProductionEventLog
    {
        // PrdPlace.pas:736
        public new const int ClassID = 131835;

        // PrdPlace.pas:744
        public override bool Execute(bool Mode, Aow.Units.EventLog.TExecuteTrigger Trigger, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            bool Result = default;
            Aow.Units.PrdPlace.TProductionPlace Place = default;
            if (Trigger == Aow.Units.EventLog.TExecuteTrigger.etUser)
            {
                Place = GetProductionPlace();
                if ((Place != null) && (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Place.FPlayer))
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(FXhx, FYhx, FLhx);
                    Aow.Units.AoWE.AoWE.AoWHSMap.TriggerExecuteEventLog(this, true, Done);
                }
            }

            Result = false;
            return Result;
        }

        // PrdPlace.pas:739
        public Aow.Units.PrdPlace.TProductionPlace GetProductionPlace()
        {
            Aow.Units.PrdPlace.TProductionPlace Result = default;
            var call__164 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(FXhx, FYhx, FLhx);
            var call__165 = call__164.FindNoneTransparentHS(typeof(Aow.Units.PrdPlace.TProductionPlace));
            Result = (Aow.Units.PrdPlace.TProductionPlace)call__165;
            return Result;
        }

        // EventLog.pas:91
        public static new Aow.Units.PrdPlace.TProductionPlaceEventLog Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PrdPlace.TProductionPlaceEventLog o = new Aow.Units.PrdPlace.TProductionPlaceEventLog();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TProductionEventLog.Meta
        {
            // PrdPlace.pas:736
            public override int ClassID() => Aow.Units.PrdPlace.TProductionPlaceEventLog.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.PrdPlace.TProductionPlaceEventLog => (Meta)MetaRegistry.Of(typeof(T));
            // EventLog.pas:91
            public new Aow.Units.PrdPlace.TProductionPlaceEventLog Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.PrdPlace.TProductionPlaceEventLog)base.Create(AOwner);
        }
    }

    public static partial class PrdPlace
    {
        // PrdPlace.pas:760
        public static void RegisterProductionPlace(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.PrdPlace.TProductionPlace>(), Aow.Units.Engine.EClass.Of<Aow.Units.PrdPlace.TProductionPlaceResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.PrdPlace.TProductionPlaceEventLog>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
