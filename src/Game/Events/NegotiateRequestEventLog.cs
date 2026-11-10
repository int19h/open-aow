// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Events;

// X/Y/L, MovePoints, ArmyID, Object, Data1/Data2. inherited join fields
public partial class NegotiateRequestEventLog : JoinEventLog
{
    // Diplomacy.pas:264
    public NegotiateRequestEventLog(ECustomNode owner) : base(owner)
    {
        FArmyID = null;
    }

    // Diplomacy.pas:279
    public new const int ClassID = 131814;

    public sbyte FX;
    public sbyte FY;
    public sbyte FL;
    public int FMovePoints;
    public ArmyID FArmyID;
    public NegotiateObject FObject;
    public int FData1;
    public int FData2;

    // Diplomacy.pas:270
    public override void Dispose()
    {
        ArmyID ID;
        ID = FArmyID;
        if (ID != null)
        {
            ID?.Dispose();
        }

        base.Dispose();
    }

    // Diplomacy.pas:284
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FX), 40);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FY), 41);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FL), 42);
        Stream.rwData(Bytes.Of(ref FObject, 1), 1, 43);
        Stream.rwInteger(ref FData1, 44);
        Stream.rwInteger(ref FData2, 45);
        Stream.rwCreateEObject(ref FArmyID, null, 46);
    }

    // Diplomacy.pas:320
    public override bool Accept(ref string ErrorText)
    {
        NegotiateTE TE;
        bool Result = base.Accept(ref ErrorText);
        if (Result)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType == PlayerType.Human && AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
            {
                return Result;
            }

            TE = CreateNegotiateTE();
            TE.FGold = FGold;
            TE.FOffer = NegotiateTE.NegotiateOffer.OfferBribe;
            TE.FResult = NegotiateTE.NegotiateResult.Accept;
            AoWHSMap.Current.FTokenManager.TokenControls[FPlayer].AddEvent(TE);
            TE.Release();
            Result = true;
        }

        return Result;
    }

    // Diplomacy.pas:359
    public override bool Cancel() => true;

    // Diplomacy.pas:339
    public override bool Reject(ref string ErrorText)
    {
        NegotiateTE TE;
        bool Result = base.Reject(ref ErrorText);
        if (Result)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType == PlayerType.Human && AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
            {
                return Result;
            }

            TE = CreateNegotiateTE();
            TE.FGold = 0;
            TE.FOffer = NegotiateTE.NegotiateOffer.ForceOffer;
            TE.FResult = NegotiateTE.NegotiateResult.Reject;
            AoWHSMap.Current.FTokenManager.TokenControls[FPlayer].AddEvent(TE);
            TE.Release();
            Result = true;
        }

        return Result;
    }

    // Diplomacy.pas:296
    public virtual NegotiateTE CreateNegotiateTE()
    {
        NegotiateTE Event;
        ArmyID ID;
        Event = new NegotiateTE(null);
        Event.FPlayer = FPlayer;
        Event.FObject = FObject;
        Event.FX = (byte)FX;
        Event.FY = (byte)FY;
        Event.FL = (byte)FL;
        Event.FData1 = FData1;
        Event.FData2 = FData2;
        ID = FArmyID;
        if (ID != null)
        {
            Event.FArmyID = (ArmyID)ID.Copy(GameTables.AoWEngine);
            Event.FMovePoints = FMovePoints;
        }
        else
        {
            Event.FArmyID = null;
        }

        return Event;
    }

    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done) => trigger == 0 && AoWHSMap.Current.FSeatedPlayer == FPlayer && base.Execute(mode, trigger, done);
}
