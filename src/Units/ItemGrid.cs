// Translated from ItemGrid.pas.
using System.Runtime.InteropServices;
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.ImageLib;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.ItemGrid;

// indices, items, stream, show availability, sort
public partial class TItemGrid : TCustomImageDrawGrid
{
    public FixedArray_byte_4 FDisplayOptions;
    public TIntegerList FIndices;
    public TItemList FItems;
    public TMemoryStream FStream;
    public bool FEditable;
    public bool FShowAvailability;
    public byte FSort;

    // ItemGrid.pas:120
    public override void Dispose()
    {
        /* TODO: Implement Grids.Destroy in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ItemGrid.pas:108
    // Retained: The Controls.ControlStyle implementation is absent. Preserve the throw before semantic base initialization.
    public TItemGrid(TComponent owner) : base(owner)
    {
        /* TODO: Implement Controls.ControlStyle in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ItemGrid.pas:190
    protected internal override void DrawCell(int ACol, int ARow, TRect ARect, TSet<TGridDrawState> AState)
    {
        /* TODO: Implement Controls.Canvas in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ItemGrid.pas:127
    public override void DelCell()
    {
        if (EditMode.Contains(TILGridEditModeElement.igDel))
        {
            DeleteItem();
        }
    }

    // ItemGrid.pas:132
    public TItem GetSelectedItem()
    {
        TItem Result;
        int Index;
        Index = ImageIndex(Col, Row);
        if (Index < FIndices.Count)
        {
            var indice = FIndices[Index];
            Result = FItems.GetItem(indice);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // ItemGrid.pas:142
    public TItem GetItem(int Index)
    {
        TItem Result;
        // { Original checks only the upper bound. }
        if (Index < FIndices.Count)
        {
            var indice = FIndices[Index];
            Result = FItems.GetItem(indice);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // ItemGrid.pas:149
    public void SetItems(TItemList Value)
    {
        FItems = Value;
        UpdateItems();
    }

    // ItemGrid.pas:155
    public void UpdateItems()
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
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
    public void EditItemDone(TEditResult ResultCode, TEObject Obj)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ItemGrid.pas:256
    public void CreateNewItem()
    {
        TItem Item = default;
        FStream.Clear();
        Item = new TItem(FItems);
        AoWEngine.ActivateEObject(Item);
        Item.Editor().Edit(Item, AoWEngine, EditItemDone);
    }

    // ItemGrid.pas:265
    public void AddItem(TItem Item)
    {
        if (Item != null)
        {
            if (FItems.GetControlStyle().Contains(TEControlStyleElement.csArrayMode))
            {
                var imageIndex = ImageIndex(Col, Row);
                if (FItems.GetItem(imageIndex) == null)
                {
                    var imageIndex2 = ImageIndex(Col, Row);
                    Item.SetArrayOwner(FItems, imageIndex2);
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
        /* TODO: Implement Grids.Bottom in the platform or editor service. */
        throw new NotImplementedException();
    }

    [Published]
    public bool ShowAvailability { get => FShowAvailability; set => FShowAvailability = value; }

    [Published]
    public bool EditItemEnabled { get => FEditable; set => FEditable = value; }
}

public partial class TItemListView : TEChangeNotifyNode
{
    public TItemListView(TECustomNode owner) : base(owner)
    { }

    // ItemGrid.pas:57
    public virtual TItem GetItem(int Index)
    {
        throw EAbstractError.Create("Abstract Error");
    }
}

[StructLayout(LayoutKind.Explicit, Size = 29)]
public struct TItemStatus
{
    [FieldOffset(0)]
    public FixedArray_byte_28 Reserved;
    [FieldOffset(28)]
    public byte Status;
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
    private string name;

    public string Name { readonly get => name ?? ""; set => name = value; }

    public FixedArray_byte_4 Reserved64;
    public byte Race;
    public FixedArray_byte_15 Reserved69;
    public bool Hidden;
}

public struct TItemView
{
    public FixedArray_byte_24 Reserved00;
    private string name;

    public string Name { readonly get => name ?? ""; set => name = value; }

    public FixedArray_byte_24 Reserved1C;
    public byte Kind;
    public FixedArray_byte_15 Reserved35;
    public bool Hidden;
}

public static partial class ItemGridUnit
{
    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
