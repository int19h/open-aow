// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Ai.Groups;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Ai;

// group cycles, group control
public partial class GroupControlLoopAIPA : AIPlayerAction, IAIPlayerActionClass
{
    // AI.pas:1119
    public GroupControlLoopAIPA(ECustomNode owner) : base(owner)
    {
    }

    public int FGroupProcessCycles;
    public AIGroupControl FGroupControl;

    // AI.pas:1124
    public override void Dispose()
    {
        DeactivateGroupControl();
        base.Dispose();
    }

    // AI.pas:1156
    public override bool Activate()
    {
        bool Result = base.Activate();
        var player = FControl.FPlayer;
        for (int I = player.FAIGroups.Count - 1; I >= 0; I--)
        {
            var groups = player.FAIGroups.GetGroups(I);
            player.FAIGroups.GetGroups(I).FStatus = groups.FStatus - Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Done);
        }

        return Result;
    }

    // AI.pas:1166
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
                    Busy = Control.Process(true);
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

    // AI.pas:1136
    public void ActivateGroupControl(AIGroup Group)
    {
        DeactivateGroupControl();
        FGroupControl = Group.CreateGroupControl();
        FGroupControl.Activate();
    }

    // AI.pas:1143
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

    // intf_P5_06.inc:517

    // AI.pas:1130
    public static new int Priority => 20;

    // AI.pas:1133
    public static new Set<AIPlayerProcessPhase> SupportedPhases => Set<AIPlayerProcessPhase>.Of(AIPlayerProcessPhase.Loop);

}
