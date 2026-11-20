// Translated from NetworkE.pas.
using Aow.Engine.Nodes;

using static Aow.Rtl.DelphiMemory;

namespace Aow.Game.Net;

// FPackageDataList. The control of a game on one system: what is sent is received again
public class LocalPlayerCommunicationControl(EngineObject creator) : PlayerCommunicationControl(creator)
{

    internal PackageDataList PackageDataList { get; } = new PackageDataList();

    // NetworkE.pas:709
    public override int GetSystemCount() => 1;

    // NetworkE.pas:714
    public override string Name() => "Local";

    // NetworkE.pas:724
    public override bool ReceivePackageData(MemoryStream stream)
    {
        if (PackageDataList.Count != 0)
        {
            stream.SetLength(PackageDataList.PackageData[0].Length);
            int size = (int)stream.Length;
            Move(PackageDataList.PackageData[0].GetBuffer().AsSpan(0, length: size), destination: stream.GetBuffer().AsSpan(0, length: size), count: size);
            PackageDataList.Delete(0);
            return true;
        }
        else
        {
            return false;
        }
    }

    // NetworkE.pas:737
    public override bool SendPackageData(int player, MemoryStream stream)
    {
        PackageDataList.Add(stream);
        return true;
    }

    // NetworkE.pas:703
    public override void Dispose()
    {
        PackageDataList?.Dispose();
        base.Dispose();
    }

    // NetworkE.pas:719
    public void Clear() => PackageDataList.Clear();

    // NetworkE.pas:743
    public new void Setup(int count)
    {
        _ = base.Setup(count);
        for (int i = PlayerCommunicationSettingsList.Count - 1; i >= 0; i--)
        {
            int localSystemId = LocalSystemId;
            PlayerCommunicationSettingsList.Players[i].SystemId = localSystemId;
        }
    }

    // NetworkE.pas:752
    public void SetConnectionMode(int index, ConnectionMode mode) => PlayerCommunicationSettingsList.Players[index].ConnectionMode = mode;

}
