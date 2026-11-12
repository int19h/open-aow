// Translated from Hexagon.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Hexes;

// TransitionLibrary (HexTrans.ILB)
public partial class HexagonResource : AbstractHexagonResource
{
    public HexagonResource(ECustomNode owner) : base(owner)
    {
        SetLevel(0);
    }

    // Hexagon.pas:89
    public new const int ClassId = 131411;

    protected internal CustomImageLibrary TransitionLibrary { get; private set; }

    // Hexagon.pas:83

    // Hexagon.pas:99
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            TransitionLibrary = Engine.HexSpriteSet.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "HexTrans.ILB");
        }

        if (message.MessageId == 65621)
        {
            message.Result = 5;
        }
    }

    // Hexagon.pas:94
    public override int ResourceUserClassId() => 131410;

    // Hexagon.pas:112
    public override MapObject PlaceResourceUser(GameMap map, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapField mapField;
        mapField = map.MapContainer.GetField(hexX, hexY, hexLevel);
        MapObject result = (MapObject)mapField.FindOwnedChild(ResourceUserClassId());
        if (result != null)
        {
            if (result.GetTerrain(0) != TerrainList.GetTerrain(0))
            {
                result.SetResource(this);
                mapField.UpdateTerrainType();
                ((AbstractHexagon)result).UpdateTransitions();
            }
            else
            {
                result.SetResource(this);
            }

            map.InvalidateStaticScene(hexX, hexY, hexLevel);
        }
        else
        {
            result = (MapObject)CreateResourceUser();
            if (!result.PlaceHex(map.MapContainer, hexX, hexY, hexLevel))
            {
                result?.Dispose();
                result = null;
            }
        }

        return result;
    }

    // Hexagon.pas:141
    public void Show(int pixelX, int pixelY, in LockedSurface lockedSurface, Hex hex)
    {
        byte sides;
        ImageLibrary[0].Show(pixelX, pixelY, lockedSurface);
        if ((hex.TransitionSides & 7) != 0)
        {
            sides = hex.TransitionSides;
            if ((sides & 2) != 0)
            {
                if ((sides & 16) != 0)
                {
                    TransitionLibrary[hex.Transitions[1]].Show(pixelX - 6, pixelY - 9 - 32, lockedSurface);
                }
                else
                {
                    TransitionLibrary[hex.Transitions[1]].Show(pixelX - 6, pixelY - 9, lockedSurface);
                }
            }

            if ((sides & 1) != 0)
            {
                if ((sides & 8) != 0)
                {
                    TransitionLibrary[hex.Transitions[0]].Show(pixelX - 6 - 32, pixelY - 9 - 16, lockedSurface);
                }
                else
                {
                    TransitionLibrary[hex.Transitions[0]].Show(pixelX - 6, pixelY - 9, lockedSurface);
                }
            }

            if ((sides & 4) != 0)
            {
                if ((sides & 32) != 0)
                {
                    TransitionLibrary[hex.Transitions[2]].Show(pixelX - 6 + 32, pixelY - 9 - 16, lockedSurface);
                }
                else
                {
                    TransitionLibrary[hex.Transitions[2]].Show(pixelX - 6, pixelY - 9, lockedSurface);
                }
            }
        }
    }

    // Hexagon.pas:32

    // Hexagon.pas:107
    public override string ResourceEditName() => s_hexText;

    private static readonly string s_hexText = "Hexagon";

}
