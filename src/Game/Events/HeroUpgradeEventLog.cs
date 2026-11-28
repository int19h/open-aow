#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Events;

// UnitId. OldLevel. NewLevel. UpgradeDay
public partial class HeroUpgradeEventLog(CustomNode owner, EngineObject holder) : EventLog(owner, holder: holder)
{
    public new const int ClassId = 131_825;

    public int UnitId { get; set; }
    public int OldLevel { get; set; }
    public int NewLevel { get; set; }
    public int UpgradeDay { get; set; }

    // Hero.PAS:57
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        UnitId = persister.Int32("UnitId", value: UnitId);
        OldLevel = persister.Byte("OldLevel", value: OldLevel.TruncateToByte());
        NewLevel = persister.Byte("NewLevel", value: NewLevel.TruncateToByte());
        UpgradeDay = persister.Int32("UpgradeDay", value: UpgradeDay);
    }

    // Hero.PAS:66
    public override string GetText()
    {
        Hero hero = GetEngine().StrategicMap.UnitControl.FindUnit<Hero>(UnitId);
        if (hero is not null)
        {
            if (NewLevel - OldLevel == 1)
            {
                string name = hero.GetName();
                return AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_madeLevelFormat), arguments: [name]);
            }
            else
            {
                string name = hero.GetName();
                return AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_madeLevelsFormat), arguments: [name, (NewLevel - OldLevel)]);
            }
        }

        return "";
    }

    // Hero.PAS:76
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        UnitControl unitControl = strategicMap.UnitControl;
        Hero hero = unitControl.FindUnit<Hero>(UnitId);
        if (hero is not null && strategicMap.SeatedPlayer == hero.Player)
        {
            if (hero.UpgradePending)
            {
                bool executeNow = trigger == ExecuteTrigger.User;
                executeNow = executeNow || (strategicMap.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvents.HeroUpgrade));
                executeNow = executeNow || (strategicMap.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvents.HeroUpgrade));
                return executeNow && base.Execute(mode, trigger, done);
            }
            else
            {
                if (GetEngine().StrategicMap.Players.GetPlayers(hero.Player).Day == UpgradeDay)
                {
                    string canOnlyUpgradeHeroOnce = AbilityRules.TranslateResourceString(GetEngine(), S: s_canOnlyUpgradeHeroOnceText);
                    GetEngine().StrategicMap.ShowWarning(canOnlyUpgradeHeroOnce);
                }
                else
                {
                    string canOnlyUpgradeOnTheDayALevelIsMade = AbilityRules.TranslateResourceString(GetEngine(), S: s_canOnlyUpgradeOnDayLevelIsMadeText);
                    GetEngine().StrategicMap.ShowWarning(canOnlyUpgradeOnTheDayALevelIsMade);
                }

                return false;
            }
        }
        else
        {
            return false;
        }
    }

    // Hero.PAS:102
    public override void ViewLocation()
    {
        Troop u = GetEngine().StrategicMap.UnitControl.FindUnit(UnitId);
        if (u is not null)
        {
            MapCoordinate coordinate19 = u.GetLocation();
            int x = coordinate19.X;
            int y = coordinate19.Y;
            int l = coordinate19.MapLevel;
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
