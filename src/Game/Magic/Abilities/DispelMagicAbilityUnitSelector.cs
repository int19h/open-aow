#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Commands;
using Aow.Game.Session;
using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities;

public sealed class DispelMagicAbilityUnitSelector(EngineObject creator) : UnitSelector(creator)
{

    // AoWAb.PAS:4403
    public override bool CanSelectUnit(Troop? troop) => troop.CanDispelEnchantment(Player);

    // AoWAb.PAS:4407
    public override bool Select()
    {
        string error = "";
        Troop? source = Creator.GetEngine().StrategicMap.UnitControl.FindUnit(SourceUnitId);
        if (CanSelect(ref error) && !Creator.GetEngine().StrategicMap.Players.GetPlayers(source.Player).GetBusy())
        {
            int i = 0;
            Troop? target = null;
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
                    SourcePlayer = PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(source.Player)),
                    TargetId = target.UnitId,
                    TargetPlayer = PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(target.Player))
                };
                Creator.GetEngine().StrategicMap.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
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
