// Translated from Arena.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.NetworkE;
using Aow.Units.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Arena
{
    // FUnitIDs
    public partial class TArenaTE : TStructureTE
    {
        public TArenaTE(TECustomNode owner) : base(owner)
        {
            var call__1 = new TIntegerList();
            FUnitIDs = call__1;
        }

        // Arena.pas:86
        public new const int ClassID = 133294;

        public TIntegerList FUnitIDs;
        // Arena.pas:80
        public override void Destroy()
        {
            FUnitIDs.Free();
            base.Destroy();
        }

        // Arena.pas:89
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStreamProc(40, FUnitIDs.LoadFromStream, FUnitIDs.SaveToStream);
        }

        // Arena.pas:74


        // Arena.pas:23


    }

    public partial class TArena : TStructure
    {
        public TArena(TECustomNode owner) : base(owner)
        {}

        // Arena.pas:95
        public new const int ClassID = 133276;

        // Arena.pas:212
        public override TStructureTE CreateTE()
        {
            TStructureTE Result = default;
            Result = new TArenaTE(null);
            return Result;
        }

        // Arena.pas:135
        public override TExecuteResult ExecuteTE(TStructureTE A1, TTokenExecuter Executer)
        {
            TExecuteResult Result = default;
            int I = default;
            TAbstractUnit AUnit = default;
            AnsiString ErrorText = default;
            Result = base.ExecuteTE(A1, Executer);
            if (A1.FAction == 1)
            {
                var call__2 = CanTrainUnits((sbyte)A1.FParam, ((TArenaTE)A1).FUnitIDs, ref ErrorText);
                if (call__2)
                {
                    {
                        I = 0;
                        int end__3 = ((TArenaTE)A1).FUnitIDs.Count - 1;
                        for (; I <= end__3; I += 1)
                        {
                            var call__4 = ((TArenaTE)A1).FUnitIDs[I];
                            var call__5 = AoWHSMap.FUnitControl.FindUnit(call__4);
                            AUnit = call__5;
                            if (AUnit != null)
                            {
                                var call__6 = AoWHSMap.FPlayers.GetPlayers(A1.FParam);
                                var call__7 = UnitTrainCost(AUnit);
                                var call__8 = AoWHSMap.FPlayers.GetPlayers(A1.FParam);
                                call__8.SetGems(call__6.FGems - call__7);
                                ExecuteTrainUnit(AUnit);
                            }
                        }
                    }

                    if (AoWHSMap.FSeatedPlayer == A1.FParam)
                    {
                        var arenaTE = (TArenaTE)A1;
                        if (arenaTE.FUnitIDs.Count == 1)
                        {
                            var call__10 = TranslateRStr(OneUnitTrainedRStr);
                            AoWHSMap.ShowMessage(call__10);
                        }
                        else
                        {
                            var call__11 = TranslateRStr(XUnitsTrainedRStr);
                            var call__12 = Rtl.Rtl.Format(call__11, [IntToStr(arenaTE.FUnitIDs.Count)]);
                            AoWHSMap.ShowMessage(call__12);
                        }
                    }
                }
            }

            return Result;
        }

        // Arena.pas:98
        public override bool CanDblClick()
        {
            bool Result = default;
            var call__13 = CanEnter(AoWHSMap.FSeatedPlayer);
            Result = call__13;
            return Result;
        }

        // Arena.pas:101
        public int UnitTrainCost(TAbstractUnit AUnit)
        {
            int Result = default;
            if (AUnit is THero)
            {
                // { The original rejects heroes, although ExecuteTrainUnit supports them. }
                Result = -1;
                return Result;
            }

            if (AUnit is TUnit)
            {
                var unit = (TUnit)AUnit;
                if (unit.GetRank() != TUnitRank.urGold)
                {
                    var call__15 = unit.GetNextLevelExperience();
                    var call__16 = AUnit.GetExperience();
                    Result = (call__15 - call__16) * 5;
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
        public void ExecuteTrainUnit(TAbstractUnit AUnit)
        {
            if (AUnit is THero)
            {
                var hero = (THero)AUnit;
                var call__18 = hero.GetNextLevelExperience();
                hero.SetExperience(call__18);
                hero.Changed();

                return;
            }

            if (AUnit is TUnit)
            {
                var unit2 = (TUnit)AUnit;
                var call__20 = unit2.GetNextLevelExperience();
                unit2.SetExperience(call__20);
                unit2.Changed();
            }
        }

        // Arena.pas:159
        public bool CanTrainUnits(sbyte Player, TIntegerList Units, ref AnsiString ErrorText)
        {
            bool Result = default;
            int Total = default;
            int I = default;
            int Cost = default;
            TAbstractUnit AUnit = default;
            if (Units.Count != 0)
            {
                Total = 0;
                I = Units.Count;
                Result = true;
                while (Result && I != 0)
                {
                    I -= 1;
                    var call__21 = Units[I];
                    var call__22 = AoWHSMap.FUnitControl.FindUnit(call__21);
                    AUnit = call__22;
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
                    var call__23 = AoWHSMap.FPlayers.GetPlayers(Player);
                    if (call__23.FGems >= Total)
                    {
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                        ErrorText = TranslateRStr(NotEnoughGoldRStr);
                    }
                }
            }
            else
            {
                Result = false;
                ErrorText = TranslateRStr(NoUnitSelectedRStr);
            }

            return Result;
        }

        // Arena.pas:194
        public bool TrainUnits(sbyte Player, TIntegerList Units, ref AnsiString ErrorText)
        {
            bool Result = default;
            TArenaTE TE = default;
            ErrorText = (AnsiString)"";
            var call__24 = CanTrainUnits(Player, Units, ref ErrorText);
            bool bool__25 = call__24;
            if (bool__25)
            {
                var call__26 = AoWHSMap.FPlayers.GetPlayers(Player);
                var call__27 = call__26.GetBusy();
                bool__25 = !call__27;
            }

            if (bool__25)
            {
                TE = (TArenaTE)CreateTE();
                SetupTE(TE);
                TE.FUnitIDs.Assign(Units);
                TE.FParam = Player;
                TE.FAction = 1;
                var call__28 = AoWHSMap.FTokenManager.GetTokenControl(Player);
                call__28.AddEvent(TE);
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
            bool Result = default;
            TArmyHS Army = default;
            var call__29 = FOwner.FindOwnedChild(131607);
            Army = (TArmyHS)call__29;
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
            TEnterArenaEventLog Log = default;
            TArmyHS Army = default;
            int I = default;
            var call__30 = CanEnter(AoWHSMap.FSeatedPlayer);
            if (call__30)
            {
                Log = new TEnterArenaEventLog(null);
                Log.FArena = this;
                var call__31 = FOwner.FindOwnedChild(131607);
                Army = (TArmyHS)call__31;
                {
                    I = 0;
                    int end__32 = Army.FArmy.Count - 1;
                    for (; I <= end__32; I += 1)
                    {
                        var call__33 = Army.FArmy.GetUnit(I);
                        Log.FUnitIDs.Add(call__33.FUnitID);
                        var call__34 = Army.FArmy.GetUnit(I);
                        var call__35 = UnitTrainCost(call__34);
                        if (call__35 != -1)
                        {
                            Log.FTrainable = (byte)(Log.FTrainable | (1 << I));
                        }
                    }
                }

                Log.Execute(true, TExecuteTrigger.etUser, null);
                Log.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

    }

    public partial class TArenaResource : TStructureResource
    {
        public TArenaResource(TECustomNode owner) : base(owner)
        {
            FCastsShadow = true;
        }

        // Arena.pas:253
        public new const int ClassID = 133277;

        // Arena.pas:244


        // Arena.pas:256
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 133276;
            return Result;
        }

        // Arena.pas:45


        // Arena.pas:250
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(ArenaRStr);
            return Result;
        }

    }

    // FArena, FSelection, FTrainable, FUnitIDs
    public partial class TEnterArenaEventLog : TEventLog
    {
        // Arena.pas:259
        public TEnterArenaEventLog(TECustomNode owner) : base(owner)
        {
            var call__36 = new TIntegerList();
            FUnitIDs = call__36;
        }

        public TArena FArena;
        public byte FSelection;
        public byte FTrainable;
        public TIntegerList FUnitIDs;
        // Arena.pas:265
        public override void Destroy()
        {
            FUnitIDs.Free();
            base.Destroy();
        }

        // Arena.pas:271
        public int GetCount()
        {
            int Result = default;
            Result = FUnitIDs.Count;
            return Result;
        }

        // Arena.pas:274
        public TAbstractUnit GetUnit(int Index)
        {
            TAbstractUnit Result = default;
            if (Index >= 0 && Index < FUnitIDs.Count)
            {
                var call__37 = FUnitIDs[Index];
                var call__38 = AoWHSMap.FUnitControl.FindUnit(call__37);
                Result = call__38;
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
        public bool Train(ref AnsiString ErrorText)
        {
            bool Result = default;
            TIntegerList Units = default;
            int I = default;
            Units = new TIntegerList();
            {
                I = 0;
                int end__39 = FUnitIDs.Count - 1;
                for (; I <= end__39; I += 1)
                {
                    if (((1 << I) & FSelection) != 0)
                    {
                        var call__40 = FUnitIDs[I];
                        Units.Add(call__40);
                    }
                }
            }

            var call__41 = FArena.TrainUnits(AoWHSMap.FSeatedPlayer, Units, ref ErrorText);
            Result = call__41;
            Units.Free();
            return Result;
        }

        // Arena.pas:294
        public bool CanTrain(ref AnsiString ErrorText)
        {
            bool Result = default;
            TIntegerList Units = default;
            int I = default;
            Units = new TIntegerList();
            {
                I = 0;
                int end__42 = FUnitIDs.Count - 1;
                for (; I <= end__42; I += 1)
                {
                    if (((1 << I) & FSelection) != 0)
                    {
                        var call__43 = FUnitIDs[I];
                        Units.Add(call__43);
                    }
                }
            }

            var call__44 = FArena.CanTrainUnits(AoWHSMap.FSeatedPlayer, Units, ref ErrorText);
            Result = call__44;
            Units.Free();
            return Result;
        }

        // Arena.pas:304
        public AnsiString GetSelectionInfo()
        {
            AnsiString Result = default;
            int Total = default;
            int I = default;
            TAbstractUnit AUnit = default;
            Total = 0;
            {
                I = 0;
                int end__45 = FUnitIDs.Count - 1;
                for (; I <= end__45; I += 1)
                {
                    if (((1 << I) & FSelection) != 0)
                    {
                        var call__46 = FUnitIDs[I];
                        var call__47 = AoWHSMap.FUnitControl.FindUnit(call__46);
                        AUnit = call__47;
                        if (AUnit != null)
                        {
                            var call__48 = FArena.UnitTrainCost(AUnit);
                            Total += call__48;
                        }
                    }
                }
            }

            if (Total != 0)
            {
                var call__49 = TranslateRStr(TotalCostXRStr);
                var call__50 = Rtl.Rtl.Format(call__49, [IntToStr(Total)]);
                Result = call__50;
            }
            else
            {
                Result = (AnsiString)"";
            }

            return Result;
        }

    }

    public static partial class Arena
    {
        // Arena.pas:318
        public static void RegisterArena(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TArena>(), EClass.Of<TArenaTE>(), EClass.Of<TArenaResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
