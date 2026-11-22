#nullable disable warnings
// Translated from Hexagon.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

// TransitionLibrary (HexTrans.ILB)
public class HexagonResource : AbstractHexagonResource
{
    public HexagonResource(CustomNode owner) : base(owner) => SetLevel(0);

    // Hexagon.pas:89
    public new const int ClassId = 131_411;

    protected internal CustomImageLibrary? TransitionLibrary { get; private set; }

    // Hexagon.pas:99
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            TransitionLibrary = Engine.HexSpriteSet.ImageLibraryManager.LinkToImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "HexTrans.ILB");
        }

        if (message.MessageId == 65621)
        {
            message.Result = 5;
        }
    }

    // Hexagon.pas:94
    public override int ResourceUserClassId() => Hex.ClassId;

    // Hexagon.pas:112
    public override MapObject? PlaceResourceUser(GameMap? map, int hexX, int hexY, int hexLevel)
    {
        MapField mapField = map.MapContainer.GetField(hexX, hexY, hexLevel);
        // The resource user class (131410) registers only Hex.
        Hex found = mapField.FindOwnedChild<Hex>(ResourceUserClassId());
        MapObject? result = found;
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
        if ((hex.TransitionSides & 0b0111) != 0)
        {
            byte sides = hex.TransitionSides;
            if ((sides & 0b0010) != 0)
            {
                if ((sides & 0b0001_0000) != 0)
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
                if ((sides & 0b1000) != 0)
                {
                    TransitionLibrary[hex.Transitions[0]].Show(pixelX - 6 - 32, pixelY - 9 - 16, lockedSurface);
                }
                else
                {
                    TransitionLibrary[hex.Transitions[0]].Show(pixelX - 6, pixelY - 9, lockedSurface);
                }
            }

            if ((sides & 0b0100) != 0)
            {
                if ((sides & 0b0010_0000) != 0)
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
