// Translated from AoWRoad.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AoWRoad
{
    public partial class TAoWRoad : Aow.Units.Road.TRoad
    {
        public TAoWRoad(TECustomNode owner) : base(owner)
        {}

        // AoWRoad.pas:35
        public override void UpdateTransitions()
        {
            byte OldTransitions = default;
            int Xhx = default;
            int Yhx = default;
            Aow.Units.HSEngine.TMapFieldMsg Msg = default;
            int HN = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            bool HasRoad = default;
            OldTransitions = unchecked((byte)FTransitions);
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            FTransitions = 0;
            Msg.MsgID = 4401;
            Msg.Param[0] = Owner;
            {
                HN = 1;
                byte end__1 = 6;
                for (; HN <= end__1; HN += 1)
                {
                    Aow.Units.HSEngine.HSEngine.HNtoHX(HN, ref Xhx, ref Yhx);
                    Aow.Units.HSEngine.HSEngine.TranslateHX(0, 0, ((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, Xhx, Yhx, ref Xhx, ref Yhx);
                    if ((((Xhx >= 0) && (Yhx >= 0)) && (Level.Width > Xhx)) && (Level.Height > Yhx))
                    {
                        MapField = Level.GetField(Xhx, Yhx);
                        if (MapField.FindOwnedHS(typeof(Aow.Units.HSEngine.TAbstractRoad)) == null)
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
                            FTransitions = (byte)(((int)FTransitions) + (1 << (HN - 1)));
                        }
                    }
                }
            }

            if ((FTransitions & 34) == 34)
            {
                FTransitions = unchecked((byte)((FTransitions & 221) + 128));
            }

            if ((FTransitions & 20) == 20)
            {
                FTransitions = unchecked((byte)((FTransitions & 235) + 64));
            }

            if (OldTransitions != FTransitions)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.InvalidateStaticScene(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ((Aow.Units.HSEngine.TMapField)Owner).Lhx);
            }
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 17)]
    public struct TRoadFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_16 Reserved;
        [FieldOffset(16)]
        public byte Transitions;
    }

    public static partial class AoWRoad
    {
        // AoWRoad.pas:76
        public static void RegisterAoWRoad(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.AoWRoad.TAoWRoad>(), Aow.Units.Engine.EClass.Of<Aow.Units.Road.TRoadResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
