// Translated from ItemGrid.pas.
using System.Runtime.InteropServices;
using Aow.Game.Draw.Images;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Grids;

// indices, items, stream, show availability, sort
public partial class ItemGrid : CustomImageDrawGrid
{
    public FixedArray_byte_4 FDisplayOptions;
    public IntegerList FIndices;
    public ItemList FItems;
    public MemoryStream FStream;
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
    public ItemGrid(Component owner) : base(owner)
    {
        /* TODO: Implement Controls.ControlStyle in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ItemGrid.pas:190
    protected internal override void DrawCell(int ACol, int ARow, Rect ARect, Set<GridDrawState> AState)
    {
        /* TODO: Implement Controls.Canvas in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ItemGrid.pas:127
    public override void DelCell()
    {
        if (EditMode.Contains(ILGridEditModeElement.Del))
        {
            DeleteItem();
        }
    }

    // ItemGrid.pas:132
    public Item GetSelectedItem()
    {
        Item Result;
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
    public Item GetItem(int Index)
    {
        Item Result;
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
    public void SetItems(ItemList Value)
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
    public void EditItemDone(EditResult ResultCode, EObject Obj)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ItemGrid.pas:256
    public void CreateNewItem()
    {
        Item Item = default;
        FStream.Clear();
        Item = new Item(FItems);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.ActivateEObject(Item);
        Item.Editor().Edit(Item, GameTables.AoWEngine, EditItemDone);
    }

    // ItemGrid.pas:265
    public void AddItem(Item Item)
    {
        if (Item != null)
        {
            if (FItems.GetControlStyle().Contains(EControlStyleElement.ArrayMode))
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

        public FixedArray_byte_24 Reserved1C;
        public byte Kind;
        public FixedArray_byte_15 Reserved35;
        public bool Hidden;
    }
}

public partial class ItemListView : EChangeNotifyNode
{
    public ItemListView(ECustomNode owner) : base(owner)
    { }

    // ItemGrid.pas:57
    public virtual Item GetItem(int Index)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }
}

[StructLayout(LayoutKind.Explicit, Size = 29)]
public struct ItemStatus
{
    [FieldOffset(0)]
    public FixedArray_byte_28 Reserved;
    [FieldOffset(28)]
    public byte Status;
}
