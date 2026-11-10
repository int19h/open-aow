// Translated from Arena.pas.
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands
{

    // FUnitIDs
    public partial class ArenaTE : StructureTE
    {
        public ArenaTE(ECustomNode owner) : base(owner)
        {
            FUnitIDs = new IntegerList();
        }

        // Arena.pas:86
        public new const int ClassID = 133294;

        public IntegerList FUnitIDs;

        // Arena.pas:80
        public override void Dispose()
        {
            FUnitIDs?.Dispose();
            base.Dispose();
        }

        // Arena.pas:89
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStreamProc(40, FUnitIDs.LoadFromStream, FUnitIDs.SaveToStream);
        }

        // Arena.pas:74

        // Arena.pas:23

    }
}

namespace Aow.Game.World.Structures
{

    public partial class Arena : Structure
    {
        public Arena(ECustomNode owner) : base(owner)
        { }

        // Arena.pas:95
        public new const int ClassID = 133276;

        // Arena.pas:212
        public override StructureTE CreateTE() => new ArenaTE(null);

        // Arena.pas:135
        public override ExecuteResult ExecuteTE(StructureTE TE, TokenExecuter Executer)
        {
            AbstractUnit AUnit;
            string ErrorText = "";
            ExecuteResult Result = base.ExecuteTE(TE, Executer);
            if (TE.FAction == 1)
            {
                if (CanTrainUnits((sbyte)TE.FParam, ((ArenaTE)TE).FUnitIDs, ref ErrorText))
                {
                    for (int I = 0, last = ((ArenaTE)TE).FUnitIDs.Count - 1; I <= last; I++)
                    {
                        var unitID = ((ArenaTE)TE).FUnitIDs[I];
                        AUnit = Map.AoWHSMap.Current.FUnitControl.FindUnit(unitID);
                        if (AUnit != null)
                        {
                            var player = Map.AoWHSMap.Current.FPlayers.GetPlayers(TE.FParam);
                            var unitTrainCost = UnitTrainCost(AUnit);
                            Map.AoWHSMap.Current.FPlayers.GetPlayers(TE.FParam).SetGems(player.FGems - unitTrainCost);
                            ExecuteTrainUnit(AUnit);
                        }
                    }

                    if (Map.AoWHSMap.Current.FSeatedPlayer == TE.FParam)
                    {
                        var arenaTE = (ArenaTE)TE;
                        if (arenaTE.FUnitIDs.Count == 1)
                        {
                            var oneUnitTrained = AbilityRules.TranslateRStr(OneUnitTrainedRStr);
                            Map.AoWHSMap.Current.ShowMessage(oneUnitTrained);
                        }
                        else
                        {
                            var format = RtlUnit.Format(AbilityRules.TranslateRStr(XUnitsTrainedRStr), [IntToStr(arenaTE.FUnitIDs.Count)]);
                            Map.AoWHSMap.Current.ShowMessage(format);
                        }
                    }
                }
            }

            return Result;
        }

        // Arena.pas:98
        public override bool CanDblClick() => CanEnter(Map.AoWHSMap.Current.FSeatedPlayer);

