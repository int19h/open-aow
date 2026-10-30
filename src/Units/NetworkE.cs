// Translated from NetworkE.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;

namespace Aow.Units.NetworkE
{
    // { NetworkE.TPlayerCommunicator (instance size 36): +8 FEngine, +$C FOnReceive, +$10
//     FReceivedPackage, +$14 FSendStream, +$18 FReceiveStream, +$1C FPCC, +$20 FOnProcess. }
    public partial class TPlayerCommunicator : Aow.Units.Engine.TEObject
    {
        internal Aow.Units.Engine.TEngine FEngine;
        internal Aow.Units.Engine.TEventList FOnReceive;
        internal Aow.Units.NetworkE.TDataPackage FReceivedPackage;
        internal Aow.Rtl.TMemoryStream FSendStream;
        internal Aow.Rtl.TMemoryStream FReceiveStream;
        internal Aow.Units.NetworkE.TPlayerCommunicationControl FPCC;
        internal Aow.Units.Engine.TEventList FOnProcess;
        // NetworkE.pas:605
        // { TPlayerCommunicator }
        public TPlayerCommunicator(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FReceivedPackage = null;
            var call__1 = new Aow.Units.Engine.TEventList();
            FOnReceive = call__1;
            var call__2 = new Aow.Rtl.TMemoryStream();
            FSendStream = call__2;
            var call__3 = new Aow.Rtl.TMemoryStream();
            FReceiveStream = call__3;
            var call__4 = new Aow.Units.Engine.TEventList();
            FOnProcess = call__4;
        }

        // NetworkE.pas:629
        public virtual bool ActivatePCC(Aow.Units.NetworkE.TPlayerCommunicationControl PCC)
        {
            bool Result = default;
            if (FPCC != null)
            {
                FPCC.Release();
                FPCC = null;
            }

            FPCC = PCC;
            if (FPCC != null)
            {
                FPCC.AddRef();
                var call__5 = FPCC.Activate(this);
                Result = call__5;
                if (!Result)
                {
                    FPCC.Release();
                    FPCC = null;
                }
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // NetworkE.pas:651
        public virtual void Process()
        {
            while (ReceiveDataPackage())
            {
                ;
            }

            FOnProcess.TriggerNotifyEvents(this);
        }

        // NetworkE.pas:615
        public override void Destroy()
        {
            FOnReceive.Free();
            FSendStream.Free();
            FReceiveStream.Free();
            FOnProcess.Free();
            if (FPCC != null)
            {
                FPCC.Release();
                FPCC = null;
            }

            base.Destroy();
        }

        // NetworkE.pas:658
        public bool ReceiveDataPackage()
        {
            bool Result = default;
            Aow.Units.Engine.TEObject Obj = default;
            FReceiveStream.Clear();
            FReceiveStream.Position = 0;
            var call__6 = FPCC.ReceivePackageData(FReceiveStream);
            if (call__6)
            {
                try
                {
                    var call__7 = FEngine.RetrieveEObject(FReceiveStream, null);
                    Obj = call__7;
                    if (Obj is Aow.Units.NetworkE.TDataPackage)
                    {
                        FReceivedPackage = (Aow.Units.NetworkE.TDataPackage)Obj;
                        FOnReceive.TriggerNotifyEvents(this);
                        FReceivedPackage.Release();
                    }
                    else
                    {
                        Obj.Free();
                    }
                }
                catch (System.Exception)
                {
                }

                FReceivedPackage = null;
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // NetworkE.pas:685
        public bool SendDataPackage(Aow.Units.NetworkE.TDataPackage Package, int ASender, int AReceiver)
        {
            bool Result = default;
            Package.FSender = ASender;
            Package.FReceiver = AReceiver;
            FSendStream.Clear();
            FEngine.StoreEObject(FSendStream, Package, null);
            FSendStream.Position = 0;
            var call__8 = FPCC.SendPackageData(AReceiver, FSendStream);
            Result = call__8;
            return Result;
        }

        public Aow.Units.Engine.TEngine Engine { get => FEngine; set => FEngine = value; }
        public Aow.Units.Engine.TEventList OnReceive { get => FOnReceive; }
        public Aow.Units.NetworkE.TDataPackage ReceivedPackage { get => FReceivedPackage; }
        public Aow.Units.NetworkE.TPlayerCommunicationControl PCC { get => FPCC; }
        public Aow.Units.Engine.TEventList OnProcess { get => FOnProcess; }

    }

    // { NetworkE.TTokenEvent (instance size 16): +8 FRefCount, +$C FTokenCounter (stored with id 10;
//     -1 until the executer numbers the event). The base of the game's token events. }
    public partial class TTokenEvent : Aow.Units.Engine.TEObject
    {
        // NetworkE.pas:950
        public new const int ClassID = Aow.Units.NetworkE.NetworkE.CID_TokenEvent;

        public TTokenEvent(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FRefCount = 1;
            FTokenCounter = -1;
        }

        internal int FRefCount;
        internal int FTokenCounter;
        // NetworkE.pas:969
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTokenCounter, 10);
        }

        // NetworkE.pas:943
        // { TTokenEvent }


        // NetworkE.pas:955
        public override int AddRef()
        {
            int Result = default;
            FRefCount += 1;
            Result = FRefCount;
            return Result;
        }

        // NetworkE.pas:961
        public override int Release()
        {
            int Result = default;
            FRefCount -= 1;
            Result = FRefCount;
            if (Result == 0)
            {
                Destroy();
            }

            return Result;
        }

        // NetworkE.pas:975
        public virtual Aow.Units.NetworkE.TExecuteResult Execute(Aow.Units.NetworkE.TTokenExecuter TE)
        {
            Aow.Units.NetworkE.TExecuteResult Result = default;
            Result = Aow.Units.NetworkE.TExecuteResult.erDone;
            return Result;
        }

        public int TokenCounter { get => FTokenCounter; set => FTokenCounter = value; }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // NetworkE.pas:950
            public override int ClassID() => Aow.Units.NetworkE.TTokenEvent.ClassID;

            // NetworkE.pas:230
            public new Aow.Units.NetworkE.TTokenEvent Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.NetworkE.TTokenEvent)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.NetworkE.TTokenEvent => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { NetworkE.TTokenEventList (instance size 12): +8 FList (stored with id 5), a reference node of
//     token events }
    public partial class TTokenEventList : Aow.Units.Engine.TEObject
    {
        // NetworkE.pas:995
        public new const int ClassID = Aow.Units.NetworkE.NetworkE.CID_TokenEventList;

        internal Aow.Units.Engine.TEReferenceNode FList;
        // NetworkE.pas:1000
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FList, 5);
        }

        // NetworkE.pas:982
        // { TTokenEventList }
        public TTokenEventList(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__9 = new Aow.Units.Engine.TEReferenceNode(null);
            FList = call__9;
        }

        // NetworkE.pas:1046
        public override void Clear()
        {
            FList.Clear();
        }

        // NetworkE.pas:1006
        public override void Assign(Aow.Units.Engine.TEObject Source, Aow.Units.Engine.TEngine AEngine)
        {
            FList.Clear();
            AddEvents((Aow.Units.NetworkE.TTokenEventList)Source);
        }

        // NetworkE.pas:988
        public override void Destroy()
        {
            Clear();
            FList.Free();
            base.Destroy();
        }

        // NetworkE.pas:1012
        public int IndexOfTokenCounter(int ATokenCounter)
        {
            int Result = default;
            var call__10 = FList.Count;
            Result = call__10;
            while (Result != 0)
            {
                Result -= 1;
                if (((Aow.Units.NetworkE.TTokenEvent)FList.FChildren.List[Result]).FTokenCounter == ATokenCounter)
                {
                    return Result;
                }
            }

            Result = -1;
            return Result;
        }

        // NetworkE.pas:1024
        public int FindTE(int AClassID)
        {
            int Result = default;
            int C = default;
            Result = 0;
            var call__11 = FList.Count;
            C = call__11;
            while (true)
            {
                bool bool__12 = (Result != C);
                if (bool__12)
                {
                    var call__13 = ((Aow.Units.NetworkE.TTokenEvent)FList.FChildren.List[Result]).EClass.ClassID;
                    bool__12 = (call__13 != AClassID);
                }

                if (!bool__12)
                    break;
                {
                    Result += 1;
                }
            }

            if (Result == C)
            {
                Result = -1;
            }

            return Result;
        }

        // NetworkE.pas:1036
        public Aow.Units.NetworkE.TTokenEvent GetEvent(int Index)
        {
            Aow.Units.NetworkE.TTokenEvent Result = default;
            Result = (Aow.Units.NetworkE.TTokenEvent)FList.FChildren.List[Index];
            return Result;
        }

        // NetworkE.pas:1041
        public int GetCount()
        {
            int Result = default;
            var call__14 = FList.Count;
            Result = call__14;
            return Result;
        }

        // NetworkE.pas:1056
        public void SortOnTokenCounter()
        {
            FList.FChildren.Sort(Aow.Units.NetworkE.NetworkE.CompareTokenCounter);
        }

        // NetworkE.pas:1061
        public void AddEvents(Aow.Units.NetworkE.TTokenEventList Events)
        {
            int I = default;
            {
                I = 0;
                int end__15 = Events.Count - 1;
                for (; I <= end__15; I += 1)
                {
                    var call__16 = Events[I];
                    FList.AddChild(call__16);
                }
            }
        }

        // NetworkE.pas:1069
        public void Add(Aow.Units.NetworkE.TTokenEvent TE)
        {
            FList.AddChild(TE);
        }

        // NetworkE.pas:1074
        public void Delete(int Index)
        {
            var call__17 = FList.Children[Index];
            FList.RemoveChild(call__17);
        }

        // NetworkE.pas:1080
        // { Inserts without TEReferenceNode.AddChild, so the reference is taken here }
        public void Insert(int Index, Aow.Units.NetworkE.TTokenEvent TE)
        {
            TE.AddRef();
            FList.FChildren.Insert(Index, TE);
        }

        public int Count { get => GetCount(); }

        public Aow.Units.NetworkE.TTokenEvent this[int Index] { get => GetEvent(Index); }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // NetworkE.pas:995
            public override int ClassID() => Aow.Units.NetworkE.TTokenEventList.ClassID;

            // NetworkE.pas:257
            public new Aow.Units.NetworkE.TTokenEventList Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.NetworkE.TTokenEventList)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.NetworkE.TTokenEventList => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { NetworkE.TTokenControl (instance size 20): +4 FEvents, +8 FTokenManager, +$C FPlayer, +$10
//     FActive. The token events of one player. }
    public partial class TTokenControl : Aow.Rtl.TObject
    {
        internal Aow.Units.NetworkE.TTokenEventList FEvents;
        internal Aow.Units.NetworkE.TTokenManager FTokenManager;
        internal int FPlayer;
        internal bool FActive;
        // NetworkE.pas:1189
        // { Keeps the events sorted on their token counters; an event without one goes to the end }
        public virtual void Add(Aow.Units.NetworkE.TTokenEvent TE)
        {
            int I = default;
            if (TE.FTokenCounter != -1)
            {
                I = 0;
                while (true)
                {
                    var call__18 = FEvents.Count;
                    bool bool__19 = (I != call__18);
                    if (bool__19)
                    {
                        var call__20 = FEvents[I];
                        bool__19 = (call__20.FTokenCounter < TE.FTokenCounter);
                    }

                    if (!bool__19)
                        break;
                    {
                        I += 1;
                    }
                }

                var call__21 = FEvents.Count;
                if (I < call__21)
                {
                    var call__22 = FEvents[I];
                    if (call__22.FTokenCounter == TE.FTokenCounter)
                    {
                        throw Aow.Rtl.Exception.Create((AnsiString)"Double TokenCounter defined");
                    }

                    FEvents.Insert(I, TE);
                }
                else
                {
                    FEvents.Add(TE);
                }
            }
            else
            {
                FEvents.Add(TE);
            }
        }

        // NetworkE.pas:1293
        // { A local player's events end with a TPassTokenTE (after a TSyncTE when the executer checks the
//   sync); a remote player's events are prefetched only when they are complete }
        public virtual bool PrefetchTokenEvents(Aow.Units.NetworkE.TTokenExecuter Executer)
        {
            bool Result = default;
            int I = default;
            int N = default;
            Aow.Units.NetworkE.TTokenEvent TE = default;
            var call__23 = Global();
            bool bool__24 = call__23;
            if (!bool__24)
            {
                var call__25 = Local();
                bool__24 = call__25;
            }

            if (bool__24)
            {
                var call__26 = FEvents.Count;
                if ((call__26 != 0) || (Executer.FHoldTC != true))
                {
                    if (Local())
                    {
                        if (Executer.FOnSyncError != null)
                        {
                            TE = new Aow.Units.NetworkE.TSyncTE(null);
                            var call__27 = Executer.GetSyncValue();
                            ((Aow.Units.NetworkE.TSyncTE)TE).FSyncValue = call__27;
                            FEvents.Insert(0, TE);
                            TE.Release();
                        }

                        TE = new Aow.Units.NetworkE.TPassTokenTE(null);
                        FEvents.Add(TE);
                        TE.Release();
                    }

                    {
                        I = 0;
                        int end__28 = FEvents.Count - 1;
                        for (; I <= end__28; I += 1)
                        {
                            var call__29 = FEvents[I];
                            Executer.PrefetchTokenEvent(call__29);
                        }
                    }

                    FEvents.Clear();
                    Result = true;
                }
                else
                {
                    Result = true;
                }
            }
            else
            {
                if (Remote())
                {
                    N = GetPrefetchableTokenEvents(Executer);
                    if (N > 0)
                    {
                        {
                            I = 0;
                            int end__30 = N - 1;
                            for (; I <= end__30; I += 1)
                            {
                                var call__31 = FEvents[I];
                                Executer.PrefetchTokenEvent(call__31);
                            }
                        }

                        {
                            I = N - 1;
                            byte end__32 = 0;
                            for (; I >= end__32; I -= 1)
                            {
                                FEvents.Delete(I);
                            }
                        }

                        Result = true;
                    }
                    else
                    {
                        Result = false;
                    }
                }
                else
                {
                    Result = true;
                }
            }

            return Result;
        }

        // NetworkE.pas:1342
        // { A local player's prefetched events go to every other player }
        public virtual void TokenEventsPrefetched(Aow.Units.NetworkE.TTokenExecuter Executer, Aow.Units.NetworkE.TTokenEventList Events)
        {
            var call__33 = Local();
            bool bool__34 = call__33;
            if (bool__34)
            {
                var call__35 = Global();
                bool__34 = !call__35;
            }

            bool bool__36 = bool__34;
            if (bool__36)
            {
                var call__37 = Events.Count;
                bool__36 = (call__37 != 0);
            }

            if (bool__36)
            {
                FTokenManager.FTokenPackageDistributer.SendTokenEvents(FPlayer, Aow.Units.NetworkE.NetworkE.AllPlayers, Events);
            }
        }

        // NetworkE.pas:1169
        // { TTokenControl }
        public TTokenControl(Aow.Units.NetworkE.TTokenManager AManager, int APlayer)
        {
            var call__38 = new Aow.Units.NetworkE.TTokenEventList(null);
            FEvents = call__38;
            FTokenManager = AManager;
            FPlayer = APlayer;
            FActive = true;
            if ((FTokenManager != null) && (FTokenManager.FTokenExecuter != null))
            {
                FTokenManager.FTokenExecuter.AddTC(this);
            }
        }

        // NetworkE.pas:1211
        public virtual void AddEvent(Aow.Units.NetworkE.TTokenEvent TE)
        {
            if (Remote())
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"Cannot Add Remote events");
            }

            Add(TE);
        }

