
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// FList, the owned points
public partial class XYZList : IDisposable
{

    internal List<XYZ> FList;

    // aowFX.pas:93
    public XYZList()
    {
        FList = new List<XYZ>();
    }

    // aowFX.pas:98
    public virtual void Dispose()
    {
        for (int I = 0, last = FList.Count - 1; I <= last; I++)
        {
            _ = GetXYZ(I);
        }

        FList?.Clear();
    }

    // aowFX.pas:108
    public void Clear()
    {
        for (int I = 0, last = FList.Count - 1; I <= last; I++)
        {
            _ = GetXYZ(I);
        }

        FList.Clear();
    }

    // aowFX.pas:117
    public void Add(XYZ XYZ)
    {
        FList.Add(XYZ);
    }

    // aowFX.pas:122
    public void Delete(int Index)
    {
        FList.RemoveAt(Index);
    }

    // aowFX.pas:128
    public XYZ GetXYZ(int Index)
    {
        XYZ Result;
        if (Index >= 0 && Index < FList.Count)
        {
            Result = FList[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // aowFX.pas:136
    public int GetCount() => FList.Count;

    public int Count { get => GetCount(); }

    public XYZ this[int Index] { get => GetXYZ(Index); }

    internal static void ScatterPath(ref XYZList List)
    {
        int RX;
        int RY;
        int DX;
        int DY;
        XYZ XYZ;
        DX = 0;
        DY = 0;
        for (int I = 1, last = List.Count - 2; I <= last; I++)
        {
            XYZ = List[I];
            RX = Random(3);
            RY = Random(3);
            if (RX - DX > 1)
            {
                RX = DX + 1;
            }

            if (DX - RX > 1)
            {
                RX = DX - 1;
            }

            if (RY - DY > 1)
            {
                RY = DY + 1;
            }

            if (DY - RY > 1)
            {
                RY = DY - 1;
            }

            XYZ.X = XYZ.X + RX - 1;
            XYZ.Y = XYZ.Y + RY - 1;
            DX = RX;
            DY = RY;
        }
    }

}
