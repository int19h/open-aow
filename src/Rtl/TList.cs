namespace Aow.Rtl;

public delegate int TListSortCompare(object? left, object? right);

public class TList : TObject
{
    private object?[] items = [];
    private int count;
    public object?[] List => items;
    public int Count
    {
        get => count;
        // CLASSES.PAS TList.SetCount.
        set
        {
            if (value < 0 || value > 134217727) Error("List count out of bounds (%d)", value);
            if (value > Capacity) Capacity = value;
            if (value > count) Array.Clear(items, count, value - count);
            count = value;
        }
    }
    public int Capacity
    {
        get => items.Length;
        // CLASSES.PAS TList.SetCapacity.
        set
        {
            if (value < count || value > 134217727) Error("List capacity out of bounds (%d)", value);
            Array.Resize(ref items, value);
        }
    }
    // CLASSES.PAS TList.Get and TList.Put.
    public object? this[int index] { get { Check(index); return items[index]; } set { Check(index); items[index] = value; } }
    private void Check(int index) { if (index < 0 || index >= count) Error("List index out of bounds (%d)", index); }
    // CLASSES.PAS TList.Error.
    public static void Error(AnsiString message, int data) => throw EListError.CreateFmt(message, data);
    // CLASSES.PAS TList.Grow.
    protected virtual void Grow() => Capacity += Capacity > 64 ? Capacity / 4 : Capacity > 8 ? 16 : 4;
    // CLASSES.PAS TList.Add.
    public int Add(object? item) { if (count == Capacity) Grow(); items[count] = item; return count++; }
    // CLASSES.PAS TList.Insert.
    public void Insert(int index, object? item)
    {
        if (index < 0 || index > count) Error("List index out of bounds (%d)", index);
        if (count == Capacity) Grow();
        Array.Copy(items, index, items, index + 1, count - index);
        items[index] = item;
        count++;
    }
    // CLASSES.PAS TList.Delete leaves the spare slot intact.
    public void Delete(int index) { Check(index); count--; Array.Copy(items, index + 1, items, index, count - index); }
    // CLASSES.PAS TList.IndexOf compares pointer identity.
    public int IndexOf(object? item) { for (int i = 0; i < count; i++) if (ReferenceEquals(items[i], item)) return i; return -1; }
    // CLASSES.PAS TList.Remove.
    public int Remove(object? item) { int i = IndexOf(item); if (i >= 0) Delete(i); return i; }
    // CLASSES.PAS TList.Exchange.
    public void Exchange(int first, int second) { Check(first); Check(second); (items[first], items[second]) = (items[second], items[first]); }
    // CLASSES.PAS TList.Move.
    public void Move(int current, int target)
    {
        if (current == target) return;
        Check(target);
        object? item = this[current]; Delete(current); Insert(target, item);
    }
    // CLASSES.PAS TList.First and TList.Last.
    public object? First() => this[0];
    public object? Last() => this[count - 1];
    // CLASSES.PAS TList.Clear.
    public void Clear() { Count = 0; Capacity = 0; }
    // CLASSES.PAS TList.Pack.
    public void Pack() { for (int i = count - 1; i >= 0; i--) if (items[i] is null) Delete(i); }
    // CLASSES.PAS TList.Expand.
    public TList Expand() { if (count == Capacity) Grow(); return this; }
    // CLASSES.PAS TList.Sort and QuickSort. Equal keys retain the Delphi swap order.
    public void Sort(TListSortCompare compare) { if (count > 0) QuickSort(0, count - 1, compare); }
    private void QuickSort(int left, int right, TListSortCompare compare)
    {
        int i;
        do
        {
            i = left;
            int j = right;
            object? pivot = items[(left + right) >> 1];
            do
            {
                while (compare(items[i], pivot) < 0) i++;
                while (compare(items[j], pivot) > 0) j--;
                if (i <= j) { (items[i], items[j]) = (items[j], items[i]); i++; j--; }
            } while (i <= j);
            if (left < j) QuickSort(left, j, compare);
            left = i;
        } while (i < right);
    }
    // CLASSES.PAS TList.Destroy.
    public override void Destroy() { Clear(); base.Destroy(); }
}
