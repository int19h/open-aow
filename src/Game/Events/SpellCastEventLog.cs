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

    public new const int ClassId = 131823;

    public int UnitId;
    public int SpellId;

    // Spells.PAS:837
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref UnitId, 20);
        stream.Int32(ref SpellId, 21);
    }

    // Spells.PAS:855
    public override string GetText()
    {
        var spell = GamePack.Current.SpellControl.GetSpell(SpellId);
        return AbilityRules.Format(AbilityRules.TranslateResourceString(s_castingReadyFormat), [spell.SpellName]);
    }

    // Spells.PAS:818
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool show;
        string reason = "";
        show = trigger == ExecuteTrigger.User;
        show = show || StrategicMap.Current.PlayMode == PlayMode.Turn && GameTables.GameEngine.Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.SpellReady);
        show = show || StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GameTables.GameEngine.Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.SpellReady);
        if (show)
        {
            var hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(UnitId);
            if (hero != null && hero.CastingSpellId == SpellId && hero.CastingReady())
            {
                GamePack.Current.AbilityControl.GetAbility(52).Activate(hero, ref reason);
            }
        }

        return false;
    }

    // Spells.PAS:843
    public override void ViewLocation()
    {
        Troop troop;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        troop = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        if (troop != null)
        {
            troop.GetLocation(ref X, ref Y, ref L);
            CenterViewTo(X, Y, L);
        }
    }

    private static readonly string s_castingReadyFormat = "%s ready";

}
