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
        int I;
        HexGeometry.PixelToHex(pixelX, pixelY, ref hexX, ref hexY);
        if (hexX >= 0 && ((HSMap)Owner)._mapContainer._width > hexX && ((HSMap)Owner)._mapContainer._height > hexY && hexY >= 0)
        {
            mapField = ((HSMap)Owner)._mapContainer.GetField(hexX, hexY, lHitPoints);
            I = mapField.Count;
            result = false;
            while (!result && I != 0)
            {
                I -= 1;
                result = mapField._hexSpriteList[I].CanSelect(pixelX, pixelY);
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
        int N;
        int hexX = default;
        int hexY = default;
        MapField mapField;
        HexSprite hexSprite;
        int I;
        HexGeometry.PixelToHex(pixelX, pixelY, ref hexX, ref hexY);
        mapField = ((HSMap)Owner)._mapContainer.GetField(hexX, hexY, lHitPoints);
        I = FindLocationHexSpriteIndex(mapField) - 1;
        hexSprite = null;
        if (I < 0)
        {
            I = mapField.Count - 1;
        }

        N = mapField.Count;
        while (N != 0 && hexSprite == null)
        {
            hexSprite = mapField._hexSpriteList[I];
            if (!hexSprite.CanSelect(pixelX, pixelY))
            {
                hexSprite = null;
            }

            N -= 1;
            I -= 1;
            if (I < 0)
            {
                I = mapField.Count - 1;
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
        int I;
        int pixelX = default;
        int pixelY = default;
        HexSprite hexSprite;
        int result = -1;
        if (_locationHexSprite.Count != 0)
        {
            I = 0;
            HexGeometry.HexToPixel(mapField._hexX, mapField._hexY, ref pixelX, ref pixelY);
            while (result == -1 && I != mapField.Count)
            {
                hexSprite = mapField._hexSpriteList[I];
                if (hexSprite.CanSelect(pixelX, pixelY))
                {
                    if (_locationHexSprite.IndexOf(hexSprite) != -1)
                    {
                        result = I;
                    }
                }

                I += 1;
            }
        }

        return result;
    }
}
