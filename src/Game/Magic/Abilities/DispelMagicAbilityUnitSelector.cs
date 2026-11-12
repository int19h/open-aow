// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Commands;

using Aow.Game.Units;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities;

public partial class DispelMagicAbilityUnitSelector : UnitSelector
{
    public DispelMagicAbilityUnitSelector(EngineObject creator) : base(creator)
    { }

    // AoWAb.PAS:4403
    public override bool CanSelectUnit(Troop troop) => troop.CanDispelEnchantment(Player);

    // AoWAb.PAS:4407
    public override bool Select()
    {
        bool result;
        Troop source;
        Troop target;
        string error = "";
        int I;
        DispelMagicAbilityTokenEvent eventValue;
        source = StrategicMap.Current.UnitControl.FindUnit(SourceUnitId);
        if (CanSelect(ref error) && !StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
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
                eventValue = new DispelMagicAbilityTokenEvent(null) { Holder = Creator.CreationHolder() };
                eventValue.SourceId = source.UnitId;
                eventValue.SourcePlayer = (byte)source.Player;
                eventValue.TargetId = target.UnitId;
                eventValue.TargetPlayer = (byte)target.Player;
                StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
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
