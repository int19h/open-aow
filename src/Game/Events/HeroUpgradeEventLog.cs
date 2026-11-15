// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Events;

// UnitId. OldLevel. NewLevel. UpgradeDay
public partial class HeroUpgradeEventLog : EventLog
{
    public HeroUpgradeEventLog(CustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131825;

    public int UnitId { get; set; }
    public byte OldLevel { get; set; }
    public byte NewLevel { get; set; }
    public int UpgradeDay { get; set; }

    // Hero.PAS:57
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        UnitId = persister.Int32("UnitId", value: UnitId);
        OldLevel = persister.Byte("OldLevel", value: OldLevel);
        NewLevel = persister.Byte("NewLevel", value: NewLevel);
        UpgradeDay = persister.Int32("UpgradeDay", value: UpgradeDay);
    }

    // Hero.PAS:66
    public override string GetText()
    {
        string result = "";
        Hero hero;
        hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(UnitId);
        if (hero != null)
        {
            if (NewLevel - OldLevel == 1)
            {
                string name = hero.GetName();
                result = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_madeLevelFormat), [name]);
            }
            else
            {
                string name = hero.GetName();
                result = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_madeLevelsFormat), [name, (NewLevel - OldLevel)]);
            }
        }

        return result;
    }

    // Hero.PAS:76
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool result = default;
        Hero hero;
        bool executeNow;
        UnitControl unitControl = StrategicMap.Current.UnitControl;
        hero = unitControl.FindUnit<Hero>(UnitId);
        if (hero != null && StrategicMap.Current.SeatedPlayer == hero.Player)
        {
            if (hero.UpgradePending)
            {
                executeNow = trigger == ExecuteTrigger.User;
                executeNow = executeNow || (StrategicMap.Current.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.HeroUpgrade));
                executeNow = executeNow || (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.HeroUpgrade));
                if (executeNow)
                {
                    result = base.Execute(mode, trigger, done);
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                if (StrategicMap.Current.Players.GetPlayers(hero.Player).Day == UpgradeDay)
                {
                    string canOnlyUpgradeHeroOnce = AbilityRules.TranslateResourceString(GetEngine(), s_canOnlyUpgradeHeroOnceText);
                    StrategicMap.Current.ShowWarning(canOnlyUpgradeHeroOnce);
                }
                else
                {
                    string canOnlyUpgradeOnTheDayALevelIsMade = AbilityRules.TranslateResourceString(GetEngine(), s_canOnlyUpgradeOnDayLevelIsMadeText);
                    StrategicMap.Current.ShowWarning(canOnlyUpgradeOnTheDayALevelIsMade);
                }

                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:102
    public override void ViewLocation()
    {
        Troop u;
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        u = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        if (u != null)
        {
            MapCoordinate coordinate19 = u.GetLocation();
            x = coordinate19.X;
            y = coordinate19.Y;
            l = coordinate19.MapLevel;
            if (x != -1)
            {
                CenterViewTo(x, y, l);
            }
        }
    }

    private static readonly string s_madeLevelFormat = "%s made a level";
    private static readonly string s_madeLevelsFormat = "%s made %d levels";
    private static readonly string s_canOnlyUpgradeHeroOnceText = "Can only upgrade hero once";
    private static readonly string s_canOnlyUpgradeOnDayLevelIsMadeText = "Can only upgrade on the day a level is made";

}
