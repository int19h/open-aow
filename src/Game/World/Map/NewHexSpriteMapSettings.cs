// Translated from HSEngine.pas.

namespace Aow.Game.World.Map;

// the parameters of a new map (THSMap.CreateNewMap)
public partial class NewHexSpriteMapSettings
{

    public NewHexSpriteMapSettings()
    {
        Width = 64;
        Height = 64;
        Levels = 1;
        HssFileName = "";
        // { THSMap.ClassID }
        HexSpriteMapClassId = 66048;
        // { TMapContainer.ClassID }
        MapContainerClassId = 65795;
        // { TMapLevel.ClassID }
        MapLevelClassId = 65794;
    }

    public int Width;
    public int Height;
    public int Levels;
    public string HssFileName = "";
    public int HexSpriteMapClassId;
    public int MapContainerClassId;
    public int MapLevelClassId;
    // HSEngine.pas:2802

}
