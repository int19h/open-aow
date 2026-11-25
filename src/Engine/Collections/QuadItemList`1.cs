// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Collections;

public sealed class QuadItemList<TA> : RecordList<QuadItem<TA>> where TA : class
{
    public int AddABCD(TA A, int B, int C, int D) => Add(new QuadItem<TA> { First = A, Second = B, Third = C, Fourth = D });

    // In range the indexer returns List[i]. ReferenceEquals keeps the loop's test.
    public int IndexOfItemA(TA A) => this.FindIndex(entry => ReferenceEquals(entry.First, A));

    public int IndexOfItemB(int B) => this.FindIndex(entry => entry.Second == B);

    public int IndexOfItemC(int C) => this.FindIndex(entry => entry.Third == C);

    public int IndexOfItemD(int D) => this.FindIndex(entry => entry.Fourth == D);

    public void SortAscendingItemB() => Sort(static (in item1, in item2) => item1.Second - item2.Second);

    public void SortAscendingItemC() => Sort(static (in item1, in item2) => item1.Third - item2.Third);

    public void SortAscendingItemD() => Sort(static (in item1, in item2) => item1.Fourth - item2.Fourth);

    public void SortDescendingItemB() => Sort(static (in item1, in item2) => item2.Second - item1.Second);

    public void SortDescendingItemC() => Sort(static (in item1, in item2) => item2.Third - item1.Third);

    public void SortDescendingItemD() => Sort(static (in item1, in item2) => item2.Fourth - item1.Fourth);
}
