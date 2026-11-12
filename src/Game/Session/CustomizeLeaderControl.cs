// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Session;

public partial class CustomizeLeaderControl : LeaderSetupControl
{
    public CustomizeLeaderControl(ECustomNode owner) : base(owner)
    { }

    public SetupControl SetupControl { get; set; }
    public int PlayerIndex { get; set; }

    // intf_P1_03.inc:99
    public override bool Done() { SetupControl.LeaderCustomized(this); return true; }
}
