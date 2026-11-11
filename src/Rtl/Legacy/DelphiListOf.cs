namespace Aow.Rtl;

public delegate int ListSortCompare<T>(T? left, T? right);

// Typed TList. Storage and algorithms are DelphiListCore, shared with DelphiList.
public class DelphiList<T> : IDisposable where T : class
{
    private T?[] items = [];
    private int count;

    public T?[] List => items;

    public int Count
    {
        get => count;
        // CLASSES.PAS TList.SetCount.
        set => DelphiListCore.SetCount(ref items, ref count, value);
    }

    public int Capacity
    {
        get => items.Length;
        // CLASSES.PAS TList.SetCapacity.
        set => DelphiListCore.SetCapacity(ref items, count, value);
    }

    // CLASSES.PAS TList.Get and TList.Put.
    public T? this[int index]
    {
        get { DelphiListCore.Check(index, count); return items[index]; }
        set { DelphiListCore.Check(index, count); items[index] = value; }
    }

    // CLASSES.PAS TList.Grow.
    protected virtual void Grow() => Capacity += DelphiListCore.Growth(Capacity);

    // CLASSES.PAS TList.Add.
    public int Add(T? item)
    {
        if (count == Capacity) Grow();
        items[count] = item;
        return count++;
    }

    // CLASSES.PAS TList.Insert.
    public void Insert(int index, T? item)
    {
        DelphiListCore.CheckInsert(index, count);
        if (count == Capacity) Grow();
        DelphiListCore.InsertAt(items, ref count, index, item);
    }

    // CLASSES.PAS TList.Delete leaves the spare slot intact.
    public void Delete(int index) => DelphiListCore.Delete(items, ref count, index);

    // CLASSES.PAS TList.IndexOf compares pointer identity, including IPointerBox aliases.
    public int IndexOf(T? item) => DelphiListCore.IndexOf(items, count, item);

    // CLASSES.PAS TList.Remove returns the index, or -1.
    public int Remove(T? item) => DelphiListCore.Remove(items, ref count, item);

    // CLASSES.PAS TList.Exchange.
    public void Exchange(int first, int second) => DelphiListCore.Exchange(items, count, first, second);

    // CLASSES.PAS TList.Move.
    public void Move(int current, int target)
    {
        if (current == target) return;
        DelphiListCore.Check(target, count);
        T? item = this[current];
        Delete(current);
        Insert(target, item);
    }

    // CLASSES.PAS TList.First and TList.Last.
    public T? First() => this[0];
    public T? Last() => this[count - 1];

    // CLASSES.PAS TList.Clear.
    public void Clear() { Count = 0; Capacity = 0; }

    // CLASSES.PAS TList.Pack.
    public void Pack() => DelphiListCore.Pack(items, ref count);

    // CLASSES.PAS TList.Expand.
    public DelphiList<T> Expand()
    {
        if (count == Capacity) Grow();
        return this;
    }

    // CLASSES.PAS TList.Sort. Equal keys retain the Delphi swap order.
    public void Sort(ListSortCompare<T> compare) => DelphiListCore.Sort(items, count, compare);

    // Keeps the order of equal items. The address tie-breaks call this, and every other sort keeps the quicksort.
    public void SortStable(ListSortCompare<T> compare) => DelphiListCore.SortStable(items, count, compare);

    // CLASSES.PAS TList.Destroy.
    public virtual void Dispose() { Clear(); }
}
