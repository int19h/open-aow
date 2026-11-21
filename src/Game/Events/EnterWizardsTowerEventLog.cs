#nullable disable warnings
// Translated from WizardsTower.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

namespace Aow.Game.Events;

// tower
public partial class EnterWizardsTowerEventLog(CustomNode? owner) : EventLog(owner)
{
    public WizardsTower Tower { get; init; }
}
