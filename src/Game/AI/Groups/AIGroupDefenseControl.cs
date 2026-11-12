// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// selector, custom state, mode, defend mode
public partial class AIGroupDefenseControl : AIGroupTask
{
    // AIGroup.pas:1807
    public AIGroupDefenseControl(ECustomNode owner) : base(owner)
    {
        _selector = new GroupDefenseSelector();
        _mode = Set<ModeElement>.Of();
        DefendMode = GroupDefendMode.Wait;
        State = 0;
    }

    private readonly GroupDefenseSelector _selector;

    public int CustomState { get; set; }

    private readonly Set<ModeElement> _mode;

    public GroupDefendMode DefendMode { get; set; }

    // AIGroup.pas:1816
    public override void Dispose()
    {
        _selector?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:1822
    public override bool GetBusy() => State != -1 && Status.Contains(AIGroupTaskStatusElement.Activated);

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
        UnitUserList units;
        int index;
        AIGroup group;
        switch (State)
        {
            case 0:
                {
                    var aiGroupControl = GroupControl;
                    _selector.Execute(aiGroupControl.Group, aiGroupControl.Location.X, aiGroupControl.Location.Y, aiGroupControl.Location.MapLevel, null);
                }

                if (_selector.Defenders.Count != 0)
                {
                    switch (DefendMode)
                    {
                        case GroupDefendMode.Split:
                            units = new UnitUserList(null);
                            {
                                var aiGroupControl = GroupControl;
                                _selector.GetDefenders(aiGroupControl.Group.Player, aiGroupControl.Location.X, aiGroupControl.Location.Y, aiGroupControl.Location.MapLevel, units);
                            }

                            index = units.IndexOfBehavior(GroupBehavior.Passive);
                            if (index == -1)
                            {
                                group = StrategicMap.Current.AIGroupManager.CreateGroup(GroupControl.Group.Player, GroupControl.Group.IndependentRelation, GroupBehavior.Passive);
                                group.AddUnits(_selector.Defenders);
                                group.SetupTarget(GroupControl.Location, 0, AIGroupTargetStatus.None, 2, -1, 0, 0);
                                group.Release();
                            }
                            else
                            {
                                group = units.GetUnit(index).Group;
                                group.AddUnits(_selector.Defenders);
                            }

                            units?.Dispose();
                            Done();
                            break;
                        case GroupDefendMode.Wait:
                            GroupControl.Done();
                            if (_mode.Contains(ModeElement.Produce))
                            {
                                State = 1;
                            }

                            break;
                        case GroupDefendMode.Custom:
                            GroupControl.State = CustomState;
                            Done();
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
        }

        return false;
    }

    public enum ModeElement : byte
    {
        Produce = 0, Recruit = 1, OptimizeLocation = 2
    }

    public enum GroupDefendMode : byte
    {
        Split = 0, Wait = 1, Custom = 2
    }

    // intf_P5_07.inc:338

}
