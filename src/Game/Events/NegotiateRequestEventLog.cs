// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Commands;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Events;

// X/Y/L, MovePoints, ArmyId, Object, Data1/Data2. inherited join fields
public partial class NegotiateRequestEventLog : JoinEventLog
{
    // Diplomacy.pas:264
    public NegotiateRequestEventLog(ECustomNode owner) : base(owner)
    {
        ArmyId = null;
    }

    // Diplomacy.pas:279
    public new const int ClassId = 131814;

    public sbyte X;
    public sbyte Y;
    public sbyte MapLevel;

    public int MovePoints { get; set; }

    public ArmyId ArmyId;
    public NegotiateObject Object;
    private int _data1;
    private int _data2;

    // Diplomacy.pas:270
    public override void Dispose()
    {
        ArmyId id;
        id = ArmyId;
        if (id != null)
        {
            id?.Dispose();
        }

        base.Dispose();
    }

    // Diplomacy.pas:284
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref X), 40);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Y), 41);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref MapLevel), 42);
        stream.Bytes(Bytes.Of(ref Object, 1), 1, 43);
        stream.Int32(ref _data1, 44);
        stream.Int32(ref _data2, 45);
        stream.Created(ref ArmyId, null, 46);
    }

    // Diplomacy.pas:320
    public override bool Accept(ref string errorText)
    {
        NegotiateTokenEvent tokenEvent;
        bool result = base.Accept(ref errorText);
        if (result)
        {
            if (StrategicMap.Current.Players.GetPlayers(_player).PlayerType == PlayerType.Human && StrategicMap.Current.Players.GetPlayers(_player).GetBusy())
            {
                return result;
            }

            tokenEvent = CreateNegotiateTokenEvent();
            tokenEvent.Gold = Gold;
            tokenEvent.Offer = NegotiateTokenEvent.NegotiateOffer.OfferBribe;
            tokenEvent.Result = NegotiateTokenEvent.NegotiateResult.Accept;
            StrategicMap.Current.TokenManager.TokenControls[_player].AddEvent(tokenEvent);
            tokenEvent.Release();
            result = true;
        }

        return result;
    }

    // Diplomacy.pas:359
    public override bool Cancel() => true;

    // Diplomacy.pas:339
    public override bool Reject(ref string errorText)
    {
        NegotiateTokenEvent tokenEvent;
        bool result = base.Reject(ref errorText);
        if (result)
        {
            if (StrategicMap.Current.Players.GetPlayers(_player).PlayerType == PlayerType.Human && StrategicMap.Current.Players.GetPlayers(_player).GetBusy())
            {
                return result;
            }

            tokenEvent = CreateNegotiateTokenEvent();
            tokenEvent.Gold = 0;
            tokenEvent.Offer = NegotiateTokenEvent.NegotiateOffer.ForceOffer;
            tokenEvent.Result = NegotiateTokenEvent.NegotiateResult.Reject;
            StrategicMap.Current.TokenManager.TokenControls[_player].AddEvent(tokenEvent);
            tokenEvent.Release();
            result = true;
        }

        return result;
    }

    // Diplomacy.pas:296
    public virtual NegotiateTokenEvent CreateNegotiateTokenEvent()
    {
        NegotiateTokenEvent eventValue;
        ArmyId id;
        eventValue = new NegotiateTokenEvent(null) { Holder = CreationHolder() };
        eventValue.Player = _player;
        eventValue.Object = Object;
        eventValue.X = (byte)X;
        eventValue.Y = (byte)Y;
        eventValue.MapLevel = (byte)MapLevel;
        eventValue.Data1 = _data1;
        eventValue.Data2 = _data2;
        id = ArmyId;
        if (id != null)
        {
            eventValue.ArmyId = (ArmyId)id.Copy(GetEngine());
            eventValue.MovePoints = MovePoints;
        }
        else
        {
            eventValue.ArmyId = null;
        }

        return eventValue;
    }

    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done) => trigger == 0 && StrategicMap.Current.SeatedPlayer == _player && base.Execute(mode, trigger, done);
}
