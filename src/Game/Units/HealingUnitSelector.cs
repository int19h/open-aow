// Translated from AoWE.pas.
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Units;

public partial class HealingUnitSelector : UnitSelector
{
    public HealingUnitSelector() : base()
    { }

    // AoWAb.PAS:3894
    public override bool CanSelectUnit(AbstractUnit AUnit)
    {
        var unitType = AUnit.GetUnitType();
        if (!(Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType) && AUnit.GetHits() != AUnit.GetHitPoints()))
        {
            return false;
        }

        return true;
    }

    // AoWAb.PAS:3904
    public override bool Select()
    {
        bool Result;
        AbstractUnit Source;
        AbstractUnit Target;
        string Error = "";
        int I;
        HealingTE Event;
        Source = AoWHSMap.Current.FUnitControl.FindUnit(FSourceUnitID);
        if (CanSelect(ref Error) && !AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            I = 0;
            Target = null;
            while (Target == null && I != FUnits.GetCount())
            {
                if ((FSelection & (1 << I)) != 0)
                {
                    Target = FUnits.GetUnit(I);
                }

                I += 1;
            }

            if (Target != null)
            {
                Event = new HealingTE(null);
                Event.FSourceID = Source.FUnitID;
                Event.FSourcePlayer = (byte)Source.FPlayer;
                Event.FTargetID = Target.FUnitID;
                Event.FTargetPlayer = (byte)Target.FPlayer;
                AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // intf_P1_08.inc:65

}
