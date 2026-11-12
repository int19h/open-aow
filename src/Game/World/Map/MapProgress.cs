// Translated from HSEngine.pas.
namespace Aow.Game.World.Map;

// { The record of THSMap.NotifyProgress (TMapLevel.Loaded fills one): the kind, the percentage
//     done of the level and the level index. }
public struct MapProgress
{
    public MapProgressType ProgressType;
    public int Percent;
    public int Level;
}
