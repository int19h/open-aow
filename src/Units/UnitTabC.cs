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
    // { TUnitTabControl (instance size 336, VMT 55800EA4):
//     $140 selection control, $144 unit index grid, $148 selection event. }
    public partial class TUnitTabControl : Aow.Rtl.TTabControl
    {
        public Aow.Units.AoWE.TUnitSelectionControl FSelectionControl;
        // { $140 }
        public Aow.Units.UnitIGrd.TUnitIndexGrid FUnitIndexGrid;
        // { $144 }
        public Aow.Units.UnitTabC.TOnSelectUnitEvent FOnSelectUnit;
        // UnitTabC.pas:42
        // { $148 }
        public override void Destroy()
        {
            /* TODO(T-shim) Platform or editor boundary ComCtrls.Destroy */
            throw new System.NotImplementedException();
        }

        // UnitTabC.pas:35
        // Retained: The ComCtrls.Create implementation is absent. Preserve the throw before semantic base initialization.
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            /* TODO(T-shim) Platform or editor boundary ComCtrls.Create */
            throw new System.NotImplementedException();
        }

        // UnitTabC.pas:48
        protected internal override void Change()
        {
            /* TODO(T-shim) Platform or editor boundary ComCtrls.Change */
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
            /* TODO(T-shim) Platform or editor boundary ComCtrls.TabIndex */
            throw new System.NotImplementedException();
        }

        // UnitTabC.pas:105
        public void UpdateSelection()
        {
            /* TODO(T-shim) Platform or editor boundary ComCtrls.Tabs */
            throw new System.NotImplementedException();
        }

        [Published]
        public Aow.Units.UnitIGrd.TUnitIndexGrid UnitIndexGrid { get => FUnitIndexGrid; set => SetUnitIndexGrid(value); }

        [Published]
        public Aow.Units.UnitTabC.TOnSelectUnitEvent OnSelectUnit { get => FOnSelectUnit; set => FOnSelectUnit = value; }

        // UnitTabC.pas:23
        public static Aow.Units.UnitTabC.TUnitTabControl Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.UnitTabC.TUnitTabControl o = new Aow.Units.UnitTabC.TUnitTabControl();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
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
