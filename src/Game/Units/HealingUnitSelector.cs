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
    public override bool CanSelectUnit(Troop troop)
    {
        var unitType = troop.GetUnitType();
        if (!(Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType) && troop.GetHits() != troop.GetHitPoints()))
        {
            return false;
        }

        return true;
    }

    // AoWAb.PAS:3904
    public override bool Select()
    {
        bool result;
        Troop source;
        Troop target;
        string error = "";
        int I;
        HealingTokenEvent eventValue;
        source = AoWHSMap.Current.UnitControl.FindUnit(SourceUnitId);
        if (CanSelect(ref error) && !AoWHSMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            I = 0;
            target = null;
            while (target == null && I != Units.GetCount())
            {
                if ((Selection & (1 << I)) != 0)
                {
                    target = Units.GetUnit(I);
                }

                I += 1;
            }

            if (target != null)
            {
                eventValue = new HealingTokenEvent(null);
                eventValue.SourceId = source.UnitId;
                eventValue.SourcePlayer = (byte)source.Player;
                eventValue.TargetId = target.UnitId;
                eventValue.TargetPlayer = (byte)target.Player;
                AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
                eventValue.Release();
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // intf_P1_08.inc:65

}
