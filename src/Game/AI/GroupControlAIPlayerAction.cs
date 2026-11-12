// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.AI;

// group cycles, group control
public partial class GroupControlAIPlayerAction : AIPlayerAction, IAIPlayerActionClass
{
    // AI.pas:1049
    public GroupControlAIPlayerAction(ECustomNode owner) : base(owner)
    {
    }

    public int GroupProcessCycles;
    public AIGroupControl GroupControl;

    // AI.pas:1054
    public override void Dispose()
    {
        DeactivateGroupControl();
        base.Dispose();
    }

    // AI.pas:1083
    public override bool Process()
    {
        bool result;
        bool busy;
        bool done;
        int I;
        AIGroupControl control;
        GroupProcessCycles += 1;
        try
        {
            busy = false;
            done = false;
            while (!done && !busy && AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 0)
            {
                if (GroupControl == null)
                {
                    Control.Player.AIGroups.SortOnProcessPriority();
                    I = 0;
                    while (I < Control.Player.AIGroups.Count && Control.Player.AIGroups.GetGroups(I).Status.Contains(AIGroupStatusElement.Done))
                    {
                        I += 1;
                    }

                    if (I < Control.Player.AIGroups.Count)
                    {
                        ActivateGroupControl(Control.Player.AIGroups.GetGroups(I));
                    }
                    else
                    {
                        done = true;
                    }
                }

                control = GroupControl;
                if (control != null)
                {
                    busy = control.Process(false);
                    if (!busy)
                    {
                        DeactivateGroupControl();
                    }
                }
                else
                {
                    done = true;
                }
            }

            result = !done;
        }
        catch
        {
            result = false;
        }

        return result;
    }

    // AI.pas:1063
    public void ActivateGroupControl(AIGroup group)
    {
        DeactivateGroupControl();
        GroupControl = group.CreateGroupControl();
        GroupControl.Activate();
    }

    // AI.pas:1070
    public void DeactivateGroupControl()
    {
        AIGroupControl control;
        control = GroupControl;
        if (control != null)
        {
            control.Deactivate();
            GroupControl?.Dispose();
            GroupControl = null;
        }
    }

    // intf_P5_06.inc:504

    // AI.pas:1060
    public static new int Priority => 20;

}
