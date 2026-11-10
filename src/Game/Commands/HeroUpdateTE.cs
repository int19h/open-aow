// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

// FUnitID. FMode. FItemID (also spell ID). FParameter
public partial class HeroUpdateTE : TokenEvent
{
    public HeroUpdateTE(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135429;

    public int FUnitID;
    public HeroUpdateMode FMode;
    public int FItemID;
    public int FParameter;

    // Hero.PAS:2279
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FUnitID, 20);
        Stream.rwData(Bytes.Of(ref FMode, 1), 1, 21);
        Stream.rwInteger(ref FItemID, 22);
        Stream.rwInteger(ref FParameter, 23);
    }

    // Hero.PAS:2291
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        ExecuteResult Result;
        bool Success;
        Success = false;
        AbstractUnit unit = AoWHSMap.Current.FUnitControl.FindUnit(FUnitID);
        if (unit == null)
        {
            // A missing id is a nil hero. The original reads FPlayer and faults.
            // ExecuteTokenEvent catches the fault, shows the message and returns erDone.
            // Returning erDoneHalt stops the rest of Process.
            _ = unit.FPlayer;
        }

        if (unit is Hero Hero && ((AoWHSMap.Current.FCombat == null && AoWHSMap.Current.FCurrentPlayer == Hero.FPlayer) || (AoWHSMap.Current.FCombat != null && AoWHSMap.Current.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)AoWHSMap.Current.FCombat.FCombatData.FCurrentPlayer).FPlayer == Hero.FPlayer)))
        {
            switch (FMode)
            {
                case HeroUpdateMode.PlaceItem:
                    Success = Hero.ExecutePlaceItem(FItemID, (HeroItemPosition)FParameter);
                    break;
                case HeroUpdateMode.SwapItem:
                    Success = Hero.ExecuteSwapItem(FItemID, FParameter);
                    break;
                case HeroUpdateMode.StartCasting:
                    Success = Hero.ExecuteStartCasting(FItemID);
                    break;
                case HeroUpdateMode.CancelCasting:
                    Success = Hero.ExecuteCancelCasting();
                    break;
            }
        }

        if (Success)
        {
            Result = ExecuteResult.Done;
        }
        else
        {
            Result = ExecuteResult.DoneHalt;
        }

        return Result;
    }

    public enum HeroUpdateMode : byte
    {
        PlaceItem = 0, SwapItem = 1, StartCasting = 2, CancelCasting = 3
    }

    // NetworkE.pas:230

}
