// Translated from Dungeon.pas.
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units
{

    // { ot=1 }
    public partial class PrisonersArmy : Army
    {
        public PrisonersArmy(ECustomNode owner) : base(owner)
        { }

        // Dungeon.pas:89
        public override bool CanAddUnit(AbstractUnit AUnit)
        {
            bool Result = base.CanAddUnit(AUnit);
            return Result && !AUnit.GetTransporter() && AUnit.ValidTerrain(1, 4);
        }

        // intf_P2_07.inc:445

    }
}

namespace Aow.Game.World.Structures
{

    // FPrisoners, FPrisonersMode
    public partial class Dungeon : ExplorationSite
    {
        public Dungeon(ECustomNode owner) : base(owner)
        {
            FPrisonersMode = PrisonersMode.Normal;
            FPrisoners = new PrisonersArmy(null);
        }

        // Dungeon.pas:108
        public new const int ClassID = 131882;

        public PrisonersArmy FPrisoners;
        public PrisonersMode FPrisonersMode;

        // Dungeon.pas:102
        public override void Dispose()
        {
            base.Dispose();
            FPrisoners?.Dispose();
        }

        // Dungeon.pas:114
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FPrisoners, 8);
            Stream.rwData(Bytes.Of(ref FPrisonersMode, 1), 1, 9);
        }

        // Dungeon.pas:95

        // Dungeon.pas:249
        public override void MsgProc(ref EMsg Msg, EObject Sender)
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
        public override ExecuteResult ExecuteTE(StructureTE TE, TokenExecuter Executer)
        {
            ExecuteResult Result = base.ExecuteTE(TE, Executer);
            if (TE.FAction == 1)
            {
                LiberatePrisoners((sbyte)TE.FParam);
                Result = ExecuteResult.Done;
            }

            return Result;
        }

        // Dungeon.pas:231
        public override void NewDay()
        {
            base.NewDay();
            if (Map.AoWHSMap.Current.FDay == 1)
            {
                GeneratePrisoners();
            }
        }

        // Dungeon.pas:111
        public override bool GetExplored() => FPrisoners.Count == 0;

        // Dungeon.pas:121
        public override void SetupCombat(Combat.Combat Combat, ArmyHS Army)
        {
            base.SetupCombat(Combat, Army);
            Combat.FItems = FPrisoners;
        }

        // Dungeon.pas:127
        public override void ExecuteSearchDone(CombatType CombatType, CombatResult CombatResult)
        {
            ArmyHS Army;
            PrisonersJoinEventLog Log;
            if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.NoDefenders).Contains(CombatResult))
            {
                Army = (ArmyHS)Owner.FindOwnedChild(131607);
                Log = new PrisonersJoinEventLog(null);
                Log.FX = GetXhx();
                Log.FY = GetYhx();
                Log.FL = GetLhx();
                Log.FPlayer = Army.FArmy.FPlayer;
                Log.FSelection = 255;
                Log.FGold = 0;
                Log.FJoinMode = JoinMode.Default;
                Log.FFirstUnitID = FPrisoners.GetUnit(0).FUnitID;
                for (int I = 0, last = FPrisoners.Count - 1; I <= last; I++)
                {
                    var unit = FPrisoners.GetUnit(I);
                    Log.FUnitIDs.Add(unit.FUnitID);
                }

                if (FPrisoners.Count == 1)
                {
                    var liberatePrisonerMessage = AbilityRules.TranslateRStr(LiberatePrisonerMessageRStr);
                    Log.FJoinMessage.Text = liberatePrisonerMessage;
                }
                else
                {
                    var liberatePrisonersMessage = AbilityRules.TranslateRStr(LiberatePrisonersMessageRStr);
                    Log.FJoinMessage.Text = liberatePrisonersMessage;
                }

                Log.Execute(false, ExecuteTrigger.Event, null);
                Log.Release();
            }
        }

        // Dungeon.pas:156
        public void GeneratePrisoners()
        {
            int Collection;
            UnitIndexList List;
            UnitIndexListCollection Collections;
            RandSeed = Map.AoWHSMap.Current.FDefendersSeed;
            if (FPrisonersMode == PrisonersMode.Random)
            {
                FPrisonersMode = (PrisonersMode)(Random(3) + 1);
            }

            Collection = ((Resource)FResource).FPrisonerCollections.GetRndCollection();
            if (Collection != -1)
            {
                List = new UnitIndexList();
                Collections = ((Resource)FResource).FPrisonerCollections;
                for (int I = Collections.GetCollectionCount(Collection) - 1; I >= 0; I--)
                {
                    List.Add(Collections.GetU(Collection, I));
                }

                Engine.Collections.IntegerList.FillWithRandomUnits(List, FPrisoners, 4, (int)FPrisonersMode * 50, Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature, UnitType.Machine));
                List?.Dispose();
            }
        }

        // Dungeon.pas:178
        public void LiberatePrisoners(sbyte Player)
        {
            LocationTextEventLog LocationLog;
            ArmyInfoEventLog ArmyLog;
            Hero Hero;
            ArmyHS Army;
            LocationLog = new LocationTextEventLog(null);
            LocationLog.FPlayer = GetPlayer();
            var name = GetName();
            LocationLog.FText = RtlUnit.Format(AbilityRules.TranslateRStr(XExploredRStr), [name]);
            var lhx = GetLhx();
            var yhx = GetYhx();
            LocationLog.SetLocation(GetXhx(), yhx, lhx);
            var yhx2 = GetYhx();
            var lhx2 = GetLhx();
            EventLog.DistributeLocationEventLog(LocationLog, Player, GetXhx(), yhx2, lhx2, false);
            LocationLog.Release();
            ArmyLog = new ArmyInfoEventLog(null);
            ArmyLog.FArmyView.Setup(FPrisoners, 255);
            var xhx = GetXhx();
            ArmyLog.FArmyView.FX = xhx;
            var yhx3 = GetYhx();
            ArmyLog.FArmyView.FY = yhx3;
            var lhx3 = GetLhx();
            ArmyLog.FArmyView.FL = lhx3;
            ArmyLog.FSelection = 255;
            ArmyLog.FPlayer = GetPlayer();
            ArmyLog.FTitle = AbilityRules.TranslateRStr(PrisonerLiberationRStr);
            ArmyLog.FDescription = "";
            Hero = FPrisoners.Hero(255, Set<HeroType>.Of(HeroType.Hero));
            if (Hero == null)
            {
                if (FPrisoners.Count == 1)
                {
                    ArmyLog.FText = AbilityRules.TranslateRStr(OnePrisonerLiberatedRStr);
                    var name2 = Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).GetName();
                    var format = RtlUnit.Format(AbilityRules.TranslateRStr(AfterLiberationOneUnitJoinedTheForcesOfXRStr), [name2]);
                    ArmyLog.FMessages.Text = format;
                }
                else
                {
                    var count = FPrisoners.Count;
                    ArmyLog.FText = RtlUnit.Format(AbilityRules.TranslateRStr(XPrisonersLiberatedRStr), [IntToStr(count)]);
                    var fPrisonersCount = FPrisoners.Count;
                    var name2 = Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).GetName();
                    var format = RtlUnit.Format(AbilityRules.TranslateRStr(AfterLiberationXunitsJoinedTheForcesOfYRStr), [fPrisonersCount, name2]);
                    ArmyLog.FMessages.Text = format;
                }
            }
            else
            {
                ArmyLog.FText = RtlUnit.Format(AbilityRules.TranslateRStr(XLiberatedRStr), [Hero.FHeroName]);
                var name2 = Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).GetName();
                var format = RtlUnit.Format(AbilityRules.TranslateRStr(AfterLiberationXJoinedTheForcesOfYRStr), [Hero.FHeroName, name2]);
                ArmyLog.FMessages.Text = format;
            }

            var yhx4 = GetYhx();
            var lhx4 = GetLhx();
            EventLog.DistributeLocationEventLog(ArmyLog, -1, GetXhx(), yhx4, lhx4, false);
            ArmyLog.Release();
            Army = new ArmyHS(null);
            Army.FArmy.BeginUpdate();
            Army.FArmy.SetPlayer(Player);
            Army.FArmy.AddUnits(FPrisoners);
            var lhx5 = GetLhx();
            var yhx5 = GetYhx();
            Army.PlaceEx(GetXhx(), yhx5, lhx5, Set<ArmyPlaceModeElement>.Of(ArmyPlaceModeElement.Safe, ArmyPlaceModeElement.ScanArea));
            Army.Activate();
            Army.FArmy.EndUpdate();
        }

        private static readonly string AfterLiberationOneUnitJoinedTheForcesOfXRStr = "After liberation 1 unit joined the forces of %s";
        private static readonly string AfterLiberationXJoinedTheForcesOfYRStr = "After liberation %s joined the forces of %s";
        private static readonly string AfterLiberationXunitsJoinedTheForcesOfYRStr = "After liberation %d units joined the forces of %s";
        private static readonly string LiberatePrisonerMessageRStr = "In gratitude for rescue, the captive offers to join you. Do you accept?";
        private static readonly string LiberatePrisonersMessageRStr = "Grateful to be rescued, the captives offer to join you. Do you accept?";
        private static readonly string OnePrisonerLiberatedRStr = "1 prisoner liberated";
        internal static readonly string PrisonerLiberationRStr = "Prisoner Liberation";
        private static readonly string XExploredRStr = "%s explored";
        private static readonly string XLiberatedRStr = "%s liberated";
        private static readonly string XPrisonersLiberatedRStr = "%s prisoners liberated";

        // FPrisonerCollections
        public new partial class Resource : ExplorationSiteResource
        {
            public Resource(ECustomNode owner) : base(owner)
            {
                FPrisonerCollections = new UnitIndexListCollection();
            }

            // Dungeon.pas:280
            public new const int ClassID = 131883;

            public UnitIndexListCollection FPrisonerCollections;

            // Dungeon.pas:271
            public override void Dispose()
            {
                FPrisonerCollections?.Dispose();
                base.Dispose();
            }

            // Dungeon.pas:286
            public override void ReadWrite(EStorageStream Stream)
            {
                base.ReadWrite(Stream);
                Stream.rwStreamProc(30, FPrisonerCollections.LoadFromStream, FPrisonerCollections.SaveToStream);
            }

            // Dungeon.pas:265

            // Dungeon.pas:283
            public override int ResourceUserClassID() => 131882;

            // Dungeon.pas:53

            // Dungeon.pas:277
            public override string ResourceEditName() => AbilityRules.TranslateRStr(DungeonRStr);

            private static readonly string DungeonRStr = "Dungeon";

        }

        public enum PrisonersMode : byte
        {
            UserDefined = 0, Low = 1, Normal = 2, High = 3, Random = 4
        }

        // Dungeon.pas:33

        // Dungeon.pas:33

    }
}

