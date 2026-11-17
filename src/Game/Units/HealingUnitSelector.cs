#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Units;

public partial class HealingUnitSelector(EngineObject creator) : UnitSelector(creator)
{

    // AoWAb.PAS:3894
    public override bool CanSelectUnit(Troop troop)
    {
        UnitType unitType = troop.GetUnitType();
        return Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType) && troop.GetHits() != troop.GetHitPoints();
    }

    // AoWAb.PAS:3904
    public override bool Select()
    {
        bool result;
        Troop source;
        Troop target;
        string error = "";
        int i;
        HealingTokenEvent eventValue;
        source = StrategicMap.Current.UnitControl.FindUnit(SourceUnitId);
        if (CanSelect(ref error) && !StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            i = 0;
            target = null;
            while (target == null && i != Units.GetCount())
            {
                if ((Selection & (1 << i)) != 0)
                {
                    target = Units.GetUnit(i);
                }

                i += 1;
            }

            if (target != null)
            {
                eventValue = new HealingTokenEvent(null)
                {
                    Holder = Creator.CreationHolder(),
                    SourceId = source.UnitId,
                    SourcePlayer = (byte)source.Player,
                    TargetId = target.UnitId,
                    TargetPlayer = (byte)target.Player
                };
                StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
                _ = eventValue.Release();
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
