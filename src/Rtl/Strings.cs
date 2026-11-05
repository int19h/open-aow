using static Aow.Rtl.Rtl;

namespace Aow.Rtl;

public sealed class IndexedProperty<TKey, TValue>(Func<TKey, TValue> get, Action<TKey, TValue>? set = null) where TKey : notnull
{
    public TValue this[TKey key] { get => get(key); set { if (set is null) throw new EInvalidOperation("Read-only property"); set(key, value); } }
}

public abstract class TStrings : TPersistent
{
    protected int UpdateCount;
    public abstract int Count { get; }
    public virtual int Capacity { get => Count; set { } }
    public abstract string this[int index] { get; set; }
    protected virtual TObject? GetObject(int index) => null;
    protected virtual void PutObject(int index, TObject? value) { }
    public IndexedProperty<int, string> Strings => new(i => this[i], (i, s) => this[i] = s);
    public IndexedProperty<int, TObject?> Objects => new(GetObject, PutObject);
    // CLASSES.PAS TStrings.GetName, GetValue and SetValue.
    public IndexedProperty<int, string> Names => new(i => { var s = this[i]; return Copy(s, 1, AnsiPos("=", s) - 1); });
    public IndexedProperty<string, string> Values => new(name =>
    {
        int i = IndexOfName(name); return i < 0 ? "" : Copy(this[i], name.Length + 2, int.MaxValue);
    }, (name, value) =>
    {
        int i = IndexOfName(name);
        if (value.Length > 0) { if (i < 0) i = Add(""); this[i] = name + "=" + value; }
        else if (i >= 0) Delete(i);
    });
    // CLASSES.PAS TStrings.Add, AddObject and InsertObject.
    public virtual int Add(string value) { int i = Count; Insert(i, value); return i; }
    public int AddObject(string value, TObject? item) { int i = Add(value); PutObject(i, item); return i; }
    public void InsertObject(int index, string value, TObject? item) { Insert(index, value); PutObject(index, item); }
    public abstract void Insert(int index, string value);
    public abstract void Delete(int index);
    public abstract void Clear();
    // CLASSES.PAS TStrings.IndexOf, IndexOfName and IndexOfObject.
    public virtual int IndexOf(string value) { for (int i = 0; i < Count; i++) if (AnsiCompareText(this[i], value) == 0) return i; return -1; }
    public int IndexOfName(string name)
    {
        for (int i = 0; i < Count; i++) { var s = this[i]; int p = AnsiPos("=", s); if (p > 0 && AnsiCompareText(Copy(s, 1, p - 1), name) == 0) return i; }
        return -1;
    }
    public int IndexOfObject(TObject? item) { for (int i = 0; i < Count; i++) if (ReferenceEquals(GetObject(i), item)) return i; return -1; }
    // CLASSES.PAS TStrings.GetTextStr and SetTextStr. The reader stops at NUL.
    public string Text
    {
        get { string text = ""; for (int i = 0; i < Count; i++) text += this[i] + "\r\n"; return text; }
        set
        {
            BeginUpdate();
            try
            {
                Clear(); int p = 1;
                while (p <= value.Length && value[p - 1] != 0)
                {
                    int start = p;
                    while (p <= value.Length && value[p - 1] is not ('\0' or '\n' or '\r')) p++;
                    Add(Copy(value, start, p - start));
                    if (p <= value.Length && value[p - 1] == 13) p++;
                    if (p <= value.Length && value[p - 1] == 10) p++;
                }
            }
            finally { EndUpdate(); }
        }
    }
    // CLASSES.PAS TStrings.GetCommaText and SetCommaText.
    public string CommaText
    {
        get
        {
            if (Count == 1 && this[0].Length == 0) return "\"\"";
            string text = "";
            for (int i = 0; i < Count; i++)
            {
                var s = this[i];
                bool quote = false;
                for (int p = 1; p <= s.Length && s[p - 1] != 0; p++) if (s[p - 1] <= 32 || s[p - 1] is '"' or ',') { quote = true; break; }
                if (quote) s = "\"" + s.Replace("\"", "\"\"") + "\"";
                if (i > 0) text += ",";
                text += s;
            }
            return text;
        }
        set
        {
            BeginUpdate();
            try
            {
                Clear(); int p = 1;
                byte At() => p <= value.Length ? Cp1252.Byte(value[p - 1]) : (byte)0;
                void Spaces() { while (At() is >= 1 and <= 32) p++; }
                Spaces();
                while (At() != 0)
                {
                    string s = "";
                    if (At() == 34)
                    {
                        p++;
                        while (At() != 0)
                        {
                            // The char itself, so a char outside the table stays itself (strings.md 2.6).
                            char character = value[p - 1]; byte c = At(); p++;
                            if (c == 34) { if (At() != 34) break; p++; }
                            s += character;
                        }
                    }
                    else
                    {
                        int start = p; while (At() > 32 && At() != 44) p++;
                        s = Copy(value, start, p - start);
                    }
                    Add(s); Spaces(); if (At() == 44) { p++; Spaces(); }
                }
            }
            finally { EndUpdate(); }
        }
    }
    // CLASSES.PAS TStrings.BeginUpdate, EndUpdate and SetUpdateState.
    public void BeginUpdate() { if (UpdateCount == 0) SetUpdateState(true); UpdateCount++; }
    public void EndUpdate() { UpdateCount--; if (UpdateCount == 0) SetUpdateState(false); }
    protected virtual void SetUpdateState(bool updating) { }
    // CLASSES.PAS TStrings.AddStrings and Assign.
    public void AddStrings(TStrings source)
    {
        BeginUpdate(); try { int count = source.Count; for (int i = 0; i < count; i++) AddObject(source[i], source.Objects[i]); } finally { EndUpdate(); }
    }
    public override void Assign(TPersistent? source)
    {
        if (source is not TStrings strings) { base.Assign(source); return; }
        BeginUpdate(); try { Clear(); AddStrings(strings); } finally { EndUpdate(); }
    }
    // CLASSES.PAS TStrings.Exchange and Move.
    public virtual void Exchange(int first, int second)
    {
        BeginUpdate();
        try { var s = this[first]; var o = GetObject(first); this[first] = this[second]; PutObject(first, GetObject(second)); this[second] = s; PutObject(second, o); }
        finally { EndUpdate(); }
    }
    public void Move(int current, int target)
    {
        if (current == target) return;
        BeginUpdate(); try { var s = this[current]; var o = GetObject(current); Delete(current); InsertObject(target, s, o); } finally { EndUpdate(); }
    }
    // CLASSES.PAS TStrings.LoadFromStream and SaveToStream.
    public virtual void LoadFromStream(TStream stream)
    {
        BeginUpdate();
        try { byte[] bytes = new byte[stream.Size - stream.Position]; stream.Read(bytes); Text = Cp1252.Decode(bytes); }
        finally { EndUpdate(); }
    }
    public virtual void SaveToStream(TStream stream) => stream.WriteBuffer(Cp1252.Encode(Text));
    // CLASSES.PAS TStrings.LoadFromFile and SaveToFile.
    public void LoadFromFile(string name) { using var stream = new TFileStream(name, fmOpenRead); LoadFromStream(stream); }
    public void SaveToFile(string name) { using var stream = new TFileStream(name, fmCreate); SaveToStream(stream); }
    // CLASSES.PAS TStrings.Error.
    public static void Error(string message, int data) => throw EStringListError.CreateFmt(message, data);
}