        // Arena.pas:101
        public int UnitTrainCost(AbstractUnit AUnit)
        {
            int Result;
            if (AUnit is Hero)
            {
                // { The original rejects heroes, although ExecuteTrainUnit supports them. }
                return -1;
            }

            if (AUnit is Unit)
            {
                var unit = (Unit)AUnit;
                if (unit.GetRank() != Unit.Rank.Gold)
                {
                    Result = (unit.GetNextLevelExperience() - AUnit.GetExperience()) * 5;
                }
                else
                {
                    Result = -1;
                }
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // Arena.pas:116
        public void ExecuteTrainUnit(AbstractUnit AUnit)
        {
            if (AUnit is Hero)
            {
                var hero = AUnit;
                hero.SetExperience(hero.GetNextLevelExperience());
                hero.Changed();

                return;
            }

            if (AUnit is Unit)
            {
                var unit = AUnit;
                unit.SetExperience(unit.GetNextLevelExperience());
                unit.Changed();
            }
        }

        // Arena.pas:159
        public bool CanTrainUnits(sbyte Player, IntegerList Units, ref string ErrorText)
        {
            bool Result;
            int Total;
            int I;
            int Cost;
            AbstractUnit AUnit;
            if (Units.Count != 0)
            {
                Total = 0;
                I = Units.Count;
                Result = true;
                while (Result && I != 0)
                {
                    I -= 1;
                    var unit = Units[I];
                    AUnit = Map.AoWHSMap.Current.FUnitControl.FindUnit(unit);
                    if (AUnit != null)
                    {
                        Cost = UnitTrainCost(AUnit);
                        if (Cost != -1)
                        {
                            Total += Cost;
                        }
                        else
                        {
                            Result = false;
                        }
                    }
                    else
                    {
                        Result = false;
                    }
                }

                if (Result)
                {
                    if (Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FGems >= Total)
                    {
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                        ErrorText = AbilityRules.TranslateRStr(GameText.NotEnoughGoldRStr);
                    }
                }
            }
            else
            {
                Result = false;
                ErrorText = AbilityRules.TranslateRStr(GameText.NoUnitSelectedRStr);
            }

            return Result;
        }

        // Arena.pas:194
        public bool TrainUnits(sbyte Player, IntegerList Units, ref string ErrorText)
        {
            bool Result;
            ArenaTE TE;
            ErrorText = "";
            if (CanTrainUnits(Player, Units, ref ErrorText) && !Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).GetBusy())
            {
                TE = (ArenaTE)CreateTE();
                SetupTE(TE);
                TE.FUnitIDs.Assign(Units);
                TE.FParam = Player;
                TE.FAction = 1;
                Map.AoWHSMap.Current.FTokenManager.GetTokenControl(Player).AddEvent(TE);
                TE.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Arena.pas:215
        public bool CanEnter(sbyte Player)
        {
            bool Result;
            ArmyHS Army;
            Army = (ArmyHS)FOwner.FindOwnedChild(131607);
            if (Army == null || Army.FArmy.FPlayer != Player)
            {
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // Arena.pas:223
        public bool Enter()
        {
            bool Result = default;
            EnterArenaEventLog Log;
            ArmyHS Army;
            if (CanEnter(Map.AoWHSMap.Current.FSeatedPlayer))
            {
                Log = new EnterArenaEventLog(null);
                Log.FArena = this;
                Army = (ArmyHS)FOwner.FindOwnedChild(131607);
                for (int I = 0, last = Army.FArmy.Count - 1; I <= last; I++)
                {
                    var unit = Army.FArmy.GetUnit(I);
                    Log.FUnitIDs.Add(unit.FUnitID);
                    if (UnitTrainCost(Army.FArmy.GetUnit(I)) != -1)
                    {
                        Log.FTrainable = (byte)(Log.FTrainable | (1 << I));
                    }
                }

                Log.Execute(true, ExecuteTrigger.User, null);
                Log.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        private static readonly string OneUnitTrainedRStr = "1 unit trained";
        private static readonly string XUnitsTrainedRStr = "%s units trained";

        public new partial class Resource : StructureResource
        {
            public Resource(ECustomNode owner) : base(owner)
            {
                FCastsShadow = true;
            }

            // Arena.pas:253
            public new const int ClassID = 133277;

            // Arena.pas:244

            // Arena.pas:256
            public override int ResourceUserClassID() => 133276;

            // Arena.pas:45

            // Arena.pas:250
            public override string ResourceEditName() => AbilityRules.TranslateRStr(ArenaRStr);

            private static readonly string ArenaRStr = "Arena";

        }

    }
}

namespace Aow.Game.Events
{

    // FArena, FSelection, FTrainable, FUnitIDs
    public partial class EnterArenaEventLog : EventLog
    {
        // Arena.pas:259
        public EnterArenaEventLog(ECustomNode owner) : base(owner)
        {
            FUnitIDs = new IntegerList();
        }

        public Arena FArena;
        public byte FSelection;
        public byte FTrainable;
        public IntegerList FUnitIDs;

        // Arena.pas:265
        public override void Dispose()
        {
            FUnitIDs?.Dispose();
            base.Dispose();
        }

        // Arena.pas:271
        public int GetCount() => FUnitIDs.Count;

        // Arena.pas:274
        public AbstractUnit GetUnit(int Index)
        {
            AbstractUnit Result;
            if (Index >= 0 && Index < FUnitIDs.Count)
            {
                var unitID = FUnitIDs[Index];
                Result = World.Map.AoWHSMap.Current.FUnitControl.FindUnit(unitID);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Arena.pas:281
        public void SetSelection(byte Value)
        {
            FSelection = (byte)(Value & FTrainable);
        }

        // Arena.pas:284
        public bool Train(ref string ErrorText)
        {
            IntegerList Units;
            Units = new IntegerList();
            for (int I = 0, last = FUnitIDs.Count - 1; I <= last; I++)
            {
                if (((1 << I) & FSelection) != 0)
                {
                    Units.Add(FUnitIDs[I]);
                }
            }

            bool Result = FArena.TrainUnits(World.Map.AoWHSMap.Current.FSeatedPlayer, Units, ref ErrorText);
            Units?.Dispose();
            return Result;
        }

        // Arena.pas:294
        public bool CanTrain(ref string ErrorText)
        {
            IntegerList Units;
            Units = new IntegerList();
            for (int I = 0, last = FUnitIDs.Count - 1; I <= last; I++)
            {
                if (((1 << I) & FSelection) != 0)
                {
                    Units.Add(FUnitIDs[I]);
                }
            }

            bool Result = FArena.CanTrainUnits(World.Map.AoWHSMap.Current.FSeatedPlayer, Units, ref ErrorText);
            Units?.Dispose();
            return Result;
        }

        // Arena.pas:304
        public string GetSelectionInfo()
        {
            string Result;
            int Total;
            AbstractUnit AUnit;
            Total = 0;
            for (int I = 0, last = FUnitIDs.Count - 1; I <= last; I++)
            {
                if (((1 << I) & FSelection) != 0)
                {
                    var unitID = FUnitIDs[I];
                    AUnit = World.Map.AoWHSMap.Current.FUnitControl.FindUnit(unitID);
                    if (AUnit != null)
                    {
                        Total += FArena.UnitTrainCost(AUnit);
                    }
                }
            }

            if (Total != 0)
            {
                Result = RtlUnit.Format(AbilityRules.TranslateRStr(TotalCostXRStr), [IntToStr(Total)]);
            }
            else
            {
                Result = "";
            }

            return Result;
        }

        private static readonly string TotalCostXRStr = "Total Cost: %s";

    }
}
