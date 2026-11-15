// Translated from HSEngine.pas.
namespace Aow.Game.World.Map;

// { The record of THSMap.NotifyProgress (TMapLevel.Loaded fills one): the kind, the percentage
//     done of the level and the level index. }
public struct MapProgress
{
    public MapProgressType ProgressType { get; init; }
    public int Percent { get; internal set; }
    public int Level { get; init; }
}
