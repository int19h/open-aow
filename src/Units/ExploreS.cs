// Translated from ExploreS.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.ExploreS
{
    // { ot=1 }
    public partial class TDefendersArmy : TArmy
    {
        public TDefendersArmy(TECustomNode owner) : base(owner)
        { }

        // ExploreS.pas:71
        public override bool CanAddUnit(TAbstractUnit AUnit)
        {
            bool Result = default;
            Result = base.CanAddUnit(AUnit);
            bool bool__1 = Result;
            if (bool__1)
            {
                var call__2 = AUnit.GetTransporter();
                bool__1 = !call__2;
            }

            bool bool__3 = bool__1;
            if (bool__3)
            {
                var call__4 = AUnit.ValidTerrain(1, 4);
                bool__3 = call__4;
            }

            Result = bool__3;
            return Result;
        }

        // intf_P2_07.inc:445

    }

    // FDefendersMode, FDefenders
    public partial class TExplorationSite : TStructure
    {
        public TExplorationSite(TECustomNode owner) : base(owner)
        {
            FDefendersMode = TDefendersMode.dmNormal;
            var call__5 = new TDefendersArmy(null);
            FDefenders = call__5;
        }

        // ExploreS.pas:90
        public new const int ClassID = 132033;

        public TDefendersMode FDefendersMode;
        public TDefendersArmy FDefenders;
        // ExploreS.pas:84
        public override void Destroy()
        {
            base.Destroy();
            FDefenders.Free();
        }

        // ExploreS.pas:293
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FDefenders, 5);
            Stream.rwData(Bytes.Of(ref FDefendersMode, 1), 1, 6);
        }

        // ExploreS.pas:77


        // ExploreS.pas:351
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            FDefenders.MsgProc(ref Msg, Sender);
        }

        // ExploreS.pas:339
        public override void Activate()
        {
            base.Activate();
            FDefenders.Activate();
        }

        // ExploreS.pas:345
        public override void Deactivate()
        {
            FDefenders.Deactivate();
            base.Deactivate();
        }

        // ExploreS.pas:217
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            int Xhx = default;
            int Yhx = default;
            if (!AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
                if (((TMapField)Owner).Xhx == Xhx && ((TMapField)Owner).Yhx == Yhx)
                {
                    Result = base.CanSelect(Xhp, Yhp);
                }
                else
                {
                    Result = false;
                }
            }
            else
            {
                Result = base.CanSelect(Xhp, Yhp);
            }

            return Result;
        }

        // ExploreS.pas:96
        public override int GetTerrainTypeImage(int TerrainType)
        {
            int Result = default;
            int Image = default;
            var call__6 = GetExplored();
            if (!call__6 || AoWHSMap.FDay == 0)
            {
                Result = TerrainType;
            }
            else
            {
                Image = TerrainType + 100;
                var call__7 = ((TExplorationSiteResource)FResource).ImageSequenceList.ImageSequences[Image << 0];
                if (call__7 != null)
                {
                    Result = Image;
                }
                else
                {
                    Result = TerrainType;
                }
            }

            return Result;
        }

        // ExploreS.pas:209
        public override TExecuteResult ExecuteTE(TStructureTE A1, TTokenExecuter Executer)
        {
            TExecuteResult Result = default;
            Result = base.ExecuteTE(A1, Executer);
            if (A1.FAction == 0)
            {
                var call__8 = ExecuteSearch((TCombatType)A1.FParam);
                if (call__8)
                {
                    Result = TExecuteResult.erDone;
                }
                else
                {
                    Result = TExecuteResult.erDoneHalt;
                }
            }

            return Result;
        }

        // ExploreS.pas:326
        public override void NewDay()
        {
            base.NewDay();
            if (AoWHSMap.FDay == 1 && FDefendersMode != TDefendersMode.dmUserDefined)
            {
                GenerateDefenders();
            }
        }

        // ExploreS.pas:333
        public override void Update()
        {
            base.Update();
            var call__9 = GetTerrainTypeImage(FTerrainType);
            FImageIndex = call__9;
        }

        // ExploreS.pas:93
        public virtual bool GetExplored()
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // ExploreS.pas:138
        public virtual void SetupCombat(TCombat A1, TArmyHS A2)
        {
            A1.FOnExecuted = CombatExecuted;
            var call__10 = A2.GetYhx();
            var call__11 = A2.GetLhx();
            var call__12 = GetXhx();
            var call__13 = GetYhx();
            var call__14 = GetLhx();
            var call__15 = A2.GetXhx();
            A1.Setup((byte)A2.FArmy.FPlayer, (byte)A2.FArmy.FIndependentRelation, call__15, call__10, call__11, (byte)FDefenders.FPlayer, (byte)FDefenders.FIndependentRelation, call__12, call__13, call__14, true);
            var call__16 = GetYhx();
            var call__17 = GetLhx();
            var call__18 = GetXhx();
            A1.AddArmy(A2.FArmy, TCombatSideIndex.csAttacker, call__18, call__16, call__17);
            A1.AddArmyEx(FDefenders, TCombatSideIndex.csDefender, -1, -1, -1, 1);
            A1.FExplorationSite = 1;
        }

        // ExploreS.pas:110
        public virtual void ExecuteSearchDone(TCombatType A1, TCombatResult A2)
        {
        }

        // ExploreS.pas:148
        public virtual bool ExecuteSearch(TCombatType A1)
        {
            bool Result = default;
            TArmyHS ArmyHS = default;
            TCombat Combat = default;
            sbyte Player = default;
            TCombatEventLog Event = default;
            var call__19 = Owner.FindOwnedChild(131607);
            ArmyHS = (TArmyHS)call__19;
            bool bool__20 = ArmyHS != null;
            if (bool__20)
            {
                var call__21 = CanSearch(ArmyHS.FArmy.FPlayer);
                bool__20 = call__21;
            }

            if (bool__20)
            {
                Player = ArmyHS.FArmy.FPlayer;
                var call__22 = FDefenders.GetCount();
                if (call__22 == 0)
                {
                    ExecuteSearchDone(TCombatType.ctFast, TCombatResult.crVictory);
                }
                else
                {
                    if (A1 == TCombatType.ctFast)
                    {
                        AoWHSMap.LockGameOverEvent();
                        var call__23 = AoWHSMap.CreateCombat(131760);
                        Combat = call__23;
                        SetupCombat(Combat, ArmyHS);
                        Combat.LockExecuted();
                        Combat.Initialize();
                        Combat.Activate();
                        Combat.Execute();
                        Combat.Deactivate();
                        Combat.Finalize();
                        var call__24 = GetName();
                        Combat.FLogbook.FFinalData.FHeader = call__24 + AnsiString.FromBytes(32) + Combat.FLogbook.FFinalData.FHeader;
                        Event = new TCombatEventLog(null);
                        Event.SetCombatLogbook(Combat.FLogbook);
                        var call__25 = AoWHSMap.FPlayers.GetPlayers(Player);
                        call__25.FEventLogbook.AddEvent(Event);
                        bool bool__26 = AoWHSMap.FSeatedPlayer == Player;
                        if (bool__26)
                        {
                            var call__27 = AoWHSMap.FPlayers.GetPlayers(Player);
                            bool__26 = call__27.FPlayerType == TPlayerType.ptHuman;
                        }

                        if (bool__26)
                        {
                            Event.Execute(false, TExecuteTrigger.etEvent, null);
                        }

                        Event.Release();
                        Combat.UnlockExecuted();
                        AoWHSMap.DestroyCombat();
                        AoWHSMap.UnlockGameOverEvent();
                    }
                    else
                    {
                        var call__28 = AoWHSMap.CreateCombat(2228496);
                        Combat = call__28;
                        SetupCombat(Combat, ArmyHS);
                        Combat.Initialize();
                        Combat.Activate();
                        if (Combat.Execute() != TCombatResult.crBusy)
                        {
                            Combat.Deactivate();
                            Combat.Finalize();
                            var call__29 = GetName();
                            Combat.FLogbook.FFinalData.FHeader = call__29 + AnsiString.FromBytes(32) + Combat.FLogbook.FFinalData.FHeader;
                            Event = new TCombatEventLog(null);
                            Event.SetCombatLogbook(Combat.FLogbook);
                            var call__30 = AoWHSMap.FPlayers.GetPlayers(Player);
                            call__30.FEventLogbook.AddEvent(Event);
                            bool bool__31 = AoWHSMap.FSeatedPlayer == Player;
                            if (bool__31)
                            {
                                var call__32 = AoWHSMap.FPlayers.GetPlayers(Player);
                                bool__31 = call__32.FPlayerType == TPlayerType.ptHuman;
                            }

                            if (bool__31)
                            {
                                Event.Execute(false, TExecuteTrigger.etEvent, null);
                            }

                            Event.Release();
                            AoWHSMap.DestroyCombat();
                        }
                    }
                }

                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // ExploreS.pas:258
        public virtual bool Search()
        {
            bool Result = default;
            TArmyHS ArmyHS = default;
            TCombatTypeRequestEventLog Request = default;
            TStructureTE TE = default;
            var call__33 = Owner.FindOwnedChild(131607);
            ArmyHS = (TArmyHS)call__33;
            bool bool__34 = ArmyHS != null;
            if (bool__34)
            {
                var call__35 = CanSearch(ArmyHS.FArmy.FPlayer);
                bool__34 = call__35;
            }

            if (bool__34)
            {
                var playerList = AoWHSMap.FPlayers;
                if (playerList.FLocalPlayers.Count == 1 || AoWHSMap.FRequestCombatType)
                {
                    Request = new TCombatTypeRequestEventLog(null);
                    var call__37 = GetLhx();
                    var call__38 = GetYhx();
                    var call__39 = GetXhx();
                    Request.SetCenterLocation((byte)call__39, (byte)call__38, call__37);
                    Request.FPlayer = ArmyHS.FArmy.FPlayer;
                    Request.FDefenderPlayer = 0;
                    AoWHSMap.TriggerExecuteEventLog(Request, true, RequestCombatTypeCallBack);
                    Request.Release();
                    Result = true;
                }
                else
                {
                    var call__40 = playerList.GetPlayers(ArmyHS.FArmy.FPlayer);
                    var call__41 = call__40.GetBusy();
                    if (!call__41)
                    {
                        TE = CreateTE();
                        SetupTE(TE);
                        TE.FAction = 0;
                        TE.FParam = 1;
                        var call__42 = AoWHSMap.FTokenManager.GetTokenControl(TE.FPlayer);
                        call__42.AddEvent(TE);
                        TE.Release();
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                    }
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // ExploreS.pas:113
        public void CombatExecuted(TCombat A1)
        {
            TCombatData Data = default;
            TCombatObject Obj = default;
            TAbstractUnit AUnit = default;
            int I = default;
            Data = A1.FCombatData;
            {
                I = 0;
                int end__43 = Data.GetCount() - 1;
                for (; I <= end__43; I += 1)
                {
                    Obj = Data.GetObjects(I);
                    if (Obj is TCombatUnit && Obj.GetPlayer() == 0)
                    {
                        AUnit = ((TCombatUnit)Obj).FUnit;
                        AUnit.AddRef();
                        ((TCombatUnit)Obj).SetUnit(null);
                        FDefenders.AddUnit(AUnit);
                        AUnit.Release();
                    }
                }
            }

            if (A1 is TFastCombat)
            {
                ExecuteSearchDone(TCombatType.ctFast, A1.FCombatResult);
            }
            else
            {
                ExecuteSearchDone(TCombatType.ctTactical, A1.FCombatResult);
            }
        }

        // ExploreS.pas:230
        public void RequestCombatTypeCallBack(TEventLog A1)
        {
            TStructureTE TE = default;
            var combatTypeRequestEventLog = (TCombatTypeRequestEventLog)A1;
            if (combatTypeRequestEventLog.FCombatType != TCombatType.ctNone)
            {
                TE = CreateTE();
                SetupTE(TE);
                TE.FAction = 0;
                TE.FParam = (int)combatTypeRequestEventLog.FCombatType;
                var call__45 = AoWHSMap.FTokenManager.GetTokenControl(TE.FPlayer);
                call__45.AddEvent(TE);
                TE.Release();
            }
        }

        // ExploreS.pas:246
        public bool CanSearch(sbyte A1)
        {
            bool Result = default;
            TArmyHS ArmyHS = default;
            if (!GetExplored())
            {
                var call__46 = Owner.FindOwnedChild(131607);
                ArmyHS = (TArmyHS)call__46;
                if (ArmyHS == null || ArmyHS.FArmy.FPlayer != A1)
                {
                    Result = false;
                }
                else
                {
                    Result = true;
                    return Result;
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // ExploreS.pas:300
        public void GenerateDefenders()
        {
            int Collection = default;
            int I = default;
            TUnitIndexList List = default;
            TUnitIndexListCollection Collections = default;
            RandSeed = AoWHSMap.FDefendersSeed;
            if (FDefendersMode == TDefendersMode.dmRandom)
            {
                var call__47 = Random(3);
                FDefendersMode = (TDefendersMode)(call__47 + 1);
            }

            var call__48 = ((TExplorationSiteResource)FResource).FDefenderCollections.GetRndCollection();
            Collection = call__48;
            if (Collection != -1)
            {
                List = new TUnitIndexList();
                Collections = ((TExplorationSiteResource)FResource).FDefenderCollections;
                {
                    I = Collections.GetCollectionCount(Collection) - 1;
                    byte end__49 = 0;
                    for (; I >= end__49; I -= 1)
                    {
                        var call__50 = Collections.GetU(Collection, I);
                        List.Add(call__50);
                    }
                }

                try
                {
                    FillWithRandomUnits(List, FDefenders, 4, (int)FDefendersMode * 50, TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature, TUnitType.utMachine));
                }
                catch (System.Exception)
                {
                }

                // { Original silently ignores defender-generation failures. }
                List.Free();
            }
        }

        // ExploreS.pas:33


    }

    // FDefenderCollections, the available defender army compositions
    public partial class TExplorationSiteResource : TStructureResource
    {
        public TExplorationSiteResource(TECustomNode owner) : base(owner)
        {
            var call__51 = new TUnitIndexListCollection();
            FDefenderCollections = call__51;
            FTerrainBound = false;
        }

        // ExploreS.pas:373
        public new const int ClassID = 132034;

        public TUnitIndexListCollection FDefenderCollections;
        // ExploreS.pas:364
        public override void Destroy()
        {
            FDefenderCollections.Free();
            base.Destroy();
        }

        // ExploreS.pas:376
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStreamProc(25, FDefenderCollections.LoadFromStream, FDefenderCollections.SaveToStream);
        }

        // ExploreS.pas:357


        // ExploreS.pas:382
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132033;
            return Result;
        }

        // ExploreS.pas:61


        // ExploreS.pas:370
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"";
            return Result;
        }

    }

    public enum TDefendersMode : byte
    {
        dmUserDefined = 0,
        dmLow = 1,
        dmNormal = 2,
        dmHigh = 3,
        dmRandom = 4
    }

    public static partial class ExploreS
    {
        // ExploreS.pas:385
        public static void RegisterExplorationSite(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TExplorationSite>(), EClass.Of<TExplorationSiteResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