        // NetworkE.pas:1218
        public virtual void AddEventEx(Aow.Units.NetworkE.TTokenEvent TE)
        {
            Add(TE);
        }

        // NetworkE.pas:1238
        public virtual void AddEvents(Aow.Units.NetworkE.TTokenEventList Events)
        {
            int I = default;
            {
                I = 0;
                int end__39 = Events.Count - 1;
                for (; I <= end__39; I += 1)
                {
                    var call__40 = Events[I];
                    AddEvent(call__40);
                }
            }
        }

        // NetworkE.pas:1180
        public override void Destroy()
        {
            if ((FTokenManager != null) && (FTokenManager.FTokenExecuter != null))
            {
                FTokenManager.FTokenExecuter.RemoveTC(this);
            }

            FEvents.Free();
            base.Destroy();
        }

        // NetworkE.pas:1223
        public void ClearEvents()
        {
            FEvents.Clear();
        }

        // NetworkE.pas:1228
        public Aow.Units.NetworkE.TTokenEvent GetEvent(int Index)
        {
            Aow.Units.NetworkE.TTokenEvent Result = default;
            var call__41 = FEvents[Index];
            Result = call__41;
            return Result;
        }

        // NetworkE.pas:1233
        public int GetCount()
        {
            int Result = default;
            var call__42 = FEvents.Count;
            Result = call__42;
            return Result;
        }

        // NetworkE.pas:1246
        public bool Local()
        {
            bool Result = default;
            var playerCommunicationControl = FTokenManager.FTokenPackageDistributer.FPC.FPCC;
            var call__44 = playerCommunicationControl.GetPlayers(FPlayer);
            Result = call__44.FConnectionMode == Aow.Units.NetworkE.TConnectionMode.cmLocal;

            return Result;
        }

