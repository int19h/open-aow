#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Commands;

using Aow.Game.Units;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities;

public partial class DispelMagicAbilityUnitSelector(EngineObject creator) : UnitSelector(creator)
{

    // AoWAb.PAS:4403
    public override bool CanSelectUnit(Troop troop) => troop.CanDispelEnchantment(Player);

    // AoWAb.PAS:4407
    public override bool Select()
    {
        string error = "";
        Troop source = StrategicMap.Current.UnitControl.FindUnit(SourceUnitId);
        bool result;
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
                DispelMagicAbilityTokenEvent eventValue = new(null)
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
