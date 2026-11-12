// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

// UnitId. Mode. ItemId (also spell ID). Parameter
public partial class HeroUpdateTokenEvent : TokenEvent
{
    public HeroUpdateTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135429;

    public int UnitId;
    public HeroUpdateTokenEventMode Mode;
    public int ItemId;
    public int Parameter;

    // Hero.PAS:2279
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref UnitId, 20);
        stream.Bytes(Bytes.Of(ref Mode, 1), 1, 21);
        stream.Int32(ref ItemId, 22);
        stream.Int32(ref Parameter, 23);
    }

    // Hero.PAS:2291
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        bool success;
        success = false;
        Troop troop = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        if (troop == null)
        {
            // A missing id is a nil hero. The original reads FPlayer and faults.
            // ExecuteTokenEvent catches the fault, shows the message and returns erDone.
            // Returning erDoneHalt stops the rest of Process.
            _ = troop.Player;
        }

        if (troop is Hero hero && ((StrategicMap.Current.Combat == null && StrategicMap.Current.CurrentPlayer == hero.Player) || (StrategicMap.Current.Combat != null && StrategicMap.Current.Combat.CombatData.Players.GetPlayers((sbyte)StrategicMap.Current.Combat.CombatData.CurrentPlayer).Player == hero.Player)))
        {
            switch (Mode)
            {
                case HeroUpdateTokenEventMode.PlaceItem:
                    success = hero.ExecutePlaceItem(ItemId, (HeroItemPosition)Parameter);
                    break;
                case HeroUpdateTokenEventMode.SwapItem:
                    success = hero.ExecuteSwapItem(ItemId, Parameter);
                    break;
                case HeroUpdateTokenEventMode.StartCasting:
                    success = hero.ExecuteStartCasting(ItemId);
                    break;
                case HeroUpdateTokenEventMode.CancelCasting:
                    success = hero.ExecuteCancelCasting();
                    break;
            }
        }

        if (success)
        {
            result = ExecuteResult.Done;
        }
        else
        {
            result = ExecuteResult.DoneHalt;
        }

        return result;
    }

    // NetworkE.pas:230

}

public enum HeroUpdateTokenEventMode : byte
{
    PlaceItem = 0, SwapItem = 1, StartCasting = 2, CancelCasting = 3
}
