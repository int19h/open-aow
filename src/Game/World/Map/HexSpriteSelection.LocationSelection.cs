#nullable disable
using Aow.Game.Draw;

namespace Aow.Game.World.Map;

public partial class HexSpriteSelection
{
    public bool CanSelectLocation(int pixelX, int pixelY, int lHitPoints)
    {
        HexGeometry.PixelToHex(pixelX, pixelY, out int hexX, out int hexY);
        bool result;
        if (hexX >= 0 && Owner.MapContainer.Width > hexX && Owner.MapContainer.Height > hexY && hexY >= 0)
        {
            MapField mapField = Owner.MapContainer.GetField(hexX, hexY, lHitPoints);
            int i = mapField.Count;
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
        HexGeometry.PixelToHex(pixelX, pixelY, out int hexX, out int hexY);
        MapField mapField = Owner.MapContainer.GetField(hexX, hexY, lHitPoints);
        int i = FindLocationHexSpriteIndex(mapField) - 1;
        HexSprite hexSprite = null;
        if (i < 0)
        {
            i = mapField.Count - 1;
        }

        int n = mapField.Count;
        while (n != 0 && hexSprite is null)
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

        if (hexSprite is not null)
        {
            if (LocationHexSprite.IndexOf(hexSprite) == -1)
            {
                UnselectLocation();
                SelectLocationHexSprite(hexSprite);
            }
        }
    }

    public int FindLocationHexSpriteIndex(MapField mapField)
    {
        int result = -1;
        if (LocationHexSprite.Count != 0)
        {
            int i = 0;
            HexGeometry.HexToPixel(mapField.HexX, mapField.HexY, out int pixelX, out int pixelY);
            while (result == -1 && i != mapField.Count)
            {
                HexSprite hexSprite = mapField.HexSpriteList[i];
                if (hexSprite.CanSelect(pixelX, pixelY))
                {
                    if (LocationHexSprite.IndexOf(hexSprite) != -1)
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
