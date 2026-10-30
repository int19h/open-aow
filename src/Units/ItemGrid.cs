// Translated from ItemGrid.pas.
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

namespace Aow.Units.ItemGrid
{
    // { TItemGrid (instance size 496, VMT 55802170); $1E0 indices, $1E4 items, $1E8 stream, $1ED show availability, $1EE sort. }
    public partial class TItemGrid : Aow.Units.ImageLib.TCustomImageDrawGrid
    {
        public Aow.Units.FixedArray_byte_4 FDisplayOptions;
        // { +$1DC }
        public Aow.Units.Engine.TIntegerList FIndices;
        // { +$1E0 }
        public Aow.Units.AoWE.TItemList FItems;
        // { +$1E4 }
        public Aow.Rtl.TMemoryStream FStream;
        // { +$1E8 }
        public bool FEditable;
        // { $1EC; AoWEd 00420664 }
        public bool FShowAvailability;
        // { $1ED }
        public byte FSort;
        // { +$1EE }
        public byte FReserved1EF;
        // ItemGrid.pas:120
        // { AoWEd 004164F4, 0041638B }
        public override void Destroy()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Destroy */
            throw new System.NotImplementedException();
        }

        // ItemGrid.pas:108
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.ControlStyle */
            throw new System.NotImplementedException();
        }

        // ItemGrid.pas:190
        protected internal override void DrawCell(int ACol, int ARow, Aow.Rtl.TRect ARect, TSet<Aow.Rtl.TGridDrawState> AState)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Canvas */
            throw new System.NotImplementedException();
        }

        // ItemGrid.pas:127
        public override void DelCell()
        {
            if (EditMode.Contains(Aow.Units.ImageLib.TILGridEditModeElement.igDel))
            {
                DeleteItem();
            }
        }

        // ItemGrid.pas:132
        public Aow.Units.AoWE.TItem GetSelectedItem()
        {
            Aow.Units.AoWE.TItem Result = default;
            int Index = default;
            var call__1 = ImageIndex(Col, Row);
            Index = call__1;
            if (Index < FIndices.Count)
            {
                var call__2 = FIndices[Index];
                var call__3 = FItems.GetItem(call__2);
                Result = call__3;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // ItemGrid.pas:142
        public Aow.Units.AoWE.TItem GetItem(int Index)
        {
            Aow.Units.AoWE.TItem Result = default;
            // { Original checks only the upper bound. }
            if (Index < FIndices.Count)
            {
                var call__4 = FIndices[Index];
                var call__5 = FItems.GetItem(call__4);
                Result = call__5;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // ItemGrid.pas:149
        public void SetItems(Aow.Units.AoWE.TItemList Value)
        {
            FItems = Value;
            UpdateItems();
        }

        // ItemGrid.pas:155
        public void UpdateItems()
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ItemGrid.pas:178
        public void SortOnName()
        {
            FSort = 0;
            UpdateItems();
        }

        // ItemGrid.pas:184
        public void SortOnType()
        {
            FSort = 1;
            UpdateItems();
        }

        // ItemGrid.pas:241
        public void EditItemDone(Aow.Units.Engine.TEditResult ResultCode, Aow.Units.Engine.TEObject Obj)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ItemGrid.pas:256
        public void CreateNewItem()
        {
            Aow.Units.AoWE.TItem Item = default;
            FStream.Clear();
            var call__6 = new Aow.Units.AoWE.TItem(FItems);
            Item = call__6;
            Aow.Units.AoWE.AoWE.AoWEngine.ActivateEObject(Item);
            var call__7 = Item.Editor();
            call__7.Edit(Item, Aow.Units.AoWE.AoWE.AoWEngine, EditItemDone);
        }

        // ItemGrid.pas:265
        // { draft }
        public void AddItem(Aow.Units.AoWE.TItem Item)
        {
            if (Item != null)
            {
                var call__8 = FItems.GetControlStyle();
                if (call__8.Contains(Aow.Units.Engine.TEControlStyleElement.csArrayMode))
                {
                    var call__9 = ImageIndex(Col, Row);
                    var call__10 = FItems.GetItem(call__9);
                    if (call__10 == null)
                    {
                        var call__11 = ImageIndex(Col, Row);
                        Item.SetArrayOwner(FItems, call__11);
                    }
                }
                else
                {
                    Item.SetOwner(FItems);
                }

                UpdateItems();
            }
        }

        // ItemGrid.pas:278
        public void DeleteItem()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Bottom */
            throw new System.NotImplementedException();
        }

        // { draft }
        [Published]
        public bool ShowAvailability { get => FShowAvailability; set => FShowAvailability = value; }

        [Published]
        public bool EditItemEnabled { get => FEditable; set => FEditable = value; }

        // ItemGrid.pas:28
        public static new Aow.Units.ItemGrid.TItemGrid Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.ItemGrid.TItemGrid o = new Aow.Units.ItemGrid.TItemGrid();
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

    public partial class TItemListView : Aow.Units.Engine.TEChangeNotifyNode
    {
        public TItemListView(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // ItemGrid.pas:57
        public virtual Aow.Units.AoWE.TItem GetItem(int Index)
        {
            throw EAbstractError.Create("Abstract Error");
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 29)]
    public struct TItemStatus
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_28 Reserved;
        [FieldOffset(28)]
        public byte Status;
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
        public Aow.Units.FixedArray_byte_24 Reserved1C;
        public byte Kind;
        public Aow.Units.FixedArray_byte_15 Reserved35;
        public bool Hidden;
    }

    public static partial class ItemGrid
    {
        // ItemGrid.pas:296
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AoW", [typeof(Aow.Units.ItemGrid.TItemGrid)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
