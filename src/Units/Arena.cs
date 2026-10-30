// Translated from Arena.pas.
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

namespace Aow.Units.Arena
{
    // { TArenaTE: size $2C; $28 FUnitIDs. }
    public partial class TArenaTE : Aow.Units.AoWE.TStructureTE
    {
        public TArenaTE(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__1 = new Aow.Units.Engine.TIntegerList();
            FUnitIDs = call__1;
        }

        // Arena.pas:86
        public new const int ClassID = 133294;

        public Aow.Units.Engine.TIntegerList FUnitIDs;
        // Arena.pas:80
        public override void Destroy()
        {
            FUnitIDs.Free();
            base.Destroy();
        }

        // Arena.pas:89
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStreamProc(40, FUnitIDs.LoadFromStream, FUnitIDs.SaveToStream);
        }

        // Arena.pas:74


        // Arena.pas:23


        public new partial class Meta : Aow.Units.AoWE.TStructureTE.Meta
        {
            // Arena.pas:86
            public override int ClassID() => Aow.Units.Arena.TArenaTE.ClassID;

            // Arena.pas:23
            public new Aow.Units.Arena.TArenaTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Arena.TArenaTE)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.Arena.TArenaTE => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TArena (instance size 48, VMT 557D6240) }
    public partial class TArena : Aow.Units.AoWE.TStructure
    {
        public TArena(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Arena.pas:95
        public new const int ClassID = 133276;

        // Arena.pas:212
        public override Aow.Units.AoWE.TStructureTE CreateTE()
        {
            Aow.Units.AoWE.TStructureTE Result = default;
            Result = new Aow.Units.Arena.TArenaTE(null);
            return Result;
        }

        // Arena.pas:135
        public override Aow.Units.NetworkE.TExecuteResult ExecuteTE(Aow.Units.AoWE.TStructureTE A1, Aow.Units.NetworkE.TTokenExecuter Executer)
        {
            Aow.Units.NetworkE.TExecuteResult Result = default;
            int I = default;
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            Aow.Rtl.AnsiString ErrorText = default;
            Result = base.ExecuteTE(A1, Executer);
            if (A1.FAction == 1)
            {
                var call__2 = CanTrainUnits(unchecked((sbyte)A1.FParam), ((Aow.Units.Arena.TArenaTE)A1).FUnitIDs, ref ErrorText);
                if (call__2)
                {
                    {
                        I = 0;
                        int end__3 = ((Aow.Units.Arena.TArenaTE)A1).FUnitIDs.Count - 1;
                        for (; I <= end__3; I += 1)
                        {
                            var call__4 = ((Aow.Units.Arena.TArenaTE)A1).FUnitIDs[I];
                            var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(call__4);
                            AUnit = call__5;
                            if (AUnit != null)
                            {
                                var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FParam);
                                var call__7 = UnitTrainCost(AUnit);
                                var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FParam);
                                call__8.SetGems(call__6.FGems - call__7);
                                ExecuteTrainUnit(AUnit);
                            }
                        }
                    }

                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FParam)
                    {
                        var arenaTE = (Aow.Units.Arena.TArenaTE)A1;
                        if (arenaTE.FUnitIDs.Count == 1)
                        {
                            var call__10 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.OneUnitTrainedRStr);
                            Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(call__10);
                        }
                        else
                        {
                            var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XUnitsTrainedRStr);
                            var call__12 = Aow.Rtl.Rtl.Format(call__11, [Aow.Rtl.Rtl.IntToStr(arenaTE.FUnitIDs.Count)]);
                            Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(call__12);
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
            var call__13 = CanEnter(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Result = call__13;
            return Result;
        }

        // Arena.pas:101
        public int UnitTrainCost(Aow.Units.AoWE.TAbstractUnit AUnit)
        {
            int Result = default;
            if (AUnit is Aow.Units.AoWE.THero)
            {
                // { The original rejects heroes, although ExecuteTrainUnit supports them. }
                Result = -1;
                return Result;
            }

            if (AUnit is Aow.Units.AoWE.TUnit)
            {
                var unit = (Aow.Units.AoWE.TUnit)AUnit;
                if (unit.GetRank() != Aow.Units.AoWE.TUnitRank.urGold)
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
        public void ExecuteTrainUnit(Aow.Units.AoWE.TAbstractUnit AUnit)
        {
            // { Unmatched: tried Pointer identity assignment to unit; Pointer cast branch receivers; explicit incoming unit local. }
            if (AUnit is Aow.Units.AoWE.THero)
            {
                var hero = (Aow.Units.AoWE.THero)AUnit;
                var call__18 = hero.GetNextLevelExperience();
                hero.SetExperience(call__18);
                hero.Changed();

                return;
            }

            if (AUnit is Aow.Units.AoWE.TUnit)
            {
                var unit2 = (Aow.Units.AoWE.TUnit)AUnit;
                var call__20 = unit2.GetNextLevelExperience();
                unit2.SetExperience(call__20);
                unit2.Changed();
            }
        }

        // Arena.pas:159
        public bool CanTrainUnits(sbyte Player, Aow.Units.Engine.TIntegerList Units, ref Aow.Rtl.AnsiString ErrorText)
        {
            bool Result = default;
            int Total = default;
            int I = default;
            int Cost = default;
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            if (Units.Count != 0)
            {
                Total = 0;
                I = Units.Count;
                Result = true;
                while (Result && (I != 0))
                {
                    I -= 1;
                    var call__21 = Units[I];
                    var call__22 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(call__21);
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
                    var call__23 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    if (call__23.FGems >= Total)
                    {
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                        ErrorText = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NotEnoughGoldRStr);
                    }
                }
            }
            else
            {
                Result = false;
                ErrorText = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoUnitSelectedRStr);
            }

            return Result;
        }

        // Arena.pas:194
        public bool TrainUnits(sbyte Player, Aow.Units.Engine.TIntegerList Units, ref Aow.Rtl.AnsiString ErrorText)
        {
            bool Result = default;
            Aow.Units.Arena.TArenaTE TE = default;
            ErrorText = (AnsiString)"";
            var call__24 = CanTrainUnits(unchecked((sbyte)Player), Units, ref ErrorText);
            bool bool__25 = call__24;
            if (bool__25)
            {
                var call__26 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                var call__27 = call__26.GetBusy();
                bool__25 = !call__27;
            }

            if (bool__25)
            {
                TE = (Aow.Units.Arena.TArenaTE)CreateTE();
                SetupTE(TE);
                TE.FUnitIDs.Assign(Units);
                TE.FParam = Player;
                TE.FAction = 1;
                var call__28 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(Player);
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
            Aow.Units.AoWE.TArmyHS Army = default;
            var call__29 = FOwner.FindOwnedChild(131607);
            Army = (Aow.Units.AoWE.TArmyHS)call__29;
            if ((Army == null) || (Army.FArmy.FPlayer != Player))
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
            Aow.Units.Arena.TEnterArenaEventLog Log = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            int I = default;
            var call__30 = CanEnter(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            if (call__30)
            {
                Log = Aow.Units.Arena.TEnterArenaEventLog.Create(null);
                Log.FArena = this;
                var call__31 = FOwner.FindOwnedChild(131607);
                Army = (Aow.Units.AoWE.TArmyHS)call__31;
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
                            Log.FTrainable = unchecked((byte)(Log.FTrainable | (1 << I)));
                        }
                    }
                }

                Log.Execute(true, Aow.Units.EventLog.TExecuteTrigger.etUser, null);
                Log.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        public new partial class Meta : Aow.Units.AoWE.TStructure.Meta
        {
            // Arena.pas:95
            public override int ClassID() => Aow.Units.Arena.TArena.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Arena.TArena => (Meta)MetaRegistry.Of(typeof(T));
            // intf_P1_09.inc:78
            public new Aow.Units.Arena.TArena Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Arena.TArena)base.Create(AOwner);
        }
    }

    // { TArenaResource (instance size 100, VMT 557D6498) }
    public partial class TArenaResource : Aow.Units.AoWE.TStructureResource
    {
        public TArenaResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
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


        public new partial class Meta : Aow.Units.AoWE.TStructureResource.Meta
        {
            // Arena.pas:253
            public override int ClassID() => Aow.Units.Arena.TArenaResource.ClassID;

            // Arena.pas:250
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ArenaRStr);
                return Result;
            }

            // Arena.pas:45
            public new Aow.Units.Arena.TArenaResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Arena.TArenaResource)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.Arena.TArenaResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TEnterArenaEventLog: size $24; $18 FArena, $1C FSelection,
