#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// (unit ID, spell ID)
public class SpellCastEventLog(CustomNode owner) : EventLog(owner)
{
    public new const int ClassId = 131_823;

    public int UnitId { get; set; }
    public int SpellId { get; set; }

    // Spells.PAS:837
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        UnitId = persister.Int32("UnitId", value: UnitId);
        SpellId = persister.Int32("SpellId", value: SpellId);
    }

    // Spells.PAS:855
    public override string GetText()
    {
        Spell spell = GamePack.Current.SpellControl[SpellId];
        return AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_castingReadyFormat), arguments: [spell.SpellName]);
    }

    // Spells.PAS:818
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool show = trigger == ExecuteTrigger.User;
        show = show || (StrategicMap.Current.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvents.SpellReady));
        show = show || (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvents.SpellReady));
        if (show)
        {
            Hero hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(UnitId);
            if (hero is not null && hero.CastingSpellId == SpellId && hero.CastingReady())
            {
                string reason = "";
                _ = GamePack.Current.AbilityControl[52].Activate(hero, ref reason);
            }
        }

        return false;
    }

    // Spells.PAS:843
    public override void ViewLocation()
    {
        Troop troop = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        if (troop is not null)
        {
            MapCoordinate coordinate21 = troop.GetLocation();
            int x = coordinate21.X;
            int y = coordinate21.Y;
            int l = coordinate21.MapLevel;
            CenterViewTo(x, y, l);
        }
    }

    private static readonly string s_castingReadyFormat = "%s ready";

}
