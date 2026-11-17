#nullable disable
// Translated from AoWE.pas.
using System.Diagnostics.CodeAnalysis;
using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.AI;

// group cycles, group control
public partial class GroupControlAIPlayerAction(CustomNode owner) : AIPlayerAction(owner), IAIPlayerActionClass
{
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Construction snapshots record this field, and the Delphi field is only written.")]
    private int _groupProcessCycles;
    private AIGroupControl _groupControl;

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
        _groupControl = group.CreateGroupControl();
        _groupControl.Activate();
    }

    // AI.pas:1070
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

    // intf_P5_06.inc:504

    // AI.pas:1060
    public static new int Priority => 20;

}
