// Translated from Engine.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Engine.Collections;

// 256 lists selected by a hash of the first and the second-to-last character and the length
public partial class StringHashTable256 : StringHashTable
, IDisposable
{
    internal FixedArray_Aow_Units_Engine_TStringHashList_256 FLists;

    // HashTab.PAS:199
    public StringHashTable256()
    {
        for (int I = 0; I <= 255; I++)
        {
            var stringHashList = new StringHashList();
            FLists[I] = stringHashList;
        }
    }

    // HashTab.PAS:207
    public virtual void Dispose()
    {
        for (int I = 0; I <= 255; I++)
        {
            FLists[I]?.Dispose();
        }
    }

    // HashTab.PAS:215
    public override void Clear()
    {
        for (int I = 0; I <= 255; I++)
        {
            FLists[I].Clear();
        }
    }

    // HashTab.PAS:222
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

    // HashTab.PAS:234
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

    // HashTab.PAS:246
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

    // Engine.pas:816

}
