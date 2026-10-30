// Translated from RaceBox.pas.
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

namespace Aow.Units.RaceBox
{
    public partial class TRaceBox : Aow.Rtl.TComboBox
    {
        public Aow.Rtl.TNotifyEvent FOnGenerateItems;
        public sbyte FRace;
        public bool FAllowRandom;
        public bool FAllowNone;
        public bool FSelectableOnly;
        public Aow.Units.AoWE.TRaceList FRaces;
        // RaceBox.pas:98
        public override void Loaded()
        {
            base.Loaded();
            GenerateItems();
        }

        // RaceBox.pas:52

        public TRaceBox(TComponent owner) : base(owner)
        {
            /* TODO: Implement StdCtrls.Create in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // RaceBox.pas:58
        public void GenerateItems()
        {
            /* TODO: Implement StdCtrls.Items in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // RaceBox.pas:77
        public void SetRace(sbyte Value)
        {
            /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // RaceBox.pas:85
        protected internal override void Change()
        {
            /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // RaceBox.pas:92
        public void SetRaces(Aow.Units.AoWE.TRaceList Value)
        {
            FRaces = Value;
            if (!ComponentState.Contains(Aow.Rtl.TComponentState.csDesigning) && !ComponentState.Contains(Aow.Rtl.TComponentState.csLoading))
            {
                GenerateItems();
            }
        }

        [Published]
        public bool OnlyEnabledRaces { get => FSelectableOnly; set => FSelectableOnly = value; }

        [Published]
        public bool AllowRandomRace { get => FAllowRandom; set => FAllowRandom = value; }

        [Published]
        public bool AllowNoRace { get => FAllowNone; set => FAllowNone = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnGenerateItems { get => FOnGenerateItems; set => FOnGenerateItems = value; }
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

    public static partial class RaceBox
    {
        // RaceBox.pas:104
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AoW", [typeof(Aow.Units.RaceBox.TRaceBox)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
