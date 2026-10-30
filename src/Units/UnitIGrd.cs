// Translated from UnitIGrd.pas.
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
using Aow.Units.UnitIDlg;
using static Aow.Units.UnitIDlg.UnitIDlg;

namespace Aow.Units.UnitIGrd
{
    // FUnitIndexList, FSelectionControl, FShowUnitLevel, FDoubleUnitsEnabled, FSorted
    public partial class TUnitIndexGrid : Aow.Units.ImageLib.TCustomImageDrawGrid
    {
        public Aow.Units.AoWE.TUnitIndexList FUnitIndexList;
        public Aow.Units.AoWE.TUnitSelectionControl FSelectionControl;
        public bool FShowUnitLevel;
        public bool FDoubleUnitsEnabled;
        public bool FSorted;
        // UnitIGrd.pas:51
        // Retained: The Graphics.Color implementation is absent. Preserve the throw before semantic base initialization.
        public TUnitIndexGrid(TComponent owner) : base(owner)
        {
            /* TODO: Implement Graphics.Color in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitIGrd.pas:89
        protected internal override void DrawCell(int ACol, int ARow, Aow.Rtl.TRect ARect, TSet<Aow.Rtl.TGridDrawState> AState)
        {
            /* TODO: Implement Graphics.Handle in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitIGrd.pas:64
        public void SetShowUnitLevel(bool Value)
        {
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitIGrd.pas:70
        public int GetUnitIndex(int Index)
        {
            int Result = default;
            var call__1 = FUnitIndexList[Index];
            Result = call__1;
            return Result;
        }

        // UnitIGrd.pas:75
        public void SetUnitIndexList(Aow.Units.AoWE.TUnitIndexList Value)
        {
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitIGrd.pas:83
        public void SetUnitIndex(int Index, int Value)
        {
            FUnitIndexList[Index] = Value;
            Count = FUnitIndexList.Count;
        }

        // UnitIGrd.pas:133
        public void AddUnit()
        {
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitIGrd.pas:147
        public void DeleteUnit()
        {
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitIGrd.pas:159
        public int GetUnit(int ACol, int ARow)
        {
            int Result = default;
            int Index = default;
            Index = ImageIndex(ACol, ARow);
            // { Original assumes a non-nil list here and in GetSelectedUnit. }
            if ((Index >= 0) && (Index < FUnitIndexList.Count))
            {
                var call__2 = FUnitIndexList[Index];
                Result = call__2;
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // UnitIGrd.pas:169
        public int GetSelectedUnit()
        {
            int Result = default;
            int Index = default;
            var call__3 = ImageIndex(Col, Row);
            Index = call__3;
            // { Original also leaves negative indices unchecked here. }
            if (Index < FUnitIndexList.Count)
            {
                var call__4 = FUnitIndexList[Index];
                Result = call__4;
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // UnitIGrd.pas:178
        protected internal override void DblClick()
        {
            /* TODO: Implement Controls.DblClick in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        [Published]
        public bool ShowUnitLevel { get => FShowUnitLevel; set => SetShowUnitLevel(value); }

        [Published]
        public bool DoubleUnitsEnabled { get => FDoubleUnitsEnabled; set => FDoubleUnitsEnabled = value; }

        [Published]
        public bool Sorted { get => FSorted; set => FSorted = value; }
    }

    public static partial class UnitIGrd
    {
        // UnitIGrd.pas:190
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AoWEngine", [typeof(Aow.Units.UnitIGrd.TUnitIndexGrid)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
