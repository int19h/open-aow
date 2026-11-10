// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Structures;

namespace Aow.Game.Ai.Groups;

public partial class AIGroupProduction : EObject
{
    public AIGroupProduction(ECustomNode owner) : base(owner)
    { }

    public AbstractUnit FUnit;
    public ProductionControl FControl;
    // Engine.pas:524

}
