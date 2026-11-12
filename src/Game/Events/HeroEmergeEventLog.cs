// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Events;

// UnitId. _day. Text. Mode. LocationName
public partial class HeroEmergeEventLog : EventLog
{
    public HeroEmergeEventLog(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131832;

    public int UnitId;
    private new int _day;
    private string _text = "";
    private HeroJoinMode _mode;
    public string LocationName = "";

    // Hero.PAS:188
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref UnitId, 39);
        stream.Int32(ref _day, 40);
        stream.String(ref _text, 41);
        stream.String(ref LocationName, 42);
        stream.Bytes(Bytes.Of(ref _mode, 1), 1, 43);
    }

    // Hero.PAS:198
    public override string GetText() => _text;

    // Hero.PAS:254
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool result = default;
        HeroJoinEventLog log;
        Hero hero = default;
        log = CreateHeroJoinEventLog();
        if (log != null)
        {
            result = log.Execute(mode, trigger, done);
            log.Release();
        }
        else
        {
            hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(UnitId);
            if (hero != null)
            {
                if (hero.Player == _player)
                {
                    var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_alreadyJoinedFormat), [hero.HeroName]);
                    StrategicMap.Current.ShowWarning(format);
                }
                else
                {
                    var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_doesNotWantToJoinAnymoreFormat), [hero.HeroName]);
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
        Troop U;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        U = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        if (U != null)
        {
            U.GetLocation(out X, out Y, out L);
            if (X != -1)
            {
                CenterViewTo(X, Y, L);
            }
        }
    }

    // Hero.PAS:175
    public void Setup(Hero hero, sbyte player, HeroJoinMode mode)
    {
        string name;
        _player = player;
        UnitId = hero.UnitId;
        _day = StrategicMap.Current.Day;
        _mode = mode;
        name = hero.HeroName;
        if (!IsEmpty(hero.GetNickname()))
        {
            _ = name + " " + hero.GetNickname();
        }

        // { Original builds Name but formats the message from FHeroName instead. }
        _text = AbilityRules.Format(AbilityRules.TranslateResourceString(s_offersToJoinFormat), [hero.HeroName]);
    }

    // Hero.PAS:214
    public HeroJoinEventLog CreateHeroJoinEventLog()
    {
        HeroJoinEventLog result;
        Hero hero;
        hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(UnitId);
        if (hero != null && hero.Player == 0 && hero.OfferToJoin(_player) && hero.Owner is not HeroList)
        {
            result = new HeroJoinEventLog(null);
            result.JoinIndex = Id;
            switch (_mode)
            {
                case HeroJoinMode.Emerged:
                    result.SetupJoinArmy(hero.ArmyOwner, _player, result.JoinMode);
                    result.FirstUnitId = hero.UnitId;
                    var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_heroEmergedOfferFormat), [hero.HeroName, LocationName, result.Gold]);
                    result.JoinMessage.Text = format;
                    break;
                case HeroJoinMode.Resurrected:
                    result.SetupJoinArmy(hero.ArmyOwner, _player, result.JoinMode);
                    result.FirstUnitId = hero.UnitId;
                    var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(s_heroResurrectedOfferFormat), [hero.HeroName, result.Gold]);
                    result.JoinMessage.Text = formattedText;
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
        bool result;
        HeroJoinEventLog log;
        log = CreateHeroJoinEventLog();
        if (log != null)
        {
            result = log.ExecuteAI();
            log.Release();
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
