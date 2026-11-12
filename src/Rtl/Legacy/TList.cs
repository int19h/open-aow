namespace Aow.Rtl;

public delegate int ListSortCompare(object? left, object? right);

public class DelphiList : IDisposable
{
    private object?[] items = [];
    private int count;

    public object?[] List => items;

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
    public object? this[int index]
    {
        get { DelphiListCore.Check(index, count); return items[index]; }
        set { DelphiListCore.Check(index, count); items[index] = value; }
    }

    // CLASSES.PAS TList.Error.
    public static void Error(string message, int data) => throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(message, data));

    // CLASSES.PAS TList.Grow.
    protected virtual void Grow() => Capacity += DelphiListCore.Growth(Capacity);

    // CLASSES.PAS TList.Add.
    public int Add(object? item)
    {
        if (count == Capacity) Grow();
        items[count] = item;
        return count++;
    }

    // CLASSES.PAS TList.Insert.
    public void Insert(int index, object? item)
    {
        DelphiListCore.CheckInsert(index, count);
        if (count == Capacity) Grow();
        DelphiListCore.InsertAt(items, ref count, index, item);
    }

    // CLASSES.PAS TList.Delete leaves the spare slot intact.
    public void Delete(int index) => DelphiListCore.Delete(items, ref count, index);

    // CLASSES.PAS TList.IndexOf compares pointer identity.
    public int IndexOf(object? item) => DelphiListCore.IndexOf(items, count, item);

    // CLASSES.PAS TList.Remove.
    public int Remove(object? item) => DelphiListCore.Remove(items, ref count, item);

    // CLASSES.PAS TList.Exchange.
    public void Exchange(int first, int second) => DelphiListCore.Exchange(items, count, first, second);

    // CLASSES.PAS TList.Move.
    public void Move(int current, int target)
    {
        if (current == target) return;
        DelphiListCore.Check(target, count);
        object? item = this[current];
        Delete(current);
        Insert(target, item);
    }

    // CLASSES.PAS TList.First and TList.Last.
    public object? First() => this[0];
    public object? Last() => this[count - 1];

    // CLASSES.PAS TList.Clear.
    public void Clear() { Count = 0; Capacity = 0; }

    // CLASSES.PAS TList.Pack.
    public void Pack() => DelphiListCore.Pack(items, ref count);

    // CLASSES.PAS TList.Expand.
    public DelphiList Expand()
    {
        if (count == Capacity) Grow();
        return this;
    }

    // CLASSES.PAS TList.Sort and QuickSort. Equal keys retain the Delphi swap order.
    public void Sort(ListSortCompare compare) => DelphiListCore.Sort(items, count, compare);

    // Keeps the order of equal items. The address tie-breaks call this, and every other sort keeps the quicksort.
    public void SortStable(ListSortCompare compare) => DelphiListCore.SortStable(items, count, compare);

    // CLASSES.PAS TList.Destroy.
    public virtual void Dispose() { Clear(); }
}
