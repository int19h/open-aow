// Translated from UnitGrid.pas.
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

namespace Aow.Units.UnitGrid
{
    // FSelectionControl, FUnits, FEditEnabled, FShowUnitLevel, FMemoryStream
    public partial class TUnitGrid : Aow.Units.ImageLib.TCustomImageDrawGrid
    {
        public Aow.Units.AoWE.TUnitSelectionControl FSelectionControl;
        public Aow.Units.AoWE.TUnitList FUnits;
        public bool FEditEnabled;
        public bool FShowUnitLevel;
        public Aow.Rtl.TMemoryStream FMemoryStream;
        // UnitGrid.pas:60
        public override void Destroy()
        {
            /* TODO: Implement Grids.Destroy in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitGrid.pas:50
        // Retained: TCustomImageDrawGrid throws at the missing Grids.Create boundary before this derived body. Retire with the grid platform implementation.
        public TUnitGrid(TComponent owner) : base(owner)
        {

            var call__1 = new Aow.Rtl.TMemoryStream();
            FMemoryStream = call__1;
            RowCount = 1;
            ColCount = 8;
            DefaultColWidth = 48;
            DefaultRowHeight = 48;
        }

        // UnitGrid.pas:98
        protected internal override void DrawCell(int ACol, int ARow, Aow.Rtl.TRect ARect, TSet<Aow.Rtl.TGridDrawState> AState)
        {
            /* TODO: Implement Graphics.Handle in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitGrid.pas:67
        public void SetShowUnitLevel(bool Value)
        {
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitGrid.pas:73
        public void SetUnits(Aow.Units.AoWE.TUnitList Value)
        {
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitGrid.pas:81
        public Aow.Units.AoWE.TAbstractUnit GetUnit(int ACol, int ARow)
        {
            Aow.Units.AoWE.TAbstractUnit Result = default;
            int Index = default;
            Index = ImageIndex(ACol, ARow);
            // { Original leaves negative indices unchecked in both getters. }
            bool bool__2 = (FUnits != null);
            if (bool__2)
            {
                var call__3 = FUnits.Count;
                bool__2 = (Index < call__3);
            }

            if (bool__2)
            {
                var call__4 = FUnits.GetUnit(Index);
                Result = call__4;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // UnitGrid.pas:90
        public Aow.Units.AoWE.TAbstractUnit GetSelectedUnit()
        {
            /* TODO: Implement Grids.Col in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitGrid.pas:133
        public void AddUnit()
        {
            /* TODO: Implement Grids.Col in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitGrid.pas:169
        public void DeleteUnit()
        {
            /* TODO: Implement Grids.Col in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitGrid.pas:184
        public void EditUnitDone(Aow.Units.Engine.TEditResult EditResult, Aow.Units.Engine.TEObject Obj)
        {
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitGrid.pas:196
        protected internal override void DblClick()
        {
            /* TODO: Implement Forms.Create in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        [Published]
        public bool ShowUnitLevel { get => FShowUnitLevel; set => SetShowUnitLevel(value); }

        [Published]
        public bool EditEnabled { get => FEditEnabled; set => FEditEnabled = value; }
    }

    public static partial class UnitGrid
    {
        // UnitGrid.pas:207
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AoWEngine", [typeof(Aow.Units.UnitGrid.TUnitGrid)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
