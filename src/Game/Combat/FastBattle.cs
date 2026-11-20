#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.Combat;

// wall siege object count
public class FastBattle(CustomNode owner) : Battle(owner)
{
    // AIGroup.pas:7128
    public new const int ClassId = 131760;

    public int WallSiegeObjectCount { get; set; }

    // AIGroup.pas:7175
    public override CombatUnit CreateCombatUnit() => new FastBattleUnit(null) { Holder = CreationHolder() };

    // AIGroup.pas:7203
    public override bool Setup(byte attackerPlayer, byte attackerRelation, int attackerX, int attackerY, int attackerL, byte defenderPlayer, byte defenderRelation, int X, int Y, int L, bool logActions) => base.Setup(attackerPlayer, attackerRelation, attackerX, attackerY, attackerL, defenderPlayer, defenderRelation, X, Y, L, logActions);

    // AIGroup.pas:7310
    public override CombatResult Execute()
    {
        _ = base.Execute();
        CombatResult = CombatResult.Busy;
        string header;
        if (ValidateWallBreaching())
        {
            if (Sides[(int)CombatSideIndex.Defender].GetUndestroyedConquerObjects() != 0)
            {
                CombatSide side = Sides[(int)CombatSideIndex.Defender];
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
                    header = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CannotInflictDamageText);
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
            header = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CannotBreachWallText);
            SetLogbookHeader(header);
            CombatResult = CombatResult.CannotBreachWall;
        }

        return CombatResult;
    }

    // AIGroup.pas:7160
    public override void Initialize() => base.Initialize();

    // AIGroup.pas:7165
    public override void Activate() => base.Activate();

    // AIGroup.pas:7170
    public override void Deactivate() => base.Deactivate();

    // AIGroup.pas:7144
    public int GetRoundDistanceMax()
    {
        int result = 0;
        int i = CombatData.GetCount();
        while (i != 0)
        {
            i -= 1;
            CombatObject objectValue = CombatData.GetObjects(i);
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
        CombatSide side = Sides[(int)CombatSideIndex.Attacker];
        int i = side.Count;
        bool canCrush = false;
        while (i != 0 && !canCrush)
        {
            i -= 1;
            canCrush = !side.GetObjects(i).Status.Contains(CombatObjectStatusElement.Destroyed) && side.GetObjects(i).WallCombatFeatures().Contains(WallCombatFeaturesElement.WallBreaching);
        }

        return canCrush;
    }

    // AIGroup.pas:7226
    public int GetAttackerBallisticCombatValue()
    {
        int combatValue = 0;
        CombatSide side = Sides[(int)CombatSideIndex.Attacker];
        for (int i = 0, last = side.Count - 1; i <= last; i++)
        {
            if (!side.GetObjects(i).Status.Contains(CombatObjectStatusElement.Destroyed) && side.GetObjects(i).SupportCombatMode(Set<CombatMode>.Of(CombatMode.MediumRange, CombatMode.LongRange)))
            {
                combatValue += side.GetObjects(i).CombatValue;
            }
        }

        return combatValue;
    }

    // AIGroup.pas:7241
    public int GetDefenderBallisticCombatValue()
    {
        int combatValue = 0;
        CombatSide side = Sides[(int)CombatSideIndex.Defender];
        for (int i = 0, last = side.Count - 1; i <= last; i++)
        {
            if (!side.GetObjects(i).Status.Contains(CombatObjectStatusElement.Destroyed) && side.GetObjects(i).SupportCombatMode(Set<CombatMode>.Of(CombatMode.MediumRange, CombatMode.LongRange)))
            {
                combatValue += side.GetObjects(i).CombatValue;
            }
        }

        return combatValue;
    }

    // AIGroup.pas:7256
    public bool ValidateWallBreaching()
    {
        CombatWall wall = CombatData.Wall;
        if (wall is not null && !wall.Status.Contains(CombatObjectStatusElement.Destroyed) && WallSiegeObjectCount == 0)
        {
            CombatResult = CombatResult.CannotBreachWall;
            return false;
        }
        else
        {
            return true;
        }
    }

    // AIGroup.pas:7268
    public void ExecuteCombatRound()
    {
        NewRound();
        bool idle = true;
        CombatObjectList objects = new();
        int round = Round;
        SetLogbookHeader(AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_roundFormat), arguments: [IntToStr(round)]));
        for (int playerIndex = 0, last = CombatData.TurnOrder.Count - 1; playerIndex <= last; playerIndex++)
        {
            NewTurn(CombatData.TurnOrder[playerIndex]);
            objects.Clear();
            int i = CombatData.GetCount() - 1;
            for (; i >= 0; i -= 1)
            {
                CombatObject objectValue = CombatData.GetObjects(i);
                if (objectValue.CombatPlayer == (sbyte)CombatData.CurrentPlayer && objectValue.GetEnabled() && !objectValue.GetLocked() && !objectValue.Status.Contains(CombatObjectStatusElement.TurnDone))
                {
                    _ = objects.Add(objectValue);
                }
            }

            int moveCost = 0;
            do
            {
                i = objects.Count;
                while (i != 0)
                {
                    i -= 1;
                    if (objects.GetObject(i).GetEnabled())
                    {
                        if (objects.GetObject(i).FastBattleExecute(moveCost))
                        {
                            idle = false;
                            objects.Delete(i);
                        }
                    }
                }

                moveCost += 1;
            }
            while (!(moveCost >= 3));
        }

        if (idle)
        {
            if (Round > GetRoundDistanceMax())
            {
                Terminate();
            }
        }

        objects?.Dispose();
    }

    private static readonly string s_roundFormat = "Round: %s";
}
