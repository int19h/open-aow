// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Units;

// inherited children, update lock. change event
public partial class ItemList : EChangeNotifyNode
{

    // Army.PAS:4516
    public ItemList(ECustomNode owner) : base(owner)
    {

    }

    public NotifyEvent<object> FOnChanged;

    // Army.PAS:4537
    // code, data
    public override void MainReadWrite(EStorageStream Stream)
    {
        base.MainReadWrite(Stream);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
            {
                if (FChildren.List[I] != null)
                {
                    ((EngineObject)FChildren.List[I]).Release();
                }
            }
        }
    }

    // Army.PAS:4546
    public override void Clear()
    {
        EngineObject Child;
        for (int I = Count - 1; I >= 0; I--)
        {
            if (FChildren.List[I] != null)
            {
                Child = (EngineObject)FChildren.List[I];
                if (Child != null)
                {
                    if (Child.Owner != this)
                    {
                        Child.Release();
                        FChildren.Delete(I);
                    }
                    else
                    {
                        Child.SetOwner(null);
                    }
                }
            }
        }

        base.Clear();
    }

    // Army.PAS:4531
    public override void DestroyAllChildren()
    {
        Clear();
        base.DestroyAllChildren();
    }

    // Army.PAS:4519
    public override int RemoveChild(EngineObject Child)
    {
        int Result = base.RemoveChild(Child);
        if (Result != -1)
        {
            Child.Release();
        }

        return Result;
    }

    // Army.PAS:4525
    public override int AddChild(EngineObject Child)
    {
        int Result = base.AddChild(Child);
        if (Result != -1)
        {
            Child.AddRef();
        }

        return Result;
    }

    // Army.PAS:4564
    public override void Changed()
    {
        base.Changed();
        if (FOnChanged != null)
        {
            FOnChanged(this);
        }
    }

    // Army.PAS:4587
    public virtual Item GetItem(int Index)
    {
        Item Result;
        if (Index >= 0 && Index < Count)
        {
            Result = (Item)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Army.PAS:4596
    public virtual void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = -1;
        Y = -1;
        L = -1;
    }

    // Army.PAS:4570
    public Item FindItem(int ItemID)
    {
        Item Result;
        int I;
        Item Item;
        if (this != null)
        {
            I = 0;
            Result = null;
            while (I != FChildren.Count && Result == null)
            {
                Item = (Item)FChildren.List[I];
                if (Item != null && ItemID == Item.FItemID)
                {
                    Result = Item;
                }

                I += 1;
            }
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Army.PAS:4593
    public void Update()
    {
        SortOnType();
    }

    // Army.PAS:4609
    public void SortOnName()
    {
        FChildren.SortStable(CompareItemNames);
    }

    // Army.PAS:4619
    public void SortOnType()
    {
        FChildren.SortStable(CompareItemTypes);
    }

    // Army.PAS:4622
    public void RegisterItems()
    {
        Item Item;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            Item = (Item)FChildren.List[I];
            if (Item != null && !Item.FStatus.Contains(ItemStatusElement.Registered))
            {
                AoWHSMap.Current.FItemControl.RegisterItem(Item);
            }
        }
    }

    // Army.PAS:4633
    public new void LockChanged()
    {
        base.LockChanged();
    }

    // Army.PAS:4636
    public new void UnlockChanged()
    {
        base.UnlockChanged();
    }

    public static int CompareItemNames(object Item1, object Item2)
    {
        if (Item1 != null && Item2 != null)
        {
            return RtlUnit.CompareText(((Item)Item1).FOwnerName, ((Item)Item2).FOwnerName);
        }
        return 0;
    }

    private static int CompareItemTypes(object Item1, object Item2)
    {
        return ((Item)Item1).FItemType - ((Item)Item2).FItemType;
    }

    // intf_P2_04.inc:118

    // intf_P2_04.inc:118

}
