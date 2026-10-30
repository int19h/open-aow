// Translated from Dungeon.pas.
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
using Aow.Units.ExploreS;
using static Aow.Units.ExploreS.ExploreS;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Dungeon
{
    // { ot=1 }
    // { TPrisonersArmy (instance size 44, VMT 557C53F4) }
    public partial class TPrisonersArmy : Aow.Units.AoWE.TArmy
    {
        public TPrisonersArmy(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // Dungeon.pas:89
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

    }

    // { TDungeon (instance size 64, VMT 557C557C): $38 FPrisoners, $3C FPrisonersMode. }
    public partial class TDungeon : Aow.Units.ExploreS.TExplorationSite
    {
        public TDungeon(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FPrisonersMode = Aow.Units.Dungeon.TPrisonersMode.pmNormal;
            var call__5 = new Aow.Units.Dungeon.TPrisonersArmy(null);
            FPrisoners = call__5;
        }

        // Dungeon.pas:108
        public new const int ClassID = 131882;

        public Aow.Units.Dungeon.TPrisonersArmy FPrisoners;
        // { $38 }
        public Aow.Units.Dungeon.TPrisonersMode FPrisonersMode;
        // { $3C }
        public Aow.Units.FixedArray_byte_3 F_Dungeon_3D;
        // Dungeon.pas:102
        public override void Destroy()
        {
            base.Destroy();
            FPrisoners.Free();
        }

        // Dungeon.pas:114
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FPrisoners, 8);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FPrisonersMode, 1), 1, 9);
        }

        // Dungeon.pas:95


        // Dungeon.pas:249
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
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
        public override Aow.Units.NetworkE.TExecuteResult ExecuteTE(Aow.Units.AoWE.TStructureTE A1, Aow.Units.NetworkE.TTokenExecuter Executer)
        {
            Aow.Units.NetworkE.TExecuteResult Result = default;
            Result = base.ExecuteTE(A1, Executer);
            if (A1.FAction == 1)
            {
                LiberatePrisoners(unchecked((sbyte)A1.FParam));
                Result = Aow.Units.NetworkE.TExecuteResult.erDone;
            }

            return Result;
        }

        // Dungeon.pas:231
        public override void NewDay()
        {
            base.NewDay();
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FDay == 1)
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
        public override void SetupCombat(Aow.Units.AoWE.TCombat A1, Aow.Units.AoWE.TArmyHS A2)
        {
            base.SetupCombat(A1, A2);
            A1.FItems = FPrisoners;
        }

        // Dungeon.pas:127
        public override void ExecuteSearchDone(Aow.Units.AoWE.TCombatType A1, Aow.Units.AoWE.TCombatResult A2)
        {
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.Dungeon.TPrisonersJoinEventLog Log = default;
            int I = default;
            if (TSet<Aow.Units.AoWE.TCombatResult>.Of(Aow.Units.AoWE.TCombatResult.crVictory, Aow.Units.AoWE.TCombatResult.crNoDefenders).Contains(A2))
            {
                var call__7 = Owner.FindOwnedChild(131607);
                Army = (Aow.Units.AoWE.TArmyHS)call__7;
                Log = new Aow.Units.Dungeon.TPrisonersJoinEventLog(null);
                var call__8 = GetXhx();
                Log.FX = unchecked((sbyte)call__8);
                var call__9 = GetYhx();
                Log.FY = unchecked((sbyte)call__9);
                var call__10 = GetLhx();
                Log.FL = unchecked((sbyte)call__10);
                Log.FPlayer = unchecked((sbyte)Army.FArmy.FPlayer);
                Log.FSelection = 255;
                Log.FGold = 0;
                Log.FJoinMode = Aow.Units.AoWE.TJoinMode.jmDefault;
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
                    var call__15 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LiberatePrisonerMessageRStr);
                    Log.FJoinMessage.Text = call__15;
                }
                else
                {
                    var call__16 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LiberatePrisonersMessageRStr);
                    Log.FJoinMessage.Text = call__16;
                }

                Log.Execute(false, Aow.Units.EventLog.TExecuteTrigger.etEvent, null);
                Log.Release();
            }
        }

        // Dungeon.pas:156
        public void GeneratePrisoners()
        {
            int Collection = default;
            int I = default;
            Aow.Units.AoWE.TUnitIndexList List = default;
            Aow.Units.AoWE.TUnitIndexListCollection Collections = default;
            Aow.Rtl.Rtl.RandSeed = Aow.Units.AoWE.AoWE.AoWHSMap.FDefendersSeed;
            if (FPrisonersMode == Aow.Units.Dungeon.TPrisonersMode.pmRandom)
            {
                var call__17 = Aow.Rtl.Rtl.Random(3);
                FPrisonersMode = (Aow.Units.Dungeon.TPrisonersMode)(call__17 + 1);
            }

            var call__18 = ((Aow.Units.Dungeon.TDungeonResource)FResource).FPrisonerCollections.GetRndCollection();
            Collection = call__18;
            if (Collection != -1)
            {
                List = new Aow.Units.AoWE.TUnitIndexList();
                Collections = ((Aow.Units.Dungeon.TDungeonResource)FResource).FPrisonerCollections;
                {
                    I = Collections.GetCollectionCount(Collection) - 1;
                    byte end__19 = 0;
                    for (; I >= end__19; I -= 1)
                    {
                        var call__20 = Collections.GetU(Collection, I);
                        List.Add(call__20);
                    }
                }

                Aow.Units.AoWE.AoWE.FillWithRandomUnits(List, FPrisoners, 4, ((int)FPrisonersMode) * 50, TSet<Aow.Units.AoWE.TUnitType>.Of(Aow.Units.AoWE.TUnitType.utHumanoid, Aow.Units.AoWE.TUnitType.utCreature, Aow.Units.AoWE.TUnitType.utMachine));
                List.Free();
            }
        }

        // Dungeon.pas:178
        public void LiberatePrisoners(sbyte Player)
        {
            Aow.Units.EventLog.TLocationTextEventLog LocationLog = default;
            Aow.Units.AoWE.TArmyInfoEventLog ArmyLog = default;
            Aow.Units.AoWE.THero Hero = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            LocationLog = new Aow.Units.EventLog.TLocationTextEventLog(null);
            var call__21 = GetPlayer();
            LocationLog.FPlayer = unchecked((sbyte)call__21);
            var call__22 = GetName();
            var call__23 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XExploredRStr);
            var call__24 = Aow.Rtl.Rtl.Format(call__23, [call__22]);
            LocationLog.FText = call__24;
            var call__25 = GetLhx();
            var call__26 = GetYhx();
            var call__27 = GetXhx();
            LocationLog.SetLocation(unchecked((sbyte)call__27), unchecked((sbyte)call__26), unchecked((sbyte)call__25));
            var call__28 = GetYhx();
            var call__29 = GetLhx();
            var call__30 = GetXhx();
            Aow.Units.EventLog.EventLog.DistributeLocationEventLog(LocationLog, unchecked((sbyte)Player), unchecked((sbyte)call__30), unchecked((sbyte)call__28), unchecked((sbyte)call__29), false);
            LocationLog.Release();
            ArmyLog = new Aow.Units.AoWE.TArmyInfoEventLog(null);
            ArmyLog.FArmyView.Setup(FPrisoners, 255);
            var call__31 = GetXhx();
            ArmyLog.FArmyView.FX = unchecked((sbyte)call__31);
            var call__32 = GetYhx();
            ArmyLog.FArmyView.FY = unchecked((sbyte)call__32);
            var call__33 = GetLhx();
            ArmyLog.FArmyView.FL = unchecked((sbyte)call__33);
            ArmyLog.FSelection = 255;
            var call__34 = GetPlayer();
            ArmyLog.FPlayer = unchecked((sbyte)call__34);
            var call__35 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PrisonerLiberationRStr);
            ArmyLog.FTitle = call__35;
            ArmyLog.FDescription = (AnsiString)"";
            var call__36 = FPrisoners.Hero(255, TSet<Aow.Units.AoWE.THeroType>.Of(Aow.Units.AoWE.THeroType.htHero));
            Hero = call__36;
            if (Hero == null)
            {
                var call__37 = FPrisoners.Count;
                if (call__37 == 1)
                {
                    var call__38 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.OnePrisonerLiberatedRStr);
                    ArmyLog.FText = call__38;
                    var call__39 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__40 = call__39.GetName();
                    var call__41 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AfterLiberationOneUnitJoinedTheForcesOfXRStr);
                    var call__42 = Aow.Rtl.Rtl.Format(call__41, [call__40]);
                    ArmyLog.FMessages.Text = call__42;
                }
                else
                {
                    var call__43 = FPrisoners.Count;
                    var call__44 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XPrisonersLiberatedRStr);
                    var call__45 = Aow.Rtl.Rtl.Format(call__44, [Aow.Rtl.Rtl.IntToStr(call__43)]);
                    ArmyLog.FText = call__45;
                    var call__46 = FPrisoners.Count;
                    var call__47 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__48 = call__47.GetName();
                    var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AfterLiberationXunitsJoinedTheForcesOfYRStr);
                    var call__50 = Aow.Rtl.Rtl.Format(call__49, [(int)call__46, call__48]);
                    ArmyLog.FMessages.Text = call__50;
                }
            }
            else
            {
                var call__51 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XLiberatedRStr);
                var call__52 = Aow.Rtl.Rtl.Format(call__51, [Hero.FHeroName]);
                ArmyLog.FText = call__52;
                var call__53 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                var call__54 = call__53.GetName();
                var call__55 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AfterLiberationXJoinedTheForcesOfYRStr);
                var call__56 = Aow.Rtl.Rtl.Format(call__55, [Hero.FHeroName, call__54]);
                ArmyLog.FMessages.Text = call__56;
            }

            var call__57 = GetYhx();
            var call__58 = GetLhx();
            var call__59 = GetXhx();
            Aow.Units.EventLog.EventLog.DistributeLocationEventLog(ArmyLog, unchecked((sbyte)-1), unchecked((sbyte)call__59), unchecked((sbyte)call__57), unchecked((sbyte)call__58), false);
            ArmyLog.Release();
            Army = Aow.Units.AoWE.TArmyHS.Create(null);
            Army.FArmy.BeginUpdate();
            Army.FArmy.SetPlayer(unchecked((sbyte)Player));
            Army.FArmy.AddUnits(FPrisoners);
            var call__60 = GetLhx();
            var call__61 = GetYhx();
            var call__62 = GetXhx();
            Army.PlaceEx(unchecked((sbyte)call__62), unchecked((sbyte)call__61), unchecked((sbyte)call__60), TSet<Aow.Units.AoWE.TArmyPlaceModeElement>.Of(Aow.Units.AoWE.TArmyPlaceModeElement.apSafe, Aow.Units.AoWE.TArmyPlaceModeElement.apScanArea));
            Army.Activate();
            Army.FArmy.EndUpdate();
        }

        // Dungeon.pas:33


        public new partial class Meta : Aow.Units.ExploreS.TExplorationSite.Meta
        {
            // Dungeon.pas:108
            public override int ClassID() => Aow.Units.Dungeon.TDungeon.ClassID;

            // Dungeon.pas:33
            public new Aow.Units.Dungeon.TDungeon Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Dungeon.TDungeon)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.Dungeon.TDungeon => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TDungeonResource (instance size 108, VMT 557C57F0): $68 FPrisonerCollections. }
    public partial class TDungeonResource : Aow.Units.ExploreS.TExplorationSiteResource
    {
        public TDungeonResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__63 = new Aow.Units.AoWE.TUnitIndexListCollection();
            FPrisonerCollections = call__63;
        }

        // Dungeon.pas:280
        public new const int ClassID = 131883;

        public Aow.Units.AoWE.TUnitIndexListCollection FPrisonerCollections;
        // Dungeon.pas:271
        // { $68 }
        public override void Destroy()
        {
            FPrisonerCollections.Free();
            base.Destroy();
        }

        // Dungeon.pas:286
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
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
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DungeonRStr);
            return Result;
        }

        public new partial class Meta : Aow.Units.ExploreS.TExplorationSiteResource.Meta
        {
            // Dungeon.pas:280
            public override int ClassID() => Aow.Units.Dungeon.TDungeonResource.ClassID;

            // Dungeon.pas:53
            public new Aow.Units.Dungeon.TDungeonResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Dungeon.TDungeonResource)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.Dungeon.TDungeonResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TPrisonersJoinEventLog (instance size 64, VMT 557C58EC): $3C FX, $3D FY, $3E FL. }
    public partial class TPrisonersJoinEventLog : Aow.Units.AoWE.TJoinEventLog
    {
        public TPrisonersJoinEventLog(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        public sbyte FX;
        // { $3C }
        public sbyte FY;
        // { $3D }
        public sbyte FL;
        // { $3E }
        public Aow.Units.FixedArray_byte_1 F_PrisonersJoinEventLog_3F;
        // Dungeon.pas:292
        public override bool Accept(ref Aow.Rtl.AnsiString ErrorText)
        {
            bool Result = default;
            Aow.Units.Dungeon.TDungeon Dungeon = default;
            Aow.Units.AoWE.TStructureTE TE = default;
            var call__64 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(FX, FY, FL);
            var call__65 = call__64.FindOwnedChild(131882);
            Dungeon = (Aow.Units.Dungeon.TDungeon)call__65;
            bool bool__66 = (Dungeon != null);
            if (bool__66)
            {
                var call__67 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__68 = call__67.GetBusy();
                bool__66 = !call__68;
            }

            if (bool__66)
            {
                TE = Dungeon.CreateTE();
                Dungeon.SetupTE(TE);
                TE.FParam = FPlayer;
                TE.FAction = 1;
                var call__69 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(FPlayer);
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
            Aow.Units.Dungeon.TDungeon Dungeon = default;
            int I = default;
            int Index = default;
            var call__70 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(FX, FY, FL);
            var call__71 = call__70.FindOwnedChild(131882);
            Dungeon = (Aow.Units.Dungeon.TDungeon)call__71;
            if (Dungeon != null)
            {
                {
                    I = 0;
                    int end__72 = Dungeon.FPrisoners.Count - 1;
                    for (; I <= end__72; I += 1)
                    {
                        var call__73 = Dungeon.FPrisoners.GetUnit(I);
                        var call__74 = call__73.GetUnitType();
                        if (TSet<Aow.Units.AoWE.TUnitType>.Of(Aow.Units.AoWE.TUnitType.utHumanoid, Aow.Units.AoWE.TUnitType.utCreature).Contains(call__74))
                        {
                            var call__75 = Dungeon.FPrisoners.GetUnit(I);
                            var call__76 = call__75.GetUnitGFXResourceIndex();
                            Index = call__76;
                            var call__77 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Index);
                            if (call__77 != null)
                            {
                                var call__78 = Aow.Rtl.Rtl.Random(15);
                                var call__79 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Index);
                                call__79.FSFXLibraries[3].PlayEx(0, 100, false, call__78, Aow.Units.Sound.TSamplePanning.spCenter, null);
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
        public static void RegisterDungeon(Aow.Units.Engine.TEngine Engine)
        {
            Engine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Dungeon.TDungeon>(), Aow.Units.Engine.EClass.Of<Aow.Units.Dungeon.TDungeonResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
