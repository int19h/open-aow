// Translated from Mine.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.InteropServices;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Mine
{
    // FState (bit 0 razed), FIncomeSource
    public partial class TMine : TPlayerStructure
    {
        public TMine(TECustomNode owner) : base(owner)
        {
            var call__1 = new TPlayerStructureIncomeSource(null);
            FIncomeSource = call__1;
            FIncomeSource.Setup(this);
        }

        // Mine.pas:82
        public new const int ClassID = 131962;

        public byte FState;
        public TPlayerStructureIncomeSource FIncomeSource;
        // Mine.pas:76
        public override void Destroy()
        {
            base.Destroy();
            FIncomeSource.Free();
        }

        // Mine.pas:172
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwData(Bytes.Of(ref FState, 1), 1, 20);
        }

        // Mine.pas:69


        // Mine.pas:142
        public override void Activate()
        {
            base.Activate();
            if (FPlayer != -1)
            {
                var call__2 = AoWHSMap.FPlayers.GetPlayers((sbyte)(byte)FPlayer);
                call__2.RegisterIncomeSource(FIncomeSource);
            }
        }

        // Mine.pas:150
        public override void Deactivate()
        {
            if (FPlayer != -1)
            {
                var call__3 = AoWHSMap.FPlayers.GetPlayers((sbyte)(byte)FPlayer);
                call__3.UnregisterIncomeSource(FIncomeSource);
            }

            base.Deactivate();
        }

        // Mine.pas:161
        public override void SetRazed(bool Value)
        {
            if (Value)
            {
                SetPlayer(-1);
                FState = (byte)(TSet<byte>.FromMask(FState) + TSet<byte>.WithBounds(0, 7).With(0)).Image;
            }
            else
            {
                FState = (byte)(TSet<byte>.FromMask(FState) - TSet<byte>.WithBounds(0, 7).With(0)).Image;
            }

            var call__4 = GetTerrainTypeImage(TerrainType);
            this.FImageIndex = call__4;
        }

        // Mine.pas:93
        public override AnsiString GetCurrentActivityText()
        {
            AnsiString Result = default;
            var call__5 = GetIncome();
            var call__6 = TranslateRStr(ProducingXIncomeRStr);
            var call__7 = Rtl.Rtl.Format(call__6, [IntToStr(call__5)]);
            Result = call__7;
            return Result;
        }

        // Mine.pas:99
        public override void UpdateAITarget(in TAITargetInfo Info, ref TAITargetData Data)
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
                    case TAITargetMode.tmAttack:
                        bool bool__11 = (byte)Info.Player != (byte)FPlayer;
                        if (bool__11)
                        {
                            var call__12 = GetRazed();
                            bool__11 = !call__12;
                        }

                        if (bool__11)
                        {
                            if ((byte)FPlayer == 0)
                            {
                                CandidatePriority = 50;
                            }
                            else
                            {
                                var call__13 = ValidAttackRelation(Info.Player, Info.IndependentRelation, (sbyte)(byte)FPlayer, 0);
                                if (call__13)
                                {
                                    var call__14 = AoWHSMap.FPlayers.GetPlayers(Info.Player);
                                    CandidatePriority = 50 - call__14.FIncome / 2;
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
                    case TAITargetMode.tmDefend:
                        if ((byte)Info.Player == (byte)FPlayer)
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
                if (FStatus.Contains(TMapObjectStatusElement.moActivated))
                {
                    if (FPlayer != -1)
                    {
                        var call__15 = AoWHSMap.FPlayers.GetPlayers((sbyte)(byte)FPlayer);
                        call__15.UnregisterIncomeSource(FIncomeSource);
                    }

                    base.SetPlayer(Player);
                    if (FPlayer != -1)
                    {
                        var call__16 = AoWHSMap.FPlayers.GetPlayers((sbyte)(byte)FPlayer);
                        call__16.RegisterIncomeSource(FIncomeSource);
                    }
                }
                else
                {
                    base.SetPlayer(Player);
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
        public override AnsiString GetCombatEventText(TCombat Combat)
        {
            AnsiString Result = default;
            if (Combat.FCombatResult == TCombatResult.crVictory || Combat.FCombatResult == TCombatResult.crNoDefenders)
            {
                Result = TranslateRStr(MineBattleWonRStr);
            }
            else
            {
                Result = (AnsiString)"";
            }

            return Result;
        }

        // Mine.pas:25


    }

    public partial class TMineResource : TPlayerStructureResource
    {
        public TMineResource(TECustomNode owner) : base(owner)
        {
            FTerrainBound = false;
            FClaimed = false;
            FRazeable = true;
            FRazeDefenderBudget = 75;
            FRebuildGold = 25;
            FRebuildDays = 2;
        }

        // Mine.pas:200
        public new const int ClassID = 131963;

        // Mine.pas:186


        // Mine.pas:203
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131962;
            return Result;
        }

        // Mine.pas:41


        // Mine.pas:197
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(MineRStr);
            return Result;
        }

    }

    [StructLayout(LayoutKind.Explicit, Size = 21)]
    public struct TMineCombatData
    {
        [FieldOffset(0)]
        public FixedArray_byte_20 Reserved;
        [FieldOffset(20)]
        public byte Outcome;
    }

    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct TMineImageData
    {
        [FieldOffset(0)]
        public FixedArray_byte_20 Reserved;
        [FieldOffset(20)]
        public int ImageIndex;
    }

    public static partial class Mine
    {
        // Mine.pas:206
        public static int MineIncome(sbyte Player)
        {
            int Result = default;
            TPlayer P = default;
            TPlayerStructure S = default;
            int I = default;
            if (AoWHSMap != null)
            {
                var call__20 = AoWHSMap.FPlayers.GetPlayers(Player);
                P = call__20;
                Result = 0;
                {
                    I = 0;
                    int end__21 = P.FStructures.Count - 1;
                    for (; I <= end__21; I += 1)
                    {
                        var call__22 = P.FStructures.GetStructure(I);
                        S = call__22;
                        var call__23 = S.EClass.ClassID;
                        if (call__23 == 131962 && (sbyte)(byte)S.FPlayer == Player)
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
        public static void RegisterMine(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TMine>(), EClass.Of<TMineResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
