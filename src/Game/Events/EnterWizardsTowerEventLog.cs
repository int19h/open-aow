// Translated from WizardsTower.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

namespace Aow.Game.Events;

// tower
public partial class EnterWizardsTowerEventLog : EventLog
{
    // WizardsTower.pas:70
    public EnterWizardsTowerEventLog(ECustomNode owner) : base(owner)
    { }

    public WizardsTower Tower { get; set; }

}
