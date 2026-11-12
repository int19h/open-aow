// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// UnitId. OldLevel. NewLevel. _day
public partial class HeroUpgradeEventLog : EventLog
{
    public HeroUpgradeEventLog(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131825;

    public int UnitId;
    public byte OldLevel;
    public byte NewLevel;
    public new int _day;

    // Hero.PAS:57
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref UnitId, 25);
        stream.Byte(ref OldLevel, 26);
        stream.Byte(ref NewLevel, 27);
        stream.Int32(ref _day, 28);
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
                var name = hero.GetName();
                result = AbilityRules.Format(AbilityRules.TranslateResourceString(s_madeLevelFormat), [name]);
            }
            else
            {
                var name = hero.GetName();
                result = AbilityRules.Format(AbilityRules.TranslateResourceString(s_madeLevelsFormat), [name, (NewLevel - OldLevel)]);
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
        var unitControl = StrategicMap.Current.UnitControl;
        hero = unitControl.FindUnit<Hero>(UnitId);
        if (hero != null && StrategicMap.Current.SeatedPlayer == hero.Player)
        {
            if (hero.UpgradePending)
            {
                executeNow = trigger == ExecuteTrigger.User;
                executeNow = executeNow || StrategicMap.Current.PlayMode == PlayMode.Turn && GameTables.GameEngine.Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.HeroUpgrade);
                executeNow = executeNow || StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GameTables.GameEngine.Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.HeroUpgrade);
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
                if (StrategicMap.Current.Players.GetPlayers(hero.Player).Day == _day)
                {
                    var canOnlyUpgradeHeroOnce = AbilityRules.TranslateResourceString(s_canOnlyUpgradeHeroOnceText);
                    StrategicMap.Current.ShowWarning(canOnlyUpgradeHeroOnce);
                }
                else
                {
                    var canOnlyUpgradeOnTheDayALevelIsMade = AbilityRules.TranslateResourceString(s_canOnlyUpgradeOnDayLevelIsMadeText);
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

    private static readonly string s_madeLevelFormat = "%s made a level";
    private static readonly string s_madeLevelsFormat = "%s made %d levels";
    private static readonly string s_canOnlyUpgradeHeroOnceText = "Can only upgrade hero once";
    private static readonly string s_canOnlyUpgradeOnDayLevelIsMadeText = "Can only upgrade on the day a level is made";

}
