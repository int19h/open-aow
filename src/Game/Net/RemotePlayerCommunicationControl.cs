// Translated from NetworkE.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net;

// FLocalSystemID, FPackageDataList, FOnSendData. The control of a network game: the transport
// sends through OnSendData and hands what arrives to DataReceived
public partial class RemotePlayerCommunicationControl : PlayerCommunicationControl
{
    internal int _localSystemId;
    internal PackageDataList _packageDataList;
    internal SendDataEvent _onSendData;

    // NetworkE.pas:819
    public override int GetLocalSystemId() => _localSystemId;

    // NetworkE.pas:782
    // { The number of different system IDs among the players }
    public override int GetSystemCount()
    {
        int J;
        int id;
        bool unique;
        int result = 0;
        for (int I = 0, last = _playerCommunicationSettingsList.Count - 1; I <= last; I++)
        {
            id = _playerCommunicationSettingsList.Players[I]._systemId;
            unique = true;
            J = 0;
            while (J != I && unique)
            {
                unique = id != _playerCommunicationSettingsList.Players[J]._systemId;
                J += 1;
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
        if (_packageDataList.Count != 0)
        {
            stream.SetLength(_packageDataList.PackageData[0].Length);
            var size = (int)stream.Length;
            Move(_packageDataList.PackageData[0].GetBuffer().AsSpan(0, size), stream.GetBuffer().AsSpan(0, size), size);
            _packageDataList.Delete(0);
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
            systemId = _playerCommunicationSettingsList.Players[player]._systemId;
        }

        if (systemId != _localSystemId)
        {
            var size = (int)stream.Length;
            result = SendData(stream.GetBuffer(), size, systemId);
        }
        else
        {
            result = true;
        }

        if (player == NetworkSession.AllPlayers || systemId == _localSystemId)
        {
            _packageDataList.Add(stream);
        }

        return result;
    }

    // NetworkE.pas:759
    public RemotePlayerCommunicationControl() : base()
    {
        _packageDataList = new PackageDataList();
    }

    // NetworkE.pas:765
    public override void Dispose()
    {
        _packageDataList?.Dispose();
        base.Dispose();
    }

    // NetworkE.pas:771
    public void Clear()
    {
        _packageDataList.Clear();
    }

    // NetworkE.pas:803
    public void SetConnectionMode(int index, ConnectionMode mode)
    {
        _playerCommunicationSettingsList.Players[index]._connectionMode = mode;
    }

    // NetworkE.pas:808
    public void SetSystemId(int index, int systemId)
    {
        _playerCommunicationSettingsList.Players[index]._systemId = systemId;
    }

    // NetworkE.pas:813
    public void Setup(int count, int localSystemId)
    {
        base.Setup(count);
        _localSystemId = localSystemId;
    }

    // NetworkE.pas:837
    public void DataReceived(object data, int size, int systemId)
    {
        _packageDataList.AddData(data, size);
    }

    // NetworkE.pas:842
    public bool SendData(object data, int size, int systemId)
    {
        bool result = default;
        if (_onSendData != null)
        {
            result = _onSendData(data, size, systemId);
        }
        else
        {
            result = false;
        }

        return result;
    }

    public SendDataEvent OnSendData { get => _onSendData; set => _onSendData = value; }

}
