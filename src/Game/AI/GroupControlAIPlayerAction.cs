#nullable disable warnings
// Translated from AoWE.pas.
using System.Diagnostics.CodeAnalysis;
using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.AI;

// group cycles, group control
public class GroupControlAIPlayerAction(CustomNode owner) : AIPlayerAction(owner), IAIPlayerActionClass
{
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Construction snapshots record this field, and the Delphi field is only written.")]
    private int _groupProcessCycles;
    private AIGroupControl? _groupControl;

    // AI.pas:1054
    public override void Dispose()
    {
        DeactivateGroupControl();
        base.Dispose();
    }

    // AI.pas:1083
    public override bool Process()
    {
        _groupProcessCycles += 1;
        try
        {
            bool busy = false;
            bool done = false;
            while (!done && !busy && StrategicMap.Current.AIExecutor.ProcessTimeLeft > 0)
            {
                if (_groupControl is null)
                {
                    Control.Player.AIGroups.SortOnProcessPriority();
                    int i = 0;
                    while (i < Control.Player.AIGroups.Count && Control.Player.AIGroups.GetGroups(i).Status.Contains(AIGroupStatuses.Done))
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

                AIGroupControl? control = _groupControl;
                if (control is not null)
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

            return !done;
        }
        catch
        {
            return false;
        }
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
        AIGroupControl? control = _groupControl;
        if (control is not null)
        {
            control.Deactivate();
            _groupControl?.Dispose();
            _groupControl = null;
        }
    }

    // AI.pas:1060
    public static new int Priority => 20;

}
