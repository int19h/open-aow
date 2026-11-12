// Translated from NetworkE.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net;

// FPackageDataList. The control of a game on one system: what is sent is received again
public partial class LocalPlayerCommunicationControl : PlayerCommunicationControl
{
    internal PackageDataList PackageDataList { get; }

    // NetworkE.pas:709
    public override int GetSystemCount() => 1;

    // NetworkE.pas:714
    public override string Name() => "Local";

    // NetworkE.pas:724
    public override bool ReceivePackageData(MemoryStream stream)
    {
        bool result;
        if (PackageDataList.Count != 0)
        {
            stream.SetLength(PackageDataList.PackageData[0].Length);
            var size = (int)stream.Length;
            Move(PackageDataList.PackageData[0].GetBuffer().AsSpan(0, size), stream.GetBuffer().AsSpan(0, size), size);
            PackageDataList.Delete(0);
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // NetworkE.pas:737
    public override bool SendPackageData(int player, MemoryStream stream)
    {
        PackageDataList.Add(stream);
        return true;
    }

    // NetworkE.pas:697
    public LocalPlayerCommunicationControl() : base()
    {
        PackageDataList = new PackageDataList();
    }

    // NetworkE.pas:703
    public override void Dispose()
    {
        PackageDataList?.Dispose();
        base.Dispose();
    }

    // NetworkE.pas:719
    public void Clear()
    {
        PackageDataList.Clear();
    }

    // NetworkE.pas:743
    public new void Setup(int count)
    {
        base.Setup(count);
        for (int I = PlayerCommunicationSettingsList.Count - 1; I >= 0; I--)
        {
            var localSystemId = GetLocalSystemId();
            PlayerCommunicationSettingsList.Players[I]._systemId = localSystemId;
        }
    }

    // NetworkE.pas:752
    public void SetConnectionMode(int index, ConnectionMode mode)
    {
        PlayerCommunicationSettingsList.Players[index]._connectionMode = mode;
    }

}
