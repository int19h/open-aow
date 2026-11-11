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
        HSSFileName = "";
        // { THSMap.ClassID }
        HSMapClassID = 66048;
        // { TMapContainer.ClassID }
        MapContainerClassID = 65795;
        // { TMapLevel.ClassID }
        MapLevelClassID = 65794;
    }

    public int Width;
    public int Height;
    public int Levels;
    public string HSSFileName = "";
    public int HSMapClassID;
    public int MapContainerClassID;
    public int MapLevelClassID;
    // HSEngine.pas:2802

}
