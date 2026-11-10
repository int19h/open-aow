// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Ai.Groups;

// selector, custom state, mode, defend mode
public partial class AIGroupDefenseControl : AIGroupControlPlugin
{
    // AIGroup.pas:1807
    public AIGroupDefenseControl(ECustomNode owner) : base(owner)
    {
        FSelector = new GroupDefenseSelector();
        FMode = Set<GroupDefenseControlModeElement>.Of();
        FDefendMode = GroupDefendMode.Wait;
        FState = 0;
    }

    public GroupDefenseSelector FSelector;
    public int FCustomState;
    public Set<GroupDefenseControlModeElement> FMode;
    public GroupDefendMode FDefendMode;

    // AIGroup.pas:1816
    public override void Dispose()
    {
        FSelector?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:1822
    public override bool GetBusy() => FState != -1 && FStatus.Contains(AIGroupControlPluginStatusElement.Activated);

    // AIGroup.pas:1827
    public override bool Activate(AIGroupControl Control)
    {
        FGroupControl = Control;
        FState = 0;
        return true;
    }

    // AIGroup.pas:1834
    public override bool Process()
    {
        UnitUserList Units;
        int Index;
        AIGroup Group;
        switch (FState)
        {
            case 0:
                {
                    var aiGroupControl = FGroupControl;
                    FSelector.Execute(aiGroupControl.FGroup, aiGroupControl.FLocation.X, aiGroupControl.FLocation.Y, aiGroupControl.FLocation.L, null);
                }

                if (FSelector.FDefenders.Count != 0)
                {
                    switch (FDefendMode)
                    {
                        case GroupDefendMode.Split:
                            Units = new UnitUserList(null);
                            {
                                var aiGroupControl = FGroupControl;
                                FSelector.GetDefenders(aiGroupControl.FGroup.FPlayer, aiGroupControl.FLocation.X, aiGroupControl.FLocation.Y, aiGroupControl.FLocation.L, Units);
                            }

                            Index = Units.IndexOfBehavior(GroupBehavior.Passive);
                            if (Index == -1)
                            {
                                Group = AoWHSMap.Current.FAIGroupManager.CreateGroup(FGroupControl.FGroup.FPlayer, FGroupControl.FGroup.FIndependentRelation, GroupBehavior.Passive);
                                Group.AddUnits(FSelector.FDefenders);
                                Group.SetupTargetEx(FGroupControl.FLocation, 0, AIGroupTargetStatus.None, 2, -1, 0, 0);
                                Group.Release();
                            }
                            else
                            {
                                Group = Units.GetUnit(Index).FGroup;
                                Group.AddUnits(FSelector.FDefenders);
                            }

                            Units?.Dispose();
                            Done();
                            break;
                        case GroupDefendMode.Wait:
                            FGroupControl.Done();
                            if (FMode.Contains(GroupDefenseControlModeElement.Produce))
                            {
                                FState = 1;
                            }

                            break;
                        case GroupDefendMode.Custom:
                            FGroupControl.FState = FCustomState;
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

    public enum GroupDefenseControlModeElement : byte
    {
        Produce = 0, Recruit = 1, OptimizeLocation = 2
    }

    public enum GroupDefendMode : byte
    {
        Split = 0, Wait = 1, Custom = 2
    }

    // intf_P5_07.inc:338

}
