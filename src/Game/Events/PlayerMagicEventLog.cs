// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// (mode, location, spell ID, value, enchantment name)
public partial class PlayerMagicEventLog : EventLog
{
    // Spells.PAS:1847
    // mode, location, spell ID, value, enchantment name
    public PlayerMagicEventLog(ECustomNode owner) : base(owner)
    {
        Location = MapCoordinate.None;
    }

    public new const int ClassId = 131824;

    public EventMode Mode { get; set; }

    public MapCoordinate Location;

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
                var spell = GamePack.Current.SpellControl.GetSpell(SpellId);
                result = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_researchedFormat), [spell.SpellName]);
                break;
            case EventMode.OutOfMana:
                if (StrategicMap.Current.SeatedPlayer == _player)
                {
                    result = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_removedOutOfManaFormat), [EnchantmentName]);
                }
                else
                {
                    result = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.RemovedFormat), [EnchantmentName]);
                }

                break;
        }

        return result;
    }

    // Spells.PAS:1874
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool result = default;
        bool show;
        switch (Mode)
        {
            case EventMode.OutOfMana:
                show = trigger == ExecuteTrigger.User;
                break;
            case EventMode.SpellResearched:
                if (trigger != ExecuteTrigger.User)
                {
                    if (StrategicMap.Current.PlayMode == PlayMode.Turn)
                    {
                        show = GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.SpellResearched);
                    }
                    else
                    {
                        show = GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.SpellResearched);
                    }
                }
                else
                {
                    show = true;
                }

                break;
            case EventMode.StartResearch:
                show = true;
                break;
            default:
                show = false;
                break;
        }

        if (show)
        {
            result = base.Execute(mode, trigger, done);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:1863
    public override void ViewLocation()
    {
        sbyte X;
        X = Location.X;
        if (X != -1)
        {
            StrategicMap.Current.CenterViewTo(X, Location.Y, Location.MapLevel);
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
