// Translated from Pad.pas.
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

namespace Aow.Units.Pad
{
    // { TPad (instance size 32, VMT 557FE6C8): $1C FPadActive. }
    public partial class TPad : Aow.Units.ILTer.TFixedILTerrainMO
    {
        public TPad(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FPadActive = true;
        }

        // Pad.pas:52
        public new const int ClassID = 131334;

        public bool FPadActive;
        // { $1C }
        public Aow.Units.FixedArray_byte_3 FReserved1D;
        // Pad.pas:46


        // Pad.pas:55
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // Pad.pas:58
        public override sbyte ForceTerrainType(Aow.Units.HSEngine.TMapLevel MapLevel, sbyte Xhx, sbyte Yhx)
        {
            sbyte Result = default;
            if (MapLevel.Lhx != 0)
            {
                Result = 12;
            }
            else
            {
                Result = unchecked((sbyte)-1);
            }

            return Result;
        }

        // Pad.pas:20


        public new partial class Meta : Aow.Units.ILTer.TFixedILTerrainMO.Meta
        {
            // Pad.pas:52
            public override int ClassID() => Aow.Units.Pad.TPad.ClassID;

            // Pad.pas:20
            public new Aow.Units.Pad.TPad Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Pad.TPad)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.Pad.TPad => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TPadResource (instance size 72, VMT 557FE864): $44 FPadSize. }
    public partial class TPadResource : Aow.Units.ILTer.TFixedILTerrainMOR
    {
        public TPadResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            SetLevel(10);
        }

        // Pad.pas:76
        public new const int ClassID = 131335;

        public byte FPadSize;
        // { $44 }
        public Aow.Units.FixedArray_byte_3 FReserved45;
        // Pad.pas:69
        public override void Destroy()
        {
            if ((Engine != null) && !Engine.HSSet.Status.Contains(Aow.Units.HSEngine.THSSetStatusElement.ssDestroying))
            {
                ((Aow.Units.AoWE.TAoWHSSet)Engine.HSSet).FPadControl.UnregisterPadResource(this, FPadSize);
            }

            base.Destroy();
        }

        // Pad.pas:63


        // Pad.pas:101
        public override Aow.Units.Engine.EditorClass Editor()
        {
            Aow.Units.Engine.EditorClass Result = default;
            var call__1 = Engine.Editors.FindEditor(131335);
            Result = call__1;
            if (Result == null)
            {
                Result = base.Editor();
            }

            return Result;
        }

        // Pad.pas:95
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 536870920)
            {
                ActivateMsg();
            }
        }

        // Pad.pas:82
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131334;
            return Result;
        }

        // Pad.pas:85
        public void ActivateMsg()
        {
            int I = default;
            FPadSize = 0;
            {
                I = 0;
                int end__2 = TerrainList.Count - 1;
                for (; I <= end__2; I += 1)
                {
                    var call__3 = TerrainList.GetTerrain(I);
                    if (call__3 >= -1)
                    {
                        FPadSize = (byte)(((int)FPadSize) + 1);
                    }
                }
            }

            ((Aow.Units.AoWE.TAoWHSSet)Engine.HSSet).FPadControl.RegisterPadResource(this, FPadSize);
        }

        // Pad.pas:32


        // Pad.pas:79
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Pad";
            return Result;
        }

        public new partial class Meta : Aow.Units.ILTer.TFixedILTerrainMOR.Meta
        {
            // Pad.pas:76
            public override int ClassID() => Aow.Units.Pad.TPadResource.ClassID;

            // Pad.pas:32
            public new Aow.Units.Pad.TPadResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Pad.TPadResource)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.Pad.TPadResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class Pad
    {
        // Pad.pas:107
        public static void RegisterPad(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Pad.TPad>(), Aow.Units.Engine.EClass.Of<Aow.Units.Pad.TPadResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
