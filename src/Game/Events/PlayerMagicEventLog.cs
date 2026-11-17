// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// (mode, location, spell ID, value, enchantment name)
public partial class PlayerMagicEventLog : EventLog
{
    public PlayerMagicEventLog(CustomNode owner) : base(owner) => Location = MapCoordinate.None;

    public new const int ClassId = 131824;

    public EventMode Mode { get; set; }

    public MapCoordinate Location { get; internal set; }

    // The fourth byte of the packed location stored at id 24, 0 from every writer and in every
    // corpus file, kept so that a loaded value saves back unchanged.
    public byte UnknownBytes { get; set; }

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
        int location = StoredCoordinateImage.From(Location, unknownBytes: UnknownBytes).ToInt32();
        location = persister.Int32("Location", value: location);
        (Location, UnknownBytes) = StoredCoordinateImage.FromInt32(location);
    }

    // Spells.PAS:1892
    public override string GetText()
    {
        string result = "";
        switch (Mode)
        {
            case EventMode.SpellResearched:
                Spell spell = GamePack.Current.SpellControl.GetSpell(SpellId);
                result = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_researchedFormat), [spell.SpellName]);
                break;
            case EventMode.OutOfMana:
                result = StrategicMap.Current.SeatedPlayer == Player
                    ? AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_removedOutOfManaFormat), [EnchantmentName])
                    : AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.RemovedFormat), [EnchantmentName]);

                break;
            case EventMode.StartResearch:
            default:
                break;
        }

        return result;
    }

    // Spells.PAS:1874
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool result = default;
        bool show = Mode switch
        {
            EventMode.OutOfMana => trigger == ExecuteTrigger.User,
            EventMode.SpellResearched => trigger == ExecuteTrigger.User || (StrategicMap.Current.PlayMode == PlayMode.Turn
                                    ? GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.SpellResearched)
                                    : GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.SpellResearched)),
            EventMode.StartResearch => true,
            _ => false,
        };
        result = show && base.Execute(mode, trigger, done);

        return result;
    }

    // Spells.PAS:1863
    public override void ViewLocation()
    {
        sbyte x;
        x = Location.X;
        if (x != -1)
        {
            StrategicMap.Current.CenterViewTo(x, Location.Y, Location.MapLevel);
            StrategicMap.Current.PlayCenterAnimation(Location.X, Location.Y, Location.MapLevel);
        }
    }

    private static readonly string s_researchedFormat = "%s researched";
    private static readonly string s_removedOutOfManaFormat = "%s removed (out of mana)";

    public enum EventMode : byte
    {
        SpellResearched = 0, OutOfMana = 1, StartResearch = 2
    }

}
