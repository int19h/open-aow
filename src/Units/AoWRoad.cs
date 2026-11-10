// Translated from AoWRoad.pas.
using System.Runtime.InteropServices;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public partial class AoWRoad : Road
{
    public AoWRoad(ECustomNode owner) : base(owner)
    { }

    // AoWRoad.pas:35
    public override void UpdateTransitions()
    {
        byte OldTransitions;
        int Xhx = default;
        int Yhx = default;
        MapFieldMsg Msg = default;
        MapLevel Level;
        MapField MapField;
        bool HasRoad;
        OldTransitions = FTransitions;
        Level = (MapLevel)Owner.Owner;
        FTransitions = 0;
        Msg.MsgID = 4401;
        Msg.Param[0] = Owner;
        for (int HN = 1; HN <= 6; HN++)
        {
            Aow.Game.World.Map.HexGeometry.HNtoHX(HN, ref Xhx, ref Yhx);
            Aow.Game.World.Map.HexGeometry.TranslateHX(0, 0, ((MapField)Owner).Xhx, ((MapField)Owner).Yhx, Xhx, Yhx, ref Xhx, ref Yhx);
            if (Xhx >= 0 && Yhx >= 0 && Level.Width > Xhx && Level.Height > Yhx)
            {
                MapField = Level.GetField(Xhx, Yhx);
                if (MapField.FindOwnedHS(typeof(AbstractRoad)) == null)
                {
                    // { Original message stores its response in Param[1], rather than Result. }
                    Msg.Param[1] = 0;
                    MapField.SendMapFieldMsg(ref Msg);
                    HasRoad = Msg.Param[1].Int != 0;
                }
                else
                {
                    HasRoad = true;
                }

                if (HasRoad)
                {
                    FTransitions = (byte)(FTransitions + (1 << (HN - 1)));
                }
            }
        }

        if ((FTransitions & 34) == 34)
        {
            FTransitions = (byte)((FTransitions & 221) + 128);
        }

        if ((FTransitions & 20) == 20)
        {
            FTransitions = (byte)((FTransitions & 235) + 64);
        }

        if (OldTransitions != FTransitions)
        {
            Aow.Game.World.Map.AoWHSMap.Current.InvalidateStaticScene(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ((MapField)Owner).Lhx);
        }
    }
}

[StructLayout(LayoutKind.Explicit, Size = 17)]
public struct RoadFields
{
    [FieldOffset(0)]
    public FixedArray_byte_16 Reserved;
    [FieldOffset(16)]
    public byte Transitions;
}
