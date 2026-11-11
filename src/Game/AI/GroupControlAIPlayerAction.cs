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

    public int FGroupProcessCycles;
    public AIGroupControl FGroupControl;

    // AI.pas:1054
    public override void Dispose()
    {
        DeactivateGroupControl();
        base.Dispose();
    }

    // AI.pas:1083
    public override bool Process()
    {
        bool Result;
        bool Busy;
        bool Done;
        int I;
        AIGroupControl Control;
        FGroupProcessCycles += 1;
        try
        {
            Busy = false;
            Done = false;
            while (!Done && !Busy && AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 0)
            {
                if (FGroupControl == null)
                {
                    FControl.FPlayer.FAIGroups.SortOnProcessPriority();
                    I = 0;
                    while (I < FControl.FPlayer.FAIGroups.Count && FControl.FPlayer.FAIGroups.GetGroups(I).FStatus.Contains(AIGroupStatusElement.Done))
                    {
                        I += 1;
                    }

                    if (I < FControl.FPlayer.FAIGroups.Count)
                    {
                        ActivateGroupControl(FControl.FPlayer.FAIGroups.GetGroups(I));
                    }
                    else
                    {
                        Done = true;
                    }
                }

                Control = FGroupControl;
                if (Control != null)
                {
                    Busy = Control.Process(false);
                    if (!Busy)
                    {
                        DeactivateGroupControl();
                    }
                }
                else
                {
                    Done = true;
                }
            }

            Result = !Done;
        }
        catch
        {
            Result = false;
        }

        return Result;
    }

    // AI.pas:1063
    public void ActivateGroupControl(AIGroup Group)
    {
        DeactivateGroupControl();
        FGroupControl = Group.CreateGroupControl();
        FGroupControl.Activate();
    }

    // AI.pas:1070
    public void DeactivateGroupControl()
    {
        AIGroupControl Control;
        Control = FGroupControl;
        if (Control != null)
        {
            Control.Deactivate();
            FGroupControl?.Dispose();
            FGroupControl = null;
        }
    }

    // intf_P5_06.inc:504

    // AI.pas:1060
    public static new int Priority => 20;

}
