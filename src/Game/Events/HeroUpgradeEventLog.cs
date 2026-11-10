// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// FUnitID. FOldLevel. FNewLevel. FDay
public partial class HeroUpgradeEventLog : EventLog
{
    public HeroUpgradeEventLog(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131825;

    public int FUnitID;
    public byte FOldLevel;
    public byte FNewLevel;
    public new int FDay;

    // Hero.PAS:57
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FUnitID, 25);
        Stream.rwByte(ref FOldLevel, 26);
        Stream.rwByte(ref FNewLevel, 27);
        Stream.rwInteger(ref FDay, 28);
    }

    // Hero.PAS:66
    public override string GetText()
    {
        string Result = "";
        Hero Hero;
        Hero = AoWHSMap.Current.FUnitControl.FindUnit<Hero>(FUnitID);
        if (Hero != null)
        {
            if (FNewLevel - FOldLevel == 1)
            {
                var name = Hero.GetName();
                Result = AbilityRules.Format(AbilityRules.TranslateRStr(XMadeALevelRStr), [name]);
            }
            else
            {
                var name = Hero.GetName();
                Result = AbilityRules.Format(AbilityRules.TranslateRStr(XMadeYLevelsRStr), [name, (FNewLevel - FOldLevel)]);
            }
        }

        return Result;
    }

    // Hero.PAS:76
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool Result = default;
        Hero Hero;
        bool ExecuteNow;
        var unitControl = AoWHSMap.Current.FUnitControl;
        Hero = unitControl.FindUnit<Hero>(FUnitID);
        if (Hero != null && AoWHSMap.Current.FSeatedPlayer == Hero.FPlayer)
        {
            if (Hero.FUpgradePending)
            {
                ExecuteNow = Trigger == ExecuteTrigger.User;
                ExecuteNow = ExecuteNow || AoWHSMap.Current.FPlayMode == PlayMode.Turn && GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.HeroUpgrade);
                ExecuteNow = ExecuteNow || AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous && GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.HeroUpgrade);
                if (ExecuteNow)
                {
                    Result = base.Execute(Mode, Trigger, Done);
                }
                else
                {
                    Result = false;
                }
            }
            else
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(Hero.FPlayer).FDay == FDay)
                {
                    var canOnlyUpgradeHeroOnce = AbilityRules.TranslateRStr(CanOnlyUpgradeHeroOnceRStr);
                    AoWHSMap.Current.ShowWarning(canOnlyUpgradeHeroOnce);
                }
                else
                {
                    var canOnlyUpgradeOnTheDayALevelIsMade = AbilityRules.TranslateRStr(CanOnlyUpgradeOnTheDayALevelIsMadeRStr);
                    AoWHSMap.Current.ShowWarning(canOnlyUpgradeOnTheDayALevelIsMade);
                }

                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:102
    public override void ViewLocation()
    {
        AbstractUnit U;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        U = AoWHSMap.Current.FUnitControl.FindUnit(FUnitID);
        if (U != null)
        {
            U.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                CenterViewTo(X, Y, L);
            }
        }
    }

    private static readonly string XMadeALevelRStr = "%s made a level";
    private static readonly string XMadeYLevelsRStr = "%s made %d levels";
    private static readonly string CanOnlyUpgradeHeroOnceRStr = "Can only upgrade hero once";
    private static readonly string CanOnlyUpgradeOnTheDayALevelIsMadeRStr = "Can only upgrade on the day a level is made";

}
