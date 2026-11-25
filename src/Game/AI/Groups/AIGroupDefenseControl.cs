#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.AI.Groups;

// selector, custom state, mode, defend mode
public class AIGroupDefenseControl : AIGroupTask
{
    // AIGroup.pas:1807
    public AIGroupDefenseControl(CustomNode? owner) : base(owner)
    {
        _selector = new GroupDefenseSelector(this);
        _mode = Modes.None;
        DefendMode = GroupDefendMode.Wait;
        State = 0;
    }

    private readonly GroupDefenseSelector _selector;

    public int CustomState { get; init; }

    private readonly Modes _mode;

    public GroupDefendMode DefendMode { get; init; }

    // AIGroup.pas:1816
    public override void Dispose()
    {
        _selector?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:1822
    public override bool Busy => State != -1 && Status.Contains(AIGroupTaskStatuses.Activated);

    // AIGroup.pas:1827
    public override bool Activate(AIGroupControl control)
    {
        GroupControl = control;
        State = 0;
        return true;
    }

    // AIGroup.pas:1834
    public override bool Process()
    {
        switch (State)
        {
            case 0:
                {
                    AIGroupControl aiGroupControl = GroupControl;
                    _ = _selector.Execute(aiGroupControl.Group, aiGroupControl.Location.X, aiGroupControl.Location.Y, L: aiGroupControl.Location.MapLevel, units: null);
                }

                if (_selector.Defenders.Count != 0)
                {
                    switch (DefendMode)
                    {
                        case GroupDefendMode.Split:
                            UnitUserList units = new(null) { Holder = CreationHolder() };
                            {
                                AIGroupControl aiGroupControl = GroupControl;
                                _ = _selector.GetDefenders(aiGroupControl.Group.Player, aiGroupControl.Location.X, aiGroupControl.Location.Y, L: aiGroupControl.Location.MapLevel, units);
                            }

                            int index = units.IndexOfBehavior(GroupBehavior.Passive);
                            AIGroup? group;
                            if (index == -1)
                            {
                                group = GetEngine().StrategicMap.AIGroupManager.CreateGroup(GroupControl.Group.Player, relation: GroupControl.Group.IndependentRelation, behavior: GroupBehavior.Passive);
                                _ = group.AddUnits(_selector.Defenders.Units);
                                _ = group.SetupTarget(GroupControl.Location, movePoints: 0, targetType: AIGroupTargetStatus.None, mode: 2, transporterId: -1, gold: 0, mana: 0);
                                _ = group.Release();
                            }
                            else
                            {
                                group = units.GetUnit(index).Group;
                                _ = group.AddUnits(_selector.Defenders.Units);
                            }

                            units?.Dispose();
                            Done();
                            break;
                        case GroupDefendMode.Wait:
                            GroupControl.Done();
                            if (_mode.Contains(Modes.Produce))
                            {
                                State = 1;
                            }

                            break;
                        case GroupDefendMode.Custom:
                            GroupControl.State = CustomState;
                            Done();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Done();
                }

                break;
            case 1:
                Done();
                break;
            default:
                break;
        }

        return false;
    }

    [Flags]
    public enum Modes : byte
    {
        None = 0,
        Produce = 1 << 0,
        Recruit = 1 << 1,
        OptimizeLocation = 1 << 2,
    }

    public enum GroupDefendMode : byte
    {
        Split = 0, Wait = 1, Custom = 2
    }
}
