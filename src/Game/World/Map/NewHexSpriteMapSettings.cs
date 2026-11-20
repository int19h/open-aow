// Translated from HSEngine.pas.

namespace Aow.Game.World.Map;

// the parameters of a new map (THSMap.CreateNewMap)
public class NewHexSpriteMapSettings
{

    public NewHexSpriteMapSettings()
    {
        Width = 64;
        Height = 64;
        Levels = 1;
        HssFileName = "";
        // { THSMap.ClassID }
        HexSpriteMapClassId = GameMap.ClassId;
        // { TMapContainer.ClassID }
        MapContainerClassId = MapContainer.ClassId;
        // { TMapLevel.ClassID }
        MapLevelClassId = 65_794;
    }

    public int Width { get; }
    public int Height { get; }
    public int Levels { get; }
    public string HssFileName { get; } = "";
    public int HexSpriteMapClassId { get; }
    public int MapContainerClassId { get; }
    public int MapLevelClassId { get; }
}
