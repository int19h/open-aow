
using Aow.Game.AI;
using Aow.Rtl;

namespace Aow.Game.Session;

// The class-side members of an AI player action class that the action manager reads without an
// instance (AI.pas TAIPlayerAction class methods). A class with its own values re-lists the
// interface and hides the inherited statics with `new`, a class without one inherits them.
public interface IAIPlayerActionClass
{
    static abstract int Priority { get; }
    static abstract Set<AIPlayerProcessPhase> SupportedPhases { get; }
}
