// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Events;

// FUnitID. FDay. FText. FMode. FLocationName
public partial class HeroEmergeEventLog : EventLog
{
    public HeroEmergeEventLog(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131832;

    public int FUnitID;
    public new int FDay;
    public string FText = "";
    public HeroJoinMode FMode;
    public string FLocationName = "";

    // Hero.PAS:188
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FUnitID, 39);
        Stream.rwInteger(ref FDay, 40);
        Stream.rwString(ref FText, 41);
        Stream.rwString(ref FLocationName, 42);
        Stream.rwData(Bytes.Of(ref FMode, 1), 1, 43);
    }

    // Hero.PAS:198
    public override string GetText() => FText;

    // Hero.PAS:254
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool Result = default;
        HeroJoinEventLog Log;
        Hero Hero = default;
        Log = CreateHeroJoinEventLog();
        if (Log != null)
        {
            Result = Log.Execute(Mode, Trigger, Done);
            Log.Release();
        }
        else
        {
            Hero = AoWHSMap.Current.FUnitControl.FindUnit<Hero>(FUnitID);
            if (Hero != null)
            {
                if (Hero.FPlayer == FPlayer)
                {
                    var format = AbilityRules.Format(AbilityRules.TranslateRStr(XAlreadyJoinedRStr), [Hero.FHeroName]);
                    AoWHSMap.Current.ShowWarning(format);
                }
                else
                {
                    var format = AbilityRules.Format(AbilityRules.TranslateRStr(XDoesntWantToJoinAnymoreRStr), [Hero.FHeroName]);
                    AoWHSMap.Current.ShowWarning(format);
                }
            }

            Result = false;
        }

        return Result;
    }

    // Hero.PAS:203
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

    // Hero.PAS:175
    public void Setup(Hero Hero, sbyte Player, HeroJoinMode Mode)
    {
        string Name;
        FPlayer = Player;
        FUnitID = Hero.FUnitID;
        FDay = AoWHSMap.Current.FDay;
        FMode = Mode;
        Name = Hero.FHeroName;
        if (!IsEmpty(Hero.GetNickname()))
        {
            _ = Name + " " + Hero.GetNickname();
        }

        // { Original builds Name but formats the message from FHeroName instead. }
        FText = AbilityRules.Format(AbilityRules.TranslateRStr(XOffersToJoinRStr), [Hero.FHeroName]);
    }

    // Hero.PAS:214
    public HeroJoinEventLog CreateHeroJoinEventLog()
    {
        HeroJoinEventLog Result;
        Hero Hero;
        Hero = AoWHSMap.Current.FUnitControl.FindUnit<Hero>(FUnitID);
        if (Hero != null && Hero.FPlayer == 0 && Hero.OfferToJoin(FPlayer) && Hero.Owner is not HeroList)
        {
            Result = new HeroJoinEventLog(null);
            Result.FJoinIndex = FID;
            switch (FMode)
            {
                case HeroJoinMode.Emerged:
                    Result.SetupJoinArmy(Hero.ArmyOwner, FPlayer, Result.FJoinMode);
                    Result.FFirstUnitID = Hero.FUnitID;
                    var format = AbilityRules.Format(AbilityRules.TranslateRStr(XEmergedAtYAndOffersToJoinYouForZgpRStr), [Hero.FHeroName, FLocationName, Result.FGold]);
                    Result.FJoinMessage.Text = format;
                    break;
                case HeroJoinMode.Resurrected:
                    Result.SetupJoinArmy(Hero.ArmyOwner, FPlayer, Result.FJoinMode);
                    Result.FFirstUnitID = Hero.FUnitID;
                    var aoWEFormat = AbilityRules.Format(AbilityRules.TranslateRStr(XwasResurrectedAndOffersToJoinYouForZgpRStr), [Hero.FHeroName, Result.FGold]);
                    Result.FJoinMessage.Text = aoWEFormat;
                    break;
            }
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Hero.PAS:242
    public bool ExecuteAI()
    {
        bool Result;
        HeroJoinEventLog Log;
        Log = CreateHeroJoinEventLog();
        if (Log != null)
        {
            Result = Log.ExecuteAI();
            Log.Release();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static readonly string XOffersToJoinRStr = "%s offers to join!";
    private static readonly string XAlreadyJoinedRStr = "%s already joined";
    private static readonly string XDoesntWantToJoinAnymoreRStr = "%s doesn't want to join anymore";
    private static readonly string XEmergedAtYAndOffersToJoinYouForZgpRStr = "%0:s emerged at %1:s and offers to join you for %d gold";
    private static readonly string XwasResurrectedAndOffersToJoinYouForZgpRStr = "%s was resurrected and offers to join you for %d gold";

    public enum HeroJoinMode : byte
    {
        Emerged = 0, Resurrected = 1
    }

}
