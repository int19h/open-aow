// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Events;

// (mode, location, spell ID, value, enchantment name)
public partial class PlayerMagicEventLog : EventLog
{
    // Spells.PAS:1847
    // mode, location, spell ID, value, enchantment name
    public PlayerMagicEventLog(ECustomNode owner) : base(owner)
    {
        Location.Packed = 16777215;
    }

    public new const int ClassId = 131824;

    public EventMode Mode;
    public MapCoordinate Location;
    public int SpellId;
    private int _value;
    public string EnchantmentName = "";

    // Spells.PAS:1854
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Bytes(Bytes.Of(ref Mode, 1), 1, 20);
        stream.Int32(ref SpellId, 21);
        stream.Int32(ref _value, 22);
        stream.String(ref EnchantmentName, 23);
        stream.Int32(ref Location.Packed, 24);
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
