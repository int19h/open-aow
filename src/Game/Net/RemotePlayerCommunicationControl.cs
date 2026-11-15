#nullable disable
// Translated from NetworkE.pas.
using Aow.Engine.Nodes;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net;

// FLocalSystemID, FPackageDataList, FOnSendData. The control of a network game: the transport
// sends through OnSendData and hands what arrives to DataReceived
public partial class RemotePlayerCommunicationControl : PlayerCommunicationControl
{
    private int _localSystemId;

    internal PackageDataList PackageDataList { get; }

    public event SendDataEvent OnSendData;

    // NetworkE.pas:819
    public override int GetLocalSystemId() => _localSystemId;

    // NetworkE.pas:782
    // { The number of different system IDs among the players }
    public override int GetSystemCount()
    {
        int j;
        int id;
        bool unique;
        int result = 0;
        for (int i = 0, last = PlayerCommunicationSettingsList.Count - 1; i <= last; i++)
        {
            id = PlayerCommunicationSettingsList.Players[i].SystemId;
            unique = true;
            j = 0;
            while (j != i && unique)
            {
                unique = id != PlayerCommunicationSettingsList.Players[j].SystemId;
                j += 1;
            }

            if (unique)
            {
                result += 1;
            }
        }

        return result;
    }

    // NetworkE.pas:776
    public override string Name() => "Remote";

    // NetworkE.pas:824
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

    // NetworkE.pas:852
    // { A package for every player (AllPlayers) goes to system 0, the transport's broadcast; a package
    //   for every player or for a player on this system is also received here. }
    public override bool SendPackageData(int player, MemoryStream stream)
    {
        bool result;
        int systemId;
        if (player == NetworkSession.AllPlayers)
        {
            systemId = 0;
        }
        else
        {
            systemId = PlayerCommunicationSettingsList.Players[player].SystemId;
        }

        if (systemId != _localSystemId)
        {
            result = SendData(size: (int)stream.Length, data: stream.GetBuffer(), systemId: systemId);
        }
        else
        {
            result = true;
        }

        if (player == NetworkSession.AllPlayers || systemId == _localSystemId)
        {
            PackageDataList.Add(stream);
        }

        return result;
    }

    // NetworkE.pas:759
    public RemotePlayerCommunicationControl(EngineObject creator) : base(creator) => PackageDataList = new PackageDataList();

    // NetworkE.pas:765
    public override void Dispose()
    {
        PackageDataList?.Dispose();
        base.Dispose();
    }

    // NetworkE.pas:771
    public void Clear() => PackageDataList.Clear();

    // NetworkE.pas:803
    public void SetConnectionMode(int index, ConnectionMode mode) => PlayerCommunicationSettingsList.Players[index].ConnectionMode = mode;

    // NetworkE.pas:808
    public void SetSystemId(int index, int systemId) => PlayerCommunicationSettingsList.Players[index].SystemId = systemId;

    // NetworkE.pas:813
    public void Setup(int count, int localSystemId)
    {
        Setup(count);
        _localSystemId = localSystemId;
    }

    // NetworkE.pas:837
    public void DataReceived(object data, int size, int systemId) => PackageDataList.AddData(data, size);

    // NetworkE.pas:842
    public bool SendData(object data, int size, int systemId)
    {
        bool result = default;
        if (OnSendData != null)
        {
            result = OnSendData(data, size, systemId);
        }
        else
        {
            result = false;
        }

        return result;
    }

}
