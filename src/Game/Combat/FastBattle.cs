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
    public new const int ClassId = 131760;

    // AIGroup.pas:7118
    public FastBattle(ECustomNode owner) : base(owner)
    {
    }

    public int WallSiegeObjectCount { get; set; }

    // AIGroup.pas:7175
    public override CombatUnit CreateCombatUnit() => new FastBattleUnit(null);

    // AIGroup.pas:7203
    public override bool Setup(byte attackerPlayer, byte attackerRelation, int attackerX, int attackerY, int attackerL, byte defenderPlayer, byte defenderRelation, int X, int Y, int L, bool logActions) => base.Setup(attackerPlayer, attackerRelation, attackerX, attackerY, attackerL, defenderPlayer, defenderRelation, X, Y, L, logActions);

    // AIGroup.pas:7310
    public override CombatResult Execute()
    {
        string header;
        CombatSide side;
        base.Execute();
        CombatResult = CombatResult.Busy;
        if (ValidateWallBreaching())
        {
            if (Sides[(int)CombatSideIndex.Defender].GetUndestroyedConquerObjects() != 0)
            {
                side = Sides[(int)CombatSideIndex.Defender];
                if (side.Count > side.ConquerCount)
                {
                    while (!Terminated)
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
                    header = GetCombatResultText();
                    SetLogbookHeader(header);
                }
                else
                {
                    CombatResult = CombatResult.CannotInflictDamage;
                    header = AbilityRules.TranslateResourceString(GameText.CannotInflictDamageText);
                    SetLogbookHeader(header);
                }
            }
            else
            {
                CombatResult = CombatResult.NoDefenders;
            }
        }
        else
        {
            header = AbilityRules.TranslateResourceString(GameText.CannotBreachWallText);
            SetLogbookHeader(header);
            CombatResult = CombatResult.CannotBreachWall;
        }

        return CombatResult;
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
        CombatObject objectValue;
        int result = 0;
        I = CombatData.GetCount();
        while (I != 0)
        {
            I -= 1;
            objectValue = CombatData.GetObjects(I);
            if (objectValue.GetEnabled())
            {
                if (result < objectValue.FastBattleRoundDistance())
                {
                    result = objectValue.FastBattleRoundDistance();
                }
            }
        }

        return result;
    }

    // AIGroup.pas:7208
    public bool AttackerCanCrushWall()
    {
        CombatSide side;
        int I;
        bool canCrush;
        side = Sides[(int)CombatSideIndex.Attacker];
        I = side.Count;
        canCrush = false;
        while (I != 0 && !canCrush)
        {
            I -= 1;
            canCrush = !side.GetObjects(I).Status.Contains(CombatObjectStatusElement.Destroyed) && side.GetObjects(I).WallCombatFeatures().Contains(WallCombatFeaturesElement.WallBreaching);
        }

        return canCrush;
    }

    // AIGroup.pas:7226
    public int GetAttackerBallisticCombatValue()
    {
        int combatValue;
        CombatSide side;
        combatValue = 0;
        side = Sides[(int)CombatSideIndex.Attacker];
        for (int I = 0, last = side.Count - 1; I <= last; I++)
        {
            if (!side.GetObjects(I).Status.Contains(CombatObjectStatusElement.Destroyed) && side.GetObjects(I).SupportCombatMode(Set<CombatMode>.Of(CombatMode.MediumRange, CombatMode.LongRange)))
            {
                combatValue += side.GetObjects(I).CombatValue;
            }
        }

        return combatValue;
    }

    // AIGroup.pas:7241
    public int GetDefenderBallisticCombatValue()
    {
        int combatValue;
        CombatSide side;
        combatValue = 0;
        side = Sides[(int)CombatSideIndex.Defender];
        for (int I = 0, last = side.Count - 1; I <= last; I++)
        {
            if (!side.GetObjects(I).Status.Contains(CombatObjectStatusElement.Destroyed) && side.GetObjects(I).SupportCombatMode(Set<CombatMode>.Of(CombatMode.MediumRange, CombatMode.LongRange)))
            {
                combatValue += side.GetObjects(I).CombatValue;
            }
        }

        return combatValue;
    }

    // AIGroup.pas:7256
    public bool ValidateWallBreaching()
    {
        bool result;
        CombatWall wall;
        wall = CombatData.Wall;
        if (wall != null && !wall.Status.Contains(CombatObjectStatusElement.Destroyed) && WallSiegeObjectCount == 0)
        {
            CombatResult = CombatResult.CannotBreachWall;
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // AIGroup.pas:7268
    public void ExecuteCombatRound()
    {
        bool idle;
        int I;
        int moveCost;
        CombatObjectList objects;
        CombatObject objectValue;
        NewRound();
        idle = true;
        objects = new CombatObjectList();
        var round = GetRound();
        SetLogbookHeader(AbilityRules.Format(AbilityRules.TranslateResourceString(s_roundFormat), [IntToStr(round)]));
        for (int playerIndex = 0, last = CombatData.TurnOrder.Count - 1; playerIndex <= last; playerIndex++)
        {
            NewTurn(CombatData.TurnOrder[playerIndex]);
            objects.Clear();
            I = CombatData.GetCount() - 1;
            for (; I >= 0; I -= 1)
            {
                objectValue = CombatData.GetObjects(I);
                if (objectValue.CombatPlayer == (sbyte)CombatData.CurrentPlayer && objectValue.GetEnabled() && !objectValue.GetLocked() && !objectValue.Status.Contains(CombatObjectStatusElement.TurnDone))
                {
                    objects.Add(objectValue);
                }
            }

            moveCost = 0;
            do
            {
                I = objects.Count;
                while (I != 0)
                {
                    I -= 1;
                    if (objects.GetObject(I).GetEnabled())
                    {
                        if (objects.GetObject(I).FastBattleExecute(moveCost))
                        {
                            idle = false;
                            objects.Delete(I);
                        }
                    }
                }

                moveCost += 1;
            }
            while (!(moveCost >= 3));
        }

        if (idle)
        {
            if (GetRound() > GetRoundDistanceMax())
            {
                Terminate();
            }
        }

        objects?.Dispose();
    }

    private static readonly string s_roundFormat = "Round: %s";

    // intf_P5_07.inc:1073

    // intf_P5_07.inc:1073

}
