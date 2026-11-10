// Translated from AoWE.pas.
using Aow.Game.Commands;

using Aow.Game.Units;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities;

public partial class DispelMagicAbilityUnitSelector : UnitSelector
{
    public DispelMagicAbilityUnitSelector() : base()
    { }

    // AoWAb.PAS:4403
    public override bool CanSelectUnit(AbstractUnit AUnit) => AUnit.CanDispelEnchantment(FPlayer);

    // AoWAb.PAS:4407
    public override bool Select()
    {
        bool Result;
        AbstractUnit Source;
        AbstractUnit Target;
        string Error = "";
        int I;
        DispelMagicAbilityTE Event;
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
                Event = new DispelMagicAbilityTE(null);
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
