using Aow.Rtl;

namespace Aow.Units.Engine;

public partial class TBitList
{
    // Engine.pas:4646. The unsigned comparison sends negative indices to the default.
    internal bool Get(int index) => (uint)index < (uint)FCount
        ? (FList[index >> 3] & (1 << (index & 7))) != 0
        : (System.Runtime.CompilerServices.Unsafe.As<bool, byte>(ref FDefault) & 1) != 0;

    // Engine.pas:4660. The original writes before the array for a negative index.
    internal void Put(int index, bool item)
    {
        if (index < 0) throw new ERangeError("Negative bit index");
        if ((uint)index >= (uint)FCount) SetCount(index + 1);
        var pointer = FList;
        byte mask = (byte)(1 << (index & 7));
        if (item) pointer[index >> 3] |= mask;
        else pointer[index >> 3] &= (byte)~mask;
    }
}
