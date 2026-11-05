// Translated from Dungeon.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.Sound;
using Aow.Units.NetworkE;
using Aow.Units.ExploreS;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Dungeon
{
    // { ot=1 }
    public partial class TPrisonersArmy : TArmy
    {
        public TPrisonersArmy(TECustomNode owner) : base(owner)
        { }

        // Dungeon.pas:89
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

    // FPrisoners, FPrisonersMode
    public partial class TDungeon : TExplorationSite
    {
        public TDungeon(TECustomNode owner) : base(owner)
        {
            FPrisonersMode = TPrisonersMode.pmNormal;
            var call__5 = new TPrisonersArmy(null);
            FPrisoners = call__5;
        }

        // Dungeon.pas:108
        public new const int ClassID = 131882;

        public TPrisonersArmy FPrisoners;
        public TPrisonersMode FPrisonersMode;
        // Dungeon.pas:102
        public override void Destroy()
        {
            base.Destroy();
            FPrisoners.Free();
        }

        // Dungeon.pas:114
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FPrisoners, 8);
            Stream.rwData(Bytes.Of(ref FPrisonersMode, 1), 1, 9);
        }

        // Dungeon.pas:95


        // Dungeon.pas:249
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            FPrisoners.MsgProc(ref Msg, Sender);
        }

        // Dungeon.pas:237
        public override void Activate()
        {
            base.Activate();
            FPrisoners.Activate();
        }

        // Dungeon.pas:243
        public override void Deactivate()
        {
            FPrisoners.Deactivate();
            base.Deactivate();
        }

        // Dungeon.pas:255
        public override TExecuteResult ExecuteTE(TStructureTE A1, TTokenExecuter Executer)
        {
            TExecuteResult Result = default;
            Result = base.ExecuteTE(A1, Executer);
            if (A1.FAction == 1)
            {
                LiberatePrisoners((sbyte)A1.FParam);
                Result = TExecuteResult.erDone;
            }

            return Result;
        }

        // Dungeon.pas:231
        public override void NewDay()
        {
            base.NewDay();
            if (AoWHSMap.FDay == 1)
            {
                GeneratePrisoners();
            }
        }

        // Dungeon.pas:111
        public override bool GetExplored()
        {
            bool Result = default;
            var call__6 = FPrisoners.Count;
            Result = call__6 == 0;
            return Result;
        }

        // Dungeon.pas:121
        public override void SetupCombat(TCombat A1, TArmyHS A2)
        {
            base.SetupCombat(A1, A2);
            A1.FItems = FPrisoners;
        }

        // Dungeon.pas:127
        public override void ExecuteSearchDone(TCombatType A1, TCombatResult A2)
        {
            TArmyHS Army = default;
            TPrisonersJoinEventLog Log = default;
            int I = default;
            if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crNoDefenders).Contains(A2))
            {
                var call__7 = Owner.FindOwnedChild(131607);
                Army = (TArmyHS)call__7;
                Log = new TPrisonersJoinEventLog(null);
                var call__8 = GetXhx();
                Log.FX = call__8;
                var call__9 = GetYhx();
                Log.FY = call__9;
                var call__10 = GetLhx();
                Log.FL = call__10;
                Log.FPlayer = Army.FArmy.FPlayer;
                Log.FSelection = 255;
                Log.FGold = 0;
                Log.FJoinMode = TJoinMode.jmDefault;
                var call__11 = FPrisoners.GetUnit(0);
                Log.FFirstUnitID = call__11.FUnitID;
                {
                    I = 0;
                    int end__12 = FPrisoners.Count - 1;
                    for (; I <= end__12; I += 1)
                    {
                        var call__13 = FPrisoners.GetUnit(I);
                        Log.FUnitIDs.Add(call__13.FUnitID);
                    }
                }

                var call__14 = FPrisoners.Count;
                if (call__14 == 1)
                {
                    var call__15 = TranslateRStr(LiberatePrisonerMessageRStr);
                    Log.FJoinMessage.Text = call__15;
                }
                else
                {
                    var call__16 = TranslateRStr(LiberatePrisonersMessageRStr);
                    Log.FJoinMessage.Text = call__16;
                }

                Log.Execute(false, TExecuteTrigger.etEvent, null);
                Log.Release();
            }
        }

        // Dungeon.pas:156
        public void GeneratePrisoners()
        {
            int Collection = default;
            int I = default;
            TUnitIndexList List = default;
            TUnitIndexListCollection Collections = default;
            RandSeed = AoWHSMap.FDefendersSeed;
            if (FPrisonersMode == TPrisonersMode.pmRandom)
            {
                var call__17 = Random(3);
                FPrisonersMode = (TPrisonersMode)(call__17 + 1);
            }

            var call__18 = ((TDungeonResource)FResource).FPrisonerCollections.GetRndCollection();
            Collection = call__18;
            if (Collection != -1)
            {
                List = new TUnitIndexList();
                Collections = ((TDungeonResource)FResource).FPrisonerCollections;
                {
                    I = Collections.GetCollectionCount(Collection) - 1;
                    byte end__19 = 0;
                    for (; I >= end__19; I -= 1)
                    {
                        var call__20 = Collections.GetU(Collection, I);
                        List.Add(call__20);
                    }
                }

                FillWithRandomUnits(List, FPrisoners, 4, (int)FPrisonersMode * 50, TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature, TUnitType.utMachine));
                List.Free();
            }
        }

        // Dungeon.pas:178
        public void LiberatePrisoners(sbyte Player)
        {
            TLocationTextEventLog LocationLog = default;
            TArmyInfoEventLog ArmyLog = default;
            THero Hero = default;
            TArmyHS Army = default;
            LocationLog = new TLocationTextEventLog(null);
            var call__21 = GetPlayer();
            LocationLog.FPlayer = call__21;
            var call__22 = GetName();
            var call__23 = TranslateRStr(XExploredRStr);
            var call__24 = Rtl.Rtl.Format(call__23, [call__22]);
            LocationLog.FText = call__24;
            var call__25 = GetLhx();
            var call__26 = GetYhx();
            var call__27 = GetXhx();
            LocationLog.SetLocation(call__27, call__26, call__25);
            var call__28 = GetYhx();
            var call__29 = GetLhx();
            var call__30 = GetXhx();
            DistributeLocationEventLog(LocationLog, Player, call__30, call__28, call__29, false);
            LocationLog.Release();
            ArmyLog = new TArmyInfoEventLog(null);
            ArmyLog.FArmyView.Setup(FPrisoners, 255);
            var call__31 = GetXhx();
            ArmyLog.FArmyView.FX = call__31;
            var call__32 = GetYhx();
            ArmyLog.FArmyView.FY = call__32;
            var call__33 = GetLhx();
            ArmyLog.FArmyView.FL = call__33;
            ArmyLog.FSelection = 255;
            var call__34 = GetPlayer();
            ArmyLog.FPlayer = call__34;
            var call__35 = TranslateRStr(PrisonerLiberationRStr);
            ArmyLog.FTitle = call__35;
            ArmyLog.FDescription = (AnsiString)"";
            var call__36 = FPrisoners.Hero(255, TSet<THeroType>.Of(THeroType.htHero));
            Hero = call__36;
            if (Hero == null)
            {
                var call__37 = FPrisoners.Count;
                if (call__37 == 1)
                {
                    var call__38 = TranslateRStr(OnePrisonerLiberatedRStr);
                    ArmyLog.FText = call__38;
                    var call__39 = AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__40 = call__39.GetName();
                    var call__41 = TranslateRStr(AfterLiberationOneUnitJoinedTheForcesOfXRStr);
                    var call__42 = Rtl.Rtl.Format(call__41, [call__40]);
                    ArmyLog.FMessages.Text = call__42;
                }
                else
                {
                    var call__43 = FPrisoners.Count;
                    var call__44 = TranslateRStr(XPrisonersLiberatedRStr);
                    var call__45 = Rtl.Rtl.Format(call__44, [IntToStr(call__43)]);
                    ArmyLog.FText = call__45;
                    var call__46 = FPrisoners.Count;
                    var call__47 = AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__48 = call__47.GetName();
                    var call__49 = TranslateRStr(AfterLiberationXunitsJoinedTheForcesOfYRStr);
                    var call__50 = Rtl.Rtl.Format(call__49, [call__46, call__48]);
                    ArmyLog.FMessages.Text = call__50;
                }
            }
            else
            {
                var call__51 = TranslateRStr(XLiberatedRStr);
                var call__52 = Rtl.Rtl.Format(call__51, [Hero.FHeroName]);
                ArmyLog.FText = call__52;
                var call__53 = AoWHSMap.FPlayers.GetPlayers(Player);
                var call__54 = call__53.GetName();
                var call__55 = TranslateRStr(AfterLiberationXJoinedTheForcesOfYRStr);
                var call__56 = Rtl.Rtl.Format(call__55, [Hero.FHeroName, call__54]);
                ArmyLog.FMessages.Text = call__56;
            }

            var call__57 = GetYhx();
            var call__58 = GetLhx();
            var call__59 = GetXhx();
            DistributeLocationEventLog(ArmyLog, -1, call__59, call__57, call__58, false);
            ArmyLog.Release();
            Army = new TArmyHS(null);
            Army.FArmy.BeginUpdate();
            Army.FArmy.SetPlayer(Player);
            Army.FArmy.AddUnits(FPrisoners);
            var call__60 = GetLhx();
            var call__61 = GetYhx();
            var call__62 = GetXhx();
            Army.PlaceEx(call__62, call__61, call__60, TSet<TArmyPlaceModeElement>.Of(TArmyPlaceModeElement.apSafe, TArmyPlaceModeElement.apScanArea));
            Army.Activate();
            Army.FArmy.EndUpdate();
        }

        // Dungeon.pas:33


    }

    // FPrisonerCollections
    public partial class TDungeonResource : TExplorationSiteResource
    {
        public TDungeonResource(TECustomNode owner) : base(owner)
        {
            var call__63 = new TUnitIndexListCollection();
            FPrisonerCollections = call__63;
        }

        // Dungeon.pas:280
        public new const int ClassID = 131883;

        public TUnitIndexListCollection FPrisonerCollections;
        // Dungeon.pas:271
        public override void Destroy()
        {
            FPrisonerCollections.Free();
            base.Destroy();
        }

        // Dungeon.pas:286
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStreamProc(30, FPrisonerCollections.LoadFromStream, FPrisonerCollections.SaveToStream);
        }

        // Dungeon.pas:265


        // Dungeon.pas:283
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131882;
            return Result;
        }

        // Dungeon.pas:53


        // Dungeon.pas:277
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(DungeonRStr);
            return Result;
        }

    }

    // FX, FY, FL
    public partial class TPrisonersJoinEventLog : TJoinEventLog
    {
        public TPrisonersJoinEventLog(TECustomNode owner) : base(owner)
        { }

        public sbyte FX;
        public sbyte FY;
        public sbyte FL;
        // Dungeon.pas:292
        public override bool Accept(ref AnsiString ErrorText)
        {
            bool Result = default;
            TDungeon Dungeon = default;
            TStructureTE TE = default;
            var call__64 = AoWHSMap.MapContainer.GetField(FX, FY, FL);
            var call__65 = call__64.FindOwnedChild(131882);
            Dungeon = (TDungeon)call__65;
            bool bool__66 = Dungeon != null;
            if (bool__66)
            {
                var call__67 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__68 = call__67.GetBusy();
                bool__66 = !call__68;
            }

            if (bool__66)
            {
                TE = Dungeon.CreateTE();
                Dungeon.SetupTE(TE);
                TE.FParam = FPlayer;
                TE.FAction = 1;
                var call__69 = AoWHSMap.FTokenManager.GetTokenControl(FPlayer);
                call__69.AddEvent(TE);
                TE.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Dungeon.pas:311
        public override bool Cancel()
        {
            bool Result = default;
            TDungeon Dungeon = default;
            int I = default;
            int Index = default;
            var call__70 = AoWHSMap.MapContainer.GetField(FX, FY, FL);
            var call__71 = call__70.FindOwnedChild(131882);
            Dungeon = (TDungeon)call__71;
            if (Dungeon != null)
            {
                {
                    I = 0;
                    int end__72 = Dungeon.FPrisoners.Count - 1;
                    for (; I <= end__72; I += 1)
                    {
                        var call__73 = Dungeon.FPrisoners.GetUnit(I);
                        var call__74 = call__73.GetUnitType();
                        if (TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature).Contains(call__74))
                        {
                            var call__75 = Dungeon.FPrisoners.GetUnit(I);
                            var call__76 = call__75.GetUnitGFXResourceIndex();
                            Index = call__76;
                            var call__77 = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Index);
                            if (call__77 != null)
                            {
                                var call__78 = Random(15);
                                var call__79 = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Index);
                                call__79.FSFXLibraries[3].PlayEx(0, 100, false, call__78, TSamplePanning.spCenter, null);
                            }
                        }
                    }
                }
            }

            Result = true;
            return Result;
        }

    }

    public enum TPrisonersMode : byte
    {
        pmUserDefined = 0,
        pmLow = 1,
        pmNormal = 2,
        pmHigh = 3,
        pmRandom = 4
    }

    public static partial class Dungeon
    {
        // Dungeon.pas:329
        public static void RegisterDungeon(TEngine Engine)
        {
            Engine.RegisterEClasses([EClass.Of<TDungeon>(), EClass.Of<TDungeonResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