public enum TDuplicates : byte { dupIgnore, dupAccept, dupError }

public class TStringList : TStrings
{
    private record struct StringItem(string Text, TObject? Object);
    private StringItem[] items = [];
    private int count;
    private bool sorted;
    public override int Count => count;
    // CLASSES.PAS TStringList.SetCapacity.
    public override int Capacity { get => items.Length; set { if (value < count) Error("List capacity out of bounds (%d)", value); Array.Resize(ref items, value); } }
    public TDuplicates Duplicates { get; set; }
    public TNotifyEvent? OnChange { get; set; }
    public TNotifyEvent? OnChanging { get; set; }
    // CLASSES.PAS TStringList.Changed, Changing and SetUpdateState.
    protected virtual void Changed() { if (UpdateCount == 0) OnChange?.Invoke(this); }
    protected virtual void Changing() { if (UpdateCount == 0) OnChanging?.Invoke(this); }
    protected override void SetUpdateState(bool updating) { if (updating) Changing(); else Changed(); }
    private void Check(int index) { if (index < 0 || index >= count) Error("List index out of bounds (%d)", index); }
    // CLASSES.PAS TStringList.Get and Put.
    public override string this[int index]
    {
        get { Check(index); return items[index].Text; }
        set { if (Sorted) Error("Operation not allowed on sorted string list", 0); Check(index); Changing(); items[index].Text = value; Changed(); }
    }
    // CLASSES.PAS TStringList.GetObject and PutObject.
    protected override TObject? GetObject(int index) { Check(index); return items[index].Object; }
    protected override void PutObject(int index, TObject? value) { Check(index); Changing(); items[index].Object = value; Changed(); }
    // CLASSES.PAS TStringList.SetSorted.
    public bool Sorted { get => sorted; set { if (sorted == value) return; if (value) Sort(); sorted = value; } }
    // CLASSES.PAS TStringList.Grow.
    private void Grow() => Capacity += Capacity > 64 ? Capacity / 4 : Capacity > 8 ? 16 : 4;
    // CLASSES.PAS TStringList.InsertItem.
    private void InsertItem(int index, string value)
    {
        Changing(); if (count == Capacity) Grow(); Array.Copy(items, index, items, index + 1, count - index); items[index] = new(value, null); count++; Changed();
    }
    // CLASSES.PAS TStringList.Add.
    public override int Add(string value)
    {
        int i = count;
        if (Sorted && Find(value, ref i))
        {
            if (Duplicates == TDuplicates.dupIgnore) return i;
            if (Duplicates == TDuplicates.dupError) Error("String list does not allow duplicates", 0);
        }
        InsertItem(i, value); return i;
    }
    // CLASSES.PAS TStringList.Insert, Delete and Clear.
    public override void Insert(int index, string value)
    {
        if (Sorted) Error("Operation not allowed on sorted string list", 0);
        if (index < 0 || index > count) Error("List index out of bounds (%d)", index);
        InsertItem(index, value);
    }
    public override void Delete(int index) { Check(index); Changing(); count--; Array.Copy(items, index + 1, items, index, count - index); items[count] = default; Changed(); }
    public override void Clear() { if (count == 0) return; Changing(); count = 0; Capacity = 0; Changed(); }
    // CLASSES.PAS TStringList.Exchange and ExchangeItems.
    public override void Exchange(int first, int second) { Check(first); Check(second); Changing(); (items[first], items[second]) = (items[second], items[first]); Changed(); }
    // CLASSES.PAS TStringList.Find. dupAccept keeps searching to the left.
    public bool Find(string value, ref int index)
    {
        bool result = false; int low = 0, high = count - 1;
        while (low <= high)
        {
            int i = (low + high) >> 1, c = AnsiCompareText(items[i].Text, value);
            if (c < 0) low = i + 1;
            else { high = i - 1; if (c == 0) { result = true; if (Duplicates != TDuplicates.dupAccept) low = i; } }
        }
        index = low; return result;
    }
    // CLASSES.PAS TStringList.IndexOf.
    public override int IndexOf(string value)
    {
        if (!Sorted) return base.IndexOf(value);
        int index = 0;
        return Find(value, ref index) ? index : -1;
    }
    // CLASSES.PAS TStringList.Sort and QuickSort.
    public void Sort() { if (Sorted || count <= 1) return; Changing(); QuickSort(0, count - 1); Changed(); }
    private void QuickSort(int left, int right)
    {
        int i;
        do
        {
            i = left; int j = right; var pivot = items[(left + right) >> 1].Text;
            do
            {
                while (AnsiCompareText(items[i].Text, pivot) < 0) i++;
                while (AnsiCompareText(items[j].Text, pivot) > 0) j--;
                if (i <= j) { (items[i], items[j]) = (items[j], items[i]); i++; j--; }
            } while (i <= j);
            if (left < j) QuickSort(left, j); left = i;
        } while (i < right);
    }
    // CLASSES.PAS TStringList.Destroy does not destroy associated objects.
    public override void Destroy() { OnChange = null; OnChanging = null; base.Destroy(); count = 0; Capacity = 0; }
}
