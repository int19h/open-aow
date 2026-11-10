using Aow.Engine.Buffers;
using Aow.Rtl;

namespace Aow.Engine.Hash;

// 256 lists selected by a hash of the first and the second-to-last character and the length
public partial class StringHashTable256 : StringHashTable
, IDisposable
{
    internal FixedArray_Aow_Units_HashTabU_TStringHashList_256 FLists;

    // HashTabU.pas:281
    public StringHashTable256()
    {
        for (int I = 0; I <= 255; I++)
        {
            var stringHashList = new StringHashList();
            FLists[I] = stringHashList;
        }
    }

    // HashTabU.pas:289
    public virtual void Dispose()
    {
        for (int I = 0; I <= 255; I++)
        {
            FLists[I]?.Dispose();
        }
    }

    // HashTabU.pas:297
    public override void Clear()
    {
        for (int I = 0; I <= 255; I++)
        {
            FLists[I].Clear();
        }
    }

    // HashTabU.pas:304
    public override void Add(object Item, string Key)
    {
        int H;
        H = Key.Length;
        if (H >= 2)
        {
            H = Cp1252.Byte(Key[0]) + Cp1252.Byte(Key[H - 2]) + (H << 4) & 255;
        }
        else
        {
            if (H == 1)
            {
                H = Cp1252.Byte(Key[0]);
            }
        }

        FLists[H].Add(Item, Key);
    }

    // HashTabU.pas:316
    public override void Delete(string Key)
    {
        int H;
        H = Key.Length;
        if (H >= 2)
        {
            H = Cp1252.Byte(Key[0]) + Cp1252.Byte(Key[H - 2]) + (H << 4) & 255;
        }
        else
        {
            if (H == 1)
            {
                H = Cp1252.Byte(Key[0]);
            }
        }

        FLists[H].Delete(Key);
    }

    // HashTabU.pas:328
    public override object Find(string Key)
    {
        int H;
        H = Key.Length;
        if (H >= 2)
        {
            H = Cp1252.Byte(Key[0]) + Cp1252.Byte(Key[H - 2]) + (H << 4) & 255;
        }
        else
        {
            if (H == 1)
            {
                H = Cp1252.Byte(Key[0]);
            }
        }

        return FLists[H].Find(Key);
    }

}
