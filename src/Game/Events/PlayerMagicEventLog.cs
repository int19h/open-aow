// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// (mode, location, spell ID, value, enchantment name)
public partial class PlayerMagicEventLog(CustomNode owner) : EventLog(owner)
{

    public new const int ClassId = 131_824;

    public EventMode Mode { get; set; }

    public MapCoordinate Location { get; internal set; } = MapCoordinate.None;

    public int SpellId { get; set; }

    private int _value;

    public string EnchantmentName { get; set; } = "";

    // Spells.PAS:1854
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Mode = persister.Enum("Mode", value: Mode);
        SpellId = persister.Int32("SpellId", value: SpellId);
        _value = persister.Int32("Value", value: _value);
        EnchantmentName = persister.String("EnchantmentName", value: EnchantmentName);
        int location = persister.Int32("Location", value: StoredCoordinateImage.From(Location).ToInt32());
        Location = StoredCoordinateImage.FromInt32(location).Location;
    }

    // Spells.PAS:1892
    public override string GetText()
    {
        switch (Mode)
        {
            case EventMode.SpellResearched:
                Spell spell = GamePack.Current.SpellControl[SpellId];
                return AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_researchedFormat), arguments: [spell.SpellName]);
            case EventMode.OutOfMana:
                return StrategicMap.Current.SeatedPlayer == Player
                    ? AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_removedOutOfManaFormat), arguments: [EnchantmentName])
                    : AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.RemovedFormat), arguments: [EnchantmentName]);
            case EventMode.StartResearch:
            default:
                break;
        }

        return "";
    }

    // Spells.PAS:1874
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool show = Mode switch
        {
            EventMode.OutOfMana => trigger == ExecuteTrigger.User,
            EventMode.SpellResearched => trigger == ExecuteTrigger.User || (StrategicMap.Current.PlayMode == PlayMode.Turn
                                    ? GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.SpellResearched)
                                    : GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.SpellResearched)),
            EventMode.StartResearch => true,
            _ => false,
        };
        return show && base.Execute(mode, trigger, done);
    }

    // Spells.PAS:1863
    public override void ViewLocation()
    {
        sbyte x = Location.X;
        if (x != -1)
        {
            StrategicMap.Current.CenterViewTo(x, Location.Y, Location.MapLevel);
            StrategicMap.Current.PlayCenterAnimation(Location.X, Location.Y, L: Location.MapLevel);
        }
    }

    private static readonly string s_researchedFormat = "%s researched";
    private static readonly string s_removedOutOfManaFormat = "%s removed (out of mana)";

    public enum EventMode : byte
    {
        SpellResearched = 0, OutOfMana = 1, StartResearch = 2
    }
}
