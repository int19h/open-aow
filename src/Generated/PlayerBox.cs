// Generated from PlayerBox.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
    // { TPlayerComboBox (instance size 372, VMT 55800248) }
    public partial class TPlayerComboBox : Aow.Rtl.TComboBox
    {
        public sbyte FPlayerIndex;
        // { +$170 }
        public bool FAllowNone;
        // { +$171 }
        public bool FAllowIndependent;
        // { +$172 }
        public bool FAllowRandom;
        // PlayerBox.pas:87
        // { +$173 }
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
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Items */
            throw new System.NotImplementedException();
        }

        // PlayerBox.pas:72
        public void SetPlayerIndex(sbyte Value)
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.ItemIndex */
            throw new System.NotImplementedException();
        }

        // PlayerBox.pas:80
        protected internal override void Change()
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.ItemIndex */
            throw new System.NotImplementedException();
        }

        [Published]
        public bool AllowIndependents { get => FAllowIndependent; set => FAllowIndependent = value; }

        [Published]
        public bool AllowNoPlayer { get => FAllowNone; set => FAllowNone = value; }

        [Published]
        public bool AllowRandom { get => FAllowRandom; set => FAllowRandom = value; }
    }

    // { needs: missing AoWE.PlayerRStr }
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
        // PlayerBox.pas:44
        // { needs: the draft lookup procedures return objects; preserve their original register ABI. }
        public static Aow.Units.AoWE.TRace GetRace(Aow.Units.AoWE.TRaceList List, int Index)
        {
            Aow.Units.AoWE.TRace Result = default;
            Result = List.GetRace(Index);
            return Result;
        }

        // PlayerBox.pas:46
        public static Aow.Units.AoWE.TRaceResource GetRaceResource(Aow.Units.AoWE.TRaceResourceList List, int Index)
        {
            Aow.Units.AoWE.TRaceResource Result = default;
            Result = List.GetRaceResource(Index);
            return Result;
        }

        // PlayerBox.pas:48
        public static Aow.Units.AoWE.TPlayer GetPlayers(Aow.Units.AoWE.TPlayerList List, int Index)
        {
            Aow.Units.AoWE.TPlayer Result = default;
            Result = List.GetPlayers(Index);
            return Result;
        }

        // PlayerBox.pas:50
        public static byte GetNumberOfPlayers(Aow.Units.AoWE.TPlayerList List)
        {
            byte Result = default;
            Result = unchecked((byte)List.GetNumberOfPlayers());
            return Result;
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
