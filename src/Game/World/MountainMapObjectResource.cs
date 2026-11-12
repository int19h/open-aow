// Translated from Mountain.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Hexes;

namespace Aow.Game.World;

public partial class MountainMapObjectResource : Hexes.Terrain.Resource
{
    public MountainMapObjectResource(ECustomNode owner) : base(owner)
    { }

    // Mountain.pas:169
    public new const int ClassId = 131433;

    // Mountain.pas:175
    public override int ResourceUserClassId() => 131432;

    // Mountain.pas:172
    public override string ResourceEditName() => AbilityRules.TranslateResourceString(GameText.MountainText);

}
