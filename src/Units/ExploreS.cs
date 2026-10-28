// Translated from ExploreS.pas.
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

namespace Aow.Units.ExploreS
{
    // { ot=1 }
    // { TDefendersArmy (instance size 44, VMT 557C12B4): no additional fields. }
    public partial class TDefendersArmy : Aow.Units.AoWE.TArmy
    {
        // ExploreS.pas:71
        public override bool CanAddUnit(Aow.Units.AoWE.TAbstractUnit AUnit)
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
        public static new Aow.Units.ExploreS.TDefendersArmy Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.ExploreS.TDefendersArmy o = new Aow.Units.ExploreS.TDefendersArmy();
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
    }

    // { TExplorationSite (instance size 56, VMT 557C1440):
//     $30 FDefendersMode, $34 FDefenders. }
    public partial class TExplorationSite : Aow.Units.AoWE.TStructure
    {
        public Aow.Units.ExploreS.TDefendersMode FDefendersMode;
        public Aow.Units.FixedArray_byte_3 F_ExplorationSite_31;
        public Aow.Units.ExploreS.TDefendersArmy FDefenders;
        // ExploreS.pas:84
        public override void Destroy()
        {
            base.Destroy();
            FDefenders.Free();
        }

        // ExploreS.pas:293
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FDefenders, 5);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FDefendersMode, 1), 1, 6);
        }

        // ExploreS.pas:77
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FDefendersMode = Aow.Units.ExploreS.TDefendersMode.dmNormal;
            var call__5 = Aow.Units.ExploreS.TDefendersArmy.Create(null);
            FDefenders = call__5;
        }

        // ExploreS.pas:351
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
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
            if (!Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                Aow.Units.HSEngine.HSEngine.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
                if ((((Aow.Units.HSEngine.TMapField)Owner).Xhx == Xhx) && (((Aow.Units.HSEngine.TMapField)Owner).Yhx == Yhx))
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
            if (!call__6 || (Aow.Units.AoWE.AoWE.AoWHSMap.FDay == 0))
            {
                Result = TerrainType;
            }
            else
            {
                Image = TerrainType + 100;
                var call__7 = ((Aow.Units.ExploreS.TExplorationSiteResource)FResource).ImageSequenceList.ImageSequences[Image << 0];
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
        public override Aow.Units.NetworkE.TExecuteResult ExecuteTE(Aow.Units.AoWE.TStructureTE A1, Aow.Units.NetworkE.TTokenExecuter Executer)
        {
            Aow.Units.NetworkE.TExecuteResult Result = default;
            Result = base.ExecuteTE(A1, Executer);
            if (A1.FAction == 0)
            {
                var call__8 = ExecuteSearch((Aow.Units.AoWE.TCombatType)A1.FParam);
                if (call__8)
                {
                    Result = Aow.Units.NetworkE.TExecuteResult.erDone;
                }
                else
                {
                    Result = Aow.Units.NetworkE.TExecuteResult.erDoneHalt;
                }
            }

            return Result;
        }

        // ExploreS.pas:326
        public override void NewDay()
        {
            base.NewDay();
            if ((Aow.Units.AoWE.AoWE.AoWHSMap.FDay == 1) && (FDefendersMode != Aow.Units.ExploreS.TDefendersMode.dmUserDefined))
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
        public virtual void SetupCombat(Aow.Units.AoWE.TCombat A1, Aow.Units.AoWE.TArmyHS A2)
        {
            A1.FOnExecuted = CombatExecuted;
            var call__10 = A2.GetYhx();
            var call__11 = A2.GetLhx();
            var call__12 = GetXhx();
            var call__13 = GetYhx();
            var call__14 = GetLhx();
            var call__15 = A2.GetXhx();
            A1.Setup(unchecked((byte)A2.FArmy.FPlayer), unchecked((byte)A2.FArmy.FIndependentRelation), call__15, call__10, call__11, unchecked((byte)FDefenders.FPlayer), unchecked((byte)FDefenders.FIndependentRelation), call__12, call__13, call__14, true);
            var call__16 = GetYhx();
            var call__17 = GetLhx();
            var call__18 = GetXhx();
            A1.AddArmy(A2.FArmy, Aow.Units.AoWE.TCombatSideIndex.csAttacker, call__18, call__16, call__17);
            A1.AddArmyEx(FDefenders, Aow.Units.AoWE.TCombatSideIndex.csDefender, -1, -1, -1, 1);
            A1.FExplorationSite = 1;
        }

        // ExploreS.pas:110
        public virtual void ExecuteSearchDone(Aow.Units.AoWE.TCombatType A1, Aow.Units.AoWE.TCombatResult A2)
        {
        }

        // ExploreS.pas:148
        public virtual bool ExecuteSearch(Aow.Units.AoWE.TCombatType A1)
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            Aow.Units.AoWE.TCombat Combat = default;
            sbyte Player = default;
            Aow.Units.AoWE.TCombatEventLog Event = default;
            var call__19 = Owner.FindOwnedChild(131607);
            ArmyHS = (Aow.Units.AoWE.TArmyHS)call__19;
            bool bool__20 = (ArmyHS != null);
            if (bool__20)
            {
                var call__21 = CanSearch(unchecked((sbyte)ArmyHS.FArmy.FPlayer));
                bool__20 = call__21;
            }

            if (bool__20)
            {
                Player = unchecked((sbyte)ArmyHS.FArmy.FPlayer);
                var call__22 = FDefenders.GetCount();
                if (call__22 == 0)
                {
                    ExecuteSearchDone(Aow.Units.AoWE.TCombatType.ctFast, Aow.Units.AoWE.TCombatResult.crVictory);
                }
                else
                {
                    if (A1 == Aow.Units.AoWE.TCombatType.ctFast)
                    {
                        Aow.Units.AoWE.AoWE.AoWHSMap.LockGameOverEvent();
                        var call__23 = Aow.Units.AoWE.AoWE.AoWHSMap.CreateCombat(131760);
                        Combat = call__23;
                        SetupCombat(Combat, ArmyHS);
                        Combat.LockExecuted();
                        Combat.Initialize();
                        Combat.Activate();
                        Combat.Execute();
                        Combat.Deactivate();
                        Combat.Finalize();
                        var call__24 = GetName();
                        Combat.FLogbook.FFinalData.FHeader = (call__24 + AnsiString.FromBytes(32)) + Combat.FLogbook.FFinalData.FHeader;
                        Event = Aow.Units.AoWE.TCombatEventLog.Create(null);
                        Event.SetCombatLogbook(Combat.FLogbook);
                        var call__25 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                        call__25.FEventLogbook.AddEvent(Event);
                        bool bool__26 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Player);
                        if (bool__26)
                        {
                            var call__27 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                            bool__26 = (call__27.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman);
                        }

                        if (bool__26)
                        {
                            Event.Execute(false, Aow.Units.EventLog.TExecuteTrigger.etEvent, null);
                        }

                        Event.Release();
                        Combat.UnlockExecuted();
                        Aow.Units.AoWE.AoWE.AoWHSMap.DestroyCombat();
                        Aow.Units.AoWE.AoWE.AoWHSMap.UnlockGameOverEvent();
                    }
                    else
                    {
                        var call__28 = Aow.Units.AoWE.AoWE.AoWHSMap.CreateCombat(2228496);
                        Combat = call__28;
                        SetupCombat(Combat, ArmyHS);
                        Combat.Initialize();
                        Combat.Activate();
                        if (Combat.Execute() != Aow.Units.AoWE.TCombatResult.crBusy)
                        {
                            Combat.Deactivate();
                            Combat.Finalize();
                            var call__29 = GetName();
                            Combat.FLogbook.FFinalData.FHeader = (call__29 + AnsiString.FromBytes(32)) + Combat.FLogbook.FFinalData.FHeader;
                            Event = Aow.Units.AoWE.TCombatEventLog.Create(null);
                            Event.SetCombatLogbook(Combat.FLogbook);
                            var call__30 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                            call__30.FEventLogbook.AddEvent(Event);
                            bool bool__31 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Player);
                            if (bool__31)
                            {
                                var call__32 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                                bool__31 = (call__32.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman);
                            }

                            if (bool__31)
                            {
                                Event.Execute(false, Aow.Units.EventLog.TExecuteTrigger.etEvent, null);
                            }

                            Event.Release();
                            Aow.Units.AoWE.AoWE.AoWHSMap.DestroyCombat();
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
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            Aow.Units.AoWE.TCombatTypeRequestEventLog Request = default;
            Aow.Units.AoWE.TStructureTE TE = default;
            var call__33 = Owner.FindOwnedChild(131607);
            ArmyHS = (Aow.Units.AoWE.TArmyHS)call__33;
            bool bool__34 = (ArmyHS != null);
            if (bool__34)
            {
                var call__35 = CanSearch(unchecked((sbyte)ArmyHS.FArmy.FPlayer));
                bool__34 = call__35;
            }

            if (bool__34)
            {
                {
                    var w__36 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers;
                    if ((w__36.FLocalPlayers.Count == 1) || Aow.Units.AoWE.AoWE.AoWHSMap.FRequestCombatType)
                    {
                        Request = Aow.Units.AoWE.TCombatTypeRequestEventLog.Create(null);
                        var call__37 = GetLhx();
                        var call__38 = GetYhx();
                        var call__39 = GetXhx();
                        Request.SetCenterLocation(unchecked((byte)call__39), unchecked((byte)call__38), call__37);
                        Request.FPlayer = unchecked((sbyte)ArmyHS.FArmy.FPlayer);
                        Request.FDefenderPlayer = 0;
                        Aow.Units.AoWE.AoWE.AoWHSMap.TriggerExecuteEventLog(Request, true, RequestCombatTypeCallBack);
                        Request.Release();
                        Result = true;
                    }
                    else
                    {
                        var call__40 = w__36.GetPlayers(ArmyHS.FArmy.FPlayer);
                        var call__41 = call__40.GetBusy();
                        if (!call__41)
                        {
                            TE = CreateTE();
                            SetupTE(TE);
                            TE.FAction = 0;
                            TE.FParam = 1;
                            var call__42 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(TE.FPlayer);
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
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // ExploreS.pas:113
        public void CombatExecuted(Aow.Units.AoWE.TCombat A1)
        {
            Aow.Units.AoWE.TCombatData Data = default;
            Aow.Units.AoWE.TCombatObject Obj = default;
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            int I = default;
            Data = A1.FCombatData;
            {
                I = 0;
                int end__43 = Data.GetCount() - 1;
                for (; I <= end__43; I += 1)
                {
                    Obj = Data.GetObjects(I);
                    if ((Obj is Aow.Units.AoWE.TCombatUnit) && (Obj.GetPlayer() == 0))
                    {
                        AUnit = ((Aow.Units.AoWE.TCombatUnit)Obj).FUnit;
                        AUnit.AddRef();
                        ((Aow.Units.AoWE.TCombatUnit)Obj).SetUnit(null);
                        FDefenders.AddUnit(AUnit);
                        AUnit.Release();
                    }
                }
            }

            if (A1 is Aow.Units.AoWE.TFastCombat)
            {
                ExecuteSearchDone(Aow.Units.AoWE.TCombatType.ctFast, A1.FCombatResult);
            }
            else
            {
                ExecuteSearchDone(Aow.Units.AoWE.TCombatType.ctTactical, A1.FCombatResult);
            }
        }

        // ExploreS.pas:230
        public void RequestCombatTypeCallBack(Aow.Units.EventLog.TEventLog A1)
        {
            Aow.Units.AoWE.TStructureTE TE = default;
            {
                var w__44 = (Aow.Units.AoWE.TCombatTypeRequestEventLog)A1;
                if (w__44.FCombatType != Aow.Units.AoWE.TCombatType.ctNone)
                {
                    TE = CreateTE();
                    SetupTE(TE);
                    TE.FAction = 0;
                    TE.FParam = (int)w__44.FCombatType;
                    var call__45 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(TE.FPlayer);
                    call__45.AddEvent(TE);
                    TE.Release();
                }
            }
        }

        // ExploreS.pas:246
        public bool CanSearch(sbyte A1)
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            if (!GetExplored())
            {
                var call__46 = Owner.FindOwnedChild(131607);
                ArmyHS = (Aow.Units.AoWE.TArmyHS)call__46;
                if ((ArmyHS == null) || (ArmyHS.FArmy.FPlayer != A1))
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
            Aow.Units.AoWE.TUnitIndexList List = default;
            Aow.Units.AoWE.TUnitIndexListCollection Collections = default;
            Aow.Rtl.Rtl.RandSeed = Aow.Units.AoWE.AoWE.AoWHSMap.FDefendersSeed;
            if (FDefendersMode == Aow.Units.ExploreS.TDefendersMode.dmRandom)
            {
                var call__47 = Aow.Rtl.Rtl.Random(3);
                FDefendersMode = (Aow.Units.ExploreS.TDefendersMode)(call__47 + 1);
            }

            var call__48 = ((Aow.Units.ExploreS.TExplorationSiteResource)FResource).FDefenderCollections.GetRndCollection();
            Collection = call__48;
            if (Collection != -1)
            {
                List = Aow.Units.AoWE.TUnitIndexList.Create();
                Collections = ((Aow.Units.ExploreS.TExplorationSiteResource)FResource).FDefenderCollections;
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
                    Aow.Units.AoWE.AoWE.FillWithRandomUnits(List, FDefenders, 4, ((int)FDefendersMode) * 50, TSet<Aow.Units.AoWE.TUnitType>.Of(Aow.Units.AoWE.TUnitType.utHumanoid, Aow.Units.AoWE.TUnitType.utCreature, Aow.Units.AoWE.TUnitType.utMachine));
                }
                catch (System.Exception)
                {
                }

                // { Original silently ignores defender-generation failures. }
                List.Free();
            }
        }

        // ExploreS.pas:33
        public static new Aow.Units.ExploreS.TExplorationSite Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.ExploreS.TExplorationSite o = new Aow.Units.ExploreS.TExplorationSite();
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

        public new partial class Meta : Aow.Units.AoWE.TStructure.Meta
        {
            // ExploreS.pas:90
            public override int ClassID()
            {
                int Result = default;
                Result = 132033;
                return Result;
            }

            // ExploreS.pas:33
            public new Aow.Units.ExploreS.TExplorationSite Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.ExploreS.TExplorationSite o = (Aow.Units.ExploreS.TExplorationSite)NewInstance();
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

            public static new Meta Of<T>()
                where T : Aow.Units.ExploreS.TExplorationSite => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TExplorationSiteResource (instance size 104, VMT 557C16C4):
//     $64 FDefenderCollections, the available defender army compositions. }
    public partial class TExplorationSiteResource : Aow.Units.AoWE.TStructureResource
    {
        public Aow.Units.AoWE.TUnitIndexListCollection FDefenderCollections;
        // ExploreS.pas:364
        // { $64 }
        public override void Destroy()
        {
            FDefenderCollections.Free();
            base.Destroy();
        }

        // ExploreS.pas:376
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStreamProc(25, FDefenderCollections.LoadFromStream, FDefenderCollections.SaveToStream);
        }

        // ExploreS.pas:357
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__51 = Aow.Units.AoWE.TUnitIndexListCollection.Create();
            FDefenderCollections = call__51;
            FTerrainBound = false;
        }

        // ExploreS.pas:382
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132033;
            return Result;
        }

        // ExploreS.pas:61
        public static new Aow.Units.ExploreS.TExplorationSiteResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.ExploreS.TExplorationSiteResource o = new Aow.Units.ExploreS.TExplorationSiteResource();
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

        public new partial class Meta : Aow.Units.AoWE.TStructureResource.Meta
        {
            // ExploreS.pas:373
            public override int ClassID()
            {
                int Result = default;
                Result = 132034;
                return Result;
            }

            // ExploreS.pas:370
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"";
                return Result;
            }

            // ExploreS.pas:61
            public new Aow.Units.ExploreS.TExplorationSiteResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.ExploreS.TExplorationSiteResource o = (Aow.Units.ExploreS.TExplorationSiteResource)NewInstance();
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

            public static new Meta Of<T>()
                where T : Aow.Units.ExploreS.TExplorationSiteResource => (Meta)MetaRegistry.Of(typeof(T));
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
        public static void RegisterExplorationSite(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.ExploreS.TExplorationSite>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.ExploreS.TExplorationSiteResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
