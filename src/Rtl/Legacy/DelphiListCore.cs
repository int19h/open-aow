namespace Aow.Rtl;

// Shared TList algorithms. DelphiList and DelphiList<T> both call these, so the spare slot,
// the pointer index and the Delphi quicksort stay one implementation.
internal static class DelphiListCore
{
    private interface ICompare<T>
    {
        int Compare(T left, T right);
    }

    private readonly struct ObjectCompare(ListSortCompare compare) : ICompare<object?>
    {
        public int Compare(object? left, object? right) => compare(left, right);
    }

    private readonly struct TypedCompare<T>(ListSortCompare<T> compare) : ICompare<T?>
        where T : class
    {
        public int Compare(T? left, T? right) => compare(left, right);
    }

    internal static int Growth(int capacity) => capacity > 64 ? capacity / 4 : capacity > 8 ? 16 : 4;

    internal static void Check(int index, int count)
    {
        if (index < 0 || index >= count) DelphiList.Error("List index out of bounds (%d)", index);
    }

    internal static void CheckInsert(int index, int count)
    {
        if (index < 0 || index > count) DelphiList.Error("List index out of bounds (%d)", index);
    }

    internal static void SetCount<T>(ref T[] items, ref int count, int value)
    {
        if (value < 0 || value > 134217727) DelphiList.Error("List count out of bounds (%d)", value);
        if (value > items.Length) SetCapacity(ref items, count, value);
        if (value > count) Array.Clear(items, count, value - count);
        count = value;
    }

    internal static void SetCapacity<T>(ref T[] items, int count, int value)
    {
        if (value < count || value > 134217727) DelphiList.Error("List capacity out of bounds (%d)", value);
        Array.Resize(ref items, value);
    }

    internal static void InsertAt<T>(T[] items, ref int count, int index, T item)
    {
        Array.Copy(items, index, items, index + 1, count - index);
        items[index] = item;
        count++;
    }

    // CLASSES.PAS TList.Delete leaves the spare slot at List[Count].
    internal static void Delete<T>(T[] items, ref int count, int index)
    {
        Check(index, count);
        count--;
        Array.Copy(items, index + 1, items, index, count - index);
    }

    internal static bool Same(object? left, object? right) =>
        ReferenceEquals(left, right) || left is IPointerBox leftBox && right is IPointerBox rightBox &&
        ReferenceEquals(leftBox.StorageIdentity, rightBox.StorageIdentity) && leftBox.ByteIndex == rightBox.ByteIndex;

    internal static int IndexOf<T>(T[] items, int count, T item)
    {
        for (int i = 0; i < count; i++)
            if (Same(items[i], item)) return i;
        return -1;
    }

    internal static int Remove<T>(T[] items, ref int count, T item)
    {
        int index = IndexOf(items, count, item);
        if (index >= 0) Delete(items, ref count, index);
        return index;
    }

    internal static void Exchange<T>(T[] items, int count, int first, int second)
    {
        Check(first, count);
        Check(second, count);
        (items[first], items[second]) = (items[second], items[first]);
    }

    internal static void Pack<T>(T[] items, ref int count)
    {
        for (int i = count - 1; i >= 0; i--)
            if (items[i] is null) Delete(items, ref count, i);
    }

    internal static void Sort(object?[] items, int count, ListSortCompare compare)
    {
        if (count > 0) QuickSort(items, 0, count - 1, new ObjectCompare(compare));
    }

    internal static void Sort<T>(T?[] items, int count, ListSortCompare<T> compare)
        where T : class
    {
        if (count > 0) QuickSort(items, 0, count - 1, new TypedCompare<T>(compare));
    }

    internal static void SortStable(object?[] items, int count, ListSortCompare compare) =>
        SortStable(items, count, new ObjectCompare(compare));

    internal static void SortStable<T>(T?[] items, int count, ListSortCompare<T> compare)
        where T : class =>
        SortStable(items, count, new TypedCompare<T>(compare));

    private static void SortStable<T, TCompare>(T[] items, int count, TCompare compare)
        where TCompare : ICompare<T>
    {
        for (int i = 1; i < count; i++)
        {
            T item = items[i];
            int j = i - 1;
            while (j >= 0 && compare.Compare(items[j], item) > 0)
            {
                items[j + 1] = items[j];
                j--;
            }

            items[j + 1] = item;
        }
    }

    // CLASSES.PAS TList.QuickSort. Pivot is the stored element. Equal keys keep the Delphi swap order.
    private static void QuickSort<T, TCompare>(T[] items, int left, int right, TCompare compare)
        where TCompare : ICompare<T>
    {
        int i;
        do
        {
            i = left;
            int j = right;
            T pivot = items[(left + right) >> 1];
            do
            {
                while (compare.Compare(items[i], pivot) < 0) i++;
                while (compare.Compare(items[j], pivot) > 0) j--;
                if (i <= j)
                {
                    (items[i], items[j]) = (items[j], items[i]);
                    i++;
                    j--;
                }
            }
            while (i <= j);
            if (left < j) QuickSort<T, TCompare>(items, left, j, compare);
            left = i;
        }
        while (i < right);
    }
}
