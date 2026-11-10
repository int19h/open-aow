// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// (unit ID, spell ID)
public partial class SpellCastEventLog : EventLog
{
    public SpellCastEventLog(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131823;

    public int FUnitID;
    public int FSpellID;

    // Spells.PAS:837
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FUnitID, 20);
        Stream.rwInteger(ref FSpellID, 21);
    }

    // Spells.PAS:855
    public override string GetText()
    {
        var spell = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
        return AbilityRules.Format(AbilityRules.TranslateRStr(P4XCastingReadyRStr), [spell.FSpellName]);
    }

    // Spells.PAS:818
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool Show;
        string Reason = "";
        Show = Trigger == ExecuteTrigger.User;
        Show = Show || AoWHSMap.Current.FPlayMode == PlayMode.Turn && GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.SpellReady);
        Show = Show || AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous && GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.SpellReady);
        if (Show)
        {
            var hero = AoWHSMap.Current.FUnitControl.FindUnit<Hero>(FUnitID);
            if (hero != null && hero.FCastingSpellID == FSpellID && hero.CastingReady())
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(52).Activate(hero, ref Reason);
            }
        }

        return false;
    }

    // Spells.PAS:843
    public override void ViewLocation()
    {
        AbstractUnit AUnit;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        AUnit = AoWHSMap.Current.FUnitControl.FindUnit(FUnitID);
        if (AUnit != null)
        {
            AUnit.GetLocation(ref X, ref Y, ref L);
            CenterViewTo(X, Y, L);
        }
    }

    private static readonly string P4XCastingReadyRStr = "%s ready";

}
