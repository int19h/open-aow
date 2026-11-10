// Translated from HSEngine.pas.
using System.Runtime.InteropServices;

namespace Aow.Game.World.Map;

// { The record of THSMap.NotifyProgress (TMapLevel.Loaded fills one): the kind, the percentage
//     done of the level and the level index. }
[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct MapProgress
{
    [FieldOffset(0)]
    public MapProgressType ProgressType;
    [FieldOffset(4)]
    public int Percent;
    [FieldOffset(8)]
    public int Level;
}
