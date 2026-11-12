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

    private int _processCycles;

    public AIPlayerControl Control { get; set; }
    public AIPlayerProcessPhase Phase { get; private set; }

    // AI.pas:4
    public virtual bool Setup(AIPlayerControl control, AIPlayerProcessPhase phase)
    {
        Control = control;
        Phase = phase;
        return ActionClass.SupportedPhases.Contains(phase);
    }

    // AI.pas:17
    public virtual bool Activate()
    {
        _processCycles = 0;
        return true;
    }

    // AI.pas:23
    public virtual void Deactivate()
    {
    }

    // AI.pas:11
    public virtual bool Process()
    {
        _processCycles += 1;
        return false;
    }

    // intf_P5_06.inc:10
    public static int Priority => throw AbstractErrorException.Create("Abstract Error");

    // AI.pas:1
    public static Set<AIPlayerProcessPhase> SupportedPhases => Set<AIPlayerProcessPhase>.Of(AIPlayerProcessPhase.Main);

}
