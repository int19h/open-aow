#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.Combat;

// wall siege object count
public partial class FastBattle(CustomNode owner) : Battle(owner)
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
        string header;
        CombatSide side;
        _ = base.Execute();
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
                    header = AbilityRules.TranslateResourceString(GetEngine(), GameText.CannotInflictDamageText);
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
            header = AbilityRules.TranslateResourceString(GetEngine(), GameText.CannotBreachWallText);
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
        int i;
        CombatObject objectValue;
        int result = 0;
        i = CombatData.GetCount();
        while (i != 0)
        {
            i -= 1;
            objectValue = CombatData.GetObjects(i);
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
        int i;
        bool canCrush;
        side = Sides[(int)CombatSideIndex.Attacker];
        i = side.Count;
        canCrush = false;
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
        int combatValue;
        CombatSide side;
        combatValue = 0;
        side = Sides[(int)CombatSideIndex.Attacker];
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
        int combatValue;
        CombatSide side;
        combatValue = 0;
        side = Sides[(int)CombatSideIndex.Defender];
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
        bool result;
        CombatWall wall;
        wall = CombatData.Wall;
        if (wall is not null && !wall.Status.Contains(CombatObjectStatusElement.Destroyed) && WallSiegeObjectCount == 0)
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
        int i;
        int moveCost;
        CombatObjectList objects;
        CombatObject objectValue;
        NewRound();
        idle = true;
        objects = new CombatObjectList();
        int round = GetRound();
        SetLogbookHeader(AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_roundFormat), [IntToStr(round)]));
        for (int playerIndex = 0, last = CombatData.TurnOrder.Count - 1; playerIndex <= last; playerIndex++)
        {
            NewTurn(CombatData.TurnOrder[playerIndex]);
            objects.Clear();
            i = CombatData.GetCount() - 1;
            for (; i >= 0; i -= 1)
            {
                objectValue = CombatData.GetObjects(i);
                if (objectValue.CombatPlayer == (sbyte)CombatData.CurrentPlayer && objectValue.GetEnabled() && !objectValue.GetLocked() && !objectValue.Status.Contains(CombatObjectStatusElement.TurnDone))
                {
                    _ = objects.Add(objectValue);
                }
            }

            moveCost = 0;
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
