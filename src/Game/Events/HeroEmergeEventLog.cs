#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Events;

// UnitId. _emergeDay. Text. Mode. LocationName
public partial class HeroEmergeEventLog(CustomNode owner) : EventLog(owner)
{
    public new const int ClassId = 131832;

    public int UnitId { get; set; }

    private int _emergeDay;
    private string _text = "";
    private HeroJoinMode _mode;

    public string LocationName { get; set; } = "";

    // Hero.PAS:188
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        UnitId = persister.Int32("UnitId", value: UnitId);
        _emergeDay = persister.Int32("EmergeDay", value: _emergeDay);
        _text = persister.String("Text", value: _text);
        LocationName = persister.String("LocationName", value: LocationName);
        _mode = persister.Enum("Mode", value: _mode);
    }

    // Hero.PAS:198
    public override string GetText() => _text;

    // Hero.PAS:254
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        HeroJoinEventLog log = CreateHeroJoinEventLog();
        bool result;
        if (log is not null)
        {
            result = log.Execute(mode, trigger, done);
            _ = log.Release();
        }
        else
        {
            Hero hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(UnitId);
            if (hero is not null)
            {
                if (hero.Player == Player)
                {
                    string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_alreadyJoinedFormat), [hero.HeroName]);
                    StrategicMap.Current.ShowWarning(format);
                }
                else
                {
                    string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_doesNotWantToJoinAnymoreFormat), [hero.HeroName]);
                    StrategicMap.Current.ShowWarning(format);
                }
            }

            result = false;
        }

        return result;
    }

    // Hero.PAS:203
    public override void ViewLocation()
    {
        Troop u = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        if (u is not null)
        {
            MapCoordinate coordinate17 = u.GetLocation();
            sbyte x = coordinate17.X;
            sbyte y = coordinate17.Y;
            sbyte l = coordinate17.MapLevel;
            if (x != -1)
            {
                CenterViewTo(x, y, l);
            }
        }
    }

    // Hero.PAS:175
    public void Setup(Hero hero, sbyte player, HeroJoinMode mode)
    {
        Player = player;
        UnitId = hero.UnitId;
        _emergeDay = StrategicMap.Current.Day;
        _mode = mode;
        string name = hero.HeroName;
        if (!IsEmpty(hero.GetNickname()))
        {
            _ = $"{name} {hero.GetNickname()}";
        }

        // { Original builds Name but formats the message from FHeroName instead. }
        _text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_offersToJoinFormat), [hero.HeroName]);
    }

    // Hero.PAS:214
    public HeroJoinEventLog CreateHeroJoinEventLog()
    {
        Hero hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(UnitId);
        HeroJoinEventLog result;
        if (hero is not null && hero.Player == 0 && hero.OfferToJoin(Player) && hero.ArmyOwner is { } army)
        {
            result = new HeroJoinEventLog(null)
            {
                Holder = CreationHolder(),
                JoinIndex = Id
            };
            switch (_mode)
            {
                case HeroJoinMode.Emerged:
                    result.SetupJoinArmy(army, Player, result.JoinMode);
                    result.FirstUnitId = hero.UnitId;
                    string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_heroEmergedOfferFormat), [hero.HeroName, LocationName, result.Gold]);
                    result.JoinMessage.Text = format;
                    break;
                case HeroJoinMode.Resurrected:
                    result.SetupJoinArmy(army, Player, result.JoinMode);
                    result.FirstUnitId = hero.UnitId;
                    string formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_heroResurrectedOfferFormat), [hero.HeroName, result.Gold]);
                    result.JoinMessage.Text = formattedText;
                    break;
                default:
                    break;
            }
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Hero.PAS:242
    public bool ExecuteAI()
    {
        HeroJoinEventLog log = CreateHeroJoinEventLog();
        bool result;
        if (log is not null)
        {
            result = log.ExecuteAI();
            _ = log.Release();
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_offersToJoinFormat = "%s offers to join!";
    private static readonly string s_alreadyJoinedFormat = "%s already joined";
    private static readonly string s_doesNotWantToJoinAnymoreFormat = "%s doesn't want to join anymore";
    private static readonly string s_heroEmergedOfferFormat = "%0:s emerged at %1:s and offers to join you for %d gold";
    private static readonly string s_heroResurrectedOfferFormat = "%s was resurrected and offers to join you for %d gold";

    public enum HeroJoinMode : byte
    {
        Emerged = 0, Resurrected = 1
    }

}
