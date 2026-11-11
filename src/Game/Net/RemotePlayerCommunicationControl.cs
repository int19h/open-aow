// Translated from NetworkE.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net;

// FLocalSystemID, FPackageDataList, FOnSendData. The control of a network game: the transport
// sends through OnSendData and hands what arrives to DataReceived
public partial class RemotePlayerCommunicationControl : PlayerCommunicationControl
{
    internal int FLocalSystemID;
    internal PackageDataList FPackageDataList;
    internal SendDataEvent FOnSendData;

    // NetworkE.pas:819
    public override int GetLocalSystemID() => FLocalSystemID;

    // NetworkE.pas:782
    // { The number of different system IDs among the players }
    public override int GetSystemCount()
    {
        int J;
        int ID;
        bool Unique;
        int Result = 0;
        for (int I = 0, last = FPCSList.Count - 1; I <= last; I++)
        {
            ID = FPCSList.Players[I].FSystemID;
            Unique = true;
            J = 0;
            while (J != I && Unique)
            {
                Unique = ID != FPCSList.Players[J].FSystemID;
                J += 1;
            }

            if (Unique)
            {
                Result += 1;
            }
        }

        return Result;
    }

    // NetworkE.pas:776
    public override string Name() => "Remote";

    // NetworkE.pas:824
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

    // NetworkE.pas:852
    // { A package for every player (AllPlayers) goes to system 0, the transport's broadcast; a package
    //   for every player or for a player on this system is also received here. }
    public override bool SendPackageData(int APlayer, MemoryStream Stream)
    {
        bool Result;
        int SystemID;
        if (APlayer == NetworkSession.AllPlayers)
        {
            SystemID = 0;
        }
        else
        {
            SystemID = FPCSList.Players[APlayer].FSystemID;
        }

        if (SystemID != FLocalSystemID)
        {
            var size = (int)Stream.Length;
            Result = SendData(Stream.GetBuffer(), size, SystemID);
        }
        else
        {
            Result = true;
        }

        if (APlayer == NetworkSession.AllPlayers || SystemID == FLocalSystemID)
        {
            FPackageDataList.Add(Stream);
        }

        return Result;
    }

    // NetworkE.pas:759
    public RemotePlayerCommunicationControl() : base()
    {
        FPackageDataList = new PackageDataList();
    }

    // NetworkE.pas:765
    public override void Dispose()
    {
        FPackageDataList?.Dispose();
        base.Dispose();
    }

    // NetworkE.pas:771
    public void Clear()
    {
        FPackageDataList.Clear();
    }

    // NetworkE.pas:803
    public void SetConnectionMode(int Index, ConnectionMode Mode)
    {
        FPCSList.Players[Index].FConnectionMode = Mode;
    }

    // NetworkE.pas:808
    public void SetSystemID(int Index, int ASystemID)
    {
        FPCSList.Players[Index].FSystemID = ASystemID;
    }

    // NetworkE.pas:813
    public void Setup(int ACount, int ALocalSystemID)
    {
        base.Setup(ACount);
        FLocalSystemID = ALocalSystemID;
    }

    // NetworkE.pas:837
    public void DataReceived(object Data, int Size, int SystemID)
    {
        FPackageDataList.AddData(Data, Size);
    }

    // NetworkE.pas:842
    public bool SendData(object Data, int Size, int SystemID)
    {
        bool Result = default;
        if (FOnSendData != null)
        {
            Result = FOnSendData(Data, Size, SystemID);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    public SendDataEvent OnSendData { get => FOnSendData; set => FOnSendData = value; }

}
