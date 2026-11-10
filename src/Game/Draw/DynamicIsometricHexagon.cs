// Translated from IsoHex.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Draw;

// owned by its TIsometricHexagon (FOwner set directly, so it is not added as a child) and shown
// from the map field of that hexagon
public partial class DynamicIsometricHexagon : HexagonSprite
{
    // IsoHex.pas:126
    public new const int ClassID = 131439;

    // IsoHex.pas:136
    public override Set<EControlStyleElement> GetControlStyle() => Set<EControlStyleElement>.Of();

    // IsoHex.pas:161
    public override void SetVisible(bool Value)
    {
    }

    // IsoHex.pas:165
    public override bool GetVisible() => false;

    // IsoHex.pas:131
    public override bool CanSelect(int Xhp, int Yhp) => false;

    // IsoHex.pas:141
    public override Set<ControlModeElement> ControlMode() => Set<ControlModeElement>.Of();

}
