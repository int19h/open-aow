// Translated from NetworkE.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net;

// FPackageDataList. The control of a game on one system: what is sent is received again
public partial class LocalPCC : PlayerCommunicationControl
{
    internal PackageDataList FPackageDataList;

    // NetworkE.pas:709
    public override int GetSystemCount() => 1;

    // NetworkE.pas:714
    public override string Name() => "Local";

    // NetworkE.pas:724
    public override bool ReceivePackageData(MemoryStream Stream)
    {
        bool Result;
        if (FPackageDataList.Count != 0)
        {
            Stream.SetLength(FPackageDataList.PackageData[0].Length);
            var size = (int)Stream.Length;
            Move(FPackageDataList.PackageData[0].GetBuffer().AsSpan(0, size), Stream.GetBuffer().AsSpan(0, size), size);
            FPackageDataList.Delete(0);
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // NetworkE.pas:737
    public override bool SendPackageData(int APlayer, MemoryStream Stream)
    {
        FPackageDataList.Add(Stream);
        return true;
    }

    // NetworkE.pas:697
    public LocalPCC() : base()
    {
        FPackageDataList = new PackageDataList();
    }

    // NetworkE.pas:703
    public override void Dispose()
    {
        FPackageDataList?.Dispose();
        base.Dispose();
    }

    // NetworkE.pas:719
    public void Clear()
    {
        FPackageDataList.Clear();
    }

    // NetworkE.pas:743
    public new void Setup(int ACount)
    {
        base.Setup(ACount);
        for (int I = FPCSList.Count - 1; I >= 0; I--)
        {
            var localSystemID = GetLocalSystemID();
            FPCSList.Players[I].FSystemID = localSystemID;
        }
    }

    // NetworkE.pas:752
    public void SetConnectionMode(int Index, ConnectionMode Mode)
    {
        FPCSList.Players[Index].FConnectionMode = Mode;
    }

}