        // NetworkE.pas:1252
        public bool Global()
        {
            bool Result = default;
            var playerCommunicationControl = FTokenManager.FTokenPackageDistributer.FPC.FPCC;
            var call__46 = playerCommunicationControl.GetPlayers(FPlayer);
            Result = call__46.FConnectionMode == Aow.Units.NetworkE.TConnectionMode.cmGlobal;

            return Result;
        }

        // NetworkE.pas:1258
        public bool Remote()
        {
            bool Result = default;
            var playerCommunicationControl = FTokenManager.FTokenPackageDistributer.FPC.FPCC;
            var call__48 = playerCommunicationControl.GetPlayers(FPlayer);
            Result = call__48.FConnectionMode == Aow.Units.NetworkE.TConnectionMode.cmRemote;

            return Result;
        }

        // NetworkE.pas:1266
        // { The number of events of a remote player that can be executed now: they must follow the
//   executer's token counter without a gap, up to and including the first TPassTokenTE }
        public int GetPrefetchableTokenEvents(Aow.Units.NetworkE.TTokenExecuter Executer)
        {
            int Result = default;
            int I = default;
            bool InOrder = default;
            bool PassToken = default;
            if (Remote())
            {
                I = 0;
                InOrder = true;
                PassToken = false;
                while (true)
                {
                    bool bool__49 = InOrder && !PassToken;
                    if (bool__49)
                    {
                        var call__50 = FEvents.Count;
                        bool__49 = (I < call__50);
                    }

                    if (!bool__49)
                        break;
                    {
                        var call__51 = FEvents[I];
                        InOrder = call__51.FTokenCounter == (Executer.FTokenCounter + I);
                        var call__52 = FEvents[I];
                        var call__53 = call__52.EClass.ClassID;
                        PassToken = call__53 == Aow.Units.NetworkE.NetworkE.CID_PassTokenTE;
                        I += 1;
                    }
                }

                if (InOrder)
                {
                    Result = I;
                }
                else
                {
                    Result = 0;
                }
            }
            else
            {
                var call__54 = FEvents.Count;
                Result = call__54;
            }

            return Result;
        }

        public Aow.Units.NetworkE.TTokenEventList Events { get => FEvents; }
        public int Player { get => FPlayer; }
        public bool Active { get => FActive; set => FActive = value; }
        public int Count { get => GetCount(); }

    }

    // { NetworkE.TTokenExecuter (instance size 92): +4 FActiveTC, +8 FPrefetchTC, +$C FActiveTCIndex,
//     +$10 FEvents, +$14 FTCList, +$18 FTokenCounter, +$1C FRound, +$20 FExecuteCount, +$24 FState,
//     +$28 FOnEventsExecuted, +$2C FOnActivateTC, +$30 FOnSyncError, +$38 FOnGetSyncValue, +$40
//     FOnExecuteTokenEvent, +$48 FExecuting, +$4C FLockCount, +$50 FSystemEvents, +$54 FHoldTC,
//     +$58 FTokenManager. }
    public partial class TTokenExecuter : Aow.Rtl.TObject
    {
        internal Aow.Units.NetworkE.TTokenControl FActiveTC;
        internal Aow.Units.NetworkE.TTokenControl FPrefetchTC;
        internal int FActiveTCIndex;
        internal Aow.Units.NetworkE.TTokenEventList FEvents;
        internal Aow.Rtl.TList FTCList;
        internal int FTokenCounter;
        internal int FRound;
        internal int FExecuteCount;
        internal TSet<Aow.Units.NetworkE.TTokenExecuterState> FState;
        internal Aow.Units.Engine.TEventList FOnEventsExecuted;
        internal Aow.Units.Engine.TEventList FOnActivateTC;
        internal Aow.Rtl.TNotifyEvent FOnSyncError;
        internal Aow.Units.NetworkE.TGetSyncValueEvent FOnGetSyncValue;
        internal Aow.Units.NetworkE.TTokenEventEvent FOnExecuteTokenEvent;
        internal bool FExecuting;
        internal int FLockCount;
        internal Aow.Units.NetworkE.TTokenEventList FSystemEvents;
        internal bool FHoldTC;
        internal Aow.Units.NetworkE.TTokenManager FTokenManager;
        // NetworkE.pas:1429
        public virtual Aow.Units.NetworkE.TExecuteResult ExecuteTokenEvent(Aow.Units.NetworkE.TTokenEvent TE)
        {
            Aow.Units.NetworkE.TExecuteResult Result = default;
            FExecuting = true;
            if (FOnExecuteTokenEvent != null)
            {
                FOnExecuteTokenEvent(TE);
            }

            try
            {
                Result = TE.Execute(this);
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.ShowMessage(((AnsiString)"Exception occured during ") + ((AnsiString)TObject.ClassNameOf(TE.GetType())));
                Result = Aow.Units.NetworkE.TExecuteResult.erDone;
            }

            FExecuteCount += 1;
            FExecuting = false;
            return Result;
        }

        // NetworkE.pas:1350
        // { TTokenExecuter }
        public TTokenExecuter(Aow.Units.NetworkE.TTokenManager AManager)
        {
            FTokenManager = AManager;
            var call__55 = new Aow.Units.NetworkE.TTokenEventList(null);
            FEvents = call__55;
            var call__56 = new Aow.Rtl.TList();
            FTCList = call__56;
            var call__57 = new Aow.Units.Engine.TEventList();
            FOnEventsExecuted = call__57;
            var call__58 = new Aow.Units.Engine.TEventList();
            FOnActivateTC = call__58;
            var call__59 = new Aow.Units.NetworkE.TTokenEventList(null);
            FSystemEvents = call__59;
        }

        // NetworkE.pas:1361
        public override void Destroy()
        {
            Reset();
            FEvents.Free();
            FTCList.Free();
            FOnEventsExecuted.Free();
            FOnActivateTC.Free();
            FSystemEvents.Free();
            base.Destroy();
        }

