// Translated from ItemCtrl.pas.
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

namespace Aow.Units.ItemCtrl
{
    // { TItemControlGrid (instance size 504, VMT 5580795C); $1DC indices, $1E0 stream, $1E6 show status, $1E8 map, $1EC sort, $1F0 unselect event. }
    public partial class TItemControlGrid : Aow.Units.ImageLib.TCustomImageDrawGrid
    {
        public Aow.Units.Engine.TIntegerList FIndices;
        // { +$1DC }
        public Aow.Rtl.TMemoryStream FStream;
        // { +$1E0 }
        public bool FDisplayMode;
        // { +$1E4 }
        public bool FUnplacedOnly;
        // { +$1E5 }
        public bool FShowStatus;
        // { $1E6 }
        public byte FReserved1E7;
        public Aow.Units.AoWE.TAoWHSMap FMap;
        // { +$1E8 }
        public byte FSort;
        // { +$1EC }
        public Aow.Units.FixedArray_byte_3 FReserved1ED;
        public Aow.Rtl.TNotifyEvent FOnUnselect;
        // ItemCtrl.pas:135
        // { +$1F0 }
        public override void Destroy()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Destroy */
            throw new System.NotImplementedException();
        }

        // ItemCtrl.pas:123
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            var call__1 = new Aow.Rtl.TMemoryStream();
            FStream = call__1;
            DefaultColWidth = 155;
            DefaultRowHeight = 46;
            GrowDirection = Aow.Units.ImageLib.TILGrowDirection.ilVertical;
            ScrollBars = Aow.Rtl.TScrollStyle.ssVertical;
            var call__2 = Aow.Units.Engine.TIntegerList.Create();
            FIndices = call__2;
            FSort = 0;
        }

        // ItemCtrl.pas:262
        protected internal override void DrawCell(int ACol, int ARow, Aow.Rtl.TRect ARect, TSet<Aow.Rtl.TGridDrawState> AState)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Canvas */
            throw new System.NotImplementedException();
        }

        // ItemCtrl.pas:207
        public override void DelCell()
        {
            if (EditMode.Contains(Aow.Units.ImageLib.TILGridEditModeElement.igDel))
            {
                DeleteItem();
            }
        }

        // ItemCtrl.pas:143
        protected internal override void MouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            /* TODO(T-shim) Platform or editor boundary Grids.MouseCoord */
            throw new System.NotImplementedException();
        }

        // ItemCtrl.pas:157
        public void ItemChanged(Aow.Rtl.TObject Sender)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ItemCtrl.pas:163
        public void ItemControlChanged(Aow.Rtl.TObject Sender)
        {
            UpdateItems();
        }

        // ItemCtrl.pas:168
        public void SetAoWHSMap(Aow.Units.AoWE.TAoWHSMap Value)
        {
            if (Value != FMap)
            {
                if (FMap != null)
                {
                    FMap.FItemControl.FOnChanged.Remove(ItemControlChanged);
                    FMap.FItemControl.FOnItemChanged.Remove(ItemChanged);
                }

                FMap = Value;
                if (FMap != null)
                {
                    FMap.FItemControl.FOnChanged.Add(ItemControlChanged);
                    FMap.FItemControl.FOnItemChanged.Add(ItemChanged);
                }

                UpdateItems();
            }
        }

        // ItemCtrl.pas:187
        public void SortOnName()
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ItemCtrl.pas:197
        public void SortOnType()
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ItemCtrl.pas:212
        public Aow.Units.AoWE.TItem GetSelectedItem()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Col */
            throw new System.NotImplementedException();
        }

        // ItemCtrl.pas:223
        public Aow.Units.AoWE.TItem GetItem(int Index)
        {
            Aow.Units.AoWE.TItem Result = default;
            int I = default;
            Aow.Units.AoWE.TAoWHSMap Map = default;
            I = Index;
            Map = FMap;
            // { Original checks only the upper bound. }
            if ((Map != null) && (Index < FIndices.Count))
            {
                var call__3 = FIndices[Index];
                var call__4 = Map.FItemControl.GetItem(call__3);
                Result = call__4;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // ItemCtrl.pas:235
        public void UpdateItems()
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ItemCtrl.pas:328
        public void Unselect()
        {
            SelectionEnabled = false;
            if (FOnUnselect != null)
            {
                FOnUnselect(this);
            }
        }

        // ItemCtrl.pas:334
        public void EditItemDone(Aow.Units.Engine.TEditResult ResultCode, Aow.Units.Engine.TEObject Obj)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ItemCtrl.pas:357
        public void DeleteItem()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Bottom */
            throw new System.NotImplementedException();
        }

        // ItemCtrl.pas:388
        // { draft }
        public void RemoveItemFromMap()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Bottom */
            throw new System.NotImplementedException();
        }

        // { AoWEd 0042B7C6 }
        [Published]
        public bool EditItemEnabled { get => FDisplayMode; set => FDisplayMode = value; }

        [Published]
        public bool ShowFreeItemsOnly { get => FUnplacedOnly; set => FUnplacedOnly = value; }

        [Published]
        public bool ShowStatus { get => FShowStatus; set => FShowStatus = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnUnselect { get => FOnUnselect; set => FOnUnselect = value; }

        // ItemCtrl.pas:30
        public static new Aow.Units.ItemCtrl.TItemControlGrid Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.ItemCtrl.TItemControlGrid o = new Aow.Units.ItemCtrl.TItemControlGrid();
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

    public partial class TGridMapView : Aow.Rtl.TObject
    {
        public Aow.Units.FixedArray_byte_240 Reserved04;
        public Aow.Units.AoWE.TItemControl ItemControl;
        public Aow.Units.AoWE.THeroControl HeroControl;
        public Aow.Units.FixedArray_byte_68 ReservedFC;
        public Aow.Units.AoWE.TPlayerList Players;
        public Aow.Units.FixedArray_byte_56 Reserved144;
        public Aow.Units.Engine.TEventList LeaderEvent;
    }

    public struct TRawView
    {
        public Aow.Units.FixedArray_object_512 Fields;
    }

    public struct TNodeView
    {
        public Aow.Units.FixedArray_byte_8 Reserved;
        public Aow.Rtl.TList Children;
    }

    [StructLayout(LayoutKind.Explicit, Size = 49)]
    public struct TEngineView
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_48 Reserved;
        [FieldOffset(48)]
        public byte Status;
    }

    public struct THeroView
    {
        public Aow.Units.FixedArray_byte_96 Reserved00;
        public Aow.Rtl.AnsiString Name;
        public Aow.Units.FixedArray_byte_4 Reserved64;
        public byte Race;
        public Aow.Units.FixedArray_byte_15 Reserved69;
        public bool Hidden;
    }

    public struct TItemView
    {
        public Aow.Units.FixedArray_byte_24 Reserved00;
        public Aow.Rtl.AnsiString Name;
        public byte Status;
        public Aow.Units.FixedArray_byte_23 Reserved1D;
        public byte Kind;
        public Aow.Units.FixedArray_byte_15 Reserved35;
        public bool Hidden;
    }

    public static partial class ItemCtrl
    {
        // ItemCtrl.pas:406
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AoW", [typeof(Aow.Units.ItemCtrl.TItemControlGrid)]);
        }

        public static readonly Aow.Rtl.AnsiString GridUsedRStr = (AnsiString)"Used";

        // ItemCtrl.pas:112
        public static int ItemControlCompareName(int A, int B)
        {
            int Result = default;
            var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FItemControl.GetItem(B);
            var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.FItemControl.GetItem(A);
            Result = Aow.Rtl.Rtl.CompareText(call__6.FOwnerName, call__5.FOwnerName);
            return Result;
        }

        // ItemCtrl.pas:117
        public static int ItemControlCompareType(int A, int B)
        {
            int Result = default;
            var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.FItemControl.GetItem(A);
            var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.FItemControl.GetItem(B);
            Result = call__7.FItemType - call__8.FItemType;
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
