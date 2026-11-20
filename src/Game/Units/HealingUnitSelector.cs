#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Units;

public sealed class HealingUnitSelector(EngineObject creator) : UnitSelector(creator)
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
        string error = "";
        Troop source = StrategicMap.Current.UnitControl.FindUnit(SourceUnitId);
        if (CanSelect(ref error) && !StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            int i = 0;
            Troop target = null;
            while (target is null && i != Units.GetCount())
            {
                if ((Selection & (1 << i)) != 0)
                {
                    target = Units.GetUnit(i);
                }

                i += 1;
            }

            if (target is not null)
            {
                HealingTokenEvent eventValue = new(null)
                {
                    Holder = Creator.CreationHolder(),
                    SourceId = source.UnitId,
                    SourcePlayer = (byte)source.Player,
                    TargetId = target.UnitId,
                    TargetPlayer = (byte)target.Player
                };
                StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
                _ = eventValue.Release();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
