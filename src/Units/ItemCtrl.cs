// Translated from ItemCtrl.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.InteropServices;
using Aow.Units.Engine;
using Aow.Units.ImageLib;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.ItemCtrl
{
    // indices, stream, show status, map, sort, unselect event
    public partial class TItemControlGrid : TCustomImageDrawGrid
    {
        public TIntegerList FIndices;
        public TMemoryStream FStream;
        public bool FDisplayMode;
        public bool FUnplacedOnly;
        public bool FShowStatus;
        public TAoWHSMap FMap;
        public byte FSort;
        public TNotifyEvent FOnUnselect;
        // ItemCtrl.pas:135
        public override void Destroy()
        {
            /* TODO: Implement Grids.Destroy in the platform or editor service. */
            throw new NotImplementedException();
        }

        // ItemCtrl.pas:123
        // Retained: TCustomImageDrawGrid throws at the missing Grids.Create boundary before this derived body. Retire with the grid platform implementation.
        public TItemControlGrid(TComponent owner) : base(owner)
        {

            var call__1 = new TMemoryStream();
            FStream = call__1;
            DefaultColWidth = 155;
            DefaultRowHeight = 46;
            GrowDirection = TILGrowDirection.ilVertical;
            ScrollBars = TScrollStyle.ssVertical;
            var call__2 = new TIntegerList();
            FIndices = call__2;
            FSort = 0;
        }

        // ItemCtrl.pas:262
        protected internal override void DrawCell(int ACol, int ARow, TRect ARect, TSet<TGridDrawState> AState)
        {
            /* TODO: Implement Controls.Canvas in the platform or editor service. */
            throw new NotImplementedException();
        }

        // ItemCtrl.pas:207
        public override void DelCell()
        {
            if (EditMode.Contains(TILGridEditModeElement.igDel))
            {
                DeleteItem();
            }
        }

        // ItemCtrl.pas:143
        protected internal override void MouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            /* TODO: Implement Grids.MouseCoord in the platform or editor service. */
            throw new NotImplementedException();
        }

        // ItemCtrl.pas:157
        public void ItemChanged(TObject Sender)
        {
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new NotImplementedException();
        }

        // ItemCtrl.pas:163
        public void ItemControlChanged(TObject Sender)
        {
            UpdateItems();
        }

        // ItemCtrl.pas:168
        public void SetAoWHSMap(TAoWHSMap Value)
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
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new NotImplementedException();
        }

        // ItemCtrl.pas:197
        public void SortOnType()
        {
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new NotImplementedException();
        }

        // ItemCtrl.pas:212
        public TItem GetSelectedItem()
        {
            /* TODO: Implement Grids.Col in the platform or editor service. */
            throw new NotImplementedException();
        }

        // ItemCtrl.pas:223
        public TItem GetItem(int Index)
        {
            TItem Result = default;
            int I = default;
            TAoWHSMap Map = default;
            I = Index;
            Map = FMap;
            // { Original checks only the upper bound. }
            if (Map != null && Index < FIndices.Count)
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
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new NotImplementedException();
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
        public void EditItemDone(TEditResult ResultCode, TEObject Obj)
        {
            /* TODO: Implement Controls.Invalidate in the platform or editor service. */
            throw new NotImplementedException();
        }

        // ItemCtrl.pas:357
        public void DeleteItem()
        {
            /* TODO: Implement Grids.Bottom in the platform or editor service. */
            throw new NotImplementedException();
        }

        // ItemCtrl.pas:388
        public void RemoveItemFromMap()
        {
            /* TODO: Implement Grids.Bottom in the platform or editor service. */
            throw new NotImplementedException();
        }

        [Published]
        public bool EditItemEnabled { get => FDisplayMode; set => FDisplayMode = value; }

        [Published]
        public bool ShowFreeItemsOnly { get => FUnplacedOnly; set => FUnplacedOnly = value; }

        [Published]
        public bool ShowStatus { get => FShowStatus; set => FShowStatus = value; }

        [Published]
        public TNotifyEvent OnUnselect { get => FOnUnselect; set => FOnUnselect = value; }
    }

    public partial class TGridMapView : TObject
    {
        public TItemControl ItemControl;
        public THeroControl HeroControl;
        public TPlayerList Players;
        public TEventList LeaderEvent;
    }

    public struct TRawView
    {
        public FixedArray_object_512 Fields;
    }

    public struct TNodeView
    {
        public FixedArray_byte_8 Reserved;
        public TList Children;
    }

    [StructLayout(LayoutKind.Explicit, Size = 49)]
    public struct TEngineView
    {
        [FieldOffset(0)]
        public FixedArray_byte_48 Reserved;
        [FieldOffset(48)]
        public byte Status;
    }

    public struct THeroView
    {
        public FixedArray_byte_96 Reserved00;
        public AnsiString Name;
        public FixedArray_byte_4 Reserved64;
        public byte Race;
        public FixedArray_byte_15 Reserved69;
        public bool Hidden;
    }

    public struct TItemView
    {
        public FixedArray_byte_24 Reserved00;
        public AnsiString Name;
        public byte Status;
        public FixedArray_byte_23 Reserved1D;
        public byte Kind;
        public FixedArray_byte_15 Reserved35;
        public bool Hidden;
    }

    public static partial class ItemCtrl
    {
        // ItemCtrl.pas:406
        public static void Register()
        {
            RegisterComponents((AnsiString)"AoW", [typeof(TItemControlGrid)]);
        }

        public static readonly AnsiString GridUsedRStr = (AnsiString)"Used";

        // ItemCtrl.pas:112
        public static int ItemControlCompareName(int A, int B)
        {
            int Result = default;
            var call__5 = AoWHSMap.FItemControl.GetItem(B);
            var call__6 = AoWHSMap.FItemControl.GetItem(A);
            Result = CompareText(call__6.FOwnerName, call__5.FOwnerName);
            return Result;
        }

        // ItemCtrl.pas:117
        public static int ItemControlCompareType(int A, int B)
        {
            int Result = default;
            var call__7 = AoWHSMap.FItemControl.GetItem(A);
            var call__8 = AoWHSMap.FItemControl.GetItem(B);
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
