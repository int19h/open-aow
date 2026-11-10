// Translated from ItemCtrl.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Draw.Images;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Grids;

// indices, stream, show status, map, sort, unselect event
public partial class ItemControlGrid : CustomImageDrawGrid
{
    public IntegerList FIndices;
    public MemoryStream FStream;
    public bool FDisplayMode;
    public bool FUnplacedOnly;
    public bool FShowStatus;
    public AoWHSMap FMap;
    public byte FSort;
    public NotifyEvent<object> FOnUnselect;

    // ItemCtrl.pas:135
    public override void Dispose()
    {
        /* TODO: Implement Grids.Destroy in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ItemCtrl.pas:123
    // Retained: TCustomImageDrawGrid throws at the missing Grids.Create boundary before this derived body. Retire with the grid platform implementation.
    public ItemControlGrid(Component owner) : base(owner)
    {

        FStream = new MemoryStream();
        DefaultColWidth = 155;
        DefaultRowHeight = 46;
        GrowDirection = ILGrowDirection.Vertical;
        ScrollBars = CustomGrid.ScrollStyle.Vertical;
        FIndices = new IntegerList();
        FSort = 0;
    }

    // ItemCtrl.pas:262
    protected internal override void DrawCell(int ACol, int ARow, Rect ARect, Set<GridDrawState> AState)
    {
        /* TODO: Implement Controls.Canvas in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ItemCtrl.pas:207
    public override void DelCell()
    {
        if (EditMode.Contains(ILGridEditModeElement.Del))
        {
            DeleteItem();
        }
    }

    // ItemCtrl.pas:143
    protected internal override void MouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        /* TODO: Implement Grids.MouseCoord in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ItemCtrl.pas:157
    public void ItemChanged(object Sender)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ItemCtrl.pas:163
    public void ItemControlChanged(object Sender)
    {
        UpdateItems();
    }

    // ItemCtrl.pas:168
    public void SetAoWHSMap(AoWHSMap Value)
    {
        if (Value != FMap)
        {
            if (FMap != null)
            {
                FMap.FItemControl.FOnChanged -= ItemControlChanged;
                FMap.FItemControl.FOnItemChanged -= ItemChanged;
            }

            FMap = Value;
            if (FMap != null)
            {
                FMap.FItemControl.FOnChanged += ItemControlChanged;
                FMap.FItemControl.FOnItemChanged += ItemChanged;
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
    public Item GetSelectedItem()
    {
        /* TODO: Implement Grids.Col in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ItemCtrl.pas:223
    public Item GetItem(int Index)
    {
        Item Result;
        AoWHSMap Map;
        Map = FMap;
        // { Original checks only the upper bound. }
        if (Map != null && Index < FIndices.Count)
        {
            var indice = FIndices[Index];
            Result = Map.FItemControl.GetItem(indice);
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
    public void EditItemDone(EditResult ResultCode, EObject Obj)
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
    public NotifyEvent<object> OnUnselect { get => FOnUnselect; set => FOnUnselect = value; }

    public partial class GridMapView
    {
        public ItemControl ItemControl;
        public HeroControl HeroControl;
        public PlayerList Players;
        public EventList LeaderEvent;
    }

    public struct RawView
    {
        public FixedArray_object_512 Fields;
    }

    public struct NodeView
    {
        public FixedArray_byte_8 Reserved;
        public DelphiList Children;
    }

    [StructLayout(LayoutKind.Explicit, Size = 49)]
    public struct EngineView
    {
        [FieldOffset(0)]
        public FixedArray_byte_48 Reserved;
        [FieldOffset(48)]
        public byte Status;
    }

    public struct HeroView
    {
        public FixedArray_byte_96 Reserved00;
        private string name;

        public string Name { readonly get => name ?? ""; set => name = value; }

        public FixedArray_byte_4 Reserved64;
        public byte Race;
        public FixedArray_byte_15 Reserved69;
        public bool Hidden;
    }

    public struct ItemView
    {
        public FixedArray_byte_24 Reserved00;
        private string name;

        public string Name { readonly get => name ?? ""; set => name = value; }

        public byte Status;
        public FixedArray_byte_23 Reserved1D;
        public byte Kind;
        public FixedArray_byte_15 Reserved35;
        public bool Hidden;
    }
}