namespace Aow.Game.Events
{

    // FX, FY, FL
    public partial class PrisonersJoinEventLog : JoinEventLog
    {
        public PrisonersJoinEventLog(ECustomNode owner) : base(owner)
        { }

        public sbyte FX;
        public sbyte FY;
        public sbyte FL;

        // Dungeon.pas:292
        public override bool Accept(ref string ErrorText)
        {
            bool Result;
            Dungeon Dungeon;
            StructureTE TE;
            Dungeon = (Dungeon)World.Map.AoWHSMap.Current.MapContainer.GetField(FX, FY, FL).FindOwnedChild(131882);
            if (Dungeon != null && !World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
            {
                TE = Dungeon.CreateTE();
                Dungeon.SetupTE(TE);
                TE.FParam = FPlayer;
                TE.FAction = 1;
                World.Map.AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayer).AddEvent(TE);
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
            Dungeon Dungeon;
            int Index;
            Dungeon = (Dungeon)World.Map.AoWHSMap.Current.MapContainer.GetField(FX, FY, FL).FindOwnedChild(131882);
            if (Dungeon != null)
            {
                for (int I = 0, last = Dungeon.FPrisoners.Count - 1; I <= last; I++)
                {
                    var unitType = Dungeon.FPrisoners.GetUnit(I).GetUnitType();
                    if (Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType))
                    {
                        Index = Dungeon.FPrisoners.GetUnit(I).GetUnitGFXResourceIndex();
                        if (AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(Index) != null)
                        {
                            var random = Random(15);
                            var unitGFXResource = AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(Index);
                            unitGFXResource.FSFXLibraries[3].PlayEx(0, 100, false, random, SamplePanning.Center, null);
                        }
                    }
                }
            }

            return true;
        }

    }
}
