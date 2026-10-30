// Translated from PlayerBox.pas.
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
using Aow.Units.UnitIGrd;
using static Aow.Units.UnitIGrd.UnitIGrd;

namespace Aow.Units.PlayerBox
{
    public partial class TPlayerComboBox : Aow.Rtl.TComboBox
    {
        public sbyte FPlayerIndex;
        public bool FAllowNone;
        public bool FAllowIndependent;
        public bool FAllowRandom;
        // PlayerBox.pas:87
        public override void Loaded()
        {
            base.Loaded();
            if (!ComponentState.Contains(Aow.Rtl.TComponentState.csDesigning))
            {
                GenerateItems();
            }
        }

        // PlayerBox.pas:52
        public void GenerateItems()
        {
            /* TODO: Implement StdCtrls.Items in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // PlayerBox.pas:72
        public void SetPlayerIndex(sbyte Value)
        {
            /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // PlayerBox.pas:80
        protected internal override void Change()
        {
            /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        [Published]
        public bool AllowIndependents { get => FAllowIndependent; set => FAllowIndependent = value; }

        [Published]
        public bool AllowNoPlayer { get => FAllowNone; set => FAllowNone = value; }

        [Published]
        public bool AllowRandom { get => FAllowRandom; set => FAllowRandom = value; }
    }

    public struct TObjectFields
    {
        public Aow.Units.FixedArray_object_512 Fields;
    }

    public struct TNameView
    {
        public Aow.Units.FixedArray_byte_36 Reserved;
        public Aow.Rtl.AnsiString Name;
    }

    [StructLayout(LayoutKind.Explicit, Size = 21)]
    public struct TRaceView
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_20 Reserved;
        [FieldOffset(20)]
        public bool Selectable;
    }

    public static partial class PlayerBox
    {
        // PlayerBox.pas:93
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AoW", [typeof(Aow.Units.PlayerBox.TPlayerComboBox)]);
        }

        public static Aow.Rtl.AnsiString PlayerRStr = (AnsiString)"Player";

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
