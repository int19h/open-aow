#nullable disable
// Translated from Hexagon.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Hexes;

// TransitionLibrary (HexTrans.ILB)
public class HexagonResource : AbstractHexagonResource
{
    public HexagonResource(CustomNode owner) : base(owner) => SetLevel(0);

    // Hexagon.pas:89
    public new const int ClassId = 131411;

    protected internal CustomImageLibrary TransitionLibrary { get; private set; }

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
        MapField mapField = map.MapContainer.GetField(hexX, hexY, hexLevel);
        // The resource user class (131410) registers only Hex.
        Hex found = mapField.FindOwnedChild<Hex>(ResourceUserClassId());
        MapObject result = found;
        if (found is not null)
        {
            if (found.GetTerrain(0) != TerrainList.GetTerrain(0))
            {
                found.SetResource(this);
                mapField.UpdateTerrainType();
                found.UpdateTransitions();
            }
            else
            {
                found.SetResource(this);
            }

            map.InvalidateStaticScene(hexX, hexY, hexLevel);
        }
        else
        {
            result = CreateResourceUser();
            if (!result.PlaceHex(map.MapContainer, hexX, hexY, hexLevel))
            {
                result?.Dispose();
                return null;
            }
        }

        return result;
    }

    // Hexagon.pas:141
    public void Show(int pixelX, int pixelY, in LockedSurface lockedSurface, Hex hex)
    {
        ImageLibrary[0].Show(pixelX, pixelY, lockedSurface);
        if ((hex.TransitionSides & 7) != 0)
        {
            byte sides = hex.TransitionSides;
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

    // Hexagon.pas:107
    public override string ResourceEditName() => s_hexText;

    private static readonly string s_hexText = "Hexagon";

}
