// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// wall siege object count
public partial class FastBattle : Battle
{
    // AIGroup.pas:7128
    public new const int ClassID = 131760;

    // AIGroup.pas:7118
    public FastBattle(ECustomNode owner) : base(owner)
    {
    }

    public int FWallSiegeObjectCount;

    // AIGroup.pas:7175
    public override CombatUnit CreateCombatUnit() => new FastBattleUnit(null);

    // AIGroup.pas:7203
    public override bool Setup(byte AttackerPlayer, byte AttackerRelation, int AttackerX, int AttackerY, int AttackerL, byte DefenderPlayer, byte DefenderRelation, int X, int Y, int L, bool LogActions) => base.Setup(AttackerPlayer, AttackerRelation, AttackerX, AttackerY, AttackerL, DefenderPlayer, DefenderRelation, X, Y, L, LogActions);

    // AIGroup.pas:7310
    public override CombatResult Execute()
    {
        string Header;
        CombatSide Side;
        base.Execute();
        FCombatResult = CombatResult.Busy;
        if (ValidateWallBreaching())
        {
            if (FSides[(int)CombatSideIndex.Defender].GetUndestroyedConquerObjects() != 0)
            {
                Side = FSides[(int)CombatSideIndex.Defender];
                if (Side.Count > Side.FConquerCount)
                {
                    while (!FTerminated)
                    {
                        if (ValidateWallBreaching())
                        {
                            ExecuteCombatRound();
                        }
                        else
                        {
                            Terminate();
                        }

                        CheckTerminate();
                    }

                    UpdateStatus();
                    Header = GetCombatResultText();
                    SetLogbookHeader(Header);
                }
                else
                {
                    FCombatResult = CombatResult.CannotInflictDamage;
                    Header = AbilityRules.TranslateRStr(GameText.CannotInflictDamageRStr);
                    SetLogbookHeader(Header);
                }
            }
            else
            {
                FCombatResult = CombatResult.NoDefenders;
            }
        }
        else
        {
            Header = AbilityRules.TranslateRStr(GameText.CannotBreachWallRStr);
            SetLogbookHeader(Header);
            FCombatResult = CombatResult.CannotBreachWall;
        }

        return FCombatResult;
    }

    // AIGroup.pas:7160
    public override void Initialize()
    {
        base.Initialize();
    }

    // AIGroup.pas:7165
    public override void Activate()
    {
        base.Activate();
    }

    // AIGroup.pas:7170
    public override void Deactivate()
    {
        base.Deactivate();
    }

    // AIGroup.pas:7144
    public int GetRoundDistanceMax()
    {
        int I;
        CombatObject Obj;
        int Result = 0;
        I = FCombatData.GetCount();
        while (I != 0)
        {
            I -= 1;
            Obj = FCombatData.GetObjects(I);
            if (Obj.GetEnabled())
            {
                if (Result < Obj.fcRoundDistance())
                {
                    Result = Obj.fcRoundDistance();
                }
            }
        }

        return Result;
    }

    // AIGroup.pas:7208
    public bool AttackerCanCrushWall()
    {
        CombatSide Side;
        int I;
        bool CanCrush;
        Side = FSides[(int)CombatSideIndex.Attacker];
        I = Side.Count;
        CanCrush = false;
        while (I != 0 && !CanCrush)
        {
            I -= 1;
            CanCrush = !Side.GetObjects(I).FStatus.Contains(CombatObjectStatusElement.Destroyed) && Side.GetObjects(I).WallCombatFeatures().Contains(WallCombatFeaturesElement.WallBreaching);
        }

        return CanCrush;
    }

    // AIGroup.pas:7226
    public int GetAttackerBalisticCV()
    {
        int CV;
        CombatSide Side;
        CV = 0;
        Side = FSides[(int)CombatSideIndex.Attacker];
        for (int I = 0, last = Side.Count - 1; I <= last; I++)
        {
            if (!Side.GetObjects(I).FStatus.Contains(CombatObjectStatusElement.Destroyed) && Side.GetObjects(I).SupportCombatMode(Set<CombatMode>.Of(CombatMode.MediumRange, CombatMode.LongRange)))
            {
                CV += Side.GetObjects(I).FCombatValue;
            }
        }

        return CV;
    }

    // AIGroup.pas:7241
    public int GetDefenderBalisticCV()
    {
        int CV;
        CombatSide Side;
        CV = 0;
        Side = FSides[(int)CombatSideIndex.Defender];
        for (int I = 0, last = Side.Count - 1; I <= last; I++)
        {
            if (!Side.GetObjects(I).FStatus.Contains(CombatObjectStatusElement.Destroyed) && Side.GetObjects(I).SupportCombatMode(Set<CombatMode>.Of(CombatMode.MediumRange, CombatMode.LongRange)))
            {
                CV += Side.GetObjects(I).FCombatValue;
            }
        }

        return CV;
    }

    // AIGroup.pas:7256
    public bool ValidateWallBreaching()
    {
        bool Result;
        CombatWall Wall;
        Wall = FCombatData.FWall;
        if (Wall != null && !Wall.FStatus.Contains(CombatObjectStatusElement.Destroyed) && FWallSiegeObjectCount == 0)
        {
            FCombatResult = CombatResult.CannotBreachWall;
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // AIGroup.pas:7268
    public void ExecuteCombatRound()
    {
        bool Idle;
        int I;
        int MoveCost;
        CombatObjectList Objects;
        CombatObject Obj;
        NewRound();
        Idle = true;
        Objects = new CombatObjectList();
        var round = GetRound();
        SetLogbookHeader(AbilityRules.Format(AbilityRules.TranslateRStr(P5RoundXRStr), [IntToStr(round)]));
        for (int PlayerIndex = 0, last = FCombatData.FTurnOrder.Count - 1; PlayerIndex <= last; PlayerIndex++)
        {
            NewTurn(FCombatData.FTurnOrder[PlayerIndex]);
            Objects.Clear();
            I = FCombatData.GetCount() - 1;
            for (; I >= 0; I -= 1)
            {
                Obj = FCombatData.GetObjects(I);
                if (Obj.FCombatPlayer == (sbyte)FCombatData.FCurrentPlayer && Obj.GetEnabled() && !Obj.GetLocked() && !Obj.FStatus.Contains(CombatObjectStatusElement.TurnDone))
                {
                    Objects.Add(Obj);
                }
            }

            MoveCost = 0;
            do
            {
                I = Objects.Count;
                while (I != 0)
                {
                    I -= 1;
                    if (Objects.GetObject(I).GetEnabled())
                    {
                        if (Objects.GetObject(I).fcExecute(MoveCost))
                        {
                            Idle = false;
                            Objects.Delete(I);
                        }
                    }
                }

                MoveCost += 1;
            }
            while (!(MoveCost >= 3));
        }

        if (Idle)
        {
            if (GetRound() > GetRoundDistanceMax())
            {
                Terminate();
            }
        }

        Objects?.Dispose();
    }

    private static readonly string P5RoundXRStr = "Round: %s";

    // intf_P5_07.inc:1073

    // intf_P5_07.inc:1073

}
