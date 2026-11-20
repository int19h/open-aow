// Translated from Mountain.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World;

public class MountainMapObjectResource(CustomNode owner) : Hexes.Terrain.Resource(owner)
{

    // Mountain.pas:169
    public new const int ClassId = 131433;

    // Mountain.pas:175
    public override int ResourceUserClassId() => 131432;

    // Mountain.pas:172
    public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: GameText.MountainText);

}
