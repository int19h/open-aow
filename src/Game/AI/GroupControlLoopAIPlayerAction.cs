#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.AI;

// group cycles, group control
public partial class GroupControlLoopAIPlayerAction : AIPlayerAction, IAIPlayerActionClass
{
    // AI.pas:1119
    public GroupControlLoopAIPlayerAction(CustomNode owner) : base(owner)
    {
    }

    private int _groupProcessCycles;
    private AIGroupControl _groupControl;

    // AI.pas:1124
    public override void Dispose()
    {
        DeactivateGroupControl();
        base.Dispose();
    }

    // AI.pas:1156
    public override bool Activate()
    {
        bool result = base.Activate();
        var player = Control.Player;
        for (int i = player.AIGroups.Count - 1; i >= 0; i--)
        {
            var groups = player.AIGroups.GetGroups(i);
            player.AIGroups.GetGroups(i).Status = groups.Status - Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Done);
        }

        return result;
    }

    // AI.pas:1166
    public override bool Process()
    {
        bool result;
        bool busy;
        bool done;
        int i;
        AIGroupControl control;
        _groupProcessCycles += 1;
        try
        {
            busy = false;
            done = false;
            while (!done && !busy && StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 0)
            {
                if (_groupControl == null)
                {
                    Control.Player.AIGroups.SortOnProcessPriority();
                    i = 0;
                    while (i < Control.Player.AIGroups.Count && Control.Player.AIGroups.GetGroups(i).Status.Contains(AIGroupStatusElement.Done))
                    {
                        i += 1;
                    }

                    if (i < Control.Player.AIGroups.Count)
                    {
                        ActivateGroupControl(Control.Player.AIGroups.GetGroups(i));
                    }
                    else
                    {
                        done = true;
                    }
                }

                control = _groupControl;
                if (control != null)
                {
                    busy = control.Process(true);
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

    // AI.pas:1136
    public void ActivateGroupControl(AIGroup group)
    {
        DeactivateGroupControl();
        _groupControl = group.CreateGroupControl();
        _groupControl.Activate();
    }

    // AI.pas:1143
    public void DeactivateGroupControl()
    {
        AIGroupControl control;
        control = _groupControl;
        if (control != null)
        {
            control.Deactivate();
            _groupControl?.Dispose();
            _groupControl = null;
        }
    }

    // intf_P5_06.inc:517

    // AI.pas:1130
    public static new int Priority => 20;

    // AI.pas:1133
    public static new Set<AIPlayerProcessPhase> SupportedPhases => Set<AIPlayerProcessPhase>.Of(AIPlayerProcessPhase.Loop);

}
