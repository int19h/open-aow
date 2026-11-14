// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Commands;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

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

    public sbyte X { get; set; }
    public sbyte Y { get; set; }
    public sbyte MapLevel { get; set; }

    public int MovePoints { get; set; }

    public ArmyId ArmyId { get; set; }
    public NegotiateObject Object { get; set; }

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
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        X = PersistSupport.UnsignedByte(persister, name: "X", value: X);
        Y = PersistSupport.UnsignedByte(persister, name: "Y", value: Y);
        MapLevel = PersistSupport.UnsignedByte(persister, name: "MapLevel", value: MapLevel);
        Object = persister.Enum("Object", value: Object);
        _data1 = persister.Int32("Data1", value: _data1);
        _data2 = persister.Int32("Data2", value: _data2);
        ArmyId = persister.Created("ArmyId", objectValue: ArmyId);
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
