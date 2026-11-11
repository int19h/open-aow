// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Structures;

namespace Aow.Game.AI.Groups;

public partial class AIGroupProduction : EngineObject
{
    public AIGroupProduction(ECustomNode owner) : base(owner)
    { }

    public Troop FUnit;
    public ProductionControl FControl;
    // Engine.pas:524

}
