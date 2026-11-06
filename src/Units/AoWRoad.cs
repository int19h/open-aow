// Translated from AoWRoad.pas.
using System.Runtime.InteropServices;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngineUnit;
using Aow.Units.Road;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.AoWRoad;

public partial class TAoWRoad : TRoad
{
    public TAoWRoad(TECustomNode owner) : base(owner)
    {}

    // AoWRoad.pas:35
    public override void UpdateTransitions()
    {
        byte OldTransitions;
        int Xhx = default;
        int Yhx = default;
        TMapFieldMsg Msg = default;
        TMapLevel Level;
        TMapField MapField;
        bool HasRoad;
        OldTransitions = FTransitions;
        Level = (TMapLevel)Owner.Owner;
        FTransitions = 0;
        Msg.MsgID = 4401;
        Msg.Param[0] = Owner;
        for (int HN = 1; HN <= 6; HN++)
        {
            HNtoHX(HN, ref Xhx, ref Yhx);
            TranslateHX(0, 0, ((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, Xhx, Yhx, ref Xhx, ref Yhx);
            if (Xhx >= 0 && Yhx >= 0 && Level.Width > Xhx && Level.Height > Yhx)
            {
                MapField = Level.GetField(Xhx, Yhx);
                if (MapField.FindOwnedHS(typeof(TAbstractRoad)) == null)
                {
                    // { Original message stores its response in Param[1], rather than Result. }
                    Msg.Param[1] = 0;
                    MapField.SendMapFieldMsg(ref Msg);
                    HasRoad = Msg.Param[1].Value != 0;
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
            AoWHSMap.InvalidateStaticScene(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ((TMapField)Owner).Lhx);
        }
    }
}

[StructLayout(LayoutKind.Explicit, Size = 17)]
public struct TRoadFields
{
    [FieldOffset(0)]
    public FixedArray_byte_16 Reserved;
    [FieldOffset(16)]
    public byte Transitions;
}

public static partial class AoWRoadUnit
{
    // AoWRoad.pas:76
    public static void RegisterAoWRoad(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TAoWRoad>(), EClass.Of<TRoadResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
