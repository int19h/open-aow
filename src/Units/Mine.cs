// Translated from Mine.pas.
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Mine
{
    // { TMine (instance size 68, VMT 557B4020): +$3C FState (bit 0 razed),
//     +$40 FIncomeSource. }
    public partial class TMine : Aow.Units.AoWE.TPlayerStructure
    {
        public byte FState;
        public Aow.Units.FixedArray_byte_3 FReserved3D;
        public Aow.Units.AoWE.TPlayerStructureIncomeSource FIncomeSource;
        // Mine.pas:76
        public override void Destroy()
        {
            base.Destroy();
            FIncomeSource.Free();
        }

        // Mine.pas:172
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FState, 1), 1, 20);
        }

        // Mine.pas:69
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__1 = Aow.Units.AoWE.TPlayerStructureIncomeSource.Create(null);
            FIncomeSource = call__1;
            FIncomeSource.Setup(this);
        }

        // Mine.pas:142
        public override void Activate()
        {
            base.Activate();
            if (FPlayer != -1)
            {
                var call__2 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(unchecked((sbyte)unchecked((byte)FPlayer)));
                call__2.RegisterIncomeSource(FIncomeSource);
            }
        }

        // Mine.pas:150
        public override void Deactivate()
        {
            if (FPlayer != -1)
            {
                var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(unchecked((sbyte)unchecked((byte)FPlayer)));
                call__3.UnregisterIncomeSource(FIncomeSource);
            }

            base.Deactivate();
        }

        // Mine.pas:161
        public override void SetRazed(bool Value)
        {
            if (Value)
            {
                SetPlayer(unchecked((sbyte)-1));
                FState = unchecked((byte)((byte)(TSet<byte>.FromMask((ulong)FState) + TSet<byte>.WithBounds(0, 7).With(0)).Image));
            }
            else
            {
                FState = unchecked((byte)((byte)(TSet<byte>.FromMask((ulong)FState) - TSet<byte>.WithBounds(0, 7).With(0)).Image));
            }

            var call__4 = GetTerrainTypeImage(TerrainType);
            this.FImageIndex = call__4;
        }

        // Mine.pas:93
        public override Aow.Rtl.AnsiString GetCurrentActivityText()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__5 = GetIncome();
            var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProducingXIncomeRStr);
            var call__7 = Aow.Rtl.Rtl.Format(call__6, [Aow.Rtl.Rtl.IntToStr(call__5)]);
            Result = call__7;
            return Result;
        }

        // Mine.pas:99
        public override void UpdateAITarget(in Aow.Units.AoWE.TAITargetInfo Info, ref Aow.Units.AoWE.TAITargetData Data)
        {
            int CandidatePriority = default;
            var call__8 = GetRazed();
            bool bool__9 = !call__8;
            if (!bool__9)
            {
                var call__10 = GetBuilding();
                bool__9 = call__10;
            }

            if (bool__9)
            {
                CandidatePriority = Data.Priority;
                switch (Info.Mode)
                {
                    case Aow.Units.AoWE.TAITargetMode.tmAttack:
                        bool bool__11 = (unchecked((byte)Info.Player) != unchecked((byte)FPlayer));
                        if (bool__11)
                        {
                            var call__12 = GetRazed();
                            bool__11 = !call__12;
                        }

                        if (bool__11)
                        {
                            if (unchecked((byte)FPlayer) == 0)
                            {
                                CandidatePriority = 50;
                            }
                            else
                            {
                                var call__13 = Aow.Units.AoWE.AoWE.ValidAttackRelation(unchecked((sbyte)Info.Player), unchecked((sbyte)Info.IndependentRelation), unchecked((sbyte)unchecked((sbyte)unchecked((byte)FPlayer))), 0);
                                if (call__13)
                                {
                                    var call__14 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Info.Player);
                                    CandidatePriority = 50 - (call__14.FIncome / 2);
                                    if (CandidatePriority < 25)
                                    {
                                        CandidatePriority = 25;
                                    }

                                    if (CandidatePriority > 75)
                                    {
                                        CandidatePriority = 75;
                                    }
                                }
                            }
                        }

                        break;
                    case Aow.Units.AoWE.TAITargetMode.tmDefend:
                        if (unchecked((byte)Info.Player) == unchecked((byte)FPlayer))
                        {
                            Data.Priority = 20;
                        }

                        break;
                }

                // { Original can overwrite the special kind-6 priority with the previous higher value. }
                if (Data.Priority < CandidatePriority)
                {
                    Data.Priority = CandidatePriority;
                }
            }
        }

        // Mine.pas:126
        public override void SetPlayer(sbyte Player)
        {
            if (Player != FPlayer)
            {
                if (FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moActivated))
                {
                    if (FPlayer != -1)
                    {
                        var call__15 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(unchecked((sbyte)unchecked((byte)FPlayer)));
                        call__15.UnregisterIncomeSource(FIncomeSource);
                    }

                    base.SetPlayer(unchecked((sbyte)Player));
                    if (FPlayer != -1)
                    {
                        var call__16 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(unchecked((sbyte)unchecked((byte)FPlayer)));
                        call__16.RegisterIncomeSource(FIncomeSource);
                    }
                }
                else
                {
                    base.SetPlayer(unchecked((sbyte)Player));
                }
            }
        }

        // Mine.pas:85
        public override int GetIncome()
        {
            int Result = default;
            var call__17 = GetRazed();
            bool bool__18 = !call__17;
            if (bool__18)
            {
                var call__19 = GetBuilding();
                bool__18 = !call__19;
            }

            if (bool__18)
            {
                Result = 10;
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // Mine.pas:178
        public override Aow.Rtl.AnsiString GetCombatEventText(Aow.Units.AoWE.TCombat Combat)
        {
            Aow.Rtl.AnsiString Result = default;
            if ((Combat.FCombatResult == Aow.Units.AoWE.TCombatResult.crVictory) || (Combat.FCombatResult == Aow.Units.AoWE.TCombatResult.crNoDefenders))
            {
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MineBattleWonRStr);
            }
            else
            {
                Result = (AnsiString)"";
            }

            return Result;
        }

        // Mine.pas:25
        public static new Aow.Units.Mine.TMine Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Mine.TMine o = new Aow.Units.Mine.TMine();
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

        public new partial class Meta : Aow.Units.AoWE.TPlayerStructure.Meta
        {
            // Mine.pas:82
            public override int ClassID()
            {
                int Result = default;
                Result = 131962;
                return Result;
            }

            // Mine.pas:25
            public new Aow.Units.Mine.TMine Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Mine.TMine o = (Aow.Units.Mine.TMine)NewInstance();
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
                where T : Aow.Units.Mine.TMine => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TMineResource (instance size 116, VMT 557B42A8) }
    public partial class TMineResource : Aow.Units.AoWE.TPlayerStructureResource
    {
        // Mine.pas:186
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FTerrainBound = false;
            FClaimed = false;
            FRazeable = true;
            FRazeDefenderBudget = 75;
            FRebuildGold = 25;
            FRebuildDays = 2;
        }

        // Mine.pas:203
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131962;
            return Result;
        }

        // Mine.pas:41
        public static new Aow.Units.Mine.TMineResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Mine.TMineResource o = new Aow.Units.Mine.TMineResource();
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

        public new partial class Meta : Aow.Units.AoWE.TPlayerStructureResource.Meta
        {
            // Mine.pas:200
            public override int ClassID()
            {
                int Result = default;
                Result = 131963;
                return Result;
            }

            // Mine.pas:197
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MineRStr);
                return Result;
            }

            // Mine.pas:41
            public new Aow.Units.Mine.TMineResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Mine.TMineResource o = (Aow.Units.Mine.TMineResource)NewInstance();
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
                where T : Aow.Units.Mine.TMineResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 21)]
    public struct TMineCombatData
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_20 Reserved;
        [FieldOffset(20)]
        public byte Outcome;
    }

    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct TMineImageData
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_20 Reserved;
        [FieldOffset(20)]
        public int ImageIndex;
    }

    public static partial class Mine
    {
        // Mine.pas:206
        public static int MineIncome(sbyte Player)
        {
            int Result = default;
            Aow.Units.AoWE.TPlayer P = default;
            Aow.Units.AoWE.TPlayerStructure S = default;
            int I = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                var call__20 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                P = call__20;
                Result = 0;
                {
                    I = 0;
                    int end__21 = P.FStructures.Count - 1;
                    for (; I <= end__21; I += 1)
                    {
                        var call__22 = P.FStructures.GetStructure(I);
                        S = call__22;
                        var call__23 = S.ClassType.ClassID();
                        if ((call__23 == 131962) && (unchecked((sbyte)unchecked((byte)S.FPlayer)) == Player))
                        {
                            Result += S.GetIncome();
                        }
                    }
                }
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // Mine.pas:226
        // { draft }
        public static void RegisterMine(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Mine.TMine>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Mine.TMineResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
