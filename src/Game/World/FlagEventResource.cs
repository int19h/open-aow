// Translated from FlagEvents.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World;

public class FlagEventResource : MapObjectResource
{
    public FlagEventResource(CustomNode owner) : base(owner) => Level = 27;

    // FlagEvents.pas:175
    public override void SetLevel(int value) => base.SetLevel(27);
}
