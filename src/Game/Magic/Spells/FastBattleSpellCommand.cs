// Translated from AoWE.pas.
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public partial class FastBattleSpellCommand : FastBattleCommand
{
    public FastBattleSpellCommand(int AbilityID) : base(AbilityID)
    { }

    public int FSpellID;

    // Spells.PAS:1
    public override string GetName()
    {
        var spell = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
        string Result = AbilityRules.Format(AbilityRules.TranslateRStr(P4CastXRStr), [spell.FSpellName]);
        if (FTarget != null)
        {
            Result = Result + " on " + FTarget.IDStr();
        }

        return Result;
    }

    private static readonly string P4CastXRStr = "Cast %s";

    // intf_P5_05.inc:10

    // intf_P5_05.inc:10

}
