#nullable disable warnings
// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Session;

public sealed class CustomizeLeaderControl(CustomNode? owner) : LeaderSetupControl(owner)
{
    public SetupControl SetupControl { get; init; }
    public int PlayerIndex { get; init; }

    // intf_P1_03.inc:99
    public override bool Done()
    {
        SetupControl.LeaderCustomized(this);
        return true;
    }
}
