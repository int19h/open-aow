#nullable disable warnings
// Translated from AoWE.pas.
using System.Diagnostics.CodeAnalysis;
using Aow.Engine.Nodes;

using Aow.Game.Session;

using Aow.Rtl;

namespace Aow.Game.AI;

// { Interface declaration run owned by P5. }
// process cycles, player control, phase
public partial class AIPlayerAction(CustomNode? owner) : EngineObject(owner), IAIPlayerActionClass
{
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Construction snapshots record this field, and the Delphi field is only written.")]
    private int _processCycles;

    public AIPlayerControl Control { get; set; }

    // AI.pas:4
    public virtual bool Setup(AIPlayerControl control, AIPlayerProcessPhase phase)
    {
        Control = control;
        return ActionClass.SupportedPhases.Contains(phase.ToFlag());
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
    public static AIPlayerProcessPhases SupportedPhases => AIPlayerProcessPhases.Main;

}
