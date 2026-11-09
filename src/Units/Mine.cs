// Translated from Mine.pas.
using System.Runtime.InteropServices;
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Mine;

// FState (bit 0 razed), FIncomeSource
public partial class TMine : TPlayerStructure
{
    public TMine(TECustomNode owner) : base(owner)
    {
        FIncomeSource = new TPlayerStructureIncomeSource(null);
        FIncomeSource.Setup(this);
    }

    // Mine.pas:82
    public new const int ClassID = 131962;

    public byte FState;
    public TPlayerStructureIncomeSource FIncomeSource;

    // Mine.pas:76
    public override void Dispose()
    {
        base.Dispose();
        FIncomeSource?.Dispose();
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
            AoWHSMap.FPlayers.GetPlayers((sbyte)(byte)FPlayer).RegisterIncomeSource(FIncomeSource);
        }
    }

    // Mine.pas:150
    public override void Deactivate()
    {
        if (FPlayer != -1)
        {
            AoWHSMap.FPlayers.GetPlayers((sbyte)(byte)FPlayer).UnregisterIncomeSource(FIncomeSource);
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

        this.FImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Mine.pas:93
    public override string GetCurrentActivityText()
    {
        var income = GetIncome();
        return RtlUnit.Format(TranslateRStr(ProducingXIncomeRStr), [IntToStr(income)]);
    }

    // Mine.pas:99
    public override void UpdateAITarget(in TAITargetInfo Info, ref TAITargetData Data)
    {
        int CandidatePriority;
        if (!GetRazed() || GetBuilding())
        {
            CandidatePriority = Data.Priority;
            switch (Info.Mode)
            {
                case TAITargetMode.tmAttack:
                    if ((byte)Info.Player != (byte)FPlayer && !GetRazed())
                    {
                        if ((byte)FPlayer == 0)
                        {
                            CandidatePriority = 50;
                        }
                        else
                        {
                            if (ValidAttackRelation(Info.Player, Info.IndependentRelation, (sbyte)(byte)FPlayer, 0))
                            {
                                CandidatePriority = 50 - AoWHSMap.FPlayers.GetPlayers(Info.Player).FIncome / 2;
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
                    AoWHSMap.FPlayers.GetPlayers((sbyte)(byte)FPlayer).UnregisterIncomeSource(FIncomeSource);
                }

                base.SetPlayer(Player);
                if (FPlayer != -1)
                {
                    AoWHSMap.FPlayers.GetPlayers((sbyte)(byte)FPlayer).RegisterIncomeSource(FIncomeSource);
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
        int Result;
        if (!GetRazed() && !GetBuilding())
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
    public override string GetCombatEventText(TCombat Combat)
    {
        string Result;
        if (Combat.FCombatResult == TCombatResult.crVictory || Combat.FCombatResult == TCombatResult.crNoDefenders)
        {
            Result = TranslateRStr(MineBattleWonRStr);
        }
        else
        {
            Result = "";
        }

        return Result;
    }

    // Mine.pas:25
    public override bool GetRazed() => (FState & 1) != 0;
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
    public override int ResourceUserClassID() => 131962;

    // Mine.pas:41

    // Mine.pas:197
    public override string ResourceEditName() => TranslateRStr(MineRStr);

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

public static partial class MineUnit
{
    // Mine.pas:206
    public static int MineIncome(sbyte Player)
    {
        int Result;
        TPlayer P;
        TPlayerStructure S;
        if (AoWHSMap != null)
        {
            P = AoWHSMap.FPlayers.GetPlayers(Player);
            Result = 0;
            for (int I = 0, last = P.FStructures.Count - 1; I <= last; I++)
            {
                S = P.FStructures.GetStructure(I);
                if (S.EClass.ClassID == 131962 && (sbyte)(byte)S.FPlayer == Player)
                {
                    Result += S.GetIncome();
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
