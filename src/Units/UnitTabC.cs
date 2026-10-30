// Translated from UnitTabC.pas.
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
using Aow.Units.UnitIGrd;
using static Aow.Units.UnitIGrd.UnitIGrd;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.UnitTabC
{
    // selection control, unit index grid, selection event
    public partial class TUnitTabControl : Aow.Rtl.TTabControl
    {
        public Aow.Units.AoWE.TUnitSelectionControl FSelectionControl;
        public Aow.Units.UnitIGrd.TUnitIndexGrid FUnitIndexGrid;
        public Aow.Units.UnitTabC.TOnSelectUnitEvent FOnSelectUnit;
        // UnitTabC.pas:42
        public override void Destroy()
        {
            /* TODO: Implement ComCtrls.Destroy in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitTabC.pas:35

        public TUnitTabControl(TComponent owner) : base(owner)
        {
            /* TODO: Implement ComCtrls.Create in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitTabC.pas:48
        protected internal override void Change()
        {
            /* TODO: Implement ComCtrls.Change in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitTabC.pas:54
        public void SetUnitIndexGrid(Aow.Units.UnitIGrd.TUnitIndexGrid Value)
        {
            FUnitIndexGrid = Value;
            if (Value != null)
            {
                FUnitIndexGrid.OnSelectCell = UnitIndexGridSelectCell;
                FUnitIndexGrid.FSelectionControl = FSelectionControl;
            }
        }

        // UnitTabC.pas:64
        public void UnitIndexGridSelectCell(Aow.Rtl.TObject Sender, int ACol, int ARow, ref bool CanSelect)
        {
            int Index = default;
            Aow.Units.AoWE.TUnitIndexList List = default;
            var call__1 = FUnitIndexGrid.ImageIndex(ACol, ARow);
            Index = call__1;
            FUnitIndexGrid.SelectionEnabled = true;
            List = FUnitIndexGrid.FUnitIndexList;
            if (List != null)
            {
                var call__2 = List[Index];
                FSelectionControl.SetSelectedUnitIndex(call__2);
            }

            if (FOnSelectUnit != null)
            {
                FOnSelectUnit(this, FSelectionControl.FSelectedUnitIndex);
            }
        }

        // UnitTabC.pas:76
        public void UpdateUnitIndexGrid()
        {
            /* TODO: Implement ComCtrls.TabIndex in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // UnitTabC.pas:105
        public void UpdateSelection()
        {
            /* TODO: Implement ComCtrls.Tabs in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        [Published]
        public Aow.Units.UnitIGrd.TUnitIndexGrid UnitIndexGrid { get => FUnitIndexGrid; set => SetUnitIndexGrid(value); }

        [Published]
        public Aow.Units.UnitTabC.TOnSelectUnitEvent OnSelectUnit { get => FOnSelectUnit; set => FOnSelectUnit = value; }
    }

    public delegate void TOnSelectUnitEvent(Aow.Rtl.TObject Sender, int UnitIndex);
    public static partial class UnitTabC
    {
        // UnitTabC.pas:112
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AoWEngine", [typeof(Aow.Units.UnitTabC.TUnitTabControl)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