//     $1D FTrainable, $20 FUnitIDs. }
    public partial class TEnterArenaEventLog : Aow.Units.EventLog.TEventLog
    {
        public Aow.Units.Arena.TArena FArena;
        public byte FSelection;
        public byte FTrainable;
        public Aow.Units.FixedArray_byte_2 FReserved1E;
        public Aow.Units.Engine.TIntegerList FUnitIDs;
        // Arena.pas:265
        public override void Destroy()
        {
            FUnitIDs.Free();
            base.Destroy();
        }

        // Arena.pas:259
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__36 = new Aow.Units.Engine.TIntegerList();
            FUnitIDs = call__36;
        }

        // Arena.pas:271
        public int GetCount()
        {
            int Result = default;
            Result = FUnitIDs.Count;
            return Result;
        }

        // Arena.pas:274
        public Aow.Units.AoWE.TAbstractUnit GetUnit(int Index)
        {
            Aow.Units.AoWE.TAbstractUnit Result = default;
            if ((Index >= 0) && (Index < FUnitIDs.Count))
            {
                var call__37 = FUnitIDs[Index];
                var call__38 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(call__37);
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
            FSelection = unchecked((byte)(Value & FTrainable));
        }

        // Arena.pas:284
        public bool Train(ref Aow.Rtl.AnsiString ErrorText)
        {
            bool Result = default;
            Aow.Units.Engine.TIntegerList Units = default;
            int I = default;
            Units = new Aow.Units.Engine.TIntegerList();
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

            var call__41 = FArena.TrainUnits(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer), Units, ref ErrorText);
            Result = call__41;
            Units.Free();
            return Result;
        }

        // Arena.pas:294
        public bool CanTrain(ref Aow.Rtl.AnsiString ErrorText)
        {
            bool Result = default;
            Aow.Units.Engine.TIntegerList Units = default;
            int I = default;
            Units = new Aow.Units.Engine.TIntegerList();
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

            var call__44 = FArena.CanTrainUnits(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer), Units, ref ErrorText);
            Result = call__44;
            Units.Free();
            return Result;
        }

        // Arena.pas:304
        public Aow.Rtl.AnsiString GetSelectionInfo()
        {
            Aow.Rtl.AnsiString Result = default;
            int Total = default;
            int I = default;
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            Total = 0;
            {
                I = 0;
                int end__45 = FUnitIDs.Count - 1;
                for (; I <= end__45; I += 1)
                {
                    if (((1 << I) & FSelection) != 0)
                    {
                        var call__46 = FUnitIDs[I];
                        var call__47 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(call__46);
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
                var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TotalCostXRStr);
                var call__50 = Aow.Rtl.Rtl.Format(call__49, [Aow.Rtl.Rtl.IntToStr(Total)]);
                Result = call__50;
            }
            else
            {
                Result = (AnsiString)"";
            }

            return Result;
        }

        // Arena.pas:61
        public static new Aow.Units.Arena.TEnterArenaEventLog Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Arena.TEnterArenaEventLog o = new Aow.Units.Arena.TEnterArenaEventLog();
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

    public static partial class Arena
    {
        // Arena.pas:318
        public static void RegisterArena(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Arena.TArena>(), Aow.Units.Engine.EClass.Of<Aow.Units.Arena.TArenaTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.Arena.TArenaResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
