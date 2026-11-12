// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

using Aow.Rtl;

namespace Aow.Game.AI;

// { Interface declaration run owned by P5. }
// process cycles, player control, phase
public partial class AIPlayerAction : EngineObject, IAIPlayerActionClass
{

    public AIPlayerAction(ECustomNode owner) : base(owner)
    { }

    public int ProcessCycles;
    public AIPlayerControl Control;
    public AIPlayerProcessPhase _phase;

    // AI.pas:4
    public virtual bool Setup(AIPlayerControl control, AIPlayerProcessPhase phase)
    {
        Control = control;
        _phase = phase;
        return ActionClass.SupportedPhases.Contains(phase);
    }

    // AI.pas:17
    public virtual bool Activate()
    {
        ProcessCycles = 0;
        return true;
    }

    // AI.pas:23
    public virtual void Deactivate()
    {
    }

    // AI.pas:11
    public virtual bool Process()
    {
        ProcessCycles += 1;
        return false;
    }

    // intf_P5_06.inc:10
    public static int Priority => throw AbstractErrorException.Create("Abstract Error");

    // AI.pas:1
    public static Set<AIPlayerProcessPhase> SupportedPhases => Set<AIPlayerProcessPhase>.Of(AIPlayerProcessPhase.Main);

}
