#nullable disable
using Aow.Game.Draw;

namespace Aow.Game.World.Map;

public partial class HexSpriteSelection
{
    public bool CanSelectLocation(int pixelX, int pixelY, int lHitPoints)
    {
        bool result;
        int hexX = default;
        int hexY = default;
        MapField mapField;
        int i;
        HexGeometry.PixelToHex(pixelX, pixelY, out hexX, out hexY);
        if (hexX >= 0 && Owner.MapContainer.Width > hexX && Owner.MapContainer.Height > hexY && hexY >= 0)
        {
            mapField = Owner.MapContainer.GetField(hexX, hexY, lHitPoints);
            i = mapField.Count;
            result = false;
            while (!result && i != 0)
            {
                i -= 1;
                result = mapField.HexSpriteList[i].CanSelect(pixelX, pixelY);
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    public void SelectLocation(int pixelX, int pixelY, int lHitPoints)
    {
        int n;
        int hexX = default;
        int hexY = default;
        MapField mapField;
        HexSprite hexSprite;
        int i;
        HexGeometry.PixelToHex(pixelX, pixelY, out hexX, out hexY);
        mapField = Owner.MapContainer.GetField(hexX, hexY, lHitPoints);
        i = FindLocationHexSpriteIndex(mapField) - 1;
        hexSprite = null;
        if (i < 0)
        {
            i = mapField.Count - 1;
        }

        n = mapField.Count;
        while (n != 0 && hexSprite == null)
        {
            hexSprite = mapField.HexSpriteList[i];
            if (!hexSprite.CanSelect(pixelX, pixelY))
            {
                hexSprite = null;
            }

            n -= 1;
            i -= 1;
            if (i < 0)
            {
                i = mapField.Count - 1;
            }
        }

        if (hexSprite != null)
        {
            if (_locationHexSprite.IndexOf(hexSprite) == -1)
            {
                UnselectLocation();
                SelectLocationHexSprite(hexSprite);
            }
        }
    }

    public int FindLocationHexSpriteIndex(MapField mapField)
    {
        int i;
        int pixelX = default;
        int pixelY = default;
        HexSprite hexSprite;
        int result = -1;
        if (_locationHexSprite.Count != 0)
        {
            i = 0;
            HexGeometry.HexToPixel(mapField.HexX, mapField.HexY, out pixelX, out pixelY);
            while (result == -1 && i != mapField.Count)
            {
                hexSprite = mapField.HexSpriteList[i];
                if (hexSprite.CanSelect(pixelX, pixelY))
                {
                    if (_locationHexSprite.IndexOf(hexSprite) != -1)
                    {
                        result = i;
                    }
                }

                i += 1;
            }
        }

        return result;
    }
}
