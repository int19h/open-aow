// Translated from FlagEvents.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World;

public partial class FlagEventResource : MapObjectResource
{
    public FlagEventResource(ECustomNode owner) : base(owner)
    {
        Level = 27;
    }

    // FlagEvents.pas:169

    // FlagEvents.pas:175
    public override void SetLevel(byte Value)
    {
        base.SetLevel(27);
    }

    // FlagEvents.pas:46

}