        // NetworkE.pas:1372
        public int GetSyncValue()
        {
            int Result = default;
            if (FOnGetSyncValue != null)
            {
                FOnGetSyncValue(ref Result);
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // NetworkE.pas:1380
        public Aow.Units.NetworkE.TTokenControl GetTC(int Index)
        {
            Aow.Units.NetworkE.TTokenControl Result = default;
            Result = (Aow.Units.NetworkE.TTokenControl)FTCList.List[Index];
            return Result;
        }

        // NetworkE.pas:1385
        public void AddTC(Aow.Units.NetworkE.TTokenControl TC)
        {
            FTCList.Add(TC);
        }

        // NetworkE.pas:1390
        public void DeleteTC(int Index)
        {
            FTCList.Delete(Index);
        }

        // NetworkE.pas:1395
        public void RemoveTC(Aow.Units.NetworkE.TTokenControl TC)
        {
            FTCList.Remove(TC);
        }

        // NetworkE.pas:1400
        public void InsertTC(int Index, Aow.Units.NetworkE.TTokenControl TC)
        {
            FTCList.Insert(Index, TC);
        }

        // NetworkE.pas:1405
        public void Reset()
        {
            FTCList.Clear();
            FActiveTC = null;
            FActiveTCIndex = -1;
            FState = TSet<Aow.Units.NetworkE.TTokenExecuterState>.Of();
            FRound = 0;
            FTokenCounter = 0;
        }

        // NetworkE.pas:1416
        // { Numbers an event that has no token counter yet (or an old one) and queues it }
        public void PrefetchTokenEvent(Aow.Units.NetworkE.TTokenEvent TE)
        {
            if (FState.Contains(Aow.Units.NetworkE.TTokenExecuterState.tsPrefetching))
            {
                if ((TE.FTokenCounter >= FTokenCounter) || (TE.FTokenCounter == -1))
                {
                    var call__60 = FEvents.Count;
                    TE.FTokenCounter = call__60 + FTokenCounter;
                    FEvents.Add(TE);
                }

                FExecuteCount = 0;
            }
        }

        // NetworkE.pas:1444
        public Aow.Units.NetworkE.TTokenEvent GetExecutingTokenEvent()
        {
            Aow.Units.NetworkE.TTokenEvent Result = default;
            bool bool__61 = FState.Contains(Aow.Units.NetworkE.TTokenExecuterState.tsExecuting);
            if (bool__61)
            {
                var call__62 = FEvents.Count;
                bool__61 = (call__62 != 0);
            }

            if (bool__61)
            {
                var call__63 = FEvents[0];
                Result = call__63;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // NetworkE.pas:1452
        public bool InsertExecuteEvent(Aow.Units.NetworkE.TTokenEvent TE)
        {
            bool Result = default;
            if (FState.Contains(Aow.Units.NetworkE.TTokenExecuterState.tsExecuting))
            {
                FEvents.Add(TE);
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // NetworkE.pas:1463
        public void Initialize(int AIndex)
        {
            FState = TSet<Aow.Units.NetworkE.TTokenExecuterState>.Of();
            if (AIndex < FTCList.Count)
            {
                var call__64 = FTCList[AIndex];
                FActiveTC = (Aow.Units.NetworkE.TTokenControl)call__64;
                FActiveTCIndex = AIndex;
                FRound = 0;
            }
        }

        // NetworkE.pas:1474
        public void ActivateTC(int Index)
        {
            var call__65 = FTCList[Index];
            FActiveTC = (Aow.Units.NetworkE.TTokenControl)call__65;
            FActiveTC.FActive = true;
            FActiveTCIndex = Index;
            FOnActivateTC.TriggerNotifyEvents(this);
            if (FState.Contains(Aow.Units.NetworkE.TTokenExecuterState.tsExecuting))
            {
                FState = FState + TSet<Aow.Units.NetworkE.TTokenExecuterState>.Of(Aow.Units.NetworkE.TTokenExecuterState.tsTCActivated);
            }
        }

        // NetworkE.pas:1485
        // { The next active token control after the current one; a wrap starts the next round }
        public void ActivateNextTC()
        {
            int N = default;
            N = 0;
            while (true)
            {
                {
                    FActiveTCIndex += 1;
                    N += 1;
                    if (FTCList.Count <= FActiveTCIndex)
                    {
                        FActiveTCIndex = 0;
                        FRound += 1;
                    }
                }

                {
                    var call__66 = FTCList[FActiveTCIndex];
                    if (((Aow.Units.NetworkE.TTokenControl)call__66).FActive || (N > FTCList.Count))
                        break;
                }
            }

            var call__68 = FTCList[FActiveTCIndex];
            if (((Aow.Units.NetworkE.TTokenControl)call__68).FActive)
            {
                ActivateTC(FActiveTCIndex);
            }
        }

        // NetworkE.pas:1503
        public void Lock()
        {
            FLockCount += 1;
        }

        // NetworkE.pas:1508
        public void Unlock()
        {
            FLockCount -= 1;
        }

        // NetworkE.pas:1513
        public bool GetLocked()
        {
            bool Result = default;
            Result = FLockCount != 0;
            return Result;
        }

        // NetworkE.pas:1519
        // { The system events run between the token controls' events; True while some remain }
        public bool ProcessSystemTokenEvents()
        {
            bool Result = default;
            Aow.Units.NetworkE.TExecuteResult R = default;
            bool bool__69 = !FState.Contains(Aow.Units.NetworkE.TTokenExecuterState.tsExecuting);
            if (bool__69)
            {
                var call__70 = FSystemEvents.Count;
                bool__69 = (call__70 > 0);
            }

            if (bool__69 && (FLockCount == 0))
            {
                var call__71 = FSystemEvents[0];
                if (call__71.FTokenCounter == -1)
                {
                    FExecuteCount = 0;
                    var call__72 = FSystemEvents[0];
                    call__72.FTokenCounter = FTokenCounter;
                }

                var call__73 = FSystemEvents[0];
                var call__74 = ExecuteTokenEvent(call__73);
                R = call__74;
                if ((R == Aow.Units.NetworkE.TExecuteResult.erDone) || (R == Aow.Units.NetworkE.TExecuteResult.erDoneHalt))
                {
                    FSystemEvents.Delete(0);
                    FTokenCounter += 1;
                    FExecuteCount = 0;
                }

                var call__75 = FSystemEvents.Count;
                Result = call__75 != 0;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // NetworkE.pas:1543
        public void AddSystemEvent(Aow.Units.NetworkE.TTokenEvent TE)
        {
            FSystemEvents.Add(TE);
        }

        // NetworkE.pas:1550
        // { Lets the active token control prefetch its events, executes them and activates the next token
//   control, for at most one round of token controls }
        public void Process()
        {
            int I = default;
            bool Prefetched = default;
            Aow.Units.NetworkE.TExecuteResult R = default;
            bool bool__76 = (FActiveTC != null);
            if (bool__76)
            {
                var call__77 = ProcessSystemTokenEvents();
                bool__76 = !call__77;
            }

            if (bool__76)
            {
                I = 0;
                Prefetched = true;
                R = Aow.Units.NetworkE.TExecuteResult.erDone;
                while ((((FLockCount == 0) && (Prefetched == true)) && (R == Aow.Units.NetworkE.TExecuteResult.erDone)) && (I < FTCList.Count))
                {
                    if (!FState.Contains(Aow.Units.NetworkE.TTokenExecuterState.tsExecuting))
                    {
                        FState = FState + TSet<Aow.Units.NetworkE.TTokenExecuterState>.Of(Aow.Units.NetworkE.TTokenExecuterState.tsPrefetching);
                        FEvents.Clear();
                        var call__78 = FActiveTC.PrefetchTokenEvents(this);
                        Prefetched = call__78;
                        FState = FState - TSet<Aow.Units.NetworkE.TTokenExecuterState>.Of(Aow.Units.NetworkE.TTokenExecuterState.tsPrefetching);
                        if (Prefetched == true)
                        {
                            FState = FState + TSet<Aow.Units.NetworkE.TTokenExecuterState>.Of(Aow.Units.NetworkE.TTokenExecuterState.tsExecuting);
                            FPrefetchTC = FActiveTC;
                            FActiveTC.TokenEventsPrefetched(this, FEvents);
                        }
                    }

                    if (FState.Contains(Aow.Units.NetworkE.TTokenExecuterState.tsExecuting))
                    {
                        while (true)
                        {
                            bool bool__79 = (R == Aow.Units.NetworkE.TExecuteResult.erDone);
                            if (bool__79)
                            {
                                var call__80 = FEvents.Count;
                                bool__79 = (call__80 != 0);
                            }

                            if (!bool__79)
                                break;
                            {
                                var call__81 = FEvents[0];
                                var call__82 = ExecuteTokenEvent(call__81);
                                R = call__82;
                                if ((R == Aow.Units.NetworkE.TExecuteResult.erDone) || (R == Aow.Units.NetworkE.TExecuteResult.erDoneHalt))
                                {
                                    FEvents.Delete(0);
                                    FTokenCounter += 1;
                                    FExecuteCount = 0;
                                }
                            }
                        }

                        var call__83 = FEvents.Count;
                        if (call__83 == 0)
                        {
                            FState = FState - TSet<Aow.Units.NetworkE.TTokenExecuterState>.Of(Aow.Units.NetworkE.TTokenExecuterState.tsExecuting);
                            FOnEventsExecuted.TriggerNotifyEvents(this);
                            if (!FHoldTC && !FState.Contains(Aow.Units.NetworkE.TTokenExecuterState.tsTCActivated))
                            {
                                ActivateNextTC();
                            }

                            FState = FState - TSet<Aow.Units.NetworkE.TTokenExecuterState>.Of(Aow.Units.NetworkE.TTokenExecuterState.tsTCActivated);
                            FPrefetchTC = null;
                            I += 1;
                        }
                    }
                }
            }
        }

        public Aow.Units.NetworkE.TTokenControl ActiveTC { get => FActiveTC; }
        public Aow.Units.NetworkE.TTokenControl PrefetchTC { get => FPrefetchTC; }
        public int ActiveTCIndex { get => FActiveTCIndex; }
        public int TokenCounter { get => FTokenCounter; }
        public int Round { get => FRound; }
        public int ExecuteCount { get => FExecuteCount; }
        public TSet<Aow.Units.NetworkE.TTokenExecuterState> State { get => FState; }
        public bool Locked { get => GetLocked(); }
        public bool HoldTC { get => FHoldTC; set => FHoldTC = value; }
        public bool Executing { get => FExecuting; }
        public Aow.Units.Engine.TEventList OnEventsExecuted { get => FOnEventsExecuted; }
        public Aow.Units.Engine.TEventList OnActivateTC { get => FOnActivateTC; }
        public Aow.Rtl.TNotifyEvent OnSyncError { get => FOnSyncError; set => FOnSyncError = value; }
        public Aow.Units.NetworkE.TGetSyncValueEvent OnGetSyncValue { get => FOnGetSyncValue; set => FOnGetSyncValue = value; }
        public Aow.Units.NetworkE.TTokenEventEvent OnExecuteTokenEvent { get => FOnExecuteTokenEvent; set => FOnExecuteTokenEvent = value; }

    }

    // { NetworkE.TTokenManager (instance size 28): +4 FTokenExecuter, +8 FTokenPackageDistributer, +$C
//     FOwner, +$10 FTokenControls (one per player, nil for a free slot), +$14 FOnMessage. }
    public partial class TTokenManager : Aow.Rtl.TObject
    {

        public TTokenManager(Aow.Rtl.TObject AOwner)
        {
            FOwner = AOwner;
            var call__102 = new Aow.Rtl.TList();
            FTokenControls = call__102;
            var call__103 = CreateTokenExecuter();
            FTokenExecuter = call__103;
            var call__104 = CreateTokenPackageDistributer();
            FTokenPackageDistributer = call__104;
        }

        internal Aow.Units.NetworkE.TTokenExecuter FTokenExecuter;
        internal Aow.Units.NetworkE.TTokenPackageDistributer FTokenPackageDistributer;
        internal Aow.Rtl.TObject FOwner;
        internal Aow.Rtl.TList FTokenControls;
        internal Aow.Units.NetworkE.TNetworkMessageEvent FOnMessage;
        // NetworkE.pas:1642
        public virtual Aow.Units.NetworkE.TTokenExecuter CreateTokenExecuter()
        {
            Aow.Units.NetworkE.TTokenExecuter Result = default;
            Result = new Aow.Units.NetworkE.TTokenExecuter(this);
            return Result;
        }

        // NetworkE.pas:1647
        public virtual Aow.Units.NetworkE.TTokenPackageDistributer CreateTokenPackageDistributer()
        {
            Aow.Units.NetworkE.TTokenPackageDistributer Result = default;
            Result = new Aow.Units.NetworkE.TTokenPackageDistributer(this);
            return Result;
        }

        // NetworkE.pas:1652
        public virtual Aow.Units.NetworkE.TTokenControl CreateTokenControl(int APlayer)
        {
            Aow.Units.NetworkE.TTokenControl Result = default;
            Result = new Aow.Units.NetworkE.TTokenControl(this, APlayer);
            return Result;
        }

        // NetworkE.pas:1628
        public virtual void Initialize(Aow.Units.NetworkE.TPlayerCommunicator PC)
        {
            int I = default;
            FTokenPackageDistributer.SetPC(PC);
            var call__84 = PC.FPCC.GetCount();
            if (call__84 > 0)
            {
                ClearPlayers();
                {
                    I = 0;
                    int end__85 = PC.FPCC.GetCount() - 1;
                    for (; I <= end__85; I += 1)
                    {
                        CreatePlayer(I);
                    }
                }
            }

            FTokenExecuter.Initialize(0);
        }

        // NetworkE.pas:1657
        public virtual void Process()
        {
            if (FTokenExecuter != null)
            {
                FTokenExecuter.Process();
            }
        }

        // NetworkE.pas:1694
        // { A player is ready when nothing of his waits: no system events, no token events, and his events
//   are not being executed }
        public virtual bool PlayerReady(int APlayer)
        {
            bool Result = default;
            bool bool__86 = (APlayer < FTokenControls.Count);
            if (bool__86)
            {
                var call__87 = FTokenControls[APlayer];
                bool__86 = (call__87 != null);
            }

            if (bool__86)
            {
                var call__88 = FTokenExecuter.FSystemEvents.Count;
                bool bool__89 = (call__88 != 0);
                if (!bool__89)
                {
                    var call__90 = FTokenControls[APlayer];
                    var call__91 = ((Aow.Units.NetworkE.TTokenControl)call__90).GetCount();
                    bool__89 = (call__91 != 0);
                }

                bool bool__92 = bool__89;
                if (!bool__92)
                {
                    var call__93 = FTokenControls[APlayer];
                    bool__92 = ((FTokenExecuter.FPrefetchTC == call__93) && FTokenExecuter.FState.Contains(Aow.Units.NetworkE.TTokenExecuterState.tsExecuting));
                }

                if (bool__92)
                {
                    Result = false;
                }
                else
                {
                    Result = true;
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // NetworkE.pas:1663
        public virtual void CreatePlayer(int APlayer)
        {
            bool bool__94 = (APlayer >= FTokenControls.Count);
            if (!bool__94)
            {
                var call__95 = FTokenControls[APlayer];
                bool__94 = (call__95 == null);
            }

            if (bool__94)
            {
                while (APlayer >= FTokenControls.Count)
                {
                    FTokenControls.Add(null);
                }

                var call__96 = CreateTokenControl(APlayer);
                FTokenControls[APlayer] = call__96;
            }
        }

        // NetworkE.pas:1673
        public virtual void DeletePlayer(int APlayer)
        {
            bool bool__97 = (APlayer < FTokenControls.Count);
            if (bool__97)
            {
                var call__98 = FTokenControls[APlayer];
                bool__97 = (call__98 != null);
            }

            if (bool__97)
            {
                var call__99 = FTokenControls[APlayer];
                ((Aow.Rtl.TObject)call__99).Free();
                FTokenControls[APlayer] = null;
            }
        }

        // NetworkE.pas:1682
        public virtual void ClearPlayers()
        {
            while (FTokenControls.Count != 0)
            {
                var call__100 = FTokenControls[FTokenControls.Count - 1];
                if (call__100 != null)
                {
                    var call__101 = FTokenControls[FTokenControls.Count - 1];
                    ((Aow.Rtl.TObject)call__101).Free();
                }

                FTokenControls.Delete(FTokenControls.Count - 1);
            }
        }

        // NetworkE.pas:1606
        // { TTokenManager }
        // { The original does not call the inherited constructor. }


        // NetworkE.pas:1614
        public override void Destroy()
        {
            ClearPlayers();
            FTokenControls.Free();
            FTokenExecuter.Free();
            FTokenPackageDistributer.Free();
            base.Destroy();
        }

        // NetworkE.pas:1623
        public int GetCount()
        {
            int Result = default;
            Result = FTokenControls.Count;
            return Result;
        }

        // NetworkE.pas:1710
        public Aow.Units.NetworkE.TTokenControl GetTokenControl(int APlayer)
        {
            Aow.Units.NetworkE.TTokenControl Result = default;
            if (APlayer < FTokenControls.Count)
            {
                var call__105 = FTokenControls[APlayer];
                Result = (Aow.Units.NetworkE.TTokenControl)call__105;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // NetworkE.pas:1718
        public void TriggerMessage(Aow.Rtl.AnsiString Msg)
        {
            if (FOnMessage != null)
            {
                FOnMessage(this, Msg);
            }
        }

        public Aow.Units.NetworkE.TTokenExecuter TokenExecuter { get => FTokenExecuter; }
        public Aow.Units.NetworkE.TTokenPackageDistributer TokenPackageDistributer { get => FTokenPackageDistributer; }
        public Aow.Rtl.TObject Owner { get => FOwner; }
        public int Count { get => GetCount(); }

        public sealed class TokenControlsAccessor(Aow.Units.NetworkE.TTokenManager target)
        {
            public Aow.Units.NetworkE.TTokenControl this[int APlayer] { get => target.GetTokenControl(APlayer); }
        }

        public TokenControlsAccessor TokenControls => new TokenControlsAccessor(this);
        public Aow.Units.NetworkE.TNetworkMessageEvent OnMessage { get => FOnMessage; set => FOnMessage = value; }

    }

    public enum TConnectionMode : byte
    {
        cmLocal = 0,
        cmRemote = 1,
        cmGlobal = 2
    }

    public enum TExecuteResult : byte
    {
        erRetry = 0,
        erDone = 1,
        erDoneHalt = 2
    }

    public enum TTokenExecuterState : byte
    {
        tsPrefetching = 0,
        tsExecuting = 1,
        tsTCActivated = 2
    }

    public delegate bool TSendDataEvent(object Data, int Size, int SystemID);
    public delegate void TGetSyncValueEvent(ref int Value);
    public delegate void TTokenEventEvent(Aow.Units.NetworkE.TTokenEvent TE);
    public delegate void TNetworkMessageEvent(Aow.Rtl.TObject Sender, Aow.Rtl.AnsiString Msg);
    // { The original reaches TENode's child list (Engine.TENode.FChildren, +8) from this unit; the
//     recovered Engine.pas keeps the field private, so this layout view of TENode reads it (a
//     typecast of a field or parameter compiles to no code) }
    public partial class TENodeView : Aow.Units.Engine.TECustomNode
    {
        public TENodeView(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        public Aow.Units.Engine.TEObjectList FChildren;
    }

    // { NetworkE.TDataPackage (instance size 20): +8 FSender, +$C FReceiver (player indices, stored
//     with ids 1 and 2), +$10 FRefCount. The base of the packages that TPlayerCommunicator sends. }
    public partial class TDataPackage : Aow.Units.Engine.TEObject
    {
        internal int FSender;
        internal int FReceiver;
        internal int FRefCount;
        // NetworkE.pas:592
        // { The original does not call the inherited ReadWrite. }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            Stream.rwInteger(ref FSender, 1);
            Stream.rwInteger(ref FReceiver, 2);
        }

        // NetworkE.pas:598
        public override void MainReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            ReadWrite(Stream);
        }

        // NetworkE.pas:566
        // { TDataPackage }
        public TDataPackage(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FRefCount = 1;
        }

        // NetworkE.pas:572
        public override int AddRef()
        {
            int Result = default;
            FRefCount += 1;
            Result = FRefCount;
            return Result;
        }

        // NetworkE.pas:578
        public override int Release()
        {
            int Result = default;
            FRefCount -= 1;
            Result = FRefCount;
            if (Result == 0)
            {
                Destroy();
            }

            return Result;
        }

        // NetworkE.pas:586
        public virtual Aow.Rtl.AnsiString Name()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)TObject.ClassNameOf(this.GetType());
            return Result;
        }

        public int Sender { get => FSender; set => FSender = value; }
        public int Receiver { get => FReceiver; set => FReceiver = value; }

    }

    // { NetworkE.TPlayerCommunicationSettings (instance size 16): +8 FConnectionMode (stored with id 5),
//     +$C FSystemID (id 6). One per player. }
    public partial class TPlayerCommunicationSettings : Aow.Units.Engine.TEObject
    {
        internal Aow.Units.NetworkE.TConnectionMode FConnectionMode;
        internal int FSystemID;
        // NetworkE.pas:452
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FConnectionMode, 1), 1, 5);
            Stream.rwInteger(ref FSystemID, 6);
        }

        // NetworkE.pas:446
        // { TPlayerCommunicationSettings }
        public TPlayerCommunicationSettings(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FConnectionMode = Aow.Units.NetworkE.TConnectionMode.cmLocal;
        }

        public Aow.Units.NetworkE.TConnectionMode ConnectionMode { get => FConnectionMode; set => FConnectionMode = value; }
        public int SystemID { get => FSystemID; set => FSystemID = value; }

    }

    // { NetworkE.TPlayerCommunicationSettingsList (instance size 12): the node that owns the players'
//     settings }
    public partial class TPlayerCommunicationSettingsList : Aow.Units.Engine.TENode
    {
        public TPlayerCommunicationSettingsList(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // NetworkE.pas:461
        // { TPlayerCommunicationSettingsList }
        public Aow.Units.NetworkE.TPlayerCommunicationSettings GetPlayers(int Index)
        {
            Aow.Units.NetworkE.TPlayerCommunicationSettings Result = default;
            Result = (Aow.Units.NetworkE.TPlayerCommunicationSettings)this.FChildren.List[Index];
            return Result;
        }

        public sealed class PlayersAccessor(Aow.Units.NetworkE.TPlayerCommunicationSettingsList target)
        {
            public Aow.Units.NetworkE.TPlayerCommunicationSettings this[int Index] { get => target.GetPlayers(Index); }
        }

        public PlayersAccessor Players => new PlayersAccessor(this);

    }

    // { NetworkE.TPlayerCommunicationControl (instance size 16): +4 FRefCount, +8 FPlayerCommunicator,
//     +$C FPCSList. The base player communication control: one system, no transport. }
    public partial class TPlayerCommunicationControl : Aow.Rtl.TObject
    {
        internal int FRefCount;
        internal Aow.Units.NetworkE.TPlayerCommunicator FPlayerCommunicator;
        internal Aow.Units.NetworkE.TPlayerCommunicationSettingsList FPCSList;
        // NetworkE.pas:549
        public virtual int GetLocalSystemID()
        {
            int Result = default;
            Result = 1;
            return Result;
        }

        // NetworkE.pas:532
        public virtual int GetSystemCount()
        {
            int Result = default;
            Result = 1;
            return Result;
        }

        // NetworkE.pas:516
        public virtual bool Activate(Aow.Units.NetworkE.TPlayerCommunicator PC)
        {
            bool Result = default;
            FPlayerCommunicator = PC;
            Result = true;
            return Result;
        }

        // NetworkE.pas:498
        public virtual Aow.Units.NetworkE.TPlayerCommunicationSettings CreatePCS()
        {
            Aow.Units.NetworkE.TPlayerCommunicationSettings Result = default;
            Result = new Aow.Units.NetworkE.TPlayerCommunicationSettings(null);
            return Result;
        }

        // NetworkE.pas:493
        public virtual Aow.Rtl.AnsiString Name()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"";
            return Result;
        }

        // NetworkE.pas:554
        public virtual bool ReceivePackageData(Aow.Rtl.TMemoryStream Stream)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // NetworkE.pas:559
        public virtual bool SendPackageData(int APlayer, Aow.Rtl.TMemoryStream Stream)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // NetworkE.pas:468
        // { TPlayerCommunicationControl }
        public TPlayerCommunicationControl()
        {
            FRefCount = 1;
            var call__106 = new Aow.Units.NetworkE.TPlayerCommunicationSettingsList(null);
            FPCSList = call__106;
        }

        // NetworkE.pas:475
        public override void Destroy()
        {
            FPCSList.Free();
            base.Destroy();
        }

        // NetworkE.pas:481
        public void AddRef()
        {
            FRefCount += 1;
        }

        // NetworkE.pas:486
        public void Release()
        {
            FRefCount -= 1;
            if (FRefCount == 0)
            {
                Destroy();
            }
        }

        // NetworkE.pas:503
        public Aow.Units.NetworkE.TPlayerCommunicationSettings GetPlayers(int Index)
        {
            Aow.Units.NetworkE.TPlayerCommunicationSettings Result = default;
            bool bool__107 = (Index >= 0);
            if (bool__107)
            {
                var call__108 = FPCSList.Count;
                bool__107 = (Index < call__108);
            }

            if (bool__107)
            {
                var call__109 = FPCSList.Players[Index];
                Result = call__109;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // NetworkE.pas:511
        public int GetCount()
        {
            int Result = default;
            var call__110 = FPCSList.Count;
            Result = call__110;
            return Result;
        }

        // NetworkE.pas:522
        public bool Setup(int ACount)
        {
            bool Result = default;
            int I = default;
            FPCSList.Clear();
            {
                I = 0;
                int end__111 = ACount - 1;
                for (; I <= end__111; I += 1)
                {
                    var call__112 = CreatePCS();
                    call__112.Owner = FPCSList;
                }
            }

            Result = true;
            return Result;
        }

        // NetworkE.pas:537
        public int IndexOfSystemID(int ASystemID)
        {
            int Result = default;
            Result = GetCount();
            while (Result != 0)
            {
                Result -= 1;
                var call__113 = FPCSList.Players[Result];
                if (call__113.FSystemID == ASystemID)
                {
                    return Result;
                }
            }

            Result = -1;
            return Result;
        }

        public int Count { get => GetCount(); }

        public sealed class PlayersAccessor(Aow.Units.NetworkE.TPlayerCommunicationControl target)
        {
            public Aow.Units.NetworkE.TPlayerCommunicationSettings this[int Index] { get => target.GetPlayers(Index); }
        }

        public PlayersAccessor Players => new PlayersAccessor(this);
        public Aow.Units.NetworkE.TPlayerCommunicator PlayerCommunicator { get => FPlayerCommunicator; }
        public Aow.Units.NetworkE.TPlayerCommunicationSettingsList PCSList { get => FPCSList; }

    }

    // { NetworkE.TPackageDataList (instance size 8): +4 FList, the memory streams of the packages that
//     wait to be received }
    public partial class TPackageDataList : Aow.Rtl.TObject
    {
        internal Aow.Rtl.TList FList;
        // NetworkE.pas:870
        // { TPackageDataList }
        public TPackageDataList()
        {
            var call__114 = new Aow.Rtl.TList();
            FList = call__114;
        }

        // NetworkE.pas:876
        public override void Destroy()
        {
            Clear();
            FList.Free();
            base.Destroy();
        }

        // NetworkE.pas:883
        public int GetCount()
        {
            int Result = default;
            Result = FList.Count;
            return Result;
        }

        // NetworkE.pas:888
        public Aow.Rtl.TMemoryStream GetPackageData(int Index)
        {
            Aow.Rtl.TMemoryStream Result = default;
            Result = (Aow.Rtl.TMemoryStream)FList.List[Index];
            return Result;
        }

        // NetworkE.pas:893
        public void Clear()
        {
            Aow.Rtl.TList L = default;
            L = FList;
            while (L.Count != 0)
            {
                ((Aow.Rtl.TObject)L.List[L.Count - 1]).Free();
                L.Delete(L.Count - 1);
            }
        }

        // NetworkE.pas:905
        public void Add(Aow.Rtl.TMemoryStream Stream)
        {
            Aow.Rtl.TMemoryStream S = default;
            S = new Aow.Rtl.TMemoryStream();
            var call__115 = Stream.Size;
            S.Size = call__115;
            var call__116 = S.Size;
            Aow.Rtl.Rtl.Move(Stream.Memory.AsSpan(0, call__116), S.Memory.AsSpan(0, call__116), call__116);
            FList.Add(S);
        }

        // NetworkE.pas:915
        public void AddData(object Data, int Size)
        {
            Aow.Rtl.TMemoryStream S = default;
            S = new Aow.Rtl.TMemoryStream();
            S.Size = Size;
            Aow.Rtl.Rtl.Move(Aow.Rtl.Rtl.BufferBytes(Data)[..Size], S.Memory.AsSpan(0, Size), Size);
            FList.Add(S);
        }

        // NetworkE.pas:925
        public void Insert(int Index, Aow.Rtl.TMemoryStream Stream)
        {
            Aow.Rtl.TMemoryStream S = default;
            S = new Aow.Rtl.TMemoryStream();
            var call__117 = Stream.Size;
            S.Size = call__117;
            var call__118 = S.Size;
            Aow.Rtl.Rtl.Move(Stream.Memory.AsSpan(0, call__118), S.Memory.AsSpan(0, call__118), call__118);
            FList.Insert(Index, S);
        }

        // NetworkE.pas:935
        public void Delete(int Index)
        {
            ((Aow.Rtl.TObject)FList.List[Index]).Free();
            FList.Delete(Index);
        }

        public int Count { get => GetCount(); }

        public sealed class PackageDataAccessor(Aow.Units.NetworkE.TPackageDataList target)
        {
            public Aow.Rtl.TMemoryStream this[int Index] { get => target.GetPackageData(Index); }
        }

        public PackageDataAccessor PackageData => new PackageDataAccessor(this);

    }

    // { NetworkE.TRemotePCC (instance size 32): +$10 FLocalSystemID, +$14 FPackageDataList, +$18
//     FOnSendData. The control of a network game: the transport sends through OnSendData and hands
//     what arrives to DataReceived. }
    public partial class TRemotePCC : Aow.Units.NetworkE.TPlayerCommunicationControl
    {
        internal int FLocalSystemID;
        internal Aow.Units.NetworkE.TPackageDataList FPackageDataList;
        internal Aow.Units.NetworkE.TSendDataEvent FOnSendData;
        // NetworkE.pas:819
        public override int GetLocalSystemID()
        {
            int Result = default;
            Result = FLocalSystemID;
            return Result;
        }

        // NetworkE.pas:782
        // { The number of different system IDs among the players }
        public override int GetSystemCount()
        {
            int Result = default;
            int I = default;
            int J = default;
            int ID = default;
            bool Unique = default;
            Result = 0;
            {
                I = 0;
                int end__119 = FPCSList.Count - 1;
                for (; I <= end__119; I += 1)
                {
                    var call__120 = FPCSList.Players[I];
                    ID = call__120.FSystemID;
                    Unique = true;
                    J = 0;
                    while ((J != I) && Unique)
                    {
                        var call__121 = FPCSList.Players[J];
                        Unique = ID != call__121.FSystemID;
                        J += 1;
                    }

                    if (Unique)
                    {
                        Result += 1;
                    }
                }
            }

            return Result;
        }

        // NetworkE.pas:776
        public override Aow.Rtl.AnsiString Name()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Remote";
            return Result;
        }

        // NetworkE.pas:824
        public override bool ReceivePackageData(Aow.Rtl.TMemoryStream Stream)
        {
            bool Result = default;
            var call__122 = FPackageDataList.Count;
            if (call__122 != 0)
            {
                var call__123 = FPackageDataList.PackageData[0];
                var call__124 = call__123.Size;
                Stream.Size = call__124;
                var call__125 = Stream.Size;
                var call__126 = FPackageDataList.PackageData[0];
                Aow.Rtl.Rtl.Move(call__126.Memory.AsSpan(0, call__125), Stream.Memory.AsSpan(0, call__125), call__125);
                FPackageDataList.Delete(0);
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // NetworkE.pas:852
        // { A package for every player (AllPlayers) goes to system 0, the transport's broadcast; a package
//   for every player or for a player on this system is also received here. }
        public override bool SendPackageData(int APlayer, Aow.Rtl.TMemoryStream Stream)
        {
            bool Result = default;
            int SystemID = default;
            if (APlayer == Aow.Units.NetworkE.NetworkE.AllPlayers)
            {
                SystemID = 0;
            }
            else
            {
                var call__127 = FPCSList.Players[APlayer];
                SystemID = call__127.FSystemID;
            }

            if (SystemID != FLocalSystemID)
            {
                var call__128 = Stream.Size;
                var call__129 = SendData(Stream.Memory, call__128, SystemID);
                Result = call__129;
            }
            else
            {
                Result = true;
            }

            if ((APlayer == Aow.Units.NetworkE.NetworkE.AllPlayers) || (SystemID == FLocalSystemID))
            {
                FPackageDataList.Add(Stream);
            }

            return Result;
        }

        // NetworkE.pas:759
        // { TRemotePCC }
        public TRemotePCC() : base()
        {
            var call__130 = new Aow.Units.NetworkE.TPackageDataList();
            FPackageDataList = call__130;
        }

        // NetworkE.pas:765
        public override void Destroy()
        {
            FPackageDataList.Free();
            base.Destroy();
        }

        // NetworkE.pas:771
        public void Clear()
        {
            FPackageDataList.Clear();
        }

        // NetworkE.pas:803
        public void SetConnectionMode(int Index, Aow.Units.NetworkE.TConnectionMode Mode)
        {
            var call__131 = FPCSList.Players[Index];
            call__131.FConnectionMode = Mode;
        }

        // NetworkE.pas:808
        public void SetSystemID(int Index, int ASystemID)
        {
            var call__132 = FPCSList.Players[Index];
            call__132.FSystemID = ASystemID;
        }

        // NetworkE.pas:813
        public void Setup(int ACount, int ALocalSystemID)
        {
            base.Setup(ACount);
            FLocalSystemID = ALocalSystemID;
        }

        // NetworkE.pas:837
        public void DataReceived(object Data, int Size, int SystemID)
        {
            FPackageDataList.AddData(Data, Size);
        }

        // NetworkE.pas:842
        public bool SendData(object Data, int Size, int SystemID)
        {
            bool Result = default;
            if (FOnSendData != null)
            {
                var call__133 = FOnSendData(Data, Size, SystemID);
                Result = call__133;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        public Aow.Units.NetworkE.TSendDataEvent OnSendData { get => FOnSendData; set => FOnSendData = value; }

    }

    // { NetworkE.TLocalPCC (instance size 20): +$10 FPackageDataList. The control of a game on one
//     system: what is sent is received again. }
    public partial class TLocalPCC : Aow.Units.NetworkE.TPlayerCommunicationControl
    {
        internal Aow.Units.NetworkE.TPackageDataList FPackageDataList;
        // NetworkE.pas:709
        public override int GetSystemCount()
        {
            int Result = default;
            Result = 1;
            return Result;
        }

        // NetworkE.pas:714
        public override Aow.Rtl.AnsiString Name()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Local";
            return Result;
        }

        // NetworkE.pas:724
        public override bool ReceivePackageData(Aow.Rtl.TMemoryStream Stream)
        {
            bool Result = default;
            var call__134 = FPackageDataList.Count;
            if (call__134 != 0)
            {
                var call__135 = FPackageDataList.PackageData[0];
                var call__136 = call__135.Size;
                Stream.Size = call__136;
                var call__137 = Stream.Size;
                var call__138 = FPackageDataList.PackageData[0];
                Aow.Rtl.Rtl.Move(call__138.Memory.AsSpan(0, call__137), Stream.Memory.AsSpan(0, call__137), call__137);
                FPackageDataList.Delete(0);
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // NetworkE.pas:737
        public override bool SendPackageData(int APlayer, Aow.Rtl.TMemoryStream Stream)
        {
            bool Result = default;
            FPackageDataList.Add(Stream);
            Result = true;
            return Result;
        }

        // NetworkE.pas:697
        // { TLocalPCC }
        public TLocalPCC() : base()
        {
            var call__139 = new Aow.Units.NetworkE.TPackageDataList();
            FPackageDataList = call__139;
        }

        // NetworkE.pas:703
        public override void Destroy()
        {
            FPackageDataList.Free();
            base.Destroy();
        }

        // NetworkE.pas:719
        public void Clear()
        {
            FPackageDataList.Clear();
        }

        // NetworkE.pas:743
        public new void Setup(int ACount)
        {
            int I = default;
            base.Setup(ACount);
            {
                I = FPCSList.Count - 1;
                byte end__140 = 0;
                for (; I >= end__140; I -= 1)
                {
                    var call__141 = GetLocalSystemID();
                    var call__142 = FPCSList.Players[I];
                    call__142.FSystemID = call__141;
                }
            }
        }

        // NetworkE.pas:752
        public void SetConnectionMode(int Index, Aow.Units.NetworkE.TConnectionMode Mode)
        {
            var call__143 = FPCSList.Players[Index];
            call__143.FConnectionMode = Mode;
        }

    }

    // { NetworkE.TTokenPackage (instance size 24): +$14 FTokenEvents (stored with id 5) }
    public partial class TTokenPackage : Aow.Units.NetworkE.TDataPackage
    {
        // NetworkE.pas:1100
        public new const int ClassID = Aow.Units.NetworkE.NetworkE.CID_TokenPackage;

        internal Aow.Units.NetworkE.TTokenEventList FTokenEvents;
        // NetworkE.pas:1105
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FTokenEvents, 5);
        }

        // NetworkE.pas:1088
        // { TTokenPackage }
        public TTokenPackage(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__144 = new Aow.Units.NetworkE.TTokenEventList(null);
            FTokenEvents = call__144;
        }

        // NetworkE.pas:1094
        public override void Destroy()
        {
            FTokenEvents.Free();
            base.Destroy();
        }

        public Aow.Units.NetworkE.TTokenEventList TokenEvents { get => FTokenEvents; }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // NetworkE.pas:1100
            public override int ClassID() => Aow.Units.NetworkE.TTokenPackage.ClassID;

            // NetworkE.pas:244
            public new Aow.Units.NetworkE.TTokenPackage Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.NetworkE.TTokenPackage)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.NetworkE.TTokenPackage => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { NetworkE.TTokenPackageDistributer (instance size 12): +4 FTokenManager, +8 FPC. Sends the token
//     events of the local players in token packages and files the received ones with their players'
//     token controls. }
    public partial class TTokenPackageDistributer : Aow.Rtl.TObject
    {
        internal Aow.Units.NetworkE.TTokenManager FTokenManager;
        internal Aow.Units.NetworkE.TPlayerCommunicator FPC;
        // NetworkE.pas:1136
        // { The token events of a package from another system go to the token control of the sending player,
//   unless they were already executed or are already there. }
        public virtual void TokenPackageReceived(Aow.Units.NetworkE.TTokenPackage TP)
        {
            int I = default;
            if (FTokenManager != null)
            {
                var call__145 = FPC.FPCC.GetLocalSystemID();
                var call__146 = FPC.FPCC.GetPlayers(TP.FSender);
                if (call__145 != call__146.FSystemID)
                {
                    {
                        I = 0;
                        int end__147 = TP.FTokenEvents.Count - 1;
                        for (; I <= end__147; I += 1)
                        {
                            var call__148 = TP.FTokenEvents[I];
                            if (call__148.FTokenCounter >= FTokenManager.FTokenExecuter.FTokenCounter)
                            {
                                var call__149 = TP.FTokenEvents[I];
                                var call__150 = FTokenManager.GetTokenControl(TP.FSender);
                                var call__151 = call__150.FEvents.IndexOfTokenCounter(call__149.FTokenCounter);
                                if (call__151 == -1)
                                {
                                    var call__152 = TP.FTokenEvents[I];
                                    var call__153 = FTokenManager.GetTokenControl(TP.FSender);
                                    call__153.Add(call__152);
                                    var call__154 = FTokenManager.GetTokenControl(TP.FSender);
                                    call__154.FEvents.SortOnTokenCounter();
                                }
                                else
                                {
                                    FTokenManager.TriggerMessage((AnsiString)"TokenEvent already exists");
                                }
                            }
                            else
                            {
                                FTokenManager.TriggerMessage((AnsiString)"TokenEvent already executed");
                            }
                        }
                    }
                }
            }
        }

        // NetworkE.pas:1113
        // { TTokenPackageDistributer }
        public TTokenPackageDistributer(Aow.Units.NetworkE.TTokenManager AManager)
        {
            FTokenManager = AManager;
        }

        // NetworkE.pas:1157
        public virtual void SendTokenEvents(int APlayer, int AReceiver, Aow.Units.NetworkE.TTokenEventList Events)
        {
            Aow.Units.NetworkE.TTokenPackage TP = default;
            TP = new Aow.Units.NetworkE.TTokenPackage(null);
            TP.FTokenEvents.Assign(Events, null);
            FPC.SendDataPackage(TP, APlayer, AReceiver);
            TP.Release();
        }

        // NetworkE.pas:1119
        public void SetPC(Aow.Units.NetworkE.TPlayerCommunicator PC)
        {
            if (FPC != null)
            {
                FPC.FOnReceive.Remove(DataPackageReceived);
            }

            FPC = PC;
            if (FPC != null)
            {
                FPC.FOnReceive.Add(DataPackageReceived);
            }
        }

        // NetworkE.pas:1128
        public void DataPackageReceived(Aow.Rtl.TObject Sender)
        {
            if (((Aow.Units.NetworkE.TPlayerCommunicator)Sender).FReceivedPackage is Aow.Units.NetworkE.TTokenPackage)
            {
                TokenPackageReceived((Aow.Units.NetworkE.TTokenPackage)((Aow.Units.NetworkE.TPlayerCommunicator)Sender).FReceivedPackage);
            }
        }

        public Aow.Units.NetworkE.TPlayerCommunicator PC { get => FPC; set => SetPC(value); }

    }

    // { NetworkE.TPassTokenTE (instance size 16): ends the turn of the token control that prefetched
//     it }
    public partial class TPassTokenTE : Aow.Units.NetworkE.TTokenEvent
    {
        public new const int ClassID = Aow.Units.NetworkE.NetworkE.CID_PassTokenTE;

        public TPassTokenTE(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        public new partial class Meta : Aow.Units.NetworkE.TTokenEvent.Meta
        {
            // NetworkE.pas:1726
            // { TPassTokenTE }
            public override int ClassID() => Aow.Units.NetworkE.TPassTokenTE.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.NetworkE.TPassTokenTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.NetworkE.TPassTokenTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.NetworkE.TPassTokenTE)base.Create(AOwner);
        }
    }

    // { NetworkE.TSyncTE (instance size 20): +$10 FSyncValue (stored with id $14), the sync value of the
//     system that sent it; another value on execution triggers OnSyncError }
    public partial class TSyncTE : Aow.Units.NetworkE.TTokenEvent
    {
        public new const int ClassID = Aow.Units.NetworkE.NetworkE.CID_SyncTE;

        public TSyncTE(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        internal int FSyncValue;
        // NetworkE.pas:1738
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FSyncValue, 20);
        }

        // NetworkE.pas:1744
        public override Aow.Units.NetworkE.TExecuteResult Execute(Aow.Units.NetworkE.TTokenExecuter TE)
        {
            Aow.Units.NetworkE.TExecuteResult Result = default;
            Result = base.Execute(TE);
            var call__155 = TE.GetSyncValue();
            if ((call__155 != FSyncValue) && (TE.FOnSyncError != null))
            {
                TE.FOnSyncError(TE);
            }

            return Result;
        }

        public int SyncValue { get => FSyncValue; set => FSyncValue = value; }

        public new partial class Meta : Aow.Units.NetworkE.TTokenEvent.Meta
        {
            // NetworkE.pas:1733
            // { TSyncTE }
            public override int ClassID() => Aow.Units.NetworkE.TSyncTE.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.NetworkE.TSyncTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.NetworkE.TSyncTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.NetworkE.TSyncTE)base.Create(AOwner);
        }
    }

    public static partial class NetworkE
    {
        // { The NetworkE unit of Network.dpl (the network message layer of Age of Wonders: the player
//   communication controls that move data packages between the players' systems, and the token
//   system that orders the game's token events and executes them on every system in the same order)
//   written for the original compiler (Delphi 3.02), class by class: tools/match.py compares the code
//   DCC32 generates for every method with the code in the game's module. Declarations follow the
//   original instance layouts and VMT orders (data/classes.json); the field names are made up, the
//   module has no RTTI for them. Network.dpl was compiled with the default switches.
//
//   A data package travels as a stored TEObject: TPlayerCommunicator.SendDataPackage stores it in a
//   memory stream with TEngine.StoreEObject and hands the stream to the active player communication
//   control (TLocalPCC queues it for itself, TRemotePCC sends it through OnSendData to every other
//   system); TPlayerCommunicator.Process retrieves the packages that arrived and triggers OnReceive.
//   The token manager keeps one token control per player; the token executer activates them in turn,
//   lets the active one prefetch its events (a local player sends them to the others in a token
//   package, a TPassTokenTE ends the turn of the token) and executes them in token counter order. }
        // { TRemotePCC.SendPackageData / TTokenControl.TokenEventsPrefetched: the package goes to every
//     player }
        public const int AllPlayers = 32768;
        // { Class IDs }
        public const short CID_TokenPackage = 1360;
        public const short CID_TokenEvent = 1361;
        public const short CID_TokenEventList = 1362;
        public const short CID_PassTokenTE = 1364;
        public const short CID_SyncTE = 1365;
        // NetworkE.pas:1751
        public static void RegisterNetworkEngine(Aow.Units.Engine.TEngine AEngine)
        {
            var call__156 = AEngine.RegisterDebugID((AnsiString)"Network Engine");
            Aow.Units.NetworkE.NetworkE.NetworkEngineDID = call__156;
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.NetworkE.TTokenPackage>(), Aow.Units.Engine.EClass.Of<Aow.Units.NetworkE.TPassTokenTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.NetworkE.TSyncTE>()]);
        }

        public static int NetworkEngineDID;
        // NetworkE.pas:1051
        public static int CompareTokenCounter(object Item1, object Item2)
        {
            int Result = default;
            Result = ((Aow.Units.NetworkE.TTokenEvent)Item1).FTokenCounter - ((Aow.Units.NetworkE.TTokenEvent)Item2).FTokenCounter;
            return Result;
        }
    }
}
