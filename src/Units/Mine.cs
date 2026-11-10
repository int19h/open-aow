// Translated from Mine.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Ai;
using Aow.Game.Combat;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// FState (bit 0 razed), FIncomeSource
public partial class Mine : PlayerStructure
{
    public Mine(ECustomNode owner) : base(owner)
    {
        FIncomeSource = new PlayerStructureIncomeSource(null);
        FIncomeSource.Setup(this);
    }

    // Mine.pas:82
    public new const int ClassID = 131962;

    public byte FState;
    public PlayerStructureIncomeSource FIncomeSource;

    // Mine.pas:76
    public override void Dispose()
    {
        base.Dispose();
        FIncomeSource?.Dispose();
    }

    // Mine.pas:172
    public override void ReadWrite(EStorageStream Stream)
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
            AoWHSMap.Current.FPlayers.GetPlayers((sbyte)(byte)FPlayer).RegisterIncomeSource(FIncomeSource);
        }
    }

    // Mine.pas:150
    public override void Deactivate()
    {
        if (FPlayer != -1)
        {
            AoWHSMap.Current.FPlayers.GetPlayers((sbyte)(byte)FPlayer).UnregisterIncomeSource(FIncomeSource);
        }

        base.Deactivate();
    }

    // Mine.pas:161
    public override void SetRazed(bool Value)
    {
        if (Value)
        {
            SetPlayer(-1);
            FState = (byte)(Set<byte>.FromMask(FState) + Set<byte>.WithBounds(0, 7).With(0)).Image;
        }
        else
        {
            FState = (byte)(Set<byte>.FromMask(FState) - Set<byte>.WithBounds(0, 7).With(0)).Image;
        }

        this.FImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Mine.pas:93
    public override string GetCurrentActivityText()
    {
        var income = GetIncome();
        return RtlUnit.Format(AbilityRules.TranslateRStr(GameText.ProducingXIncomeRStr), [IntToStr(income)]);
    }

    // Mine.pas:99
    public override void UpdateAITarget(in AITargetInfo Info, ref AITargetData Data)
    {
        int CandidatePriority;
        if (!GetRazed() || GetBuilding())
        {
            CandidatePriority = Data.Priority;
            switch (Info.Mode)
            {
                case AITargetMode.Attack:
                    if ((byte)Info.Player != (byte)FPlayer && !GetRazed())
                    {
                        if ((byte)FPlayer == 0)
                        {
                            CandidatePriority = 50;
                        }
                        else
                        {
                            if (AbilityRules.ValidAttackRelation(Info.Player, Info.IndependentRelation, (sbyte)(byte)FPlayer, 0))
                            {
                                CandidatePriority = 50 - AoWHSMap.Current.FPlayers.GetPlayers(Info.Player).FIncome / 2;
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
                case AITargetMode.Defend:
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
            if (FStatus.Contains(MapObjectStatusElement.Activated))
            {
                if (FPlayer != -1)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers((sbyte)(byte)FPlayer).UnregisterIncomeSource(FIncomeSource);
                }

                base.SetPlayer(Player);
                if (FPlayer != -1)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers((sbyte)(byte)FPlayer).RegisterIncomeSource(FIncomeSource);
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
    public override string GetCombatEventText(Combat.Battle Combat)
    {
        string Result;
        if (Combat.FCombatResult == CombatResult.Victory || Combat.FCombatResult == CombatResult.NoDefenders)
        {
            Result = AbilityRules.TranslateRStr(MineBattleWonRStr);
        }
        else
        {
            Result = "";
        }

        return Result;
    }

    // Mine.pas:25
    public override bool GetRazed() => (FState & 1) != 0;

    private static readonly string MineBattleWonRStr = "Mine captured";

    public new partial class Resource : PlayerStructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
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
        public override string ResourceEditName() => AbilityRules.TranslateRStr(MineRStr);

        private static readonly string MineRStr = "Mine";

    }
}

[StructLayout(LayoutKind.Explicit, Size = 21)]
public struct MineCombatData
{
    [FieldOffset(0)]
    public FixedArray_byte_20 Reserved;
    [FieldOffset(20)]
    public byte Outcome;
}

[StructLayout(LayoutKind.Explicit, Size = 24)]
public struct MineImageData
{
    [FieldOffset(0)]
    public FixedArray_byte_20 Reserved;
    [FieldOffset(20)]
    public int ImageIndex;
}
